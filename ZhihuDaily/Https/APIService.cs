using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Xaml.Media.Imaging;
using ZhihuDaily.Model;
using ZhihuDaily.Tools;

namespace ZhihuDaily.Https
{
    public class APIService : APIBaseService
    {
        /// <summary>
        /// 应用文件夹路径
        /// </summary>
        private string _localPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path;

        /// <summary>
        /// 获取最新日报
        /// </summary>
        /// <param name="executeAtRuntime"></param>
        /// <returns></returns>
        public async Task<Latest> GetLatest(bool executeAtRuntime = false)
        {
            try
            {
                //没有网络则读取缓存
                if (NetWorkManager.GetInternetStatus() == string.Empty)
                {
                    Latest Cache = await FileCacheHelper.Current.ReadObjectAsync<Latest>("latest.json");
                    return Cache;
                }
                else
                {
                    JsonObject json = await GetJson(Latest);
                    if (json != null)
                    {
                        ObservableCollection<Story> storiesList = new ObservableCollection<Story>();
                        ObservableCollection<Story> topStoriesList = new ObservableCollection<Story>();
                        Story temp;
                        var stories = json["stories"];
                        WriteableBitmap wb = null;
                        string imageExt = "jpg";
                        string[] sitem = null;
                        if (stories != null)
                        {
                            JsonArray jsonArray = stories.GetArray();
                            JsonObject jsonObject;
                            string image;

                            foreach (var item in jsonArray)
                            {
                                jsonObject = item.GetObject();
                                image = jsonObject["images"].GetArray()[0].GetString();
                                temp = new Story { Date = json["date"].GetString(), Id = jsonObject["id"].GetNumber().ToString(), Image = image, Title = jsonObject["title"].ToString(), GaPrefix = jsonObject["ga_prefix"].ToString(), Type = jsonObject["type"].GetNumber().ToString() };
                                //没有缓存
                                if (!await FileCacheHelper.Current.IsCacheExist(temp.Id + "_story_image." + imageExt))
                                {
                                    //下载图片
                                    wb = await GetImage(temp.Image);
                                    if (!temp.Image.Equals(""))
                                    {
                                        sitem = temp.Image.Split('.');
                                        imageExt = sitem[sitem.Count() - 1];
                                    }
                                    await
                                        FileCacheHelper.Current.WriteImageAsync(wb, temp.Id + "_story_image." + imageExt);
                                }
                                if (!temp.Image.Equals(""))
                                {
                                    temp.Image = _localPath + "\\image_cache\\" + temp.Id + "_story_image." + imageExt;
                                }
                                storiesList.Add(temp);
                            }
                        }

                        var topStories = json["top_stories"];
                        if (topStories != null)
                        {
                            JsonArray jsonArray = topStories.GetArray();
                            JsonObject jsonObject;
                            string image;
                            foreach (var item in jsonArray)
                            {
                                jsonObject = item.GetObject();
                                temp = new Story { Date = json["date"].GetString(), GaPrefix = jsonObject["ga_prefix"].ToString(), Id = jsonObject["id"].GetNumber().ToString(), Image = jsonObject["image"].GetString(), Title = jsonObject["title"].GetString(), Type = jsonObject["type"].GetNumber().ToString() };

                                if (!await FileCacheHelper.Current.IsCacheExist(temp.Id + "_story_top_image." + imageExt))
                                {
                                    if (!temp.Image.Equals(""))
                                    {
                                        sitem = temp.Image.Split('.');
                                        imageExt = sitem[sitem.Count() - 1];
                                    }
                                    if (!executeAtRuntime)
                                    {
                                        wb = await GetImage(temp.Image);
                                        await
                                            FileCacheHelper.Current.WriteImageAsync(wb,
                                                temp.Id + "_story_top_image." + imageExt);
                                    }
                                    else
                                    {
                                        //runtime component 重下载图片
                                        await GetImageInRuntimeComponent(temp.Image, temp.Id + "_story_top_image." + imageExt);
                                    }
                                }
                                if (!temp.Image.Equals(""))
                                {
                                    temp.Image = _localPath + "\\image_cache\\" + temp.Id + "_story_top_image." +
                                                 imageExt;
                                }
                                topStoriesList.Add(temp);
                            }
                        }
                        Latest lastest = new Latest { Date = json["date"].ToString(), TopStories = topStoriesList, Stories = storiesList };
                        await FileCacheHelper.Current.WriteObjectAsync<Latest>(lastest, "latest.json");
                        return lastest;
                    }
                    else
                    {
                        //return null;
                        Latest cache = await FileCacheHelper.Current.ReadObjectAsync<Latest>("latest.json");
                        return cache;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取日报内容
        /// </summary>
        /// <param name="id"></param>
        /// <param name="executeAtRuntime"></param>
        /// <returns></returns>
        public async Task<StoryContent> GetStoryContent(string id, bool executeAtRuntime = false)
        {
            try
            {
                JsonObject json = await GetJson(Story + id);
                WriteableBitmap wb = null;
                string imageExt = "jpg";
                string[] sitem = null;
                if (json != null)
                {
                    var temp = new StoryContent();
                    temp.Body = json["body"].GetString();
                    temp.ImageSource = json["image_source"].GetString();
                    temp.Title = json["title"].GetString();
                    temp.Image = json["image"].GetString();
                    temp.ShareUrl = json["share_url"].GetString();
                    temp.GaPrefix = json["ga_prefix"].GetString();
                    temp.Images = json["images"].GetArray()[0].GetString();
                    temp.Type = json["type"].GetNumber().ToString();
                    temp.Id = json["id"].GetNumber().ToString();
                    temp.Css = json["css"].GetArray()[0].GetString();

                    //下载图片
                    if (!await FileCacheHelper.Current.IsCacheExist(temp.Id + "_story_content_image." + imageExt))
                    {
                        if (!temp.Image.Equals(""))
                        {
                            sitem = temp.Image.Split('.');
                            imageExt = sitem[sitem.Count() - 1];
                        }
                        if (!executeAtRuntime)
                        {
                            wb = await GetImage(temp.Image);
                            await FileCacheHelper.Current.WriteImageAsync(wb, temp.Id + "_story_content_image." + imageExt);
                        }
                        else
                        {
                            //重下载图片
                            await GetImageInRuntimeComponent(temp.Image, temp.Id + "_story_content_image." + imageExt);
                        }
                    }
                    if (!temp.Image.Equals(""))
                    {
                        temp.Image = _localPath + "\\image_cache\\" + temp.Id + "_story_content_image." + imageExt;
                    }

                    await FileCacheHelper.Current.WriteObjectAsync<StoryContent>(temp, id + "_story_content_data.json");
                    return temp;

                }
                else
                {
                    StoryContent cache = await FileCacheHelper.Current.ReadObjectAsync<StoryContent>(id + "_story_content_data.json");
                    return cache;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
