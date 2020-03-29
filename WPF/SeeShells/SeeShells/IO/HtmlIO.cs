﻿using System;
using System.Collections.Generic;
using System.IO;
using SeeShells.UI.Node;

namespace SeeShells.IO
{
    public static class HtmlIO
    {
        /// <summary>
        /// Creates a HTML file with ShellBag data in a given location.
        /// </summary>
        /// <param name="nodeList">A list of Nodes containg the filtered list of events from the timeline</param>
        /// <param name="filePath">The directory in which to output the HTML file including the HTML file name</param>
        /// <returns></returns>
        public static void OutputHtmlFile(List<Node> nodeList, String filePath)
        {
            // Creates the html file
            StreamWriter outputFile;
            outputFile = File.CreateText(filePath);
            outputFile.WriteLine("<html>");

            // Implements the CSS Styling
            outputFile.WriteLine("<head>");
            outputFile.WriteLine("<style>");
            outputFile.WriteLine(".card {box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);transition: 0.3s;width: 80 %;}");
            outputFile.WriteLine(".card:hover {box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);}");
            outputFile.WriteLine(".container {padding: 2px 16px;}");
            outputFile.WriteLine(".grid-container {display: grid;grid-template-columns: 25% 75%;padding: 10px;}");
            outputFile.WriteLine(".grid-item {padding: 10px;border-left: 1px solid rgba(0, 0, 0, 0.8);text-align: center;}");
            outputFile.WriteLine("</style>");
            outputFile.WriteLine("</head>");

            // Adding the shellItems to the body of thr html
            outputFile.WriteLine("<body>");
            outputFile.WriteLine("<h2>Timeline HTML Output</h2>");

            // Iterates through the list of IEvents and adds the information to the HTML file
            foreach (Node node in nodeList)
            {
                outputFile.WriteLine("<div class=\"card\">");
                outputFile.WriteLine("<div class=\"container\">");
                outputFile.WriteLine("<div class=\"grid-container\">");
                // This is the opening of the main shellItem
                outputFile.WriteLine("<div class=\"grid-item\">");
                outputFile.WriteLine("<h4><b>" + node.aEvent.Name + "</b></h4>");
                outputFile.WriteLine("<p>" + node.aEvent.EventTime + "</p>");
                outputFile.WriteLine("<p>" + node.aEvent.timeZone.StandardName + "</p>");
                outputFile.WriteLine("<p>" + node.aEvent.EventType + "</p>");
                outputFile.WriteLine("</div>");
                
                // This shows the extra information about the shellItem
                outputFile.WriteLine("<div class=\"grid-item\" style=\"column-count: 3;\" > ");
                foreach (KeyValuePair<string, string> property in node.aEvent.Parent.GetAllProperties())
                {
                    outputFile.WriteLine("<p>" + property + "</p>");
                }

                outputFile.WriteLine("</div>");
                outputFile.WriteLine("</div>");
                outputFile.WriteLine("</div>");
                outputFile.WriteLine("</div>");
            }

            outputFile.WriteLine("</body>");
            outputFile.WriteLine("</html>");
            outputFile.Close();
        }
    }
}
