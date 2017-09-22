//----------------------------------------------------------------------------
//
// <copyright file="MainWindow.xaml.cs" company="Appliberated">
// Copyright (c) 2017 Appliberated
// https://appliberated.com
// Licensed under the MIT. See LICENSE file in the project root for full license information.
// </copyright>
//
//---------------------------------------------------------------------------

namespace SimpleColorFile
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using Microsoft.Win32;
    using SimpleColorFile.Utils;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The current color.
        /// </summary>
        private Color currentColor;

        /// <summary>
        /// The Simple Color File name template.
        /// </summary>
        private string fileNameTemplate;

        /// <summary>
        /// The Simple Color File content template.
        /// </summary>
        private string fileContentTemplate;

        // *********************************************************************
        // Initialization
        // *********************************************************************

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.fileNameTemplate = this.FindResource("fileNameTemplate") as string;
            this.fileContentTemplate = this.FindResource("fileContentTemplate") as string;

            this.FocusColorTextBox();
        }

        // *********************************************************************
        // Functionality Helper Methods
        // *********************************************************************

        /// <summary>
        /// Updates the UI to signal that an invalid color has been entered.
        /// </summary>
        private void BadColor()
        {
            if (this.SaveButton != null)
            {
                this.CurrentColorBrush.Color = Colors.Transparent;
                this.SaveButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Updates the UI with the new valid color that has been entered.
        /// </summary>
        /// <param name="color">The color value to set as the current color.</param>
        private void GoodColor(Color color)
        {
            this.currentColor = color;

            if (this.CurrentColorBrush != null)
            {
                this.CurrentColorBrush.Color = this.currentColor;
            }

            if (this.SaveButton != null)
            {
                this.SaveButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// The main functionality method: creates the specified Simple Color File and writes the specified color.
        /// </summary>
        /// <param name="colorString">The color string to write.</param>
        /// <param name="fileName">The Simple Color File to write to.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Catching a large number of exceptions and informing the user")]
        private void DoSaveColorFile(string colorString, string fileName)
        {
            try
            {
                string contents = string.Format(CultureInfo.InvariantCulture, this.fileContentTemplate, colorString);
                File.WriteAllText(fileName, contents);
                this.ShowToast(this.FindResource("ColorSavedToast") as string);
            }
            catch (Exception ex)
            {
                this.ShowToast(ex.Message);
            }
        }

        /// <summary>
        /// Shows a toast message.
        /// </summary>
        /// <param name="message">The message string to show.</param>
        private void ShowToast(string message)
        {
            this.ToastTextBlock.Text = message;

            Storyboard sb = this.FindResource("ToastStoryboard") as Storyboard;
            sb.Begin(this.ToastTextBlock);
        }

        /// <summary>
        /// Sets focus to the Color text box and moves the cursor at the end of the text.
        /// </summary>
        private void FocusColorTextBox()
        {
            if (this.ColorTextBox != null)
            {
                this.ColorTextBox.Focus();
                this.ColorTextBox.Select(this.ColorTextBox.Text.Length, 0);
            }
        }

        // *********************************************************************
        // User Interface Events
        // *********************************************************************

        /// <summary>
        /// Color Rectangle -> MouseLeftButtonDown:
        /// Generates and applies a random color when the user double clicks the colored rectangle.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void ColorRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.ColorTextBox.Text = ColorUtils.ToHtml(ColorUtils.RandomColor());
                this.FocusColorTextBox();
            }
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
                color = (Color)ColorConverter.ConvertFromString(this.ColorTextBox.Text);
            }
            catch (FormatException)
            {
                this.BadColor();
                return;
            }

            this.GoodColor(color);
        }

        /// <summary>
        /// Pick Color Link Button -> Click:
        /// Opens a Color dialog box (from Windows Forms) to allow the user to visually pick a custom color.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void PickColorLinkButton_Click(object sender, RoutedEventArgs e)
        {
            // Use the ColorDialog class from Windows Forms (WPF does not contain a standard color common dialog)
            using (var colorDialog = new System.Windows.Forms.ColorDialog())
            {
                colorDialog.FullOpen = true;
                colorDialog.Color = ColorUtils.ToSDColor(this.currentColor);

                // Show the Color dialog and apply any new selected color
                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.ColorTextBox.Text = ColorUtils.ToHtml(colorDialog.Color);
                    this.FocusColorTextBox();
                }
            }
        }

        /// <summary>
        /// Save Button -> Click:
        /// Saves the current color to the specified Simple Color File.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Although we are targeting .NET Framework 3.5, we will get the "modern" Save File Dialog
            // on Windows 10 because we are using <supportedRuntime version="v4.0" /> as the first line in app.config
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = this.FindResource("SaveFileDialogDefaultExt") as string,
                FileName = string.Format(CultureInfo.InvariantCulture, this.fileNameTemplate, this.ColorTextBox.Text),
                Filter = this.FindResource("SaveFileDialogFilter") as string
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                // Save the color to the Simple Color File selected by the user in the Save File Dialog
                this.DoSaveColorFile(this.ColorTextBox.Text, saveFileDialog.FileName);
                this.FocusColorTextBox();
            }
        }
    }
}
