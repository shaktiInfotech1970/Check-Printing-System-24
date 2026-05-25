using CPS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CPS.Views.Preferences
{
    /// <summary>
    /// Interaction logic for Backup.xaml
    /// </summary>
    public partial class Backup : UserControl
    {
        DatabaseBackup objDatabaseBackup = new DatabaseBackup();
        public Backup()
        {
            InitializeComponent();
            LoadBackupDetails();
        }

        private void LoadBackupDetails()
        {
            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<DatabaseBackup>(context);
                var dbBackup = repository.GetAll().FirstOrDefault();
                if (dbBackup != null)
                {
                    objDatabaseBackup = dbBackup;
                    txtBackUpPath.Text = dbBackup.Path;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBackUpPath.Text))
            {
                MessageBox.Show("Database backup path is required..", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            objDatabaseBackup.Path = txtBackUpPath.Text;

            using (var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<DatabaseBackup>(context);
                var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                if (repository.SaveOrUpdate(objDatabaseBackup, errors))
                {
                    context.SaveChanges();
                    MessageBox.Show("Database backup path saved sucessfully.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show(string.Join(Environment.NewLine, errors.Select(o => o.ErrorMessage)), "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnBackupPath_MouseDown(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog(txtBackUpPath);
        }

        private void OpenFolderDialog(TextBox obj)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result.ToString() == "OK")
                obj.Text = dialog.SelectedPath;
        }

    }
}
