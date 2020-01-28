using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;

namespace SeeShells.IO
{
    /// <summary>
    /// This class is used to manage the output of ShellBags data in the form of a printable HTML timeline.
    /// </summary>
    public static class HtmlIO
    {
        /// <summary>
        /// Creates a HTML graphics output with ShellBag data that is print friendly.
        /// </summary>
        /// <param name="shellItems">A list of ShellItems containg the data that will be used to create the ShellItem HTML object.</param>
        /// <returns></returns>
        public static void OutputHtmlFile(List<IShellItem> shellItems)
        {
            // Retrieves the main information to be places in the timeline
            // Reads all properties of ShellItem and adds the complete information in an index
            foreach (IShellItem shellItem in shellItems)
            {
                // Creates the element on the html timeline and connects it with an index look up
                int index = 1;
                string name = shellItem.Name;
                DateTime modifiedDate = shellItem.ModifiedDate;
                DateTime accessedDate = shellItem.AccessedDate;
                DateTime creationDate = shellItem.CreationDate;

                CreateHTMLElement(name, modifiedDate, accessedDate, creationDate, index);

                // Creates the index look up wih the full information of the ShellItem
                //foreach (KeyValuePair<string, string> property in shellItem.GetAllProperties())
                //{

                //}
            }
        }
        public static void CreateHTMLElement(String name, DateTime modifiedDate, DateTime accessedDate, DateTime creationDate, int index)
        {
            // Initialize StringWriter instance.
            StringWriter stringWriter = new StringWriter();

            // Put HtmlTextWriter in using block because it needs to call Dispose.
            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                writer.RenderBeginTag("MyTag");
                writer.Write("Contents of MyTag");
                writer.RenderEndTag();
                writer.WriteLine();
            }
        }
    }
}
