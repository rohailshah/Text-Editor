// Project:     Rohail's Text Editor (2021 C# version)
// Author:      Rohail Shah
// Start Date:  March 22, 2021
// Last Date:   March 28, 2021
// Description:
//  This application will allow very basic text editor
//  functionality including typing text, copying, pasting,
//  and some basic file management.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class formTextEditor : Form
    {

        // This is the filepath of the active files, if applicable.
        string filePath = String.Empty;

        // This is a boolean variable that tracks if the textbox's contents have been changed.
        bool isUnchanged = true;

        public formTextEditor()
        {
            InitializeComponent();
        }

        #region "Event Handlers"

        /// <summary>
        /// Activates when any text has changed from the textbox and turns isUnchanged boolean to false and 
        /// also updates the title with an asterisk to identify the change in text.
        /// </summary>
        private void TextModified(object sender, EventArgs e)
        {
            // Turns isUnchanged boolean variable to false showing the textbox's contents have been changed.
            isUnchanged = false;

            // Updates the title bar text with an asterisk to identify the change in the textbox's contents.
            UpdateTitle();
        }

        /// <summary>
        /// Clears the textbox, voids the current filePath and updates the title.
        /// </summary>
        private void FileNew(object sender, EventArgs e)
        {
            // Prompts for confirmation clearing the current text.
            if (MessageBox.Show("This will clear the current file. Would you like to proceed?", "Confirm New File", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Clears the textbox field.
                textBoxEditor.Clear();

                // Clears filepath of the active files.
                filePath = String.Empty;

                // Turns isUnchanged boolean variable to false showing the textbox's contents have been unchanged.
                isUnchanged = true;

                // Update the form's title text to delete the open file path if one exists and
                // delete the asterisk identifying change in the text.
                UpdateTitle();
            }
        }

        /// <summary>
        /// Clears the textbox, opens an existing file and updates the title.
        /// </summary>
        private void FileOpen(object sender, EventArgs e)
        {
            // If the textbox's contents have been changed...
            if (!isUnchanged)
            {
                // Prompt a messagebox that asks if the user wants to abandon changes.
                if (MessageBox.Show("This will clear the current file. Would you like to proceed?", "Confirm Open", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // If yes, opens a text file and fills the textbox with its contents, sets isUnchanged boolean to true,
                    // and updates the title with the filename chosen.
                    OpenTextFile();
                }
            }
            else
            {
                // Otherwise it just opens a text file, fills the textbox with its contents, sets isUnchanged boolean to true,
                // and updates the title with the filename chosen.
                OpenTextFile();
            }
        }

        /// <summary>
        /// Saves the file if the path is known, or calls "Save As" if it is not known.
        /// </summary>
        private void FileSave(object sender, EventArgs e)
        {
            // If there is not already a filepath...
            if (filePath == String.Empty)
            {
                // Then call the Save As... event handler!
                FileSaveAs(sender, e);
            }
            // If there IS a filepath...
            else
            {
                // Then save it.
                SaveTextFile(filePath);

                // Set isUnchanged boolean to true.
                isUnchanged = true;

                // Update the title with the filename chosen.
                UpdateTitle();
            }
        }

        /// <summary>
        /// Open a save dialog and save the file to the location chosen by the user.
        /// </summary>
        private void FileSaveAs(object sender, EventArgs e)
        {
            // Create a save dialog class and variable.
            SaveFileDialog saveDialog = new SaveFileDialog();

            // Sets the current file name filter string, which determines the choices that appear in the
            // "Save as file type" box in the dialog box.
            saveDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            // Ignore further processing if the user clicks "Cancel".
            if(saveDialog.ShowDialog() == DialogResult.OK)
            {
                // Otherwise set filePath to the file name chosen.
                filePath = saveDialog.FileName;

                // Save the file with the file name chosen. 
                SaveTextFile(filePath);

                // Set isUnchanged boolean to true.
                isUnchanged = true;

                // Update the title with the filename chosen.
                UpdateTitle();
            }
        }

        /// <summary>
        /// Closes the application after confirming with user.
        /// </summary>
        private void FileClose(object sender, EventArgs e)
        {
            // // Prompt a messagebox that asks if the user wants to abandon changes.
            if (MessageBox.Show("This will close the current file. Would you like to proceed?", "Confirm Close", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // If yes, clear the text box.
                textBoxEditor.Clear();

                // Clears filepath of the active files.
                filePath = String.Empty;

                // Set isUnchanged boolean to true.
                isUnchanged = true;

                // Update the title with no filename.
                UpdateTitle();
            }
        }

        /// <summary>
        /// Calls the form closing event.
        /// </summary>
        private void FileExit(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Copies the selected text into clipboard memory and deletes the selected text.
        /// </summary>
        private void EditCut(object sender, EventArgs e)
        {
            // Calls the clipboard function to set the selected text into memory.
            Clipboard.SetData(DataFormats.Text, (Object)textBoxEditor.SelectedText);

            // Deletes the selected text.
            textBoxEditor.SelectedText = "";
        }

        /// <summary>
        /// Copies the selected text into clipboard memory.
        /// </summary>
        private void EditCopy(object sender, EventArgs e)
        {
            // Calls the clipboard function to set the selected text into memory.
            Clipboard.SetData(DataFormats.Text, (Object)textBoxEditor.SelectedText);
        }

        /// <summary>
        /// Pastes text in clipboard memory onto the textbox where selected.
        /// </summary>
        private void EditPaste(object sender, EventArgs e)
        {
            // If the clipboard contains text in memory...
            if (Clipboard.ContainsText())
            {
                // Create a temporary variable to hold the text in memory.
                string clipboardData;

                // Set the text in clipboard memory into the clipboardData variable.
                clipboardData = Clipboard.GetText();

                // Pastes text in clipboardData variable onto the textbox where selected.
                textBoxEditor.SelectedText = clipboardData;
            }
        }

        /// <summary>
        /// Select all text in the textbox.
        /// </summary>
        private void EditSelectAll(object sender, EventArgs e)
        {
            // Highlight all text in the textbox.
            textBoxEditor.SelectAll();
        }

        /// <summary>
        /// Displays a little message about this application.
        /// </summary>
        private void HelpAbout(object sender, EventArgs e)
        {
            // Displays the a popup box the name, author, for which course, month and year of the application.
            MessageBox.Show("Rohail's Text Editor\n" + "By Rohail Shah\n\n" + "For NETD 2202\n" + "March 2021", "About this Application");
        }

        /// <summary>
        /// When the form is closing, prompts the user if they want to abandon unsaved changes, 
        /// if no, it will cancel the form closing event.
        /// </summary>
        private void ExitingApp(object sender, FormClosingEventArgs e)
        {
            // While exiting the application, if the text in the textbox has changed since saving, or is a new instance...
            if (!isUnchanged)
            {
                // Prompt user if they want to abandon unsaved changes.
                if (MessageBox.Show("This will close the application regardless of changes. Would you like to proceed?", "Confirm Exit", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    //if no, cancel the form closing event.
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region "Other Functions"

        /// <summary>
        /// Update the form's title text to include an open file path if one exists 
        /// and update the title with an asterisk to identify the change in text.
        /// </summary>
        public void UpdateTitle()
        {
            // Sets the title to the name of the application.
            this.Text = "Rohail's Text Editor";

            // If there is a filepath set...
            if (filePath != String.Empty)
            {
                // Add the file url to the title of the application.
                this.Text += " - " + filePath;
            }

            // If the textbox's contents have been changed...
            if (!isUnchanged)
            {
                // Add an asterisk identifying the change in the textbox's contents.
                this.Text += "*";
            }
        }

        /// <summary>
        /// Opens a text file, fills the textbox with its contents, sets isUnchanged boolean to true,
        /// and updates the title with the filename chosen.
        /// </summary>
        public void OpenTextFile()
        {
            // Create an empty string variable called fileContent to hold contents of chosen file to open.
            string fileContent = string.Empty;

            // Create an open file dialog class and variable.
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Sets the initial directory displayed by the file dialog box.
                openFileDialog.InitialDirectory = "c:\\";

                // Sets the current file name filter string, which determines the choices that appear in the
                // "Save as file type" box in the dialog box.
                openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                // Sets the index of the filter currently selected in the file dialog box.
                // This by default shows the user only text files to open.
                // Set to 2 if user should see all files to open.
                openFileDialog.FilterIndex = 1;

                // Sets a value indicating whether the dialog box restores the directory to the previously
                // selected directory before closing.
                openFileDialog.RestoreDirectory = true;

                // Ignore further processing if the user clicks "Cancel".
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of specified file
                    filePath = openFileDialog.FileName;

                    // Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    // Create a StreamReader class and variable using the contents of the filestream variable.
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        // Read chosen file for text and put into fileContent variable to hold contents of chosen file.
                        fileContent = reader.ReadToEnd();
                    }
                }
            }

            // Fill read text in fileContent variable into the textbox.
            textBoxEditor.Text = fileContent;

            // Set isUnchanged boolean to true.
            isUnchanged = true;

            // Update the title to add the url the file chosen.
            UpdateTitle();
        }

        /// <summary>
        /// Save the current contents of the textbox to a text file.
        /// </summary>
        /// <param name="path">The path of the file to write to.</param>
        public void SaveTextFile(string path)
        {
            FileStream myFile = new FileStream(path, FileMode.Create, FileAccess.Write);

            StreamWriter writer = new StreamWriter(myFile);

            writer.Write(textBoxEditor.Text);

            writer.Close();
        }

        #endregion

    }
}
