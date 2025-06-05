using System.Reflection;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor;
using Xunit.Sdk;

namespace UiPath.Extensions.CommandLine.E2E.Tests.TestData;

internal class CrossPlatformCliWithInlineArgsAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        yield return [new CliExecutor(CliCompatibility.CrossPlatform, CommandType.VerboseInline)];
        yield return [new CliExecutor(CliCompatibility.CrossPlatform, CommandType.ShortInline)];
    }
}
