using UiPath.Extensions.CommandLine.E2E.Tests.Dtos.InputParam;

namespace UiPath.Extensions.CommandLine.E2E.Tests.Builders;

internal class TestSetInputParamBuilder
{
    private readonly TestSetInputParamDto _inputParam;

    private TestSetInputParamBuilder()
    {
        _inputParam = new TestSetInputParamDto();
    }

    public static TestSetInputParamBuilder Init()
    {
        return new TestSetInputParamBuilder();
    }

    public TestSetInputParamBuilder WithName(string name)
    {
        _inputParam.Name = name;
        return this;
    }

    public TestSetInputParamBuilder WithType(string type)
    {
        _inputParam.Type = type;
        return this;
    }

    public TestSetInputParamBuilder WithValue(string value)
    {
        _inputParam.Value = value;
        return this;
    }

    public TestSetInputParamDto Build()
    {
        return _inputParam;
    }
}
