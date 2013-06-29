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

        //private bool _isAffected;
        public bool IsAffected
        {
            get { return OldName.Length != NewName.Length; }
        }

        public RenamableFile(string oldName)
        {
            FullPath = Path.GetFullPath(oldName);
            int lastSlashIndex = oldName.LastIndexOf('\\');
            if (lastSlashIndex != -1)
                oldName = oldName.Substring(lastSlashIndex + 1);
            OldName = NewName = oldName;
            
            Debug.Print(FullPath);
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

            this.FullPath = newAbsolutePath;
            int lastSlashIndex = newAbsolutePath.LastIndexOf('\\');
            if (lastSlashIndex != -1)
                newAbsolutePath = newAbsolutePath.Substring(lastSlashIndex + 1);
            OldName = NewName = newAbsolutePath;

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
