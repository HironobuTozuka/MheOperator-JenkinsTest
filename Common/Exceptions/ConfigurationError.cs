using System;
using Microsoft.Extensions.Configuration;

namespace Common.Exceptions
{
    public class ConfigurationError : Exception
    {
        public IConfigurationSection configurationSection { get; set; }
        
        public override string ToString()
        {
            return $"Unable to parse {configurationSection}";
        }
    }
}