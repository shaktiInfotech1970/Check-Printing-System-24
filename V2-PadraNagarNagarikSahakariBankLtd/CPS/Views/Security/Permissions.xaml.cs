using CPS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CPS.Views.Security
{
    /// <summary>
    /// Interaction logic for Permissions.xaml
    /// </summary>
    public partial class Permissions : UserControl
    {
        public Permissions()
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
                dgUserPermission.ItemsSource = repository.GetAll().Where(o => o.UserId == userId).ToList();
            }
        }

        private void btnSavePermission_Click(object sender, RoutedEventArgs e)
        {
            var olPermission = (List<PermissionDTO>)dgUserPermission.ItemsSource;

            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<PermissionDTO>(context);
                olPermission.ForEach(o => repository.SaveOrUpdate(o));
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
}
