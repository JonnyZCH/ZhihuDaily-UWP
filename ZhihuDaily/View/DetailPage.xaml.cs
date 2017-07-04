using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZhihuDaily.Model;
using ZhihuDaily.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZhihuDaily.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        private bool _domLoaded = false;
        public DetailViewModel ViewModel { get; set; }
        public DetailPage()
        {
            ViewModel = new DetailViewModel();
            this.InitializeComponent();

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            Banner.Visibility = Visibility.Collapsed;
            ViewModel.IsLoading = true;
            var story = e.Parameter as Story;
            var html = await ViewModel.GetDetailHtml(story);
            MyWebView.NavigateToString(html);
            Banner.Visibility = Visibility.Visible;
            ViewModel.IsLoading = false;
        }

        /// <summary>
        /// 动态加载js隐藏WebView自带的滚动条
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void MyWebView_OnDOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            // 动态加载 Css/Js
            var js = @"var myCss = document.createElement(""link"");
                     myCss.rel = ""stylesheet"";
                     myCss.type = ""text/css"";
                     myCss.href = ""ms-appx-web:///HTML/CSS/myCss.css"";
                     document.body.appendChild(myCss);
                     document.body.style.overflow = 'hidden';
                     window.external.notify(JSON.stringify(document.body.scrollHeight));";

            await sender.InvokeScriptAsync("eval", new[] { js });
            _domLoaded = true;
        }

        /// <summary>
        /// 设置WebView的高度为所加载页面的高度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyWebView_OnScriptNotify(object sender, NotifyEventArgs e)
        {
            var webview = sender as WebView;
            webview.Height = Convert.ToDouble(e.Value);
            webview.Width = Banner.Width;
        }

        /// <summary>
        /// WebView重新加载新页面时动态加载js
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MyWebView_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_domLoaded)
            {
                var js = @"window.external.notify(JSON.stringify(document.body.scrollHeight));";

                var webview = sender as WebView;
                //webview.Refresh();
                await webview.InvokeScriptAsync("eval", new[] { js });
            }
            //_domLoaded = false;
        }
    }
}
