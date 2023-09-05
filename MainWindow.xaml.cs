using Microsoft.Win32;
using System;
using System.Windows;
using System.IO;
using WolcenData;
using ModernWpf;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Drawing;
using Color = System.Drawing.Color;
using System.Drawing.Imaging;

namespace WolcenEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CharacterData? cd;
        public static List<Act> ActData = new List<Act>();

        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.SetRequestedTheme(this, ElementTheme.Dark);

            ActData = QuestStepParser.GetActsFromDir(Directory.GetCurrentDirectory() + "\\Data\\Game\\Umbra\\Quests\\Quests\\StoryQuests\\");

            WindowBackgroundImage.ImageSource = CreateImageSourceWithAlphaOverlay(WolcenEditor.Properties.Resources.Background, 50);
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

        private void MenuItem_Save_Click(object sender, RoutedEventArgs e)
        {
            if (cd == null) return;
            if(MessageBox.Show("Are you sure you want to save?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                cd.Save();
            }
        }

        private ImageSource? CreateImageSourceWithAlphaOverlay(byte[] bitmapBytes, int opacity)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(bitmapBytes))
                using (Bitmap originalBitmap = new Bitmap(memoryStream))
                {
                    int width = originalBitmap.Width;
                    int height = originalBitmap.Height;

                    using (Bitmap newBitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    using (Graphics graphics = Graphics.FromImage(newBitmap))
                    {
                        // Draw black background
                        graphics.Clear(Color.FromArgb(255, 25, 25, 25));

                        // Set up the image attributes to control opacity
                        ColorMatrix colorMatrix = new ColorMatrix();
                        colorMatrix.Matrix33 = opacity / 255f; // Set opacity value in the alpha channel
                        ImageAttributes imageAttributes = new ImageAttributes();
                        imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                        // Draw the original image with opacity over the black background
                        graphics.DrawImage(originalBitmap, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, imageAttributes);

                        // Convert the Bitmap to ImageSource
                        IntPtr hBitmap = newBitmap.GetHbitmap();
                        ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(
                            hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        imageSource.Freeze();

                        // Release the HBitmap handle
                        DeleteObject(hBitmap);

                        return imageSource;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
