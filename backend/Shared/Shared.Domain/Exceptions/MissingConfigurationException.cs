namespace Shared.Domain.Exceptions;

public class MissingConfigurationException : Exception
{
    public MissingConfigurationException(string key)
        : base($"Missing configuration value: {key}") { }
}