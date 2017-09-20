//----------------------------------------------------------------------------
//
// <copyright file="MainWindow.xaml.cs" company="Appliberated">
// Copyright (c) 2017 Appliberated
// https://appliberated.com/
// Licensed under the Apache License, Version 2.0. See LICENSE file in the project root for full license information.
// </copyright>
//
// Description: The main window of the Simple Color File application.
//
//---------------------------------------------------------------------------

namespace SimpleColorFile
{
    using System;
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

            this.fileNameTemplate = (string)Application.Current.FindResource("fileNameTemplate");
            this.fileContentTemplate = (string)Application.Current.FindResource("fileContentTemplate");
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

        private void DoSaveColorFile(string fileName)
        {
            try
            {
                string contents = string.Format(this.fileContentTemplate, this.ColorTextBox.Text);
                File.WriteAllText(fileName, contents);
                this.ShowToast("Color saved");
            }
            catch (Exception ex)
            {
                this.ShowToast(ex.Message);
            }
        }

        private void ShowToast(string message)
        {
            this.ToastTextBlock.Text = message;

            Storyboard sb = this.FindResource("ToastStoryboard") as Storyboard;
            sb.Begin(this.ToastTextBlock);
        }

        // *********************************************************************
        // User Interface Events
        // *********************************************************************

        /// <summary>
        /// Border -> MouseLeftButtonDown:
        /// Generates and applies a random color when the user double clicks the (colored) border.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="e">The event data.</param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                this.ColorTextBox.Text = ColorUtils.ToHtml(ColorUtils.RandomColor());
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
            string colorString = CommonDialogs.ShowColorDialog(this.currentColor);
            if (!string.IsNullOrEmpty(colorString))
            {
                this.ColorTextBox.Text = colorString;
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
                DefaultExt = ".html",
                FileName = string.Format(this.fileNameTemplate, this.ColorTextBox.Text),
                Filter = "Simple Color Files (.html)|*.html"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                // Save the color to the Simple Color File selected by the user in the Save File Dialog
                this.DoSaveColorFile(saveFileDialog.FileName);
            }
        }
    }
}
