using Notebook_App.Model;
using Notebook_App.Services;
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
                IsCreateNotebookVisible = Visibility.Collapsed;    
                IsRichTextBoxVisible = Visibility.Collapsed;
                IsCreateNoteVisible = Visibility.Visible;
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
                IsCreateNotebookVisible = Visibility.Collapsed;
                IsCreateNoteVisible = Visibility.Collapsed;
                IsRichTextBoxVisible = Visibility.Visible;
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

        private Visibility isCreateNotebookVisible;
        public Visibility IsCreateNotebookVisible
        {
            get { return isCreateNotebookVisible; }
            set 
            { 
                isCreateNotebookVisible = value;
                OnPropertyChanged("IsCreateNotebookVisible");
            }
        }

        private Visibility isCreateNoteVisible;
        public Visibility IsCreateNoteVisible
        {
            get { return isCreateNoteVisible; }
            set
            {
                isCreateNoteVisible = value;
                OnPropertyChanged("IsCreateNoteVisible");
            }
        }

        private Visibility isRichTextBoxVisible;
        public Visibility IsRichTextBoxVisible
        {
            get { return isRichTextBoxVisible; }
            set 
            { 
                isRichTextBoxVisible = value;
                OnPropertyChanged("IsRichTextBoxVisible");
            }
        }


        private string searchQuery;
        public string SearchQuery
        {
            get { return searchQuery; }
            set
            {
                searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                GetNotes();
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
            IsCreateNotebookVisible = Visibility.Visible;
            IsCreateNoteVisible = Visibility.Collapsed;
            IsRichTextBoxVisible = Visibility.Collapsed;

			GetNoteBooks();
		}


		// Notebooks
		// Method for creating new notebook
		public void CreateNotebook()
		{
			NotebookService.CreateNotebook();
			GetNoteBooks();
		}

        // Method to get notebooks from database helper
        public void GetNoteBooks()
        {
            var notebooks = DatabaseHelper.Read<Notebook>().Where(n => n.UserId == App.UserID).ToList();

            Notebooks.Clear();
            foreach (var notebook in notebooks)
            {
                Notebooks.Add(notebook);
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
            NotebookService.DeleteNotebook(notebook);
            GetNoteBooks();
            Notes.Clear();
        }


        // Notes
        // Method for creating new note
        public void CreateNote(int notebookId)
		{
			NoteService.CreateNote(notebookId);
			GetNotes();
		}

        // Method to get notes from database helper but also adjusts to search query
        private void GetNotes()
        {
            if (SelectedNotebook != null)
            {
                var notes = DatabaseHelper.Read<Note>()
                                          .Where(n => n.NotebookId == SelectedNotebook.Id)
                                          .ToList();

                if (!string.IsNullOrWhiteSpace(SearchQuery))
                {
                    notes = notes.Where(n => n.Title != null &&
                                             n.Title.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                                 .ToList();
                }

                Notes.Clear();
                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
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
			NoteService.DeleteNote(note);
            GetNotes();
        }


        private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
    }
}
