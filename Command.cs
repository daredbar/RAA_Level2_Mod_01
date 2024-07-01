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

            bool checkBox3Value = currentForm.GetCheckbox3();

            string rabioButtonValue = currentForm.GetGroup1();

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create Levels");

                int counterLevel = 0;
                int counterFP = 0;
                int counterCP = 0;
                foreach (string[] lData in dataList)
                {
                    double height = 0;
                    double hImperial = 0;
                    double hMetric = 0;

                    //get height and convert from string to double
                    bool convertFeet = double.TryParse(lData[1], out hImperial);
                    bool convertMeters = double.TryParse(lData[2], out hMetric);

                    if (rabioButtonValue == "Imperial")
                    {
                        height = hImperial;
                    }

                    if (rabioButtonValue == "Metric")
                    {
                        //if using metric, convert meters to feet
                        //double hMetric2 = hMetric * 3.28084;
                        height = UnitUtils.ConvertToInternalUnits(hMetric * 3.28084, UnitTypeId.Meters);
                    }

                    //TaskDialog.Show("Test", rabioButtonValue);

                    //create level and rename
                    Level cLevel = Level.Create(doc, height);
                    cLevel.Name = lData[0];

                    FilteredElementCollector coll1 = new FilteredElementCollector(doc);
                    coll1.OfClass(typeof(ViewFamilyType));

                    FilteredElementCollector coll2 = new FilteredElementCollector(doc);
                    coll2.OfCategory(BuiltInCategory.OST_TitleBlocks);
                    coll2.WhereElementIsElementType();


                    if (checkBox1Value == true)
                    {
                        ViewFamilyType floorPlanVFT = null;
                        foreach (ViewFamilyType curVFT in coll1)
                        {
                            if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                            {
                                floorPlanVFT = curVFT;
                                break;
                            }
                        }

                        // create a view by specifying the document, view family type, and level
                        ViewPlan newPlan = ViewPlan.Create(doc, floorPlanVFT.Id, cLevel.Id);
                        newPlan.Name = lData[0];
                        counterFP++;

                        if (checkBox3Value == true)
                        {
                            ViewSheet newSheet = ViewSheet.Create(doc, coll2.FirstElementId());
                            newSheet.Name = ($"Floor Plan+{  lData[0]}");
                            newSheet.SheetNumber = counterFP.ToString();
                        }

                        //TaskDialog.Show("Test", "Check box 1 Floor Plan");
                    }

                    if (checkBox2Value == true)
                    {
                        ViewFamilyType ceilingPlanVFT = null;
                        foreach (ViewFamilyType curVFT in coll1)
                        {
                            if (curVFT.ViewFamily == ViewFamily.CeilingPlan)
                            {
                                ceilingPlanVFT = curVFT;
                                break;
                            }
                        }

                        // create a ceiling plan using the ceiling plan view family type
                        ViewPlan newCeilingPlan = ViewPlan.Create(doc, ceilingPlanVFT.Id, cLevel.Id);
                        newCeilingPlan.Name = lData[0];
                        counterCP++;

                        if (checkBox3Value == true)
                        {
                            ViewSheet newSheet = ViewSheet.Create(doc, coll2.FirstElementId());
                            newSheet.Name = ($"Ceiling Plan+ {lData[0]}");
                            newSheet.SheetNumber = counterCP+10.ToString();
                        }
                        //TaskDialog.Show("Test", "Check box 2 Ceiling Plan");
                    }

                    counterLevel++;

                }

                t.Commit();

                TaskDialog.Show("Complete Levels", "Created " + counterLevel.ToString() + " Levels.");
                TaskDialog.Show("Floor Plans", $"{counterFP} Floor Plans created & {counterCP} Ceiling Plans created.");
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
