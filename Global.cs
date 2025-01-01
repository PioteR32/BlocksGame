using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlocksGame
{
    public class Global
    {

        public static MainPageView MainPageView { get; set; }
        public static MainPage MainPage { get; set; }
        public static GamePage GamePage { get; set; }
        public static string GlobalPath { get; } = Microsoft.Maui.Storage.FileSystem.AppDataDirectory;
        /// <summary>
        /// Create grid columns and rows
        /// </summary>
        /// <param name="grid"> </param>
        /// <param name="width"> number of columns </param>
        /// <param name="height"> number of rows </param>
        public static void CreateGrid(Grid grid ,uint width, uint height)
        {
            for (int i = 0; i < width; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto});
            }
            for (int i = 0; i < height; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
        }
        public static double Impressivenes(double number ,int potęga)
        {
            if (potęga == 0)
                return 1;
            potęga--;
            return number * Impressivenes(number , potęga);
        }
        public static bool CreateFolder(string nameOfFolder)
        {
            string folderPath = "";
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                folderPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, nameOfFolder);
            }
            else if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                folderPath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, nameOfFolder);
            }
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                return true;
            }
            else
            {
                return false;
            }
        }
    }


}
