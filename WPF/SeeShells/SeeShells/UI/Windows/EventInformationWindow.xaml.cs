﻿using System;
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
using System.Windows.Shapes;

namespace SeeShells.UI.Windows
{
    /// <summary>
    /// Interaction logic for EventInformationWindow.xaml
    /// </summary>
    public partial class EventInformationWindow : Window
    {
        public string EventTitle { get; set; }
        public string EventBody { get; set; }

        public EventInformationWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
    }
}
