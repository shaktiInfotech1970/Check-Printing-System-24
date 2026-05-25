using CPS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CPS.Views.Security
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : UserControl
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var oLoggedInUser = (UserMasterDTO)Application.Current.Windows[0].Tag;
            if (oLoggedInUser.Password == txtOldPassword.Password)
            {
                if (txtNewPassword.Password == txtConfirmNewPassword.Password && !string.IsNullOrWhiteSpace(txtNewPassword.Password) && txtNewPassword.Password.Length >= 3)
                {
                    oLoggedInUser.Password = txtNewPassword.Password;
                    using(var context = new CPSDbContext())
                    {
                        var repository = new PersistenceBase<UserMasterDTO>(context);
                         var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                         if (repository.SaveOrUpdate(oLoggedInUser, errors))
                         {
                             context.SaveChanges();
                             MessageBox.Show("Password changed successfully!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                             CPS.Common.Helper.ClearFormData(this);
                         }
                         else
                         {
                             MessageBox.Show(string.Join(Environment.NewLine, errors.Select(o => o.ErrorMessage)), "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                         }
                    }
                }
                else
                {
                    MessageBox.Show("New Password and Confirm New Password must be same and at-least 3 characters!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Incorrect old password!", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CPS.Common.Helper.ClearFormData(this);
        }

    }
}
