using SimpleColorFile.Utils;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimpleColorFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Color _currentColor;

        private string _fileNameTemplate;
        private string _fileContentTemplate;

        public MainWindow()
        {
            InitializeComponent();

            _fileNameTemplate = (string)Application.Current.FindResource("fileNameTemplate");
            _fileContentTemplate = (string)Application.Current.FindResource("fileContentTemplate");
        }

        private void ColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Color color;

            try
            {
                color = (Color)ColorConverter.ConvertFromString(ColorTextBox.Text);
            }
            catch (FormatException)
            {
                BadColor();
                return;
            }

            GoodColor(color);
        }

        private void BadColor()
        {
            if (SavePanel != null)
            {
                CurrentColorBrush.Color = Colors.Transparent;
                SavePanel.IsEnabled = false;
            }
        }

        private void GoodColor(Color color)
        {
            _currentColor = color;

            CurrentColorBrush.Color = _currentColor;

            if (SavePanel != null)
            {
                SavePanel.IsEnabled = true;
            }

            if (FileNameTextBox?.IsEnabled == false)
            {
                UpdateAutoFileName();
            }
        }

        private void UpdateAutoFileName()
        {
            FileNameTextBox.Text = string.Format(_fileNameTemplate, ColorTextBox.Text);
        }

        /// <summary>
        /// Pick Color Hyperlink -> Click:
        /// Opens a Color dialog box (from Windows Forms) to allow the user to visually pick a custom color.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void PickColorLinkButton_Click(object sender, RoutedEventArgs e)
        {
            string colorString = CommonDialogs.ShowColorDialog(_currentColor);
            if (!string.IsNullOrEmpty(colorString))
            {
                ColorTextBox.Text = colorString;
            }
        }

        /// <summary>
        /// Browse For Folder Hyperlink -> Click:
        /// Opens a Folder Browser dialog box (from Windows Forms) to allow the user to visually select a folder path.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BrowseFolderLinkButton_Click(object sender, RoutedEventArgs e)
        {
            string path = CommonDialogs.ShowFolderBrowserDialog(FolderTextBox.Text);
            if (!string.IsNullOrEmpty(path))
            {
                FolderTextBox.Text = path;
            }
        }

        private void CustomFileNameCheckBox_Unchecked(object sender, RoutedEventArgs e) => UpdateAutoFileName();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string contents = string.Format(_fileContentTemplate, ColorTextBox.Text);
            string path = Path.Combine(FolderTextBox.Text, FileNameTextBox.Text);
            File.WriteAllText(path, contents);
        }

        private void EditAutoLinkButton_Click(object sender, RoutedEventArgs e)
        {
            FileNameTextBox.IsEnabled = !FileNameTextBox.IsEnabled;
            EditAutoLinkButton.Content = Application.Current.FindResource(FileNameTextBox.IsEnabled ? "AutoLinkButtonContent" : "EditLinkButtonContent");
        }
    }
}
