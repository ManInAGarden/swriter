using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using System.IO;

namespace SWriter
{
    /// <summary>
    /// Interaktionslogik für HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            FileStream fs = null;

            try
            {
                String strPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                fs = new FileStream(strPath + "\\help\\MainHelpFile.xaml", FileMode.Open, FileAccess.Read);

                FlowDocument doc = new FlowDocument();
                TextRange ra = new TextRange(doc.ContentStart,
                    doc.ContentEnd);
                ra.Load(fs, DataFormats.Xaml);

                helpDocReader.Document = doc; 
                
                helpDocReader.GoToPage(0);
            }
            catch
            {
                MessageBox.Show(this, "Die Hilfedatei wurde nicht gefunden", "Fehler");
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

        }


        public static DependencyObject GetScrollViewer(DependencyObject o) 
        {     
            // Return the DependencyObject if it is a ScrollViewer     
            
            if (o is ScrollViewer)     
            { 
                return o; 
            }      
            
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(o); i++)
            {         
                var child = VisualTreeHelper.GetChild(o, i);
                var result = GetScrollViewer(child);
                if (result == null)
                {             
                    continue;         
                }         
                else
                {             
                    return result;         
                }     
            }      
            
            return null; 
        } 
    }
}
