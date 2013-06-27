using RegexRenamer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Configuration;

namespace RegexRenamer.ViewModels
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
                return this._replace; }
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
            get {
                if (_find== null)
                    _find  = "";
                return _find; }
            set
            {
                _find = value;
                this.OnPropertyChanged();
                this.PreviewRegex();
            }
        }

        public ObservableCollection<RenamedFile> _files;
        public ObservableCollection<RenamedFile> Files
        {
            get
            {
                if (_files == null)
                    _files = new ObservableCollection<RenamedFile>();
                return _files;
            }
            set
            {
                _files = value;
                OnPropertyChanged();
            }

        }

        private bool _canRename = true;

        public static RoutedUICommand RenameCommand { get; set; }

        public MainWindowViewModel()
        {
            RenameCommand = new RoutedUICommand("Rename", "Rename", typeof(MainWindowViewModel));

            base.RegisterCommand(RenameCommand, param => this.CanRename, param => this.Rename());

        }

        public bool CanRename
        {
            get { return _canRename; }
        }

        private void Rename()
        {
            throw new NotImplementedException();
        }

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

        private void AddDirectory(string fileName)
        {
            foreach (var file in Directory.EnumerateDirectories(fileName))
                AddDirectory(file);

            foreach (var file in Directory.EnumerateFiles(fileName))
                Files.Add(new RenamedFile(file));
        }

        private void AddFile(string fileName)
        {
            Files.Add(new RenamedFile(fileName));
        }

        private void PreviewRegex()
        {
            
            try
            {
                foreach (var file in this.Files)
                {
                    file.PreviewRegex(@Find, @Replace);
                }

            }
            catch (ArgumentException e)
            {

            }
            
        }
    }
}
