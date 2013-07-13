using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexRename.Models
{
    class RenamableFile: INotifyPropertyChanged
    {
        private string _fullPath;
        public string FullPath
        {
            get { return _fullPath; }
            set
            {
                _fullPath = value;
                OnPropertyChanged();
            }
        }

        private string _oldName;
        public string OldName
        {
            get { return _oldName; }
            set
            {
                _oldName = value;
                OnPropertyChanged();
                OnPropertyChanged("IsAffected");
            }
        }

        private string _newName;
        public string NewName 
        {
            get { return _newName; }
            set
            {
                _newName = value;
                OnPropertyChanged();
                OnPropertyChanged("IsAffected");
            }
        }

        public bool IsAffected
        {
            get { return !OldName.Equals(NewName); }
        }

        public RenamableFile(string oldName)
        {
            oldName = UpdatePath(oldName);
            
            Debug.Print(FullPath);
        }

        private string UpdatePath(string path)
        {
            FullPath = Path.GetFullPath(path);
            int lastSlashIndex = path.LastIndexOf('\\');
            if (lastSlashIndex != -1)
                path = path.Substring(lastSlashIndex + 1);
            OldName = NewName = path;
            return path;
        }

        public void PreviewRegex(string find, string replaceWith)
        {
            try
            {
                NewName = Regex.Replace(OldName, find, replaceWith).ToString();
            }
            catch (ArgumentException e)
            {
                Debug.Print(e.Message);
                throw;
            }
            
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged([CallerMemberName] string propertyName="")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Rename()
        {
            var newAbsolutePath = this.FullPath.Replace(this.OldName, this.NewName);

            File.Move(this.FullPath, newAbsolutePath);

            UpdatePath(newAbsolutePath);

        }

        #region Overrides
        
        public override bool Equals(object obj)
        {
            var rfile = obj as RenamableFile;
            if (rfile == null)
                return false;

            return this.FullPath.Equals(rfile.FullPath);
        }

        public override int GetHashCode()
        {
            return this.FullPath.GetHashCode();
        }

        #endregion
    }
}
