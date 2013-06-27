using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using RegexRenamer.ViewModels;

namespace RegexRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel model = new MainWindowViewModel();
        
        public MainWindow()
        {
            this.DataContext = model;
            InitializeComponent();
            
        }

        private void FilesDroppedHandler(object sender, DragEventArgs e)
        {
            model.AddFiles((string[])e.Data.GetData(DataFormats.FileDrop, false));
        }
    }
}
