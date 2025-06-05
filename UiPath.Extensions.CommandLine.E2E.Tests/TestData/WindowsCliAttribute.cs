using System.Reflection;
using System.Runtime.InteropServices;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using Xunit.Sdk;

namespace UiPath.Extensions.CommandLine.E2E.Tests.TestData;

internal class WindowsCliAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) yield break;

        yield return [new CliExecutor(CliCompatibility.Windows)];
    }
}
