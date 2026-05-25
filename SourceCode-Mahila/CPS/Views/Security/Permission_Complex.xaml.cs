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
    /// Interaction logic for Permission.xaml
    /// </summary>
    public partial class Permission : UserControl
    {
        public Permission()
        {
            InitializeComponent();
            btnSavePermission.IsEnabled = false;
            cbUser.ItemsSource = UserMasterDTO.GetLookups();
            cbUser.DisplayMemberPath = "Value";
            cbUser.SelectedValuePath = "Key";
        }

        private void BindDataGrid()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<PermissionDTO>(context);
                var userId = Convert.ToInt32(cbUser.SelectedValue);
                var response = repository.GetAll().Where(o => o.UserId == userId).ToList();
                if (response != null)
                {
                    var oPermissionList = new List<PermissionList>();
                    foreach (var permission in response)
                    {
                        var oPermission = new PermissionList();
                        oPermission.Id = permission.Id;
                        oPermission.Page = permission.Page;
                        switch (permission.Permission)
                        {
                            case (int)Common.Permission.Read:
                                oPermission.Read = true;
                                break;
                            case (int)Common.Permission.Write:
                                oPermission.Write = true;
                                break;
                            case (int)Common.Permission.Print:
                                oPermission.Print = true;
                                break;
                            case (int)Common.Permission.All:
                                oPermission.Read = true;
                                oPermission.Write = true;
                                oPermission.Print = true;
                                break;
                            case 3:
                                oPermission.Read = true;
                                oPermission.Write = true;
                                break;
                            case 5:
                                oPermission.Read = true;
                                oPermission.Print = true;
                                break;
                            case 6:
                                oPermission.Write = true;
                                oPermission.Print = true;
                                break;
                            default:
                                break;
                        }
                        oPermissionList.Add(oPermission);
                    }
                    dgUserPermission.ItemsSource = oPermissionList;
                }

            }
        }

        void OnChecked(object sender, RoutedEventArgs e)
        {
        }

        private void btnSavePermission_Click(object sender, RoutedEventArgs e)
        {
            var olPermission = (List<PermissionList>)dgUserPermission.ItemsSource;
            var olPermissionDTO = new List<PermissionDTO>();



            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<PermissionDTO>(context);
                var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

                foreach (var obj in olPermission)
                {
                    var oPermission = new PermissionDTO();
                    oPermission.Id = obj.Id;
                    oPermission.UserId = Convert.ToInt32(cbUser.SelectedValue);
                    oPermission.Page = obj.Page;
                    oPermission.Permission = (obj.Read ? 1 : 0) + (obj.Write ? 1 : 0) + (obj.Print ? 1 : 0);
                    if (!repository.SaveOrUpdate(oPermission, errors))
                    {
                        MessageBox.Show(string.Join(Environment.NewLine, errors.Select(o => o.ErrorMessage)), "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                context.SaveChanges();
                MessageBox.Show("Success!", "Message", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void cbUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BindDataGrid();
            btnSavePermission.IsEnabled = true;
        }
    }

    public class PermissionList
    {
        public int Id { get; set; }
        public CPS.Common.Page Page { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Print { get; set; }
    }
}
