using SeeShells.ShellParser.Scripting;
using SeeShells.ShellParser.ShellItems;
using SeeShells.UI;
using SeeShells.UI.Node;
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
        /// Creates an instance of the EventCollection class so that the whole program can access the list of events. 
        /// </summary>
        public static EventCollection eventCollection = new EventCollection();

        /// <summary>
        /// Creates an instance of the NodeCollection class so that the whole program can access the list of nodes. 
        /// </summary>
        public static NodeCollection nodeCollection = new NodeCollection();

        /// <summary>
        /// Collectin of <see cref="ShellItem"/> which is populated after a parsing operation.
        /// </summary>
        public static List<IShellItem> ShellItems { get; set; }
    }
}
