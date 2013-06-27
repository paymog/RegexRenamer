using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexRenamer.Models
{
    class RenamedFile: INotifyPropertyChanged
    {
        private string _oldName;
        public string OldName
        {
            get { return _oldName; }
            set
            {
                _oldName = value;
                OnPropertyChanged();
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
            }
        }

        public RenamedFile(string oldName)
        {
            OldName = NewName = oldName;
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
    }
}
