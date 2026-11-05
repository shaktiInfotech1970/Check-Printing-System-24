using CPS.Business;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
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

namespace CPS.Views.Preferences
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : UserControl
    {
        PrinterPreference objPrinterPreference = new PrinterPreference();

        public Preferences()
        {
            InitializeComponent();
            BindComboBox();
            LoadPrinterPreference();
        }

        private void LoadPrinterPreference()
        {
            using(var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<PrinterPreference>(context);
                var printerPreference = repository.GetAll().FirstOrDefault();
                if (printerPreference != null)
                {
                    objPrinterPreference = printerPreference;

                    cbPrinter.SelectedValue = printerPreference.Name;
                    LoadPrinterTrays(printerPreference.Name);
                    cbRequestTray.SelectedValue = printerPreference.RequestTray;
                    cbChequeTray.SelectedValue = printerPreference.ChequeTray;
                }
            }
        }

        private void LoadPrinterTrays(string printerName)
        {
            PrinterSettings ps = new PrinterSettings();
            ps.PrinterName = printerName;

            cbRequestTray.ItemsSource = ps.PaperSources.Cast<PaperSource>().Select(s => new LookupItem<int, string> { Key = s.RawKind, Value = s.SourceName }).ToList();
            cbRequestTray.DisplayMemberPath = "Value";
            cbRequestTray.SelectedValuePath = "Key";

            cbChequeTray.ItemsSource = ps.PaperSources.Cast<PaperSource>().Select(s => new LookupItem<int, string> { Key = s.RawKind, Value = s.SourceName }).ToList();
            cbChequeTray.DisplayMemberPath = "Value";
            cbChequeTray.SelectedValuePath = "Key";
        }

        private void BindComboBox()
        {
            cbPrinter.ItemsSource = PrinterSettings.InstalledPrinters.Cast<string>().Select(s => new LookupItem<string, string> { Key = s, Value = s }).ToList();
           cbPrinter.DisplayMemberPath = "Value";
           cbPrinter.SelectedValuePath = "Key";
        }

        private void cbPrinter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadPrinterTrays((string)cbPrinter.SelectedValue);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbPrinter.SelectedValue == null || cbRequestTray.SelectedValue == null || cbChequeTray.SelectedValue == null)
            {
                MessageBox.Show("Printer Preferences are not valid.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            objPrinterPreference.Name = (string)cbPrinter.SelectedValue;
            objPrinterPreference.RequestTray = (int)cbRequestTray.SelectedValue;
            objPrinterPreference.ChequeTray = (int)cbChequeTray.SelectedValue;

            using(var context = new CPSDbContext())
            {
                var repository = new PersistenceBase<PrinterPreference>(context);
                var errors = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
                if (repository.SaveOrUpdate(objPrinterPreference, errors))
                {
                    context.SaveChanges();
                    MessageBox.Show("Printer Preferences save sucessfully.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    MessageBox.Show(string.Join(Environment.NewLine, errors.Select(o => o.ErrorMessage)), "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
