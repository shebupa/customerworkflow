namespace ETX.Workflow.Customer.Fixtures;

[ExcludeFromCodeCoverage]
public class LoadDataAttribute : DataAttribute
{
    private readonly string _fileName;
    private readonly string _section;
    private static string _fileData = default;

    public LoadDataAttribute(
        string section)
    {
        _fileName = "Data/test-conditions-data.json";
        _section = section;
    }

    public override IEnumerable<object[]> GetData(
        MethodInfo testMethod)
    {
        if (testMethod == null)
        {
            throw new ArgumentNullException(nameof(testMethod));
        }

        var path = Path.IsPathRooted(_fileName)
            ? _fileName
            : Path.GetRelativePath(Directory.GetCurrentDirectory(), _fileName);

        if (!File.Exists(path))
        {
            throw new ArgumentException($"File not found: {path}");
        }

        _fileData = string.IsNullOrWhiteSpace(_fileData) ? File.ReadAllText(_fileName) : _fileData;
        if (string.IsNullOrEmpty(_section))
        {
            return JsonConvert.DeserializeObject<List<string[]>>(_fileData);
        }

        var allData = JObject.Parse(_fileData);
        var data = allData[_section];
        return new List<object[]>
            {
                new[] {
                    data.ToObject(testMethod.GetParameters().First().ParameterType)
                     }
            };
    }
}