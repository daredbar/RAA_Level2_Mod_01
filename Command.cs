#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

#endregion

namespace RAA_Level2_Mod_01
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // put any code needed for the form here

            // open form
            MyForm currentForm = new MyForm()
            {
                Width = 800,
                Height = 400,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            currentForm.ShowDialog();

            // get form data and do something
            if (currentForm.DialogResult == false )
            {
                return Result.Cancelled;
            }

            // do something
            List<string[]> dataList = new List<string[]>();
            string textboxresult = currentForm.GetTextBoxValue();

            string[] dataArray = System.IO.File.ReadAllLines(textboxresult);

            foreach (string data in dataArray)
            {
                string[] cellString = data.Split(',');
                dataList.Add(cellString);
            }

            // remove header row
            dataList.RemoveAt(0);

            bool checkBox1Value = currentForm.GetCheckbox1();

            bool checkBox2Value = currentForm.GetCheckbox2();

            string rabioButtonValue = currentForm.GetGroup1();

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Levels");

                int counter = 0;
                foreach (string[] lData in dataList)
                {
                    double hImperial = 0;
                    double hMetric   = 0;
                    if (checkBox1Value == true)
                    {
                        return hImperial as double;
                    }

                    if (checkBox2Value == true)
                    {
                        return hMetric
                    }

                    //get height and convert from string to double
                    bool convertFeet = double.TryParse(lData[1], out hImperial);
                    bool convertMeters = double.TryParse(lData[2], out hMetric);

                    //if using metric, convert meters to feet
                    double heightMetersConvert = hMetric * 3.28084;
                    double heightMetersConvert2 = UnitUtils.ConvertToInternalUnits(hMetric, UnitTypeId.Meters);

                    TaskDialog.Show("Test", rabioButtonValue);

                    //create level and rename
                    Level cLevel = Level.Create(doc, rabioButtonValue);
                    cLevel.Name = lData[0];


                    if (checkBox1Value == true)
                    {
                        TaskDialog.Show("Test", "Check box 1 Floor Plan");
                    }

                    if (checkBox2Value == true)
                    {
                        TaskDialog.Show("Test", "Check box 2 Ceiling Plan");
                    }

                }

                t.Commit();

                TaskDialog.Show("Complete", "Created " + counter.ToString() + " Levels.")
            }


            return Result.Succeeded;

        }

        public static String GetMethod()
        {
            var method = MethodBase.GetCurrentMethod().DeclaringType?.FullName;
            return method;
        }
    }
}
