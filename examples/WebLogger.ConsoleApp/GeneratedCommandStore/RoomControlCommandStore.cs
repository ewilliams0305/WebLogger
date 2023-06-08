namespace WebLogger.ConsoleApp.GeneratedCommandStore;

[CommandStore]
public partial class RoomControlCommandStore
{
    [TargetCommand(
        "PWRON",
        "Powers on the room",
        "Parameters: NA")]
    public ICommandResponse PowerOnCommand(string command, List<string> args)
    {
        //Power on the room and return the results.
        return CommandResponse.Success(command, "We did the thing worth doing");
    }

    [TargetCommand(
        "PWROFF",
        "Powers off the room",
        "Parameters: NA")]
    public ICommandResponse PowerOffCommand(string command, List<string> args)
    {
        //Power off the room and return the results.
        throw new NotImplementedException("Room power off threw exception");
    }
    
    [TargetCommand(
        "SOURCE",
        "Selects the specified source in the room",
        "Parameters: NA")]
    public ICommandResponse SelectSourceCommand(string command, List<string> args)
    {
        return args.Count > 0 
            ? CommandResponse.Success(command, $"Selected source: {args[0]}") 
            : CommandResponse.Failure(command, "Please provide a valid Source key argument.");
    }

    /// <summary>
    /// Note, this command is not generated.
    /// </summary>
    public void TestInvalid()
    {

    }
}