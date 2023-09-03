using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;
using WolcenData;
using ModernWpf;

namespace WolcenEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CharacterData? cd;

        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.SetRequestedTheme(this, ElementTheme.Light);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuItem_Open_Click(object sender, RoutedEventArgs e)
        {
            string savedGamesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Saved Games");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open a Wolcen Save File";
            openFileDialog.DefaultExt = ".json";
            openFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";
            openFileDialog.InitialDirectory = savedGamesPath;

            openFileDialog.Multiselect = false;

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                cd = new CharacterData(selectedFilePath);
                OpenCharacterTxt.Text = cd.FilePath;
                DataContext = cd;
            }
        }
    }
}
