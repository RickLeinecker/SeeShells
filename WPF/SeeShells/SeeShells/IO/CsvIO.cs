using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;

namespace SeeShells.IO
{
    /// <summary>
    /// This class is used to manage output of ShellBag data to a CSV file and import of ShellBag data from a CSV file to main memory.
    /// </summary>
    public static class CsvIO
    {
        /// <summary>
        /// Creates a CSV file with ShellBag data in a given location.
        /// </summary>
        /// <param name="ShellItems">A list of ShellItems containg the data to be exported in the CSV file</param>
        /// <param name="dir">The directory in which to output the CSV file</param>
        /// <returns></returns>
        public static void OutputCSVFile(List<IShellItem> ShellItems, String dir)
        {

        }

        /// <summary>
        /// Creates a ShellItem list from ShellBag data parsed from a CSV.
        /// </summary>
        /// <param name="dir">The directory of a source CSV file</param>
        /// <returns>A list of ShellItems</returns>
        public static List<IShellItem> ImportCSVFile(String dir)
        {
            return new List<IShellItem>();
        }
    }
}
