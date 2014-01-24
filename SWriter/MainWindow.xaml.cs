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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using System.Printing;
using System.IO;
using System.Windows.Xps;

namespace SWriter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand Search = new RoutedCommand();

        public static RoutedCommand SetBold = new RoutedCommand();
        public static RoutedCommand SetItalic = new RoutedCommand();

        public static RoutedCommand Settings = new RoutedCommand();
        public static RoutedCommand About = new RoutedCommand();
        public static RoutedCommand Help = new RoutedCommand();

        private bool overMenu = false;
        private string theFileName = null;
        private string m_timeString = DateTime.Now.ToString("hh:mm");
        private float m_zoomFaktor = 1.0f;
        private bool m_textChanged = false;
        private List<string> m_lastSearches = new List<string>(20);
        private int m_lastSearchListIndex = 0;
        private FindReplaceHelper frepHelper = null;
        private TextRange m_startOfSearch;
        private string lastSearchedFor = "";
        private bool m_inOverwrite = false;
        private int m_linesPerPage = 40;
        private const int m_initialLinesPerPage = 40;
        DispatcherTimer m_autosaveTimer = null;
        DispatcherTimer m_searchBoxVanisher = null;
        DispatcherTimer m_wordCountTimer = null;
        DispatcherTimer m_setClockTimer = null;


        private bool InOverwrite
        {
            set { 
                m_inOverwrite = value;
                if (value)
                {
                    SetIndicator(overwriteIndicator, "Ü");
                }
                else
                {
                    SetIndicator(overwriteIndicator, "");
                }
            }
            get { return m_inOverwrite; }
        }


        private float ZoomFaktor
        {
            set
            {
                m_zoomFaktor = value;
                if (Math.Abs(m_zoomFaktor - 1.0f) < 0.1f)
                    m_zoomFaktor = 1.0f;

                if (m_zoomFaktor == 1.0f)
                {
                    SetIndicator(zoomIndicator, "");
                }
                else
                {
                    SetIndicator(zoomIndicator, m_zoomFaktor.ToString("0%"));
                }
            }
            get { return m_zoomFaktor; }
        }

        private string TimeString
        {
            set
            {
                m_timeString = value;
                if (clockIndicator != null)
                {
                    if (value != null)
                        SetIndicator(clockIndicator, value);
                    else
                        SetIndicator(clockIndicator, "");
                }
            }

            get { return m_timeString; }
        }


        private bool TextChanged
        {
            set
            {
                m_textChanged = value;
                if (changeIndicator != null)
                {
                    if (value)
                    {
                        SetIndicator(changeIndicator, "S");

                        if (!string.IsNullOrEmpty(theFileName))
                            ActivateAutoSave(true);
                    }
                    else
                    {
                        SetIndicator(changeIndicator, "");
                        ActivateAutoSave(false);
                    }
                }
            }
            get { return m_textChanged; }
        }


        private List<string> LastSearches
        {
            get { return m_lastSearches; }
            set { m_lastSearches = value; }
        }


        private void ActivateAutoSave(bool doStart)
        {
            if (m_autosaveTimer == null)
                return;

            if (doStart)
            {
                if ((m_autosaveTimer.Interval.TotalSeconds > 30.0) && !m_autosaveTimer.IsEnabled)
                    m_autosaveTimer.IsEnabled = true;
            }
            else
                m_autosaveTimer.IsEnabled = false;
        }


        private void ActivateSearchBoxVanisher(bool doStart)
        {
            if (m_searchBoxVanisher == null)
                return;

            if (doStart)
                m_searchBoxVanisher.Start();
            else
                m_searchBoxVanisher.Stop();
        }


        /// <summary>
        /// Count words in a FlowDocument
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private int WordCount(FlowDocument doc)
        {
            TextRange ra = new TextRange(doc.ContentStart,
                doc.ContentEnd);

            return WordCount(ra.Text);
        }

        /// <summary>
        /// Count the words in a string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int WordCount(string s) 
        { 
            s = s.TrimEnd(); 
            if (String.IsNullOrEmpty(s)) 
                return 0; 
            
            int count = 0; 
            bool lastWasWordChar = false; 
            
            foreach (char c in s) 
            { 
                if (Char.IsLetterOrDigit(c) || c == '_' || c == '\'' || c == '-') 
                { 
                    lastWasWordChar = true; 
                    continue; 
                } if (lastWasWordChar) 
                { 
                    lastWasWordChar = false; 
                    count++; 
                } 
            } 
            
            if (!lastWasWordChar) 
                count--; 
            
            return count + 1; 
        }

        private void SetIndicator(TextBlock indicator, string ins)
        {
            indicator.Text = ins;

            if (indicator.Parent is Border)
            {
                Border b = indicator.Parent as Border;
                if (!string.IsNullOrEmpty(ins))
                    b.BorderThickness = new Thickness(1.0);
                else
                    b.BorderThickness = new Thickness(0.0);
            }
        }

        private void SetIndicator(TextBox tb, string ins)
        {
            tb.Text = ins;

            if (!string.IsNullOrEmpty(ins))
            {
                tb.Visibility = System.Windows.Visibility.Visible;
                if (ins == "Suche hier eingeben")
                {
                    tb.SelectAll();
                }

                this.ActivateSearchBoxVanisher(true);
            }
            else
            {
                tb.Visibility = System.Windows.Visibility.Collapsed;
                this.ActivateSearchBoxVanisher(false);
            }

            if (tb.Parent is Border)
            {
                Border b = tb.Parent as Border;
                if (!string.IsNullOrEmpty(ins))
                {
                    b.Visibility = System.Windows.Visibility.Visible;
                    //b.BorderThickness = new Thickness(1.0);
                }
                else
                {
                    //b.BorderThickness = new Thickness(0.0);
                    b.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }


        public MainWindow()
        {
            InitializeComponent();
        }


        private void Close_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// The close command is executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (TextChanged)
            {
                if (MessageBox.Show(this,
                    "Die Datei wurde noch nicht gesichert. Wirklich beenden?",
                    "Frage",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
              
                    Environment.Exit(0);
                }
            }
            else
            {
              
                Environment.Exit(0);
            }
        }

        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = "rtf";
            ofd.Filter = "rtf-Datei|*.rtf|txt-Dateien|*.txt|XAML-Dateien|*.xaml";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                string ext = System.IO.Path.GetExtension(ofd.FileName);
                switch (ext)
                {
                    case ".rtf":
                        OpenFromFile(ofd.FileName, DataFormats.Rtf);
                        break;
                    case ".xaml":
                        OpenFromFile(ofd.FileName, DataFormats.Xaml);
                        break;
                    case ".txt":
                        OpenFromFile(ofd.FileName, DataFormats.Text);
                        break;
                    default:
                        ShowError("Dateien im Format <" + ext + "> können von SWriter nicht verarbeitet werden.");
                        break;
                }

            }
        }


        private void Open_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveAs_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.DefaultExt = "rtf";
            sfd.Filter = "rtf-Datei|*.rtf|txt-Dateien|*.txt|XAML-Dateien|*.xaml"; ;
            if (sfd.ShowDialog() == true)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".rtf":
                        SaveToFile(sfd.FileName, DataFormats.Rtf);
                        break;
                    case ".xaml":
                        SaveToFile(sfd.FileName, DataFormats.Xaml);
                        break;
                    case ".txt":
                        SaveToFile(sfd.FileName, DataFormats.Text);
                        break;
                    default:
                        ShowError("SWriter kann nicht im Format <" + ext + ">  speichern.\nBitte txt, rtf oder xaml verwenden.");
                        break;
                }
            }
        }




        private void SaveAs_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// Save command is executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(theFileName))
            {
                string ext = System.IO.Path.GetExtension(theFileName);
                switch (ext)
                {
                    case ".rtf":
                        SaveToFile(theFileName, DataFormats.Rtf);
                        break;
                    case ".txt":
                        SaveToFile(theFileName, DataFormats.Text);
                        break;
                    case ".xaml":
                        SaveToFile(theFileName, DataFormats.Xaml);
                        break;
                    default:
                        ShowError("SWriter kann die Datei nicht im <" + ext + ">-Format speichern.");
                        break;
                }
            }
            else
                SaveAs_Executed(sender, e);
        }

        /// <summary>
        /// checks if save command can be executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Preview_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ViewerWindow vw = new ViewerWindow();

            vw.Document = sWriterRichTextBox.Document;
            vw.Show();
        }

        private void Preview_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        /// <summary>
        /// The print document gets executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Print_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Clone the source document's content into a new FlowDocument.
            FlowDocument flowDocumentCopy = CloneDoc(sWriterRichTextBox.Document);


            // Create a XpsDocumentWriter object, open a Windows common print dialog.

            // This methods returns a ref parameter that represents information about the dimensions of the printer media. 
            PrintDocumentImageableArea ia = null;
            XpsDocumentWriter docWriter = PrintQueue.CreateXpsDocumentWriter(ref ia);


            if (docWriter != null && ia != null)
            {
                DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDocumentCopy).DocumentPaginator;


                // Change the PageSize and PagePadding for the document to match the CanvasSize for the printer device.
                paginator.PageSize = new Size(ia.MediaSizeWidth, ia.MediaSizeHeight);
                //Thickness pagePadding = flowDocumentCopy.PagePadding;
                Thickness pagePadding = sWriterRichTextBox.Document.PagePadding;
                flowDocumentCopy.PagePadding = new Thickness(
                        Math.Max(ia.OriginWidth, pagePadding.Left),
                        Math.Max(ia.OriginHeight, pagePadding.Top),
                        Math.Max(ia.MediaSizeWidth - (ia.OriginWidth + ia.ExtentWidth), pagePadding.Right),
                        Math.Max(ia.MediaSizeHeight - (ia.OriginHeight + ia.ExtentHeight), pagePadding.Bottom));
                flowDocumentCopy.ColumnWidth = double.PositiveInfinity;


                // Send DocumentPaginator to the printer.
                docWriter.Write(paginator);
            }

        }

        /// <summary>
        /// The print command is executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Print_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            TextRange range = new TextRange(sWriterRichTextBox.Document.ContentStart,
                sWriterRichTextBox.Document.ContentEnd);

            e.CanExecute = !range.IsEmpty;

        }

        /// <summary>
        /// The about command is executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void About_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AboutWindow aw = new AboutWindow();
            aw.Owner = this;
            aw.ShowDialog();
            e.Handled = true;
        }

        /// <summary>
        /// The help command is executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            HelpWindow hw = new HelpWindow();
            //hw.Owner = this;
            hw.Show();

            e.Handled = true;
        }


        private void MenuSwitchOn_MouseMove(object sender, MouseEventArgs e)
        {
            if (overMenu)
                return;

            Point pos = e.GetPosition(this);

            if (pos.Y < 20)
            {
                mainMenu.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                mainMenu.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            overMenu = true;
        }

        private void MenuItem_MouseLeave(object sender, MouseEventArgs e)
        {
            overMenu = false;
        }



        private void New_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            bool doit = false;
            TextRange range = new TextRange(sWriterRichTextBox.Document.ContentStart,
                sWriterRichTextBox.Document.ContentEnd);

            if (!range.IsEmpty && TextChanged)
            {
                if (MessageBox.Show(this,
                    "Wirklich den Text löschen und neu beginnen?", "Frage", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    doit = true;
            }
            else if (!TextChanged)
            {
                doit = true;
            }

            if (doit)
            {
                sWriterRichTextBox.Document.Blocks.Clear();
                sWriterRichTextBox.Document.LineHeight = Properties.Settings.Default.LineHeight;
                TextChanged = false;
                theFileName = null;
            }
        }

        /// <summary>
        /// Checks if new command can be executed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void sWriterRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitFromProps();

            //create a new timer, but do not start it
           
            m_autosaveTimer = new DispatcherTimer();
            m_autosaveTimer.Interval = new TimeSpan(0, 0, Properties.Settings.Default.AutoSavePeriod);
            m_autosaveTimer.Tick += new EventHandler(m_autosaveTimer_Tick);
            m_autosaveTimer.Start();
            ActivateAutoSave(false);

            m_wordCountTimer = new DispatcherTimer();
            m_wordCountTimer.Interval = new TimeSpan(0, 0, 5);
            m_wordCountTimer.Tick += new EventHandler(m_wordCountTimer_Tick);
            m_wordCountTimer.Start();

            m_setClockTimer = new DispatcherTimer();
            m_setClockTimer.Interval = new TimeSpan(0, 0, 30);
            m_setClockTimer.Tick += new EventHandler(m_setClockTimer_Tick);
            m_setClockTimer.Start();

            m_searchBoxVanisher = new DispatcherTimer();
            m_searchBoxVanisher.Interval = new TimeSpan(0, 0, 30);
            m_searchBoxVanisher.Tick += new EventHandler(m_searchBoxVanisher_Tick);
            
            TextChanged = false;
            SetIndicator(wordCountIndicator, WordCount(sWriterRichTextBox.Document).ToString());
            ZoomFaktor = 1.0f;

            InOverwrite = false;
            sWriterRichTextBox.CaretPosition = sWriterRichTextBox.Document.ContentStart;
            sWriterRichTextBox.Focus();
            DataObject.AddPastingHandler(sWriterRichTextBox, new DataObjectPastingEventHandler(OnPaste));
        }

        void m_setClockTimer_Tick(object sender, EventArgs e)
        {
            TimeString = DateTime.Now.ToShortTimeString();
        }

        void m_wordCountTimer_Tick(object sender, EventArgs e)
        {
            int words = WordCount(sWriterRichTextBox.Document);

            if(words!=0)
                SetIndicator(wordCountIndicator, words.ToString());
            else
                SetIndicator(wordCountIndicator, "");
        }


        void m_searchBoxVanisher_Tick(object sender, EventArgs e)
        {
            SetIndicator(searchTextBox, "");
        }

        void m_autosaveTimer_Tick(object sender, EventArgs e)
        {
            //show that autosave ist carried out - is automatically deleted by Save...

            SetIndicator(changeIndicator, "A");

            if (TextChanged && !string.IsNullOrEmpty(theFileName))
            {
                string ext = System.IO.Path.GetExtension(theFileName);
                switch (ext)
                {
                    case ".txt":
                        SaveToFile(theFileName, DataFormats.Text);
                        break;
                    case ".rtf":
                        SaveToFile(theFileName, DataFormats.Rtf);
                        break;
                    case ".xaml":
                        SaveToFile(theFileName, DataFormats.Xaml);
                        break;
                    default:
                        ShowError("Autosave fehlgeschlagen. Kann nicht im Format <" + ext + "> speichern.");
                        break;
                }
            }

        }


        /// <summary>
        /// The user is rolling the mouse wheel over the text area
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Handle_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (StrgIsPressed())
            {
                ZoomFaktor += (float)e.Delta / 1000f;
                ApplyZoomFactor();
                e.Handled = true;
            }
        }

        private void SetBold_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextSelection sel = sWriterRichTextBox.Selection;
            if (sel.GetPropertyValue(RichTextBox.FontWeightProperty).ToString() != "Bold")
                sel.ApplyPropertyValue(RichTextBox.FontWeightProperty, "Bold");
            else
                sel.ApplyPropertyValue(RichTextBox.FontWeightProperty, "Normal");
        }

        private void SetBold_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !sWriterRichTextBox.Selection.IsEmpty;
        }

        private void SetItalic_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextSelection sel = sWriterRichTextBox.Selection;

            if (sel.GetPropertyValue(RichTextBox.FontStyleProperty).ToString() != "Italic")
                sel.ApplyPropertyValue(RichTextBox.FontStyleProperty, "Italic");
            else
                sel.ApplyPropertyValue(RichTextBox.FontStyleProperty, "Normal");
        }

        private void SetItalic_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !sWriterRichTextBox.Selection.IsEmpty;
        }

        
        private void OpenFromFile(string fileName, string docFormat)
        {
            FileStream fstream = null;

            try
            {
                fstream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                TextRange range = new TextRange(sWriterRichTextBox.Document.ContentStart,
                    sWriterRichTextBox.Document.ContentEnd);
                range.Load(fstream, docFormat);

                if (!Properties.Settings.Default.KeepTextColorsInRtf)
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, BrushFromCol(Properties.Settings.Default.TextColor));
                }

                //set my favourite fontfamily to the whole text whatever it was before
                range.ApplyPropertyValue(TextElement.FontFamilyProperty, 
                    FontFamilyFromFont(Properties.Settings.Default.WriterFont));

                theFileName = fileName;
                TextChanged = false;
                sWriterRichTextBox.CaretPosition = sWriterRichTextBox.Document.ContentStart;
            }
            finally
            {
                if (fstream != null)
                    fstream.Close();
            }
        }

        /// <summary>
        /// Creates a clone of the orginal document
        /// </summary>
        /// <param name="indoc"></param>
        /// <returns></returns>
        protected FlowDocument CloneDoc(FlowDocument indoc)
        {
            TextRange sourceRange = new TextRange(indoc.ContentStart,
               indoc.ContentEnd);

            MemoryStream stream = new MemoryStream();
            sourceRange.Save(stream, DataFormats.Xaml);


            // Clone the source document's content into a new FlowDocument.
            FlowDocument fdCopy = new FlowDocument();
            TextRange copyRange = new TextRange(fdCopy.ContentStart,
                fdCopy.ContentEnd);

            copyRange.Load(stream, DataFormats.Xaml);

            return fdCopy;
        }


        /// <summary>
        /// Apply the zoom factor to any relevant settings
        /// </summary>
        private void ApplyZoomFactor()
        {
            if (ZoomFaktor < 0.7f)
                ZoomFaktor = 0.7f;

            if (ZoomFaktor > 4f)
                ZoomFaktor = 4f;

            sWriterRichTextBox.LayoutTransform = new ScaleTransform(ZoomFaktor,
                ZoomFaktor,
                sWriterRichTextBox.Width / 2.0f,
                0.0);

            m_linesPerPage = (int)(m_initialLinesPerPage / ZoomFaktor);
            if (m_linesPerPage < 1)
                m_linesPerPage = 1;
        }


        /// <summary>
        /// Initialize everything from the properties
        /// </summary>
        private void InitFromProps()
        {
            sWriterRichTextBox.Background = BrushFromCol(Properties.Settings.Default.BackgroundColor);
            sWriterRichTextBox.Foreground = BrushFromCol(Properties.Settings.Default.TextColor);
            sWriterRichTextBox.FontFamily = FontFamilyFromFont(Properties.Settings.Default.WriterFont);
            sWriterRichTextBox.FontSize = FontSizeFromFont(Properties.Settings.Default.WriterFont);
            sWriterRichTextBox.SpellCheck.IsEnabled = Properties.Settings.Default.SpellCheckActive;
            sWriterRichTextBox.SelectionBrush = BrushFromCol(Properties.Settings.Default.SelectionColor);
            TextRange wholeRange = new TextRange(sWriterRichTextBox.Document.ContentStart,
                sWriterRichTextBox.Document.ContentEnd);

            wholeRange.ApplyPropertyValue(TextElement.FontFamilyProperty, sWriterRichTextBox.FontFamily);
            wholeRange.ApplyPropertyValue(TextElement.ForegroundProperty, BrushFromCol(Properties.Settings.Default.TextColor));

            searchTextBox.SelectionBrush = BrushFromCol(Properties.Settings.Default.SelectionColor);
            InitChangeIndicatorFromProps(searchTextBox);

            InitChangeIndicatorFromProps(changeIndicator);
            InitChangeIndicatorFromProps(zoomIndicator);
            InitChangeIndicatorFromProps(wordCountIndicator);
            InitChangeIndicatorFromProps(overwriteIndicator);
            InitChangeIndicatorFromProps(clockIndicator);

            ZoomFaktor = Properties.Settings.Default.InitialZoom;

           

            ApplyZoomFactor();
        }


        private void InitChangeIndicatorFromProps(TextBox tb)
        {
            tb.FontFamily = FontFamilyFromFont(Properties.Settings.Default.WriterFont);
            tb.Foreground = BrushFromCol(Properties.Settings.Default.TextColor);
            tb.Background = BrushFromCol(Properties.Settings.Default.BackgroundColor);
            tb.Visibility = System.Windows.Visibility.Collapsed;
            if (tb.Parent is Border)
            {
                Border bor = tb.Parent as Border;
                bor.BorderBrush = BrushFromCol(Properties.Settings.Default.TextColor);
                bor.BorderThickness = new Thickness(0);
            }
        }

        private void InitChangeIndicatorFromProps(TextBlock indicator)
        {
            indicator.FontFamily = FontFamilyFromFont(Properties.Settings.Default.WriterFont);
            indicator.Foreground = BrushFromCol(Properties.Settings.Default.TextColor);

            if (indicator.Parent is Border)
            {
                Border bor = indicator.Parent as Border;
                bor.BorderBrush = BrushFromCol(Properties.Settings.Default.TextColor);
            }
        }

        /// <summary>
        /// Derives and returns the font size from a font (System.DrawingFont)
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        private double FontSizeFromFont(System.Drawing.Font font)
        {
            return font.Size;
        }


        /// <summary>
        /// Derives and returns the font family from a system.drawing.font - takne from the properties
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static System.Windows.Media.FontFamily FontFamilyFromFont(System.Drawing.Font font)
        {
            System.Windows.Media.FontFamily ff = new FontFamily(font.FontFamily.Name);

            return ff;
        }

        /// <summary>
        /// Create a color brush from a drawing color - which has been read from the props
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static SolidColorBrush BrushFromCol(System.Drawing.Color color)
        {
            Color col = new Color();
            col.R = color.R;
            col.B = color.B;
            col.G = color.G;
            col.A = color.A;

            return new SolidColorBrush(col);
        }


        /// <summary>
        /// Create a color brush from a drawing color - which has been read from the props
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static SolidColorBrush BrushFromCol(System.Windows.Media.Color color)
        {
            Color col = new Color();
            col.R = color.R;
            col.B = color.B;
            col.G = color.G;
            col.A = color.A;

            return new SolidColorBrush(col);
        }



        /// <summary>
        /// Save the contents of the rich text box to file with the given file name
        /// in the given format
        /// </summary>
        /// <param name="fileName">The full ptah of the file where to save the text</param>
        /// <param name="format">The format like from DataFormats.XXXX</param>
        private void SaveToFile(string fileName, string format)
        {
            TextRange range = null;
            FileStream fstr = null;

            try
            {
                fstr = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            }
            catch
            {
                ShowError("Die Datei konnte nicht geöffnet werden. Möglicherweise wird sie von einem anderen Prozess gesperrt.");

                return;
            }

            try
            {
                FlowDocument doc = CloneDoc(sWriterRichTextBox.Document);
                range = new TextRange(doc.ContentStart,
                           doc.ContentEnd);

                if (!Properties.Settings.Default.KeepTextColorsInRtf)
                {
                    range.ApplyPropertyValue(TextElement.ForegroundProperty, "Black");
                }

                range.Save(fstr, format);
                TextChanged = false;
            }
            catch
            {
                ShowError("Das Schreiben der Datei " + fileName + " ist nicht vollständig erfolgt."
                    + " Möglicherweise ist der Speicherplatz erschöpft.");
            }
            finally
            {
                if (fstr != null)
                    fstr.Close();

                theFileName = fileName;
            }
        }


        /// <summary>
        /// Detect if the left or right ctr-key is currently down. For use outside of keyboard events
        /// </summary>
        /// <returns>Returns true when tle left or right key is currently pressed. False otherwise.</returns>
        private bool StrgIsPressed()
        {
            return (((Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down)
                || ((Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down));

        }

        private void Settings_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.Owner = this;
            if (sw.ShowDialog() == true)
            {
                Properties.Settings.Default.Save();

                InitFromProps();

                if (Properties.Settings.Default.AutoSavePeriod < 30)
                    ActivateAutoSave(false);

                m_autosaveTimer.Interval = new TimeSpan(0, 0, Properties.Settings.Default.AutoSavePeriod);
            }
        }

        private void Search_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Search_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (LastSearches.Count == 0)
                SetIndicator(searchTextBox, "Suche hier eingeben");
            else
                SetIndicator(searchTextBox, LastSearches.Last());

            searchTextBox.Focus();
        }

        /// <summary>
        /// Handle special keypresses in the search text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender != searchTextBox)
                return;

            switch (e.Key)
            {
                case Key.Enter:
                    if (string.IsNullOrEmpty(searchTextBox.Text))
                        return;

                    PerformSearch();
                    break;

                case Key.Up:
                    if (m_lastSearchListIndex > 0)
                    {
                        m_lastSearchListIndex--;
                        searchTextBox.Text = m_lastSearches[m_lastSearchListIndex];
                    }
                    break;

                case Key.Down:
                    if (m_lastSearchListIndex < (m_lastSearches.Count - 1))
                    {
                        m_lastSearchListIndex++;
                        searchTextBox.Text = m_lastSearches[m_lastSearchListIndex];
                    }
                    break;

            }

        }

        /// <summary>
        /// Do a search with the text in searchTextBox. Update SearchList when a new search key has been entered
        /// </summary>
        private void PerformSearch()
        {
            m_searchBoxVanisher.Stop();
            
            try
            {
                char[] trimc = { '\r', '\n' };
                string searchFor = searchTextBox.Text.Trim(trimc);

                if (frepHelper == null)
                    frepHelper = new FindReplaceHelper(sWriterRichTextBox.Document);

                if (LastSearches.Count == 0)
                {
                    LastSearches.Add(searchFor);
                    m_lastSearchListIndex = LastSearches.Count - 1;
                    frepHelper.CurrentPosition = sWriterRichTextBox.Selection.End.GetInsertionPosition(LogicalDirection.Forward);
                }
                else if ((LastSearches.Count > 0) && (LastSearches.Last() != searchFor))
                {
                    LastSearches.Add(searchFor);
                    m_lastSearchListIndex = LastSearches.Count - 1;
                    frepHelper.CurrentPosition = sWriterRichTextBox.Selection.End.GetInsertionPosition(LogicalDirection.Forward);
                }

                searchTextBox.Text = searchFor;
                searchTextBox.CaretIndex = searchFor.Length;

                TextRange found = frepHelper.FindNext(searchFor, SWriter.FindOptions.None);

                if (found != null)
                {
                    SelectPortion(found);
                }
                else if ((m_startOfSearch != null) && (frepHelper.CurrentPosition.CompareTo(m_startOfSearch.End) > 0))
                {
                    frepHelper.CurrentPosition = sWriterRichTextBox.Document.ContentStart;
                    found = frepHelper.FindNext(searchFor, SWriter.FindOptions.None);
                    if (found != null)
                    {
                        SelectPortion(found);
                    }
                    else
                        ShowMessage("Der Suchbegriff wurde nicht gefunden");
                        
                }
                else
                    ShowMessage("Der Suchbegriff wurde nicht gefunden");
            }
            finally
            {
                m_searchBoxVanisher.Start(); //start vanisher from beginning
            }
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(this,
                message, 
                "Hinweis",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
                            
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this,
                message,
                "Fehler",
                MessageBoxButton.OK, 
                MessageBoxImage.Error);

        }

        /// <summary>
        /// Select a part of the text in the rich text box.
        /// </summary>
        /// <param name="selrange">The range of text to select</param>
        private void SelectPortion(TextRange selrange)
        {
            //When we have a new string start a search from the current cursor position
            if (m_startOfSearch != null)
            {
                int comp = selrange.Start.CompareTo(m_startOfSearch.Start);

                if (selrange.Text.ToUpper() != m_startOfSearch.Text.ToUpper())
                {
                    m_startOfSearch = selrange;
                    lastSearchedFor = m_startOfSearch.Text;
                }
                else if (comp==0)
                {
                    //in an old search when we reach the first find again we inform the user about that
                    ShowMessage("Die Suche hat ihren Ausgangspunkt wieder erreicht");
                }
            }
            else
                m_startOfSearch = selrange;

            //now mark the found string in the rich text box
            TextRange docSelRange = new TextRange(sWriterRichTextBox.Document.ContentStart,
                sWriterRichTextBox.Document.ContentEnd);

            docSelRange = sWriterRichTextBox.Selection;
            docSelRange.Select(selrange.Start, selrange.End);

        }

        private void sWriterRichTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Insert)
            {
                InOverwrite = !InOverwrite;
            }
        }

        /// <summary>
        /// Event routinge called when the user pastes something into the rich text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.SourceDataObject.GetDataPresent(DataFormats.Rtf, true)) return;
            var rtf = e.SourceDataObject.GetData(DataFormats.Rtf) as string;

            rtf = StripColorsFromRtf(rtf);
            rtf = StripFontsFromRtf(rtf);

            e.DataObject = new DataObject(DataFormats.Rtf, rtf);
        }

        private string StripColorsFromRtf(string rtf)
        {
            int i = 1; // colors start with /cf1
            int oldLen = 0;

            while (oldLen != rtf.Length)
            {
                oldLen = rtf.Length;
                rtf = rtf.Replace("\\cf" + i, "");
                i++;
            }

            return rtf;
        }

        private string StripFontsFromRtf(string rtf)
        {
            int i = 1; // colors start with /cf1
            int oldLen = 0;

            while (oldLen != rtf.Length)
            {
                oldLen = rtf.Length;
                rtf = rtf.Replace("\\f" + i, "");
                i++;
            }

            return rtf;
        }

        /// <summary>
        /// Handle KeyUp and KeyDow here because the rich text box in the current constellation only goes to end and
        /// top of document on PageUp and PageDown key presses. Here I rather go up or down by a fixed amount of lines
        /// which also ist not so nice. To make it better I modify that fixed amount with the zoom factor for the text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sWriterRichTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextPointer ntp;

            switch (e.Key)
            {
                case Key.PageDown:
                    
                    ntp = sWriterRichTextBox.CaretPosition.GetLineStartPosition(m_linesPerPage);
                    if (ntp == null)
                        ntp = sWriterRichTextBox.Document.ContentEnd;

                    sWriterRichTextBox.CaretPosition = ntp;

                    e.Handled = true;
                    break;
                case Key.PageUp:
                    ntp = sWriterRichTextBox.CaretPosition.GetLineStartPosition(-m_linesPerPage);
                    if (ntp == null)
                        ntp = sWriterRichTextBox.Document.ContentStart;

                    sWriterRichTextBox.CaretPosition = ntp;

                    e.Handled = true;
                    break;
            }
        }





        public EventHandler m_clockTimer_Tick { get; set; }
    }
}
