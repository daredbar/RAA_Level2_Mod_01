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
    public class Module02 : IExternalCommand
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
            MyFormPt1M2 currentForm2 = new MyFormPt1M2(doc, null, null)
            {
                Width = 800,
                Height = 400,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            currentForm2.ShowDialog();

            // get form data and do something

            List<Reference> reflist = new List<Reference>();
            bool flag = true;

            while (flag == true)
            {
                try
                {
                    Reference curRef = uidoc.Selection.PickObject(ObjectType.Element, "Pick an Item");
                    reflist.Add(curRef);
                }
                catch (Exception)
                { 
                    flag = false;
                }
            }

            string returnString = "There are " + reflist.Count.ToString() + " selected elements";
            TaskDialog.Show("Test", returnString);
            List<string> returnStrings = new List<string> { };
            foreach (var it in reflist)
            {
                returnStrings.Add(it.ElementId.ToString());
            }

            List<int> cmbString = new List<int> {10,20,30,40};

            // open form
            MyFormPt1M2 currentForm3 = new MyFormPt1M2(doc, returnStrings, cmbString)
            {
                Width = 800,
                Height = 400,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                Topmost = true,
            };

            currentForm3.ShowDialog();

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Viewports Renumber");
                foreach (var it in reflist)
                {
                    var vpNum = currentForm3.GetSelectedComboboxItem();
                    vpNum = vpNum + 1;
                    Viewport.get_parameter(BuiltInParameter.VIEWPORT_DETAIL_NUMBER).Value = vpNum;
                }
                t.Commit();
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
