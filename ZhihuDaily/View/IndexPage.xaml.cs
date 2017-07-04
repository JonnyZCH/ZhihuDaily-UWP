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
    public sealed partial class IndexPage : Page
    {
        public IndexViewModel ViewModel { get; set; }
        public IndexPage()
        {
            ViewModel = new IndexViewModel();
            this.InitializeComponent();
        }

        private void FlipViewCenter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.IsTimerStart)
            {
                //每三秒切换一次顶部banner内容
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 3);
                timer.Tick += (p, args) =>
                {
                    ViewModel.CenterImageIndex = ViewModel.CenterImageIndex < ViewModel.TopStory.Count - 1
                        ? ++ViewModel.CenterImageIndex
                        : 0;
                };
                timer.Start();
                ViewModel.IsTimerStart = false;
            }


            ViewModel.UpdateBanner();
            #region 添加banner索引
            //while (!ViewModel.IsBannerIndexLoaded)
            //{
            //    for (int i = 0; i < ViewModel.TopStory.Count; i++)
            //    {
            //        Rectangle rec = new Rectangle();
            //        rec.Width = 20;
            //        rec.Height = 5;
            //        rec.Name = "Rec" + i;
            //        rec.Fill = new SolidColorBrush(Colors.White);
            //        rec.Opacity = 0.5;
            //        rec.Margin = new Thickness(20, 0, 0, 0);
            //        BannerIndex.Children.Add(rec);
            //    }
            //    ViewModel.IsBannerIndexLoaded = true;
            //}

            //if (ViewModel.CanUpdateBannerIndex)
            //{
            //    BannerIndex.Children[ViewModel.CenterImageIndex].Opacity = 1;
            //    BannerIndex.Children[ViewModel.LeftImageIndex].Opacity = 0.5;
            //    BannerIndex.Children[ViewModel.RightImageIndex].Opacity = 0.5;
            //} 
            #endregion

        }

        /// <summary>
        /// 日报列表点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DailyList_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var story = e.ClickedItem as Story;
            MainPage.Current.DetailFrame.Navigate(typeof(DetailPage), story);
        }

        private void FlipViewCenter_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var topStory = FlipViewCenter.SelectedItem as Story;
            MainPage.Current.DetailFrame.Navigate(typeof(DetailPage), topStory);
        }
    }
}