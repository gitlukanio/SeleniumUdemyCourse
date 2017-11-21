using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.PageObjects;

namespace SeleniumUdemyTrainingConsole
{
    public class Program : Base
    {


        static void Main(string[] args)
        {
            //Driver = new FirefoxDriver();
            Driver = new InternetExplorerDriver();
            Driver.Navigate().GoToUrl("file:///C:/Users/luczkluk/Documents/Moje/ProjektyVisualStudio/SeleniumUdemyTrainingSolution/SimpleTable.html");

            TablePage page = new TablePage();

            // Read table
            Utilities.ReadTable(page.table);

            // Get the cell value from the table
            Console.WriteLine("Dane z komórki:{0}", Utilities.ReadCell("Email", 1));


        }
    }
    //======================
    public class Base
    {
        public static IWebDriver Driver;
    }
    //=======================
    public class TablePage : Base
    {
        public TablePage()
        {
            PageFactory.InitElements(Driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//table")]
        public IWebElement table { get; set; }

    }

    public class Utilities
    {
        static List<TableDataCollection> _tableDataCollections = new List<TableDataCollection>();

        public static void ReadTable(IWebElement table)
        {
            // Get all the collumns from the table
            var columns = table.FindElements(By.TagName("th"));

            // Get all the rows
            var rows = table.FindElements(By.TagName("tr"));

            //Create row index
            int rowIndex = 0;

            foreach (var row in rows)
            {
                int colIndex = 0;
                var colData = row.FindElements(By.TagName("td"));
                foreach (var colValue in colData)
                {
                    _tableDataCollections.Add(new TableDataCollection
                    {
                        RowNumber = rowIndex,
                        ColumnName = columns[colIndex].Text,
                        ColumnValue = colValue.Text
                    });
                    // Move to next collumn
                    colIndex++;
                }
                rowIndex++;
            }
        }

        public static string ReadCell(string columnName, int rowNumber)
        {
            var data = (from e in _tableDataCollections
                        where e.ColumnName == columnName && e.RowNumber == rowNumber
                        select e.ColumnValue).SingleOrDefault();

            return data;
        }

    }

    public class TableDataCollection
    {
        public int RowNumber { get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
    }
}
