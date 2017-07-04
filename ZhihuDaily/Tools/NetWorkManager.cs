using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace ZhihuDaily.Tools
{
    public class NetWorkManager
    {
        /// <summary>
        /// 获取当前网络环境
        /// </summary>
        /// <returns>网络环境信息</returns>
        public static string GetInternetStatus()
        {
            const string IIG = "2G";
            const string IIIG = "3G";
            const string IVG = "4G";
            const string Wifi = "WIFI";
            const string Lan = "LAN";

            string InternetType = null;

            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile.IsWwanConnectionProfile)
            {
                if (profile.WwanConnectionProfileDetails == null)
                {
                    InternetType = string.Empty;
                }
                WwanDataClass connectionClass = profile.WwanConnectionProfileDetails.GetCurrentDataClass();

                switch (connectionClass)
                {
                    //2G网络
                    case WwanDataClass.Edge:
                    case WwanDataClass.Gprs:
                        InternetType = IIG;
                        break;
                    //3G网络
                    case WwanDataClass.Cdma1xEvdo:
                    case WwanDataClass.Cdma1xEvdoRevA:
                    case WwanDataClass.Cdma1xEvdoRevB:
                    case WwanDataClass.Cdma1xEvdv:
                    case WwanDataClass.Cdma1xRtt:
                    case WwanDataClass.Cdma3xRtt:
                    case WwanDataClass.CdmaUmb:
                    case WwanDataClass.Umts:
                    case WwanDataClass.Hsdpa:
                    case WwanDataClass.Hsupa:
                        InternetType = IIIG;
                        break;
                    //4G
                    case WwanDataClass.LteAdvanced:
                        InternetType = IVG;
                        break;
                    //无网
                    case WwanDataClass.None:
                        InternetType = string.Empty;
                        break;
                    default:
                        InternetType = string.Empty;
                        break;
                }
            }
            else if (profile.IsWlanConnectionProfile)
            {
                InternetType = Wifi;
            }
            else
            {
                //不是Wifi也不是蜂窝数据判断为Lan
                InternetType = Lan;
            }
            return InternetType;
        }

    }
}
