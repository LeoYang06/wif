/**************************************************************************
*      File Name：NetHelper.cs
*    Description：NetHelper.cs class description...
*      Copyright：Copyright © 2020 LeoYang-Chuese. All rights reserved.
*        Creator：Leo Yang
*    Create Time：2020/12/15
*Project Address：https://github.com/LeoYang-Chuese/wif
**************************************************************************/


using System;
using System.Net;
using System.Net.Sockets;

namespace Frontier.Wif.Utilities.Helpers
{
    /// <summary>
    /// Defines the <see cref="NetHelper" />
    /// </summary>
    public class NetHelper
    {
        #region Methods

        /// <summary>
        /// 获得广播地址。
        /// </summary>
        /// <param name="ipAddress">IP地址。</param>
        /// <param name="subnetMask">子网掩码。</param>
        /// <returns>广播地址。</returns>
        public static string GetBroadcast(string ipAddress, string subnetMask)
        {
            var ip = IPAddress.Parse(ipAddress).GetAddressBytes();
            var sub = IPAddress.Parse(subnetMask).GetAddressBytes();

            // 广播地址=子网按位求反 再 或IP地址 
            for (var i = 0; i < ip.Length; i++) ip[i] = (byte) (~sub[i] | ip[i]);
            return new IPAddress(ip).ToString();
        }

        /// <summary>
        /// 获取本地IPv4地址。
        /// </summary>
        /// <returns>IPv4地址。</returns>
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