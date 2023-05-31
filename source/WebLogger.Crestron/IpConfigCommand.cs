using Crestron.SimplSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLogger.Crestron
{
    /// <summary>
    /// Displays IP information back to the console
    /// </summary>
    public sealed class IpConfigCommand : IWebLoggerCommand
    {
        /// <inheritdoc />
        public string Command => "IPCONFIG";
        /// <inheritdoc />
        public string Description => "Displays the IP configuration";
        /// <inheritdoc />
        public string Help => "Not parameters";
        /// <inheritdoc />
        public Func<string, List<string>, ICommandResponse> CommandHandler => GetEthernetInformation;

        /// <summary>
        /// Creates a new default instance of the Ip Config command handler.
        /// </summary>
        public IpConfigCommand()
        {
         
        }

        /// <summary>
        /// IpConfig command Handler
        /// Reports the servers IP/Mask/Gateway
        /// </summary>
        /// <param name="command">Command String</param>
        /// <param name="args">Arguments</param>
        private ICommandResponse GetEthernetInformation(string command, List<string> args)
        {
            try
            {
                var adapters = InitialParametersClass.NumberOfEthernetInterfaces;

                var builder = new StringBuilder();

                for (short i = 0; i < adapters; i++)
                {
                    builder.Append($"VC4> Ethernet Adaptor <span style=\"color:#DDDD11;\">[{i + 1}]</>:\r");
                    builder.Append($"        DHCP ........... : <span style=\"color:#FF00FF;\">{CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_DHCP_STATE, i)}</>\r");
                    builder.Append($"        MAC ADDRESS .... : <span style=\"color:#FF00FF;\">{CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_MAC_ADDRESS, i)}</>\r");
                    builder.Append($"        IP ADDRESS ..... : <span style=\"color:#FF00FF;\">{CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_IP_ADDRESS, i)}</>\r");
                    builder.Append($"        SUBNET MASK .... : <span style=\"color:#FF00FF;\">{CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_IP_MASK, i)}</>\r");
                    builder.Append($"        GATEWAY ........ : <span style=\"color:#FF00FF;\">{CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_ROUTER, i)}</>\r");
                    builder.Append("");

                    foreach (var dns in CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_DNS_SERVER, i).Split(','))
                        builder.Append($"        DNS SERVERS .... : <span style=\"color:#FF00FF;\">{dns}</>");

                    builder.Append("");
                }

                return CommandResponse.Success(this, builder.ToString());
            }
            catch (Exception e)
            {
                return CommandResponse.Error(this, e);
            }
        }
    }
}