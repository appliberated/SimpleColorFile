using System.Windows.Forms;
using System.Windows.Media;

namespace SimpleColorFile.Utils
{
    class CommonDialogs
    {
        /// <summary>
        /// Runs a common color dialog box with a default owner.
        /// </summary>
        /// <param name="initialColor">The initial color to be selected in the dialog box.</param>
        /// <returns>The selected color as a HTML color code, or an empty string if the dialog is cancelled.</returns>
        public static string ShowColorDialog(Color initialColor)
        {
            // Use the ColorDialog class from Windows Forms (WPF does not contain a standard color common dialog)
            ColorDialog colorDialog = new ColorDialog
            {
                FullOpen = true,
                Color = ColorUtils.ToSDColor(initialColor)
            };

            // Show the Color dialog and return any new selected color
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                return ColorUtils.ToHtml(colorDialog.Color);
            }

            return string.Empty;
        }

        /// <summary>
        /// Runs a common folder browser dialog box with a default owner.
        /// </summary>
        /// <param name="initialPath">The initial path to be selected in the dialog box.</param>
        /// <returns>The selected path, or an empty string if the dialog is cancelled.</returns>
        public static string ShowFolderBrowserDialog(string initialPath)
        {
            // Use the FolderBrowserDialog class from Windows Forms (WPF does not contain a standard folder browser
            // common dialog)
            var dialog = new FolderBrowserDialog
            {
                Description = (string)System.Windows.Application.Current.FindResource("selectFolder"),
                SelectedPath = initialPath
            };

            // Show the Folder Browser dialog and return any new selected path
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }

            return string.Empty;
        }
    }
}
