using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhihuDaily.Common
{
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public void OnPropertyChanged(string propertyName)
        {
            //数据源发生变化  通知所有绑定该属性的对象进行数据同步
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
