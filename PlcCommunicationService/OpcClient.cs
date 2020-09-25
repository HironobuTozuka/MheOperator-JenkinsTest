using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using Workstation.ServiceModel.Ua;
using Workstation.ServiceModel.Ua.Channels;
using System.Reactive.Linq;
using System.Threading;
using Common.Models.Plc;
using Microsoft.Extensions.Configuration;
using PlcCommunicationService.Models;
using PlcRequestQueueService;
using Timer = System.Timers.Timer;


namespace PlcCommunicationService
{
    public abstract class OpcClient : IDisposable 
    {
        protected readonly ILoggerFactory LoggerFactory;
        protected readonly ILogger Logger;
        private IPlcReadNotificationListener _plcNotificationListener;
        public UaTcpSessionChannel Channel { get; private set; }
        
        private Timer _connectTimer;

        private ApplicationDescription _applicationDescription;
        
        private readonly string _endpointUrl;
        private readonly string _securityPolicyUri;
        private readonly Type[] _additionalTypes;

        private readonly List<MonitoredItemDefinition> _monitoredItems;
        private bool _faulted = true;
        private Heartbeat _heartbeat;

        public IInSignals InSignals { get; protected set; }
        public IOutSignals OutSignals { get; protected set; }
        public SignalReader SignalReader { get; private set; }

        public CommunicationState PlcStatus => Channel.State;

        public bool AllCommunicationOK { get
        {
            if (Channel == null) return false;
            lock (this)
            {
                return (Channel.State == CommunicationState.Opened && !_faulted);
            }

        } }

        public OpcClient(ILoggerFactory loggerFactory, ILogger logger, Type type, List<MonitoredItemDefinition> monitoredItems,
            IConfiguration configuration)
        {
            LoggerFactory = loggerFactory;
            Logger = logger;
            _monitoredItems = monitoredItems;
            
            try
            {
                // ReSharper disable once VirtualMemberCallInConstructor
                var clientConfiguration = GetClientConfiguration(configuration);
                _endpointUrl = (string)clientConfiguration.GetValue(typeof(string), "Uri");
                _securityPolicyUri = (string)clientConfiguration.GetValue(typeof(string), "SecurityPolicyUri");

                _additionalTypes = type.GetAllPublicConstantValues<Type[]>("Types").First();
            }
            catch (Exception e)
            {
                Logger.LogError("Unable to retrieve required fields form PLC connection configuration, application will not be connected to {0} PLC: {1}", this.GetType().ToString(), e.Message);
            }
        }
        
        protected abstract IConfigurationSection GetClientConfiguration(IConfiguration configuration);
        protected abstract void CreateIO();
        protected abstract string ClientId();

        public void CreateConnections(IPlcReadNotificationListener plcNotificationListener)
        {
            _plcNotificationListener = plcNotificationListener;
            try
            {
                _applicationDescription = new ApplicationDescription
                {
                    ApplicationName = "RomsMheOperator",
                    ApplicationUri = $"urn:{Dns.GetHostName()}:" + "RomsMheOperator",
                    ApplicationType = ApplicationType.Client
                };

                _connectTimer = new Timer(1000);
                _connectTimer.AutoReset = false;
                var subscriptionListener = new SubscriptionListener(LoggerFactory, ClientId(), _monitoredItems, this, _plcNotificationListener);
                _heartbeat = new Heartbeat(LoggerFactory, this);
                Connect();
                
                
            }
            catch (Exception e)
            {
                Logger.LogError("Unable to create signal Subscriptions, application will not be connected to {0} PLC: {1}", this.GetType().ToString(), e.Message);
            }

        }

        void IDisposable.Dispose()
        {
            Channel.AbortAsync();
        }
        
        private void Connect()
        {
            lock (this)
            {
                try
                {
                    Channel?.CloseAsync().Wait();

                    // create a 'UaTcpSessionChannel', a client-side channel that opens a 'session' with the server.
                    Channel = new UaTcpSessionChannel(
                        _applicationDescription,
                        null, // no x509 certificates
                        new AnonymousIdentity(), // no user identity
                        _endpointUrl, // the public endpoint of a server at opcua.rocks.
                        _securityPolicyUri,
                        LoggerFactory,
                        additionalTypes: _additionalTypes
                    );

                    Logger.LogInformation("Opc Connection attempt started");
                    Channel.OpenAsync().Wait();
                    Logger.LogInformation("Opc Connection attempt success");
                    CreateIO();
                    SignalReader = new SignalReader(Channel, LoggerFactory.CreateLogger<SignalReader>());
                    _heartbeat.ResetHeartbeatTimer();
                    _faulted = false;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Opc Connection attempt failed, starting reconnection timer.");
                    _connectTimer.Elapsed += ConnectTimer_Elapsed;
                    _connectTimer.Enabled = true;
                    _connectTimer.Start();
                }
            }
        }

        public void ChannelFaulted()
        {
            lock (this)
            {
                if (_faulted) return;
                _faulted = true;
                Connect();
            }
        }

        private void ConnectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _connectTimer.Stop();
            _connectTimer.Elapsed -= ConnectTimer_Elapsed;
            _connectTimer.Enabled = false;
            Logger.LogInformation("Opc Reconnection timer elapsed");
            Logger.LogInformation("Opc Trying to reconnect");
            Connect();
        }


    }
}
