using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace RegexRenamer.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private string _regex;
        public string Regex
        {
            get { return this._regex; }
            set
            {
                this._regex = value;
                Debug.WriteLine(this._regex);
                this.OnPropertyChanged();
            }
        }

        private string _find;
        public string Find
        {
            get { return _find; }
            set
            {
                _find = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<String> _fileNames;
        public ObservableCollection<String> FileNames
        {
            get { return _fileNames; }
            set
            {
                _fileNames = value;
                this.OnPropertyChanged();
            }
        }

        public ObservableCollection<string> _newFileNames;
        public ObservableCollection<string> NewFileNames
        {
            get { return _newFileNames; }
            set
            {
                _newFileNames = value;
                this.OnPropertyChanged();
            }
        }

        private bool _canRename = true;

        public static RoutedUICommand RenameCommand { get; set; }

        public MainWindowViewModel()
        {
            FileNames = new ObservableCollection<string>();
            NewFileNames = new ObservableCollection<string>();
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

        public void AddFiles(string [] fileNames)
        {
            foreach (string fileName in fileNames)
            {
                foreach (var file in Directory.EnumerateDirectories(fileName))
                    AddFiles(new string[] { file });

                foreach (var file in Directory.EnumerateFiles(fileName))
                    if (file != null)
                        AddFile(file);
            }            
        }

        public void AddFile(string fileName)
        {
            FileNames.Add(fileName);
            NewFileNames.Add(fileName);
        }
    }
}
