using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Serialization;
using UiPath.Extensions.CommandLine.E2E.Tests.Common;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.Options;
using UiPath.Extensions.CommandLine.E2E.Tests.Exceptions;
using UiPath.Extensions.CommandLine.E2E.Tests.Executor.ReturnObjects;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Executor;

public class CliExecutor
{
    private readonly string _cliPath;
    private readonly CliCompatibility _compatibility;
    private readonly CommandType _commandType;

    public CliExecutor(CliCompatibility compatibility) : this(compatibility, CommandType.JsonFile) { }

    public CliExecutor(CliCompatibility compatibility, CommandType commandType)
    {
        CheckCompatibility(compatibility);

        _cliPath = GetCliPath(compatibility);
        _compatibility = compatibility;
        _commandType = commandType;
    }

    public async Task<CliExecutionResult> Execute(RunOptions options)
    {
        Console.WriteLine("Start cli execution");
        var result = new CliExecutionResult();

        var optionsPath = GetOptionsFilePath(options);
        string commandArgs = _commandType switch
        {
            CommandType.VerboseInline => options.Options.GetInlineCommandArgs(),
            CommandType.ShortInline => options.Options.GetInlineShortCommandArgs(),
            _ => $"run {optionsPath}",
        };

        var processStartInfo = GetProcessStartInfo(commandArgs);
        
        using var process = new Process
        {
            StartInfo = processStartInfo,
            EnableRaisingEvents = true,
        };

        process.OutputDataReceived += (sender, args) =>
        {
            if (args.Data != null)
            {
                result.OutputLines.Add(args.Data);
            }
        };

        process.ErrorDataReceived += (sender, args) =>
        {
            if (args.Data != null)
            {
                result.ErrorLines.Add(args.Data);
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        await process.WaitForExitAsync();

        result.ExitCode = process.ExitCode switch
        {
            0 => CliExitCode.Success,
            _ => CliExitCode.Failure
        };

        Console.WriteLine($"Cli execution is done. ExitCode = {process.ExitCode}");
        return result;
    }

    private static string GetOptionsFilePath(RunOptions options)
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter> { new StringEnumConverter() },
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        string jsonString = JsonConvert.SerializeObject(options, settings);

        string tmpFile = Path.GetTempFileName();
        File.WriteAllText(tmpFile, jsonString);

        return tmpFile;
    }

    private static string GetCliPath(CliCompatibility compatibility)
    {
        var cliPath = compatibility.Equals(CliCompatibility.Windows) ? Environment.GetEnvironmentVariable(AgentEnvironment.WindowsCliPath) :
            Environment.GetEnvironmentVariable(AgentEnvironment.XPlatformCliPath);

        return cliPath ?? throw new EnvironmentVariableNotDefinedException($"{AgentEnvironment.WindowsCliPath} or {AgentEnvironment.XPlatformCliPath} is not found.");
    }

    private ProcessStartInfo GetProcessStartInfo(string commandArgs)
    {
        var processStartInfo = _compatibility.Equals(CliCompatibility.Windows) ? new ProcessStartInfo(_cliPath, commandArgs) :
            new ProcessStartInfo("dotnet", $"\"{_cliPath}\" " + commandArgs);

        processStartInfo.CreateNoWindow = true;
        processStartInfo.UseShellExecute = false;
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardError = true;

        return processStartInfo;
    }

    private static void CheckCompatibility(CliCompatibility compatibility)
    {
        if (compatibility.Equals(CliCompatibility.Windows) && !RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new CliCompatibilityException($"You cannot use a CLI.Windows on a CrossPlatform agent.");
    }
}
