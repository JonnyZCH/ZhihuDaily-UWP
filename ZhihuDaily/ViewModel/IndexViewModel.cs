using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhihuDaily.Https;
using ZhihuDaily.Model;

namespace ZhihuDaily.ViewModel
{
    public class IndexViewModel : ViewModelBase
    {
        APIService api = new APIService();

        private int _leftImageIndex;
        public int LeftImageIndex
        {
            get { return _leftImageIndex; }
            set
            {
                SetProperty(ref _leftImageIndex, value);
            }
        }

        private int _centerImageIndex;
        public int CenterImageIndex
        {
            get { return _centerImageIndex; }
            set
            {
                SetProperty(ref _centerImageIndex, value);
            }
        }

        private int _rightImageIndex;
        public int RightImageIndex
        {
            get { return _rightImageIndex; }
            set
            {
                SetProperty(ref _rightImageIndex, value);
            }
        }

        private bool _isBannerVisible;
        public bool IsBannerVisible
        {
            get { return _isBannerVisible; }
            set
            {
                SetProperty(ref _isBannerVisible, value);
            }
        }


        private bool _isTimerStart;
        public bool IsTimerStart
        {
            get { return _isTimerStart; }
            set
            {
                SetProperty(ref _isTimerStart, value);
            }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                SetProperty(ref _title, value);
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private ObservableCollection<Story> _topStory;
        public ObservableCollection<Story> TopStory
        {
            get { return _topStory; }
            set { SetProperty(ref _topStory, value); }
        }

        private ObservableCollection<Story> _allStory;
        public ObservableCollection<Story> AllStory
        {
            get { return _allStory; }
            set { SetProperty(ref _allStory, value); }
        }

        public IndexViewModel()
        {
            Update();

        }

        public async void Update()
        {
            IsTimerStart = false;
            IsBannerVisible = false;
            IsLoading = true;
            var latest = await api.GetLatest();
            AllStory = latest.Stories;
            TopStory = latest.TopStories;
            IsTimerStart = true;
            IsBannerVisible = true;
            LeftImageIndex = 0;
            RightImageIndex = 2;
            CenterImageIndex = 1;
            IsLoading = false;
        }

        public void UpdateBanner()
        {
            try
            {
                if (CenterImageIndex == 0)
                {
                    LeftImageIndex = TopStory.Count - 1;
                    RightImageIndex = CenterImageIndex + 1;
                }
                else if (CenterImageIndex == TopStory.Count - 1)
                {
                    LeftImageIndex = CenterImageIndex - 1;
                    RightImageIndex = 0;
                }
                else
                {
                    LeftImageIndex = CenterImageIndex - 1;
                    RightImageIndex = CenterImageIndex + 1;
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
