using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZhihuDaily.Common
{
    public class TitleSpecialCharactersConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var temp = value as string;
            temp = temp.Replace("\r", "\\r");
            temp = temp.Replace("\t", "\\t");
            temp = temp.Replace("\n", "\\n");
            return temp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
