using SimpleColorFile.Utils;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SimpleColorFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The current color.
        /// </summary>
        private Color _currentColor;

        /// <summary>
        /// The Simple Color File name template.
        /// </summary>
        private string _fileNameTemplate;

        /// <summary>
        /// The Simple Color File content template.
        /// </summary>
        private string _fileContentTemplate;

        // *********************************************************************
        // Initialization
        // *********************************************************************

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _fileNameTemplate = (string)Application.Current.FindResource("fileNameTemplate");
            _fileContentTemplate = (string)Application.Current.FindResource("fileContentTemplate");
        }

        // *********************************************************************
        // Functionality Helper Methods
        // *********************************************************************

        /// <summary>
        /// Updates the UI to signal that an invalid color has been entered.
        /// </summary>
        private void BadColor()
        {
            if (SavePanel != null)
            {
                CurrentColorBrush.Color = Colors.Transparent;
                SavePanel.IsEnabled = false;
            }
        }

        /// <summary>
        /// Updates the UI with the new valid color that has been entered.
        /// </summary>
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

        /// <summary>
        /// Updates the file name based on the color value.
        /// </summary>
        private void UpdateAutoFileName()
        {
            FileNameTextBox.Text = string.Format(_fileNameTemplate, ColorTextBox.Text);
        }

        // *********************************************************************
        // User Interface Events
        // *********************************************************************

        /// <summary>
        /// Border Parent Content Control -> MouseDoubleClick:
        /// Generates and applies a random color when the user double clicks the (colored) border.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void BorderParentContentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ColorTextBox.Text = ColorUtils.ToHtml(ColorUtils.RandomColor());
        }

        /// <summary>
        /// Color Text Box -> TextChanged:
        /// Checks and updates the new color typed by the user.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
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

        /// <summary>
        /// Pick Color Link Button -> Click:
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
        /// Browse For Folder Link Button -> Click:
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

        /// <summary>
        /// Edit/Auto File Name Link Button -> Click:
        /// Enables the File Name text box to let the user enter a custom file name, or disables it to autogenerate
        /// the file name.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void EditAutoFileNameLinkButton_Click(object sender, RoutedEventArgs e)
        {
            FileNameTextBox.IsEnabled = !FileNameTextBox.IsEnabled;
            EditAutoLinkButton.Content = Application.Current.FindResource(
                FileNameTextBox.IsEnabled ? "AutoFileNameLinkButtonContent" : "EditFileNameLinkButtonContent");
            if (!FileNameTextBox.IsEnabled) UpdateAutoFileName();
        }

        /// <summary>
        /// Save Button -> Click:
        /// Saves the current color to the specified Simple Color File.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string contents = string.Format(_fileContentTemplate, ColorTextBox.Text);
            string path = Path.Combine(FolderTextBox.Text, FileNameTextBox.Text);
            File.WriteAllText(path, contents);
        }

    }
}
