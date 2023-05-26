using Crestron.SimplSharp;
using System;
using System.Collections.Generic;

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
        public Action<string, List<string>> CommandHandler => GetEthernetInformation;

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
        private void GetEthernetInformation(string command, List<string> args)
        {
            var adaptors = InitialParametersClass.NumberOfEthernetInterfaces;

            for (short i = 0; i < adaptors; i++)
            {
                _logger.WriteLine($"VC4> Ethernet Adaptor <span style=\"color:#DDDD11;\">[{i + 1}]</>:");
                _logger.WriteLine($"        DHCP ........... : <span style=\"color:#FF00FF;\">{CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_DHCP_STATE, i)}</>");
                _logger.WriteLine($"        MAC ADDRESS .... : <span style=\"color:#FF00FF;\">{CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_MAC_ADDRESS, i)}</>");
                _logger.WriteLine($"        IP ADDRESS ..... : <span style=\"color:#FF00FF;\">{CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_IP_ADDRESS, i)}</>");
                _logger.WriteLine($"        SUBNET MASK .... : <span style=\"color:#FF00FF;\">{CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_IP_MASK, i)}</>");
                _logger.WriteLine($"        GATEWAY ........ : <span style=\"color:#FF00FF;\">{CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_CURRENT_ROUTER, i)}</>");
                _logger.WriteLine("");

                foreach (var dns in CrestronEthernetHelper.GetEthernetParameter(CrestronEthernetHelper.ETHERNET_PARAMETER_TO_GET.GET_DNS_SERVER, i).Split(','))
                    _logger.WriteLine($"        DNS SERVERS .... : <span style=\"color:#FF00FF;\">{dns}</>");

                _logger.WriteLine("");
            }
        }
    }
}