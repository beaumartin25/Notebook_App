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

namespace Notebook_App.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
		// properties
        public event PropertyChangedEventHandler? PropertyChanged;

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

		// Command properties
		public NewNotebookCommand NewNotebookCommand { get; set; }
		public NewNoteCommand NewNoteCommand { get; set; }

		// Constructor
		public NotesVM()
		{
			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);

			Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();

			GetNoteBooks();
		}

		// Method for creating new notebook
		public void CreateNotebook()
		{
			Notebook newNotebook = new Notebook()
			{
				Name = "new notebook"
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
		private void GetNoteBooks()
		{
			var notebooks = DatabaseHelper.Read<Notebook>();

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

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
    }
}
