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

using Microsoft.Win32;
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
            if (SaveButton != null)
            {
                CurrentColorBrush.Color = Colors.Transparent;
                SaveButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Updates the UI with the new valid color that has been entered.
        /// </summary>
        private void GoodColor(Color color)
        {
            _currentColor = color;

            if (CurrentColorBrush != null) CurrentColorBrush.Color = _currentColor;

            if (SaveButton != null)
            {
                SaveButton.IsEnabled = true;
            }
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
            if (e.ClickCount == 2) ColorTextBox.Text = ColorUtils.ToHtml(ColorUtils.RandomColor());
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
                FileName = string.Format(_fileNameTemplate, ColorTextBox.Text),
                Filter = "Simple Color Files (.html)|*.html"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                string contents = string.Format(_fileContentTemplate, ColorTextBox.Text);
                File.WriteAllText(saveFileDialog.FileName, contents);
            }
        }
    }
}
