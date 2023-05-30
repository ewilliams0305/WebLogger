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
        private readonly IWebLogger _logger;

        public string Command => "IPCONFIG";
        public string Description => "Displays the IP configuration";
        public string Help => "Not parameters";
        public Func<string, List<string>, string> CommandHandler => GetEthernetInformation;

        public IpConfigCommand(IWebLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// IpConfig command Handler
        /// Reports the servers IP/Mask/Gateway
        /// </summary>
        /// <param name="command">Command String</param>
        /// <param name="args">Arguments</param>
        private string GetEthernetInformation(string command, List<string> args)
        {
            var adaptors = InitialParametersClass.NumberOfEthernetInterfaces;

            var builder = new StringBuilder();

            for (short i = 0; i < adaptors; i++)
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

            return builder.ToString();
        }
    }
}