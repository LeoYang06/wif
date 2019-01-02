using System;
using System.Management;
using System.Net;
using System.Net.Sockets;

namespace Frontier.Wif.Utils.Helpers
{
    /// <summary>
    /// Defines the <see cref="NetHelper" />
    /// </summary>
    public class NetHelper
    {
        #region Methods

        /// <summary>
        /// 获得广播地址
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="subnetMask">子网掩码</param>
        /// <returns>广播地址</returns>
        public static string GetBroadcast(string ipAddress, string subnetMask)
        {
            var ip = IPAddress.Parse(ipAddress).GetAddressBytes();
            var sub = IPAddress.Parse(subnetMask).GetAddressBytes();

            // 广播地址=子网按位求反 再 或IP地址 
            for (var i = 0; i < ip.Length; i++) ip[i] = (byte) (~sub[i] | ip[i]);
            return new IPAddress(ip).ToString();
        }

        /// <summary>
        /// 获取子网掩码。
        /// </summary>
        /// <returns></returns>
        public static string GetBroadcastAddress()
        {
            var mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            var nics = mc.GetInstances();
            foreach (var o in nics)
            {
                var nic = (ManagementObject) o;
                if (Convert.ToBoolean(nic["ipEnabled"]))
                {
                    var mac = nic["MacAddress"].ToString(); //Mac地址
                    var ip = (nic["IPAddress"] as string[])?[0]; //IP地址
                    var ipsubnet = (nic["IPSubnet"] as string[])?[0]; //子网掩码
                    var ipgateway = (nic["DefaultIPGateway"] as string[])?[0]; //默认网关

                    var broadcastAddress = GetBroadcast(ip, ipsubnet);
                    return broadcastAddress;
                }
            }

            return "";
        }

        /// <summary>
        /// 获取本地IPv4地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            var localIP = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily != AddressFamily.InterNetwork) continue;
                localIP = ip.ToString();
                break;
            }

            return localIP;
        }

        #endregion
    }
}