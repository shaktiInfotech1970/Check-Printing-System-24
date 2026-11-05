using CPS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CPS.Common
{
    public class Helper
    {
        public static void ClearFormData(DependencyObject obj)
        {
            switch (obj.GetType().Name)
            {
                case "TextBox":
                    (obj as TextBox).Text = "";
                    break;
                case "ComboBox":
                    (obj as ComboBox).SelectedIndex = -1;
                    break;
                case "PasswordBox":
                    (obj as PasswordBox).Password = "";
                    break;
                case "CheckBox":
                    (obj as CheckBox).IsChecked = false;
                    break;
                case "DatePicker":
                    (obj as DatePicker).SelectedDate = null;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj as DependencyObject); i++)
                ClearFormData(VisualTreeHelper.GetChild(obj, i));
        }


        public static void ToggleDGCheckbox(object sender, DataGrid dg)
        {
            var printRequest = dg.Items.OfType<PrintRequest>();
            var IsChecked = ((CheckBox)sender).IsChecked ?? false;
            foreach (var item in printRequest)
            {
                item.Request.IsSelected = IsChecked;
            }
            dg.ItemsSource = printRequest;
            dg.Items.Refresh();

        }
    }
}
