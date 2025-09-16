using MCM.Abstractions.Base.Global;

namespace MCMv5.Tests;

public abstract class BaseTestGlobalSettings<T> : AttributeGlobalSettings<T> where T : GlobalSettings, new()
{
    public override string FolderName => "MCMv5.Tests";

    public override string FormatType => "json2";
}