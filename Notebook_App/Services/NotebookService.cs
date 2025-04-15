using Notebook_App.Model;
using Notebook_App.ViewModel;
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
    public class NotebookService
    {
        // Method for creating new notebook
        public static void CreateNotebook()
        {
            Notebook newNotebook = new Notebook()
            {
                Name = "Notebook",
                UserId = App.UserID
            };

            DatabaseHelper.Insert(newNotebook);
        }

        // Method to delete a notebook and notes corresponding with notebook
        public static void DeleteNotebook(Notebook notebook)
        {
            var notes = DatabaseHelper.Read<Note>().Where(n => n.NotebookId == notebook.Id).ToList();
            foreach (var note in notes)
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
            DatabaseHelper.Delete(notebook);
        }
    }
}
