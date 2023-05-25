using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TelegramSorterSpace
{
    internal class TelegramSorter
    {
        private string path = null;

        public TelegramSorter(string path)
        {
            this.path = path;
        }
        public TelegramSorter() { }

        public static T[] removeDuplicates<T>(T[] array)
        {
            HashSet<T> set = new HashSet<T>(array);
            T[] result = new T[set.Count];
            set.CopyTo(result);
            return result;
        }

        public void SortByPath(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new DirectoryInfo(path);

                FileInfo[] files = directory.GetFiles();

                string[] extensions_ = new string[files.Length];
                int lenExtensions = 0;
                foreach (FileInfo file in files) extensions_[lenExtensions++] = file.Extension;
                string[] extensions = removeDuplicates(extensions_);

                foreach (string extension in extensions)
                {
                    Directory.CreateDirectory(path + $"{extension.Substring(1).ToUpper()}");
                    foreach (var file in files)
                        if (file.Extension == extension) { file.MoveTo(path + @$"{extension.Substring(1).ToUpper()}\{file.Name}"); }
                }
                Console.Clear();
                Console.WriteLine("Сортировка прошла успешно!");
                Console.Write("Открыть данную папку?\n1. Да\n2. Нет\n-> ");
                string user = Console.ReadLine();

                if (user == "1") Process.Start("explorer.exe", $"{path}");
            }
            else { Console.Clear(); Console.WriteLine("Директория не найдена!"); }
        }

        private List<string> GetFolderList(string path) // Данная функция взята из интернета
        {
            try
            {
                List<string> dirList = new List<string>(100000);
                string[] dirs = Directory.GetDirectories(path);
                foreach (string subdirectory in dirs)
                {
                    dirList.Add(subdirectory);
                    try
                    {
                        dirList.AddRange(GetFolderList(subdirectory));
                    }
                    catch { }
                }
                return dirList;
            }
            catch(Exception) { return null; }
        }

        public void Sort()
        {
            Console.Write("Вы хотите видеть процесс поиска?\n1. Да\n2. Нет\n-> ");
            string user = Console.ReadLine();

            if (Directory.Exists(@$"C:\Users\{Environment.UserName}\Downloads\Telegram Desktop"))
            {
                SortByPath(@$"C:\Users\{Environment.UserName}\Downloads\Telegram Desktop\");
                return;
            }

            string[] accessDenied = { "Documents and Settings", "System Volume Information", "Windows", "Microsoft" };
            DriveInfo[] driveInfo = DriveInfo.GetDrives();
            List<string> foundPath;
            Random random = new Random();

            if (user != "1") Console.WriteLine("Пожалуйста, подождите! Выполняется поиск папки Telegram Desktop по всем дискам...");

            foreach (var drive in driveInfo)
            {
                if (drive.IsReady)
                {

                    string[] dirs = Directory.GetDirectories($@"{drive.Name}");

                    foreach (var dir in dirs)
                    {
                        string[] words = dir.Split(new char[] { '\\' });

                        if (!accessDenied.Contains(words[words.Length - 1])) foundPath = GetFolderList(@$"{dir}");
                        else foundPath = null;

                        if (foundPath != null)
                        {
                            foreach (var path in foundPath)
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                words = path.Split(new char[] { '\\' });
                                if (words[words.Length - 1] == "Telegram Desktop") { SortByPath(path + @"\"); return; }

                                if (user == "1")
                                {
                                    ConsoleColor color = (ConsoleColor)random.Next(1, 16);
                                    //ConsoleColor backColor = (ConsoleColor)random.Next(1, 16);

                                    Console.ForegroundColor = color;
                                    //Console.BackgroundColor = backColor;
                                    Console.WriteLine(path);
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("К сожалению, папку не удалось найти! Введите путь вручную.");
        }
    }
}