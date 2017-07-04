using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZhihuDaily.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZhihuDaily.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; set; }
        public static MainPage Current;
        //public ApplicationViewTitleBar TitleBar { get; }
        public MainPage()
        {
            ViewModel = new MainViewModel();
            Current = this;
            this.InitializeComponent();
            ApplyPropertyTitleBar();
            NavigationCacheMode = NavigationCacheMode.Required;

            //    Master/Detail模式实现
            DetailFrame.Navigated += DetailFrame_Navigated;
            MasterFrame.Navigated += MasterFrame_Navigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }

        /// <summary>
        /// 应用标题栏样式
        /// </summary>
        private void ApplyPropertyTitleBar()
        {
            var view = ApplicationView.GetForCurrentView();

            //Active
            view.TitleBar.BackgroundColor = Color.FromArgb(255, 21, 21, 21);
            view.TitleBar.ForegroundColor = Colors.White;
            //InActive
            view.TitleBar.InactiveBackgroundColor = Color.FromArgb(255, 21, 21, 21);
            view.TitleBar.InactiveForegroundColor = Colors.White;
            //TitleButton
            view.TitleBar.ButtonBackgroundColor = Color.FromArgb(255, 21, 21, 21);
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 80, 80, 80);
            view.TitleBar.ButtonHoverForegroundColor = Colors.White;
            view.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 70, 70, 70);
            view.TitleBar.ButtonPressedForegroundColor = Colors.White;
            view.TitleBar.ButtonInactiveBackgroundColor = Color.FromArgb(255, 21, 21, 21);
            view.TitleBar.ButtonInactiveForegroundColor = Colors.White;


        }

        /// <summary>
        /// 后退事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (DetailFrame.CanGoBack)
            {
                DetailFrame.GoBack();
                e.Handled = true;
            }
            else if (MasterFrame.CanGoBack)
            {
                MasterFrame.GoBack();
                e.Handled = true;
            }

        }

        #region Master/Detail模式更新UI
        private void MasterFrame_Navigated(object sender, NavigationEventArgs e)
        {
            UpdateUI();
        }

        private void DetailFrame_Navigated(object sender, NavigationEventArgs e)
        {
            while (DetailFrame.BackStack.Count > 1)
            {
                DetailFrame.BackStack.RemoveAt(1);
            }
            UpdateUI();
        }
        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MasterFrame.Navigate(typeof(IndexPage));
            DetailFrame.Navigate(typeof(DetailBackgroundPage));
        }

        /// <summary>
        /// 汉堡菜单按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HamburgerBtn_OnClick(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            if (MySplitView.IsPaneOpen == true)
            {
                MySplitView.Content.Opacity = 0.6;
            }
        }

        /// <summary>
        /// AdaptiveStates改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdaptiveStates_OnCurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// Master/Detail模式更新UI
        /// </summary>
        private void UpdateUI()
        {
            if (AdaptiveStates.CurrentState.Name == "MobileState")
            {
                DetailFrame.Visibility = DetailFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                DetailFrame.Visibility = Visibility.Visible;
            }
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = DetailFrame.CanGoBack || MasterFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        /// <summary>
        /// 汉堡菜单关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void MySplitView_OnPaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            MySplitView.Content.Opacity = 1;
        }

        /// <summary>
        /// 主页按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Home_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MasterFrame.CanGoBack)
            {
                MasterFrame.GoBack();
                MySplitView.IsPaneOpen = false;
            }
            else if (DetailFrame.CanGoBack)
            {
                DetailFrame.GoBack();
                MySplitView.IsPaneOpen = false;
            }
            else
            {
                MySplitView.IsPaneOpen = false;
            }
        }

        /// <summary>
        /// 清理缓存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearCache_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.ClearCache();
        }
    }
}
