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

namespace SeeShells.UI.Templates
{
    /// <summary>
    /// Interaction logic for CheckBoxWithLabel.xaml
    /// </summary>
    public partial class CheckBoxWithLabel : UserControl
    {

        public event EventHandler CheckClicked;
        public event EventHandler CheckUnclicked;
        public CheckBoxWithLabel()
        {
            InitializeComponent();
        }

        public string LabelContent
        {
            get { return box.Content.ToString(); }
            set
            {
                box.Content = value;
            }
        }

        public bool? IsChecked
        { 
            get { return box.IsChecked; }
        }

        private void box_Checked(object sender, RoutedEventArgs e)
        {
            CheckClicked?.Invoke(this, EventArgs.Empty);
        }

        private void box_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckUnclicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
