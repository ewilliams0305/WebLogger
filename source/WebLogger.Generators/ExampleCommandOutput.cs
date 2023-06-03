//using System;
//using System.Collections.Generic;
//using WebLogger.Generators;

//namespace WebLogger.ConsoleApp.Commands
//{
//    public partial class General : IWebLoggerCommand
//    {
//        private General()
//        {
//            GeneratedCommands.Commands.Add(this);
//        }
//        public string Command => "ReplaceMeCommand";
//        public string Description => "ReplaceMeDescription";
//        public string Help => "ReplaceMeHelp";
//        public Func<string, List<string>, ICommandResponse> CommandHandler => ExecuteCommand;

//        protected ICommandResponse ExecuteCommand(string command, List<string> args)
//        {
//            try
//            {
//                return ReplaceMethodNameHere(command, args);
//            }
//            catch (Exception e)
//            {
//                return CommandResponse.Error(this, e);
//            }
//        }
//    }
//}

