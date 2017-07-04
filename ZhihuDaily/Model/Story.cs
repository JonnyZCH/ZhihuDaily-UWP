using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZhihuDaily.Model
{
    [DataContract]
    public class Story
    {
        [DataMember]
        public string Image { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string GaPrefix { get; set; }
        [DataMember]
        public string Title { get; set; }

        //private bool _isFavorite;
        //[DataMember]
        //public bool IsFavorite
        //{
        //    get { return _isFavorite; }
        //    set
        //    {
        //        _isFavorite = value;
        //        SetProperty(ref _isFavorite, value);
        //    }
        //}

        public Story()
        {
            //ClickCommand = new Command(async p =>
            //{
            //    await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //    {
            //        //var detail = Window.Current.Content;

            //    });
            //});
        }

        //public Command ClickCommand { get; set; }
    }
}
