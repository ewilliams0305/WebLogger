using System.Text.Json.Serialization;
using WebLogger.Generators;

namespace WebLogger.ConsoleApp.GeneratedCommands;

[CommandStore]
public partial class TestCommandStore
{
    //this is a command
    [TargetCommand(
        "My Custom Command Function",
        "There is a thing worth hi",
        "Of course I want to explain what this does in detail.")]
    public ICommandResponse DoWorkMethod(string command, List<string> args)
    {
        return CommandResponse.Success(command, "We did the thing worth doing");
    }
    
    [TargetCommand(
        "ChildCommand",
        "There is a thing worth doing",
        "Of course I want to explain what this does in deta")]
    public ICommandResponse ExecutedMethods(string command, List<string> args)
    {
        throw new NotImplementedException("This BROKE");
    }

    public void TestInvalid()
    {

    }

}

public partial class DontGenerate
{

    //test a comment
    [TargetCommand(
        "DoWorkCommand",
        "There is a thing worth doing",
        "Of course I want to explain what this does in detail.")]
    public ICommandResponse DoWorkMethod(string command, List<string> args)
    {
        return CommandResponse.Success(command, "We did the thing worth doing");
    }

    [TargetCommand(
        "ExecutesTheMethod",
        "There is a thing worth doing",
        "Of course I want to explain what this does in detail.")]
    public ICommandResponse ExecutedMethod(string command, List<string> args)
    {
        throw new NotImplementedException("This BROKE!");
    }
}