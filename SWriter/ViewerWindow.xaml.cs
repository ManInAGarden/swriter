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

namespace SWriter
{
    /// <summary>
    /// Interaktionslogik für ViewerWindow.xaml
    /// </summary>
    public partial class ViewerWindow : Window
    {
        public FlowDocument Document { set; get; }
        public ViewerWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Document != null)
                theDocViewer.Document = Document;
        }
        
    }
}
