using Notebook_App.Model;
using Notebook_App.ViewModel.Commands;
using Notebook_App.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Notebook_App.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
		// properties
        public event PropertyChangedEventHandler? PropertyChanged;
		public event EventHandler SelectedNoteChanged;

        public ObservableCollection<Notebook> Notebooks { get; set; }
        public ObservableCollection<Note> Notes { get; set; }


        private Notebook selectedNotebook;
		public Notebook SelectedNotebook
		{
			get { return selectedNotebook; }
			set
			{
				selectedNotebook = value;
				OnPropertyChanged("SelectedNotebook");
				GetNotes();
			}
		}

		private Note selectedNote;
		public Note SelectedNote
		{
			get { return selectedNote; }
			set
			{
				selectedNote = value;
				OnPropertyChanged("SelectedNote");
				SelectedNoteChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		private Visibility isRenameNotebookVisible;
		public Visibility IsRenameNotebookVisible
        {
			get { return isRenameNotebookVisible; }
			set 
			{
                isRenameNotebookVisible = value;
				OnPropertyChanged("IsRenameNotebookVisible");
			}
		}

        private Visibility isRenameNoteVisible;
        public Visibility IsRenameNoteVisible
        {
            get { return isRenameNoteVisible; }
            set
            {
                isRenameNoteVisible = value;
                OnPropertyChanged("IsRenameNoteVisible");
            }
        }


        // Command properties
        public NewNotebookCommand NewNotebookCommand { get; set; }
		public NewNoteCommand NewNoteCommand { get; set; }
        public RenameNotebookCommand RenameNotebookCommand { get; set; }
		public EndRenameNotebookCommand EndRenameNotebookCommand { get; set; }
		public DeleteNotebookCommand DeleteNotebookCommand { get; set; }
		public RenameNoteCommand RenameNoteCommand { get; set; }
		public EndRenameNoteCommand EndRenameNoteCommand { get; set; }
		public DeleteNoteCommand DeleteNoteCommand { get; set; }

        // Constructor
        public NotesVM()
		{
			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);
			RenameNotebookCommand = new RenameNotebookCommand(this);
			EndRenameNotebookCommand = new EndRenameNotebookCommand(this);
			DeleteNotebookCommand = new DeleteNotebookCommand(this);
            RenameNoteCommand = new RenameNoteCommand(this);
            EndRenameNoteCommand = new EndRenameNoteCommand(this);
            DeleteNoteCommand = new DeleteNoteCommand(this);

            Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();

            IsRenameNotebookVisible = Visibility.Collapsed;
			IsRenameNoteVisible = Visibility.Collapsed;

			GetNoteBooks();
		}

		// Method for creating new notebook
		public void CreateNotebook()
		{
			Notebook newNotebook = new Notebook()
			{
				Name = "Notebook",
				UserId = App.UserID
			};

			DatabaseHelper.Insert(newNotebook);

			GetNoteBooks();
		}

		// Method for creating new note
		public void CreateNote(int notebookId)
		{
			Note newNote = new Note()
			{
				NotebookId = notebookId,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Title = $"New Note"
			};

			DatabaseHelper.Insert(newNote);

			GetNotes();
		}

		// Method to get noteboosk from database helper
		public void GetNoteBooks()
		{
			var notebooks = DatabaseHelper.Read<Notebook>().Where(n => n.UserId == App.UserID).ToList();

			Notebooks.Clear();
			foreach (var notebook in notebooks)
			{
				Notebooks.Add(notebook);
			}
		}

        // Method to get notes from database helper
        private void GetNotes()
        {
			if (SelectedNotebook != null)
			{
				var notes = DatabaseHelper.Read<Note>().Where(n => n.NotebookId == SelectedNotebook.Id).ToList();

				Notes.Clear();
				foreach (var note in notes)
				{
					Notes.Add(note);
				}
			}
        }

		// Method to start show the renaming text box
		public void StartRenamingNotebook()
		{
            IsRenameNotebookVisible = Visibility.Visible;
		}

		// Method to stop showing renaming text box and update notebook name
        public void StopRenamingNotebook(Notebook notebook)
        {
            IsRenameNotebookVisible = Visibility.Collapsed;
			DatabaseHelper.Update(notebook);
			GetNoteBooks();
        }

		// Method to delete a notebook and notes corresponding with notebook
		public void DeleteNotebook(Notebook notebook)
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
            GetNoteBooks();
			Notes.Clear();
		}

        // Method to start show the renaming text box
        public void StartRenamingNote()
        {
            IsRenameNoteVisible = Visibility.Visible;
        }

        // Method to stop showing renaming text box and update note name
        public void StopRenamingNote(Note note)
        {
            IsRenameNoteVisible = Visibility.Collapsed;
            DatabaseHelper.Update(note);
            GetNotes();
        }

        // Method to delete the note
        public void DeleteNote(Note note)
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
            GetNotes();
        }

        private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
    }
}
