using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZhihuDaily.Model
{
    [DataContract]
    public class Latest
    {
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public ObservableCollection<Story> Stories { get; set; }
        [DataMember]
        public ObservableCollection<Story> TopStories { get; set; }
    }
}
