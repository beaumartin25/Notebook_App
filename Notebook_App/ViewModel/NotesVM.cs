using Notebook_App.Model;
using Notebook_App.ViewModel.Commands;
using Notebook_App.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

		private Visibility isRenameVisible;
		public Visibility IsRenameVisible
		{
			get { return isRenameVisible; }
			set 
			{
                isRenameVisible = value;
				OnPropertyChanged("IsRenameVisible");
			}
		}


		// Command properties
		public NewNotebookCommand NewNotebookCommand { get; set; }
		public NewNoteCommand NewNoteCommand { get; set; }
        public RenameNotebookCommand RenameNotebookCommand { get; set; }
		public EndRenameNotebookCommand EndRenameNotebookCommand { get; set; }

        // Constructor
        public NotesVM()
		{
			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);
			RenameNotebookCommand = new RenameNotebookCommand(this);
			EndRenameNotebookCommand = new EndRenameNotebookCommand(this);

			Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();

			IsRenameVisible = Visibility.Collapsed;

			GetNoteBooks();
		}

		// Method for creating new notebook
		public void CreateNotebook()
		{
			Notebook newNotebook = new Notebook()
			{
				Name = "new notebook",
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
				Title = $"New note {DateTime.Now.ToString()}"
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
		public void StartRenaming()
		{
			IsRenameVisible = Visibility.Visible;
		}

		// Method to stop showing renaming text box and update notebook name
        public void StopRenaming(Notebook notebook)
        {
            IsRenameVisible = Visibility.Collapsed;
			DatabaseHelper.Update(notebook);
			GetNoteBooks();
        }

        private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
    }
}
