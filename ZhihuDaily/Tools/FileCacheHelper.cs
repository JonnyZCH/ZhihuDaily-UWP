using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ZhihuDaily.Tools
{
    /// <summary>
    /// 缓存文件夹helper
    /// </summary>
    public class FileCacheHelper
    {
        private static FileCacheHelper _current;

        public static FileCacheHelper Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new FileCacheHelper();
                }
                return _current;
            }
        }

        private Windows.Storage.StorageFolder _localFolder;

        public FileCacheHelper()
        {
            //应用本地文件夹
            _localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Initialize();

        }

        /// <summary>
        /// 初始化缓存文件夹
        /// </summary>
        private async void Initialize()
        {
            await _localFolder.CreateFolderAsync("image_cache", CreationCollisionOption.OpenIfExists);
            await _localFolder.CreateFolderAsync("data_cache", CreationCollisionOption.OpenIfExists);
        }

        /// <summary>
        /// 写入数据缓存
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task WriteObjectAsync<T>(T obj, string filename)
        {
            try
            {
                var folder = await _localFolder.CreateFolderAsync("data_cache", CreationCollisionOption.OpenIfExists);
                using (var data = await folder.OpenStreamForWriteAsync(filename, CreationCollisionOption.ReplaceExisting))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    serializer.WriteObject(data, obj);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 读取数据缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<T> ReadObjectAsync<T>(string filename) where T : class
        {
            try
            {
                var folder = await _localFolder.CreateFolderAsync("data_cache", CreationCollisionOption.OpenIfExists);
                using (var data = await folder.OpenStreamForReadAsync(filename))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                    return serializer.ReadObject(data) as T;
                }
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 图片缓存
        /// </summary>
        /// <param name="image"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task WriteImageAsync(WriteableBitmap image, string filename)
        {
            try
            {
                if (image == null)
                {
                    return;
                }
                Guid bitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
                if (filename.EndsWith("jpg"))
                {
                    bitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
                }
                else if (filename.EndsWith("png"))
                {
                    bitmapEncoderGuid = BitmapEncoder.PngEncoderId;
                }
                else if (filename.EndsWith("bmp"))
                {
                    bitmapEncoderGuid = BitmapEncoder.BmpEncoderId;
                }
                else if (filename.EndsWith("gif"))
                {
                    bitmapEncoderGuid = BitmapEncoder.GifEncoderId;
                }

                var folder = await _localFolder.CreateFolderAsync("image_cache", CreationCollisionOption.OpenIfExists);
                var file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(bitmapEncoderGuid, stream);
                    Stream pix = image.PixelBuffer.AsStream();
                    byte[] pixels = new byte[pix.Length];
                    await pix.ReadAsync(pixels, 0, pixels.Length);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)image.PixelWidth,
                        (uint)image.PixelHeight, 90, 90, pixels);
                    await encoder.FlushAsync();
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 清理缓存
        /// </summary>
        /// <returns></returns>
        public async Task DeleteCache()
        {
            try
            {
                StorageFolder dataFolder = await _localFolder.GetFolderAsync("data_cache");
                StorageFolder folder = await _localFolder.GetFolderAsync("image_cache");
                if (dataFolder != null || folder != null)
                {
                    IReadOnlyCollection<StorageFile> files =
                        await folder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);
                    List<IAsyncAction> list = new List<IAsyncAction>();
                    foreach (var item in files)
                    {
                        list.Add(item.DeleteAsync(StorageDeleteOption.PermanentDelete));
                    }


                    IReadOnlyCollection<StorageFile> datas = await dataFolder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);
                    List<IAsyncAction> list2 = new List<IAsyncAction>();
                    foreach (var item in datas)
                    {
                        list2.Add(item.DeleteAsync(StorageDeleteOption.PermanentDelete));
                    }
                    //List<Task> list2 = new List<Task>();
                    //list.ForEach((p) => list2.Add(p.AsTask()));
                    //await Task.Run(() => { Task.WaitAll(list2.ToArray()); });
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 获取当前缓存大小
        /// </summary>
        /// <returns></returns>
        public async Task<double> GetCacheSize()
        {
            try
            {
                StorageFolder dataFolder = await _localFolder.GetFolderAsync("data_cache");
                var dataFiles = await dataFolder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);
                StorageFolder imageFolder = await _localFolder.GetFolderAsync("image_cache");
                var imageFiles = await imageFolder.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.DefaultQuery);

                double size = 0;
                BasicProperties bp;
                foreach (var item in dataFiles)
                {
                    bp = await item.GetBasicPropertiesAsync();
                    size += bp.Size;
                }

                foreach (var item in imageFiles)
                {
                    bp = await item.GetBasicPropertiesAsync();
                    size += bp.Size;
                }
                return size;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 判断是否存在缓存文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task<bool> IsCacheExist(string filename)
        {
            try
            {
                var folder = await _localFolder.TryGetItemAsync("image_cache");
                if (folder != null)
                {
                    var subFodler = await (folder as StorageFolder).TryGetItemAsync(filename);
                    if (subFodler == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
