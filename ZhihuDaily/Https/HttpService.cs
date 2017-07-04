using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Web.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Windows.Storage.Streams;

namespace ZhihuDaily.Https
{
    public class HttpService
    {
        /// <summary>
        /// 发送GET请求，返回从服务器接收的数据(string)
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <returns></returns>
        public async static Task<string> GetRequest(string url)
        {
            try
            {
                var http = new HttpClient();
                Uri uri = new Uri(url);
                var response = await http.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// 发送GET请求，返回从服务器接收的数据（byte[]）
        /// </summary>
        /// <param name="url">服务器地址</param>
        /// <returns></returns>
        public async static Task<IBuffer> GetRequestBytes(string url)
        {
            try
            {
                var http = new HttpClient();
                Uri uri = new Uri(url);
                var response = await http.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsBufferAsync();

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
