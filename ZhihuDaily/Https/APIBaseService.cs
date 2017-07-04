using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ZhihuDaily.Https
{
    public class APIBaseService
    {

        #region API-URl

        /// <summary>
        /// 启动图像
        /// {0}，图像分辨率
        /// 320*432     480*728     720*1184      1080*1776
        /// </summary>
        public static string StartImage = "http://news-at.zhihu.com/api/4/start-image/{0}";

        /// <summary>
        /// 最新日报信息（包含头条）
        /// Date为当日日期，Stories为所有当日文章信息，Top_Stories为头条文章
        /// </summary>
        public static string Latest = "http://news-at.zhihu.com/api/4/news/latest";

        /// <summary>
        /// 文章内容
        /// {0}为文章编号
        /// </summary>
        public static string Story = "http://news-at.zhihu.com/api/4/story/";

        /// <summary>
        /// 首页分页文章（按日期）
        /// {0}为日期,ex:20160918
        /// </summary>
        public static string BeforeStories = "http://news-at.zhihu.com/api/4/stories/before/{0}";

        /// <summary>
        /// 主题列表
        /// </summary>
        public static string Themes = "http://news-at.zhihu.com/api/4/themes";

        /// <summary>
        /// 主题文章
        /// {0}为主题id
        /// </summary>
        public static string ThemeStories = "http://news-at.zhihu.com/api/4/theme/{0}";

        /// <summary>
        /// 分页获取主题文章
        /// {0}为主题编号,{1}为日期
        /// </summary>
        public static string BeforeThemeStories = "http://news-at.zhihu.com/api/4/theme/{0}/before/{1}";

        /// <summary>
        /// 文章额外信息（评论数、推荐数等）
        /// 0 文章id
        /// </summary>
        public static string StoryExtra = "http://news-at.zhihu.com/api/4/story-extra/{0}";

        /// <summary>
        /// 短评论
        /// 0 文章id
        /// </summary>
        public static string ShortComments = "http://news-at.zhihu.com/api/4/story/{0}/short-comments";

        #endregion

        /// <summary>
        /// 获取Json数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<JsonObject> GetJson(string url)
        {
            try
            {
                var json = await HttpService.GetRequest(url);
                if (json != null)
                {
                    return JsonObject.Parse(json);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task<WriteableBitmap> GetImage(string url)
        {
            try
            {
                IBuffer buffer = await HttpService.GetRequestBytes(url);
                if (buffer != null)
                {
                    BitmapImage image = new BitmapImage();
                    WriteableBitmap wbImage = null;
                    Stream streamToWrite;
                    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                    {
                        streamToWrite = stream.AsStreamForWrite();
                        await streamToWrite.WriteAsync(buffer.ToArray(), 0, (int)buffer.Length);
                        await streamToWrite.FlushAsync();
                        stream.Seek(0);

                        await image.SetSourceAsync(stream);
                        wbImage = new WriteableBitmap(image.PixelWidth, image.PixelHeight);
                        stream.Seek(0);
                        await wbImage.SetSourceAsync(stream);

                        return wbImage;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        protected async Task GetImageInRuntimeComponent(string url, string name)
        {
            try
            {
                var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("image_cache", CreationCollisionOption.OpenIfExists);

                var file = await StorageFile.CreateStreamedFileFromUriAsync(name, new Uri(url), RandomAccessStreamReference.CreateFromUri(new Uri(url)));
                file = await file.CopyAsync(folder);
            }
            catch
            {

            }
        }
    }
}
