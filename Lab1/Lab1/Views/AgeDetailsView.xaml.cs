using Lab1.ViewModels;
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

namespace Lab1.Views
{
    /// <summary>
    /// Interaction logic for AgeDetailsView.xaml
    /// </summary>
    public partial class AgeDetailsView : UserControl
    {
        #region Fields
        private AgeDetailsViewModel _viewModel;
        #endregion

        public AgeDetailsView()
        {
            InitializeComponent();
            DataContext = _viewModel = new AgeDetailsViewModel();
        }

        private void DpBirthday_DateValidationError(object sender, DatePickerDateValidationErrorEventArgs e)
        {
            DateTime toParse;
            DatePicker datePicker = (DatePicker)sender;

            if(!DateTime.TryParse(e.Text, out toParse))
            {
                MessageBox.Show("Invalid Date Format");
            }
        }
    }
}
