using System.Diagnostics;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Dtos;

internal class AnalyzeMessage
{
    public string ErrorCode { get; set; }
    public TraceLevel ErrorSeverity {  get; set; }
    public string RuleName { get; set; }
    public string Description { get; set; }
    public string Recommendation { get; set; }

    public override bool Equals(object? obj)
    {
        return Equals(obj as AnalyzeMessage);
    }

    public bool Equals(AnalyzeMessage? other)
    {
        return other is not null &&
                ErrorCode == other.ErrorCode &&
                ErrorSeverity == other.ErrorSeverity;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ErrorCode, ErrorSeverity);
    }
}
