using SeeShells.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SeeShells
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// creates an instance of the EventCollection class so that the whole program can access the list of events. 
        /// </summary>
       public static EventCollection eventCollection = new EventCollection();
    }
}
