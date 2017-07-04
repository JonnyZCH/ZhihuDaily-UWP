using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ZhihuDaily.Common;

namespace ZhihuDaily.ViewModel
{
    public abstract class ViewModelBase : NotifyPropertyChanged
    {
        public void SetProperty<NotiProperty>(ref NotiProperty original, NotiProperty NewValue, [CallerMemberName]string propName = null)
        {
            //原始值与新值相同时
            if (Equals(original, NewValue))
            {
                return;
            }
            //不相等时赋值
            original = NewValue;
            if (string.IsNullOrEmpty(propName))
            {
                return;
            }
            OnPropertyChanged(propName);
        }
    }
}
