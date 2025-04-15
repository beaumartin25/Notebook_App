using Notebook_App.Model;
using Notebook_App.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Notebook_App.Services
{
    public class NoteService
    {
        // Method for creating new note
        public static Note CreateNote(int notebookId)
        {
            var newNote = new Note
            {
                NotebookId = notebookId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = "New Note"
            };

            DatabaseHelper.Insert(newNote);
            return newNote;
        }

        // Method to delete the note
        public static void DeleteNote(Note note)
        {

            if (!string.IsNullOrEmpty(note.FileLocation) && File.Exists(note.FileLocation))
            {
                try
                {
                    File.Delete(note.FileLocation);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete file: {note.FileLocation}\nError: {ex.Message}");
                }
            }
            DatabaseHelper.Delete(note);
        }

    }
}
