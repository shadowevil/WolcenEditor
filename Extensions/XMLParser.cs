using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WolcenEditor
{
    public static class XMLParser
    {
        public static KeyValuePair<string, string>? SearchRowInXml(string xmlFilePath, string searchString)
        {
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(xmlFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred while loading the XML file: {e.Message}");
                return null;
            }

            XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";

            var rowElement = xdoc.Descendants(ss + "Row")
                                 .FirstOrDefault(row => row.Elements(ss + "Cell")
                                                           .Elements(ss + "Data")
                                                           .Any(data => data.Value.ToLower().Contains(searchString.Substring(1).ToLower())));

            if (rowElement == null)
            {
                return null;
            }

            var cells = rowElement.Elements(ss + "Cell")
                                  .Select(cell => cell.Element(ss + "Data")?.Value)
                                  .ToList();

            if (cells.Count < 2)
            {
                return null;
            }

            return new KeyValuePair<string, string>(cells[0]!, cells[1]!);
        }
    }
}
