using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhihuDaily.Https;
using ZhihuDaily.Model;

namespace ZhihuDaily.ViewModel
{
    public class DetailViewModel : ViewModelBase
    {
        APIService api = new APIService();
        private StoryContent _currentStory;
        public StoryContent CurrentStory
        {
            get { return _currentStory; }
            set { SetProperty(ref _currentStory, value); }
        }


        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        public DetailViewModel()
        {

        }

        public async Task<string> GetDetailHtml(Story story)
        {
            var currentStory = await api.GetStoryContent(story.Id);
            CurrentStory = currentStory;
            //var html = currentStory.Body.Replace("<div class=\"headline\">\n\n<div class=\"img-place-holder\"></div>\n\n\n\n</div>", "");
            var html = currentStory.Body.Replace("<div class=\"img-place-holder\"></div>", "");

            var css = currentStory.Css;
            html = "<html><head><link rel=\"stylesheet\" type=\"text/css\" href=\" " + css + " \" /></head>" + html + "</html>";
            return html;
        }
    }
}
