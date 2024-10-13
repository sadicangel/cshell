using System.Text.Json.Serialization;

namespace CShell.DataModel;

[JsonDerivedType(typeof(ShellScalar))]
[JsonDerivedType(typeof(ShellArray))]
[JsonDerivedType(typeof(ShellRecord))]
public abstract record class ShellObject;
