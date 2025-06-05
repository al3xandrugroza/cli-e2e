using System.Reflection;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Utils;

internal static class AssemblyDetails
{
    private static string _productVersion;

    public static string GetProductVersion()
    {
        if (_productVersion == null)
        {
            var attribute = (AssemblyInformationalVersionAttribute)Assembly
                                                                   .GetExecutingAssembly()
                                                                   .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), true)
                                                                   .Single();
            _productVersion = attribute.InformationalVersion;
        }
        
        return _productVersion;
    }
}