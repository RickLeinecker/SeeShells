using SeeShells.ShellParser.ShellItems;
using System;
using System.Collections.Generic;
using System.IO;

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
        /// <param name="shellItems">A list of ShellItems containg the data to be exported in the CSV file</param>
        /// <param name="filePath">The directory in which to output the CSV file including the CSV file name</param>
        /// <returns></returns>
        public static void OutputCSVFile(List<IShellItem> shellItems, String filePath)
        {
            // Reads each propery key of each ShellItem in order to determine the headers of the CSV.
            HashSet<String> keys = new HashSet<String>();
            foreach (IShellItem shellItem in shellItems)
            {
                foreach (KeyValuePair<string, string> property in shellItem.GetAllProperties())
                {
                    keys.Add(property.Key);
                } 
            }

            // Converts the HashSet of CSV headers to an array in order to alphabetize the headers and
            // establish indexes to be used to track the columns in which the property values should go.
            String[] keysArray = new String[keys.Count];
            keys.CopyTo(keysArray);
            Array.Sort(keysArray);

            // Converts the array of alphabetized headers to a Map with the array indexes as values.
            Dictionary<string, int> keysMap = new Dictionary<string, int>();
            for(int i = 0; i < keysArray.Length; i++)
            {
                keysMap.Add(keysArray[i], i);
            }

            // Writes the headers to the CSV and parses over the properties of each ShellItem again in oder to obtain the values
            // for each property, which are then placed in an array acording to the index that they map to and written to the CSV.
            using (var writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(string.Join(",", keysArray));
                writer.Flush();

                foreach (IShellItem shellItem in shellItems)
                {
                    String[] line = new String[keysArray.Length];
                    foreach (KeyValuePair<string, string> property in shellItem.GetAllProperties())
                    {
                        line[keysMap[property.Key]] = property.Value.Replace("," , ""); // Any commas are filtered from the output
                    }
                    writer.WriteLine((string.Join(",", line)));
                    writer.Flush();
                }
            }
        }

        /// <summary>
        /// Creates a ShellItem list from ShellBag data parsed from a CSV.
        /// </summary>
        /// <param name="filePath">The path to a source CSV file</param>
        /// <returns>A list of ShellItems</returns>
        public static List<IShellItem> ImportCSVFile(String filePath)
        {
            List<IShellItem> retList = new List<IShellItem>();
            using (var reader = new StreamReader(filePath))
            {
                // Red the header which contains the property values
                String headerLine = reader.ReadLine();
                String[] propertyValues = headerLine.Split(',');
                int propertyValuesCurrIndex = 0;

                while (!reader.EndOfStream)
                {
                    String shellItemLine = reader.ReadLine();
                    String[] shellItemPropertyValues = shellItemLine.Split(',');
                    
                    IDictionary<string, string> properties = new Dictionary<string, string>();
                    foreach (String property in shellItemPropertyValues)
                    {
                        if(property != "")
                        {
                            properties.Add(propertyValues[propertyValuesCurrIndex], property);
                        }
                        propertyValuesCurrIndex++;
                    }
                    propertyValuesCurrIndex = 0;
                    retList.Add(new CsvParsedShellItem(properties));
                }
            }
                return retList;
        }
    }
}
