namespace WebLogger.ConsoleApp.GeneratedCommandStore;

//No attribute applied.
public partial class DoNotGenerate
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