using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RAA_Level2_Mod_01
{
    /// <summary>
    /// Interaction logic for Window.xaml
    /// </summary>
    public partial class MyFormPt2M2 : Window
    {
        public MyFormPt2M2(Document doc, string msg, List<string> listChange)
        {
            InitializeComponent();

            string tx = msg;
            lbxText2.Items.Add(tx);

            foreach (string item in listChange)
            {
                lbxText2.Items.Add(item);
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
