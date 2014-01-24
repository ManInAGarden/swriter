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
    /// Interaktionslogik für SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadFontListToCombo();
           
            fontSizeTextBox.Text = Properties.Settings.Default.WriterFont.SizeInPoints.ToString();
            textColorTextBox.Foreground = MainWindow.BrushFromCol(Properties.Settings.Default.TextColor);
            textColorTextBox.Background = MainWindow.BrushFromCol(Properties.Settings.Default.BackgroundColor);
            textColorTextBox.FontFamily = MainWindow.FontFamilyFromFont(Properties.Settings.Default.WriterFont);

            initialZoomTextBox.Text = (Properties.Settings.Default.InitialZoom * 100f).ToString();

            keepColorsCheckBox.IsChecked = Properties.Settings.Default.KeepTextColorsInRtf;
            spellCheckerCheckBox.IsChecked = Properties.Settings.Default.SpellCheckActive;
            autosavePeriodTextBox.Text = Properties.Settings.Default.AutoSavePeriod.ToString();
        }

        private void LoadFontListToCombo()
        {
            foreach(FontFamily ff in Fonts.SystemFontFamilies)
            {
                fontFamilyComboBox.Items.Add(ff.Source);
            }

            fontFamilyComboBox.SelectedValue = Properties.Settings.Default.WriterFont.Name;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

            this.Close();
        }

        private void oKButton_Click(object sender, RoutedEventArgs e)
        {
            float fontSize;
            
            if(!float.TryParse(fontSizeTextBox.Text, out fontSize))
                fontSize=12.0f;

            
            if(fontFamilyComboBox.SelectedItem!=null)
            {
                string fontName = fontFamilyComboBox.SelectedValue as string;
            
                System.Drawing.Font newFont = new System.Drawing.Font(fontName,
                    fontSize);

                Properties.Settings.Default.WriterFont = newFont;
            }

            float initialZoom;
            if(!float.TryParse(initialZoomTextBox.Text, out initialZoom))
                initialZoom = 100f;

            if(initialZoom<70f) 
                initialZoom = 70f;

            if(initialZoom>400)
                initialZoom = 400f;

            Properties.Settings.Default.InitialZoom = initialZoom /100f;

            Properties.Settings.Default.KeepTextColorsInRtf = keepColorsCheckBox.IsChecked==true;
            Properties.Settings.Default.SpellCheckActive = spellCheckerCheckBox.IsChecked==true;

            int autos;

            if(!Int32.TryParse(autosavePeriodTextBox.Text, out autos))
                autos = 60;

            if (autos < 30)
            {
                autos = 0;
                MessageBox.Show("Bei Zeiten von weniger als 30 Sekunden wird Autosave komplett abgeschaltet");
            }

            Properties.Settings.Default.AutoSavePeriod = autos;

            Properties.Settings.Default.BackgroundColor = DrawingColorFromBrush(textColorTextBox.Background);
            Properties.Settings.Default.TextColor = DrawingColorFromBrush(textColorTextBox.Foreground);

            this.DialogResult = true;

            this.Close();
        }

        private System.Drawing.Color DrawingColorFromBrush(Brush brush)
        {

            SolidColorBrush sbrush = brush as SolidColorBrush;

            System.Drawing.Color answ = System.Drawing.Color.FromArgb(sbrush.Color.A,
                sbrush.Color.R,
                sbrush.Color.G,
                sbrush.Color.B);

            return answ;
        }

        private void pickTextColorButton_Click(object sender, RoutedEventArgs e)
        {
            WPFColorPickerLib.ColorDialog cdial = new WPFColorPickerLib.ColorDialog(ColorFromBrush(textColorTextBox.Foreground));

            if (cdial.ShowDialog() == true)
            {
                textColorTextBox.Foreground = MainWindow.BrushFromCol(cdial.SelectedColor);
            }
        }

        private Color ColorFromBrush(Brush brush)
        {
            SolidColorBrush scol = brush as SolidColorBrush;

            return scol.Color;
        }

        private System.Windows.Media.Color MediaColor(System.Drawing.Color incol)
        {
            return Color.FromArgb(incol.A,
                incol.R,
                incol.G,
                incol.B);
        }

        private void pickTextBackgroundBUtton_Click(object sender, RoutedEventArgs e)
        {
            WPFColorPickerLib.ColorDialog cdial = new WPFColorPickerLib.ColorDialog(ColorFromBrush(textColorTextBox.Background));

            if (cdial.ShowDialog() == true)
            {
                textColorTextBox.Background = MainWindow.BrushFromCol(cdial.SelectedColor);
            }
        }

        private void fontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fontFamilyComboBox.SelectedItem == null)
                return;

            string ffName = fontFamilyComboBox.SelectedItem as string;
            if (ffName == null)
                return;

            textColorTextBox.FontFamily = new FontFamily(ffName);
        }

        private void fontSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double size;
            
            if (string.IsNullOrEmpty(fontSizeTextBox.Text))
                return;

            if (Double.TryParse(fontSizeTextBox.Text, out size))
            {
                if((size>5) && (size<=50))
                    textColorTextBox.FontSize = size;
            }


        }
    }
}
