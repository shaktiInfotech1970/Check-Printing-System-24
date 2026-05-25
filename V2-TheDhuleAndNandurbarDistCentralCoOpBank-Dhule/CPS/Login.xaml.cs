using CPS.Business;
using CPS.Common;
using System;
using System.Linq;
using System.Windows;

namespace CPS
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            using (var context = new CPSDbContext())
            {
                var userRepository = new PersistenceBase<UserMasterDTO>(context);
                {
                    userRepository.FindBy(o => o.Id == 1).FirstOrDefault();
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserId.Text))
            {
                MessageBox.Show("User Id is required.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Password is required.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                using (var context = new CPSDbContext())
                {
                    var userRepository = new PersistenceBase<UserMasterDTO>(context);
                    {
                        var user = userRepository.FindBy(f => f.UserId == txtUserId.Text).FirstOrDefault();
                        if (user != null && String.Equals(txtPassword.Password, user.Password, StringComparison.Ordinal))
                        {
                            if (user.IsLocked == false)
                            {
                                var fingerPrint = FingerPrint.Value();
                                if (String.Equals(user.Name, "superadmin", StringComparison.Ordinal) || GetActivationCode() == fingerPrint)
                                {
                                    if (GetActivationCode() != fingerPrint)
                                    {
                                        Register(fingerPrint);
                                    }

                                    var repository = new PersistenceBase<PermissionDTO>(context);
                                    UserPermission LoggedInUserPermission = new UserPermission();
                                    LoggedInUserPermission.Permissions = repository.GetAll().Where(o => o.UserId == user.Id).ToList();
                                    mainWindow window = new mainWindow(LoggedInUserPermission);
                                    window.Title = string.Format("Cheque Printing Software            LoginDateTime: {0}            UserId: {1}            User Name: {2}            Support:- Hemant Pawar: {3}            Technical Support:- Dhwanil Pawar: {4}",
                                                                   System.DateTime.Now,
                                                                   user.UserId,
                                                                   user.Name,
                                                                   "+91-9825605331",
                                                                   "+91-8141620099");
                                    window.SubTitle.Text = "Technical Support:- Dhwanil Pawar: +91-8141620099, Amit Bane: +91-8200403694   Support:- Hemant Pawar: +91-9825605331";

                                    window.Show();
                                    this.Close();
                                    window.Tag = user;
                                }
                                else
                                {
                                    MessageBox.Show("This product is not register to use on this machine. Please contact to admin.", "Unregister Product", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("User is locked, Please contact to admin.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid User Id or Password.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
        }

        private string GetActivationCode()
        {
            var activationCode = string.Empty;
            try
            {
                activationCode = System.IO.File.ReadAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "activation.key"));
            }
            catch
            {
            }

            return activationCode;
        }

        private void Register(string fingerPrint)
        {
            try
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "activation.key"), fingerPrint);
            }
            catch
            {
            }
        }
    }
}
