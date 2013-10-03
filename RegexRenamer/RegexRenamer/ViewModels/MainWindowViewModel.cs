using RegexRename.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Configuration;
using System.Windows.Data;
using MVVMToolkit;

namespace RegexRename.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private string _replace;
        public string Replace
        {
            get
            {
                if (_replace == null)
                    _replace = "";
                return this._replace;
            }
            set
            {
                this._replace = value;
                this.OnPropertyChanged();
                this.PreviewRegex();
            }
        }

        private string _find;
        public string Find
        {
            get
            {
                if (_find == null)
                    _find = "";
                return _find;
            }
            set
            {
                _find = value;
                this.OnPropertyChanged();
                this.PreviewRegex();
            }
        }

        public ObservableCollection<RenamableFile> _files;
        public ObservableCollection<RenamableFile> Files
        {
            get
            {
                if (_files == null)
                    _files = new ObservableCollection<RenamableFile>();
                return _files;
            }
            set
            {
                _files = value;
                OnPropertyChanged();
            }
        }

        private const string HELP_URL = @"http://msdn.microsoft.com/en-us/library/az24scfc.aspx";

        public static RoutedUICommand RenameCommand { get; set; }
        public static RoutedUICommand RemoveAllCommand { get; set; }
        public static RoutedUICommand FindHelpCommand { get; set; }
        public static RoutedUICommand OpenFolderCommand { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Windows.Input.RoutedUICommand.#ctor(System.String,System.String,System.Type)")]
        public MainWindowViewModel()
        {
            RenameCommand = new RoutedUICommand("Rename the affected files", "Rename", typeof(MainWindowViewModel));
            RemoveAllCommand = new RoutedUICommand("Remove all of the files", "RemoveAll", typeof(MainWindowViewModel));
            FindHelpCommand = new RoutedUICommand("Find some regex help", "FindHelp", typeof(MainWindowViewModel));
            OpenFolderCommand = new RoutedUICommand("Open a folder", "OpenFolder", typeof(MainWindowViewModel));

            base.RegisterCommand(RenameCommand, param => this.CanRename, param => this.Rename());
            base.RegisterCommand(RemoveAllCommand, param => this.CanRemoveAll, param => this.RemoveAll());
            base.RegisterCommand(FindHelpCommand, param => true, param => this.FindHelp());
            base.RegisterCommand(OpenFolderCommand, param => true, param => this.OpenFolder());


        }

        #region Commands


        public bool CanRename
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Find))
                {
                    foreach (RenamableFile f in this.Files)
                    {
                        if (f.IsAffected)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        private void Rename()
        {
            foreach (var file in Files)
                if (file.IsAffected)
                    file.Rename();
        }

        public bool CanRemoveAll
        {
            get { return this.Files.Count > 0; }
        }

        private void RemoveAll()
        {
            this.Files.Clear();
        }

        public bool CanRemoveSelected
        {
            get
            {
                return CollectionViewSource.GetDefaultView(this.Files).CurrentItem != null;
            }
        }

        public void RemoveSelected()
        {
            throw new NotImplementedException();
        }

        private void FindHelp()
        {
            Process.Start(HELP_URL);
        }

        private void OpenFolder()
        {
            var dlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = dlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.AddFiles(new string[] { dlg.SelectedPath });
            }
        }
        #endregion

        #region Adding files

        public void AddFiles(string[] fileNames)
        {
            foreach (string fileName in fileNames)
            {
                FileAttributes attr = File.GetAttributes(fileName);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    AddDirectory(fileName);
                else
                    AddFile(fileName);
            }
        }

        private void AddDirectory(string directoryPath)
        {
            foreach (var directory in Directory.EnumerateDirectories(directoryPath))
                AddDirectory(directory);

            foreach (var file in Directory.EnumerateFiles(directoryPath))
                AddFile(file);
        }

        private void AddFile(string fileName)
        {
            var f = new RenamableFile(fileName);

            if (!Files.Contains(f))
                Files.Add(new RenamableFile(fileName));
        }

        #endregion

        private void PreviewRegex()
        {
            foreach (var file in this.Files)
            {
                file.PreviewRegex(@Find, @Replace);
            }


        }
    }
}
