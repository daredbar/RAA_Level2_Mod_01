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
    public partial class MyFormPt1M2 : Window
    {
        public Document myDoc;
        public MyFormPt1M2(Document doc, List<string> listBoxItems, List<int> comBoxItems)
        {
            InitializeComponent();
            myDoc = doc;

            if (listBoxItems == null && comBoxItems == null)
            {
                string tx = "(EMPTY)";
                lbxText1.Items.Add(tx);
                cmbNum.Items.Add(tx);
            }
            else
            {
                foreach (string item in listBoxItems)
                {
                    lbxText1.Items.Add(item);
                }
                foreach (int item in comBoxItems)
                {
                    cmbNum.Items.Add(item);
                }
            }
        }

        public List<string> GetSelectedListboxItems()
        {
            List<string> returnList = new List<string>();

            foreach (var item in lbxText1.SelectedItems)
            {
                returnList.Add(item.ToString());
            }

            return returnList;
        }

        public string GetSelectedComboboxItem()
        {
            return cmbNum.SelectedItem.ToString();
        }

        private void btnSelectPt1M2_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
