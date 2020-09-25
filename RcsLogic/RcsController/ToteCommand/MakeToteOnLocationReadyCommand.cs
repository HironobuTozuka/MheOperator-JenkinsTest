using Common;
using Common.Exceptions;
using Common.Models.Location;
using Common.Models.Tote;
using Data;
using Microsoft.Extensions.Logging;

namespace RcsLogic.RcsController.ToteCommand
{
    public class MakeToteOnLocationReadyCommand : IToteCommand
    {
        private readonly Tote _tote;
        private readonly ToteRepository _toteRepository;
        private ILogger<MakeToteOnLocationReadyCommand> _logger;

        public MakeToteOnLocationReadyCommand(ILoggerFactory loggerFactory, Tote tote, ToteRepository toteRepository)
        {
            _tote = tote;
            _toteRepository = toteRepository;
            _logger = loggerFactory.CreateLogger<MakeToteOnLocationReadyCommand>();
        }

        public void Execute()
        {
            try
            {
                if (Barcode.IsWrongBarcode(_tote.toteBarcode))
                {
                    _logger.LogTrace("Not updating tote {0} status, since is wrong barcode", _tote);
                    return;
                }
                _logger.LogTrace("Updating tote {0} status to Ready", _tote);
                _toteRepository.UpdateToteStatus(_tote, ToteStatus.Ready);
            }
            catch (ToteNotFoundException ex)
            {
                _logger.LogError(ex.ToString());
            }
            
        }
    }
}