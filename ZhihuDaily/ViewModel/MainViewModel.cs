using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using ZhihuDaily.Tools;

namespace ZhihuDaily.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {

        }

        public async void ClearCache()
        {
            FileCacheHelper _fileHelper = new FileCacheHelper();
            var _cacheSize = await _fileHelper.GetCacheSize();
            if (_cacheSize > 0)
            {
                var msgDialog = new MessageDialog("Do you want to clear " + (_cacheSize / 1024 / 1024).ToString("f2") + "MB" + " cache?") { Title = "Warning" };


                msgDialog.Commands.Add(new UICommand("OK", async uiCommand =>
                {

                    await _fileHelper.DeleteCache();

                }));


                msgDialog.Commands.Add(new UICommand("No", uiCommand => { }));
                await msgDialog.ShowAsync();
            }
            else
            {
                await new MessageDialog("Cache has been cleared.").ShowAsync();
            }
        }
    }
}
