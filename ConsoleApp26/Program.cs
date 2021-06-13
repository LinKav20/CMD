using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace ConsoleApp26
{
    class Program
    {
        static string[] command = { "help", "dir", "wmic", "cd", "more", "copy", "move", "delete", "create", "merge", "clear", "mkdir", "close" };
        static string[] commandDis = { "Помощь",
            "Просмотров всех файлов и директорий в текущей",
            "Просмотр существующих дисков",
            "Переход в другую директорию или на другой диск",
            "Вывод содержимого текстового файла",
            "Копирование текстового файла",
            "Перемещение текстового файла",
            "Удаление текстового файла в текущей директории",
            "Создание текстового файла в текущей директории",
            "Конкатенация содержимого двух или более текстовых файлов",
            "Очистка экрана консоли",
            "Создание директории",
            "Закрыть консоль"
        };

        /// <summary>
        /// Копирует текстовый файл в новый файл.
        /// </summary>
        /// <param name="pathFrom">Копируемый файл.</param>
        /// <param name="pathTo">Новый файл, куда копируется старый.</param>
        static void Copy(string pathFrom, string pathTo)
        {
            try
            {
                File.Copy(pathFrom, pathTo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Функция для очистки консоли.
        /// </summary>
        static void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        /// Вывод содержимого текстового файла в консоль.
        /// </summary>
        /// <param name="path">Файл, содержимое которого выводится на экран.</param>
        /// <param name="encoding">Кодировка, в которой выводятся данные.</param>
        static void More(string path, string encoding = "UTF8")
        {
            try
            {
                // Установка котировки.
                Encoding en;
                switch (encoding)
                {
                    case "Unicode":
                        en = Encoding.Unicode;
                        break;
                    case "ASCII":
                        en = Encoding.ASCII;
                        break;
                    default:
                        en = Encoding.UTF8;
                        break;
                }
                string[] file = File.ReadAllLines(path, en);
                foreach (string str in file)
                {
                    Console.WriteLine(str);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Перемещает текстовый файл в другую директорию.
        /// </summary>
        /// <param name="filePath">Перемещаемый файл.</param>
        /// <param name="path">Новый пусть файла.</param>
        static void Move(string filePath, string path)
        {
            try
            {
                string[] fileName = filePath.Split(Path.DirectorySeparatorChar);
                File.Move(filePath, Path.Combine(path, fileName[fileName.Length - 1]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Удаление текстового файла.
        /// </summary>
        /// <param name="path">Файл, который будет удален.</param>
        static void Delete(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Создание нового текстового файла.
        /// </summary>
        /// <param name="name">Имя файла.</param>
        /// <param name="encoding">Кодировка файла.</param>
        static void Create(string name, string encoding = "UTF8")
        {
            try
            {
                Encoding en;
                switch (encoding)
                {
                    case "Unicode":
                        en = Encoding.Unicode;
                        break;
                    case "ASCII":
                        en = Encoding.ASCII;
                        break;
                    default:
                        en = Encoding.UTF8;
                        break;
                }
                TextWriter tw = new StreamWriter(name, true, en);
                string[] name_ = name.Split(Path.DirectorySeparatorChar);
                tw.Write("");
                tw.Close();
                Console.WriteLine($"Создан текстовый файл {name_[name_.Length-1]} в кодировке {en}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Совмещение нескольких тектовых файлов в один новый тектсовый файл.
        /// </summary>
        /// <param name="args">
        /// args[0] - команда "merge".
        /// args[1] - новый текстовый файл, который будет содержать в себе остальные.
        /// args[2...] - тектовые файлы.
        /// ...
        /// args[args.Lenght - 2] - МОЖЕТ БЫТЬ кодировка.
        /// args[args.Lenght - 1] - МОЖЕТ БЫТЬ аргумент \w - для вывода реузльтата в консоль.
        /// Последние два параметра могут отсутствовать == аргументам могут быть только тектовые файлы.
        /// </param>
        static void Merge(string[] args)
        {
            List<string> allText = new List<string>();
            Encoding en;
            string encoding = FindEncoding(args);
            switch (encoding)
            {
                case "Unicode":
                    en = Encoding.Unicode;
                    break;
                case "ASCII":
                    en = Encoding.ASCII;
                    break;
                default:
                    en = Encoding.UTF8;
                    break;
            }
            string[] file;
            for (int i = 2; i < args.Length; i++)
            {
                if (args[i] != encoding && args[i] != @"\w")
                {
                    file = File.ReadAllLines(args[i], en);
                    foreach (string str in file)
                    {
                        allText.Add(str);
                    }
                }
            }
            TextWriter tw = new StreamWriter(args[1]);
            foreach (string line in allText)
            {
                tw.WriteLine(line);
            }
            tw.Close();
            if (args[args.Length - 1] == @"\w")
            {
                More(args[1]);
            }
        }

        /// <summary>
        /// Переход в другую директорию, если есть параметры.
        /// Вывод текущей позиции, если парметры отсутюствуют.
        /// </summary>
        /// <param name="to">Путь к новой директории.</param>
        static void Cd(string to = "")
        {
            if (to == "")
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
            }
            else
            {
                try
                {
                    Directory.SetCurrentDirectory(to);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Создание новой директории в текущей директории.
        /// </summary>
        /// <param name="name">Название директории.</param>
        static void Mkdir(string name)
        {
            try
            {
                Directory.CreateDirectory(name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Закрытие консоли.
        /// </summary>
        static void Close()
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Просмотр всех возможных дисков.
        /// </summary>
        static void Wmic()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine(d.Name);
            }
        }

        /// <summary>
        /// Просмотр содержимого текущей директории.
        /// </summary>
        static void Dir_()
        {
            string[] fileEntries = Directory.GetFiles(Directory.GetCurrentDirectory());
            string[] dirEntries = Directory.GetDirectories(Directory.GetCurrentDirectory());
            string[] str;

            if (fileEntries.Length == 0 && dirEntries.Length == 0)
            {
                Console.WriteLine("Данная директория пуста");
            }
            foreach (string dir in dirEntries)
            {
                str = dir.Split(Path.DirectorySeparatorChar);
                Console.WriteLine("     " + "<DIR>" + "   " + str[str.Length - 1]);
            }
            foreach (string file in fileEntries)
            {
                str = file.Split(Path.DirectorySeparatorChar);
                Console.WriteLine("             " + str[str.Length - 1]);
            }
        }

        /// <summary>
        /// Выводит описание комнды и примеры ее использования, если есть аргумент.
        /// Если аргуммента нет, выывод список всех возможных команд.
        /// </summary>
        /// <param name="command">Команда, по которой требуется помощь.</param>
        static void Help(string command = "")
        {
            if (command == "")
            {
                AllCommands();
            }
            else
            {
                switch (command)
                {
                    case "dir":
                        Console.WriteLine("Вывод содержимого текущей директории");
                        Console.WriteLine("Пример использования: dir");
                        break;
                    case "close":
                        Console.WriteLine("Закрытие консоли");
                        Console.WriteLine("Пример использования: close");
                        break;
                    case "wmic":
                        Console.WriteLine("Просмотр существующих дисков");
                        Console.WriteLine("Пример использования: wmic");
                        break;
                    case "cd":
                        Console.WriteLine("Переходит в заданную директорию, если есть аргументы, если их нет - выводит текущее положение");
                        Console.WriteLine("Пример использования: cd ");
                        Console.WriteLine("                      cd <path>");
                        Console.WriteLine("                      cd .. - переход в директорию выше");
                        Console.WriteLine("                      cd ../.. - переход в директорию выше на две");
                        Console.WriteLine("                      cd <disk>");
                        break;
                    case "more":
                        Console.WriteLine("Вывод содержимого файла в консоль");
                        Console.WriteLine("Возможные параметры: кодировка одна из предложенных (UTF8 - дефолтная, Unicode, ASCII)");
                        Console.WriteLine("Пример использования: more <file.txt> ");
                        Console.WriteLine("                      more <file.txt> <encoding>");
                        break;
                    case "copy":
                        Console.WriteLine("Копирование одного текстового файла в другой");
                        Console.WriteLine("Пример использования: copy <file.txt> <newFile.txt>");
                        break;
                    case "move":
                        Console.WriteLine("Перемещение текстового файла в другую директорию");
                        Console.WriteLine("Пример использования: move <file> <newFullDirecoryPath>");
                        Console.WriteLine("                      move test.txt C:\\Users\\Student\\");
                        break;
                    case "delete":
                        Console.WriteLine("Удаление текстового файла");
                        Console.WriteLine("Пример использования: delete <file.txt>");
                        break;
                    case "help":
                        Console.WriteLine("Информация о конкретной команде");
                        Console.WriteLine("Пример использования: help ");
                        Console.WriteLine("                      help <command>");
                        break;
                    case "mkdir":
                        Console.WriteLine("Создание директории");
                        Console.WriteLine("Пример использования: mkdir <name>");
                        break;
                    case "create":
                        Console.WriteLine("Создание текстового файла");
                        Console.WriteLine("Возможные параметры: кодировка одна из предложенных (UTF8 - дефолтная, Unicode, ASCII)");
                        Console.WriteLine("Пример использования: create <file.txt> ");
                        Console.WriteLine("                      create <file.txt> <encoding>");
                        break;
                    case "merge":
                        Console.WriteLine("Конкатенация содержимого двух или более текстовых файлов");
                        Console.WriteLine("Возможные параметры: кодировка одна из предложенных (UTF8 - дефолтная, Unicode, ASCII)");
                        Console.WriteLine("                     \\w в конце комнады - вывод результат команды в консоль");
                        Console.WriteLine("Пример использования: merge <fileMergedN.txt> <file1.txt>  <file2.txt> ... <fileN.txt> ");
                        Console.WriteLine("                      merge <fileMergedN.txt> <file1.txt>  <file2.txt> ... <fileN.txt> <encoding>");
                        Console.WriteLine("                      merge <fileMergedN.txt> <file1.txt>  <file2.txt> ... <fileN.txt> <encoding> \\w");
                        Console.WriteLine("                      merge <fileMergedN.txt> <file1.txt>  <file2.txt> ... <fileN.txt> \\w");
                        Console.WriteLine("                      где <fileMergedN.txt> - новый текстовый файл, содрежащий в себе последующие n файлов");
                        break;
                    default:
                        Console.WriteLine("Указанная вами команда не существует");
                        break;
                }
            }
        }

        /// <summary>
        /// Вывод список всех возможных комнад.
        /// </summary>
        static void AllCommands()
        {
            int maxCanLen = command.Max().Length > 12 ? command.Max().Length : 12;
            for (int i = 0; i < command.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("    " + command[i].ToLower());
                for (int j = 0; j < maxCanLen - command[i].Length; j++)
                {
                    Console.Write(" ");
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(commandDis[i]);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Оставляет во входной строке ровно по одному пробелу между словами.
        /// Делит полученную строку на слова по пробелам.
        /// </summary>
        /// <param name="str">Входная строка.</param>
        /// <returns>Массив из слов, путсых элементов не существует.</returns>
        static string[] DeleteSpacesAndSplit(string str)
        {
            string ans = "";
            if (str != "")
            {
                if (str[0] != 32)
                {
                    ans += str[0];
                }
                for (int i = 1; i < str.Length; i++)
                {
                    if (str[i - 1] == 32 && str[i] != 32)
                    {
                        ans += str[i - 1];
                        ans += str[i];
                    }
                    else if (str[i - 1] != 32 && str[i] != 32)
                    {
                        ans += str[i];
                    }

                }
                if (ans[0] == 32)
                {
                    ans = ans.Substring(1);
                }
            }
            return ans.Split();
        }

        /// <summary>
        /// Проверяет входные данные на корректность.
        /// </summary>
        /// <param name="str">Входные данные.</param>
        /// <returns>true если данные корректны, иначе false.</returns>
        static bool Correction(string[] str)
        {
            switch (str[0])
            {
                case "help":
                    if (str.Length > 2)
                    {
                        Error(str[0]);
                        return false;
                    }
                    break;
                case "dir":
                    if (str.Length > 1)
                    {
                        Error(str[0]);
                        return false;
                    }
                    break;
                case "close":
                    if (str.Length > 1)
                    {
                        Error(str[0]);
                        return false;
                    }
                    break;
                case "wmic":
                    if (str.Length > 1)
                    {
                        Error(str[0]);
                        return false;
                    }
                    break;
                case "cd":
                    if (str.Length > 2)
                    {
                        Error(str[0]);
                        return false;
                    }
                    break;
                case "create":
                    if (!(str.Length == 3 || str.Length == 2))
                    {
                        Error(str[0]);
                        return false;
                    }
                    if (CorrectFile(str[1]) && CheckTxt(str[1]))
                    {
                        str[1] = FullPath(str[1]);
                    }
                    else
                    {
                        Error(str[0]);
                        return false;
                    }
                    if (str.Length == 3)
                    {
                        if (!CheckEncoding(str[2]))
                        {
                            Error(str[0]);
                            return false;
                        }
                    }
                    break;
                case "delete":
                    if (str.Length != 2)
                    {
                        Error(str[0]);
                        return false;
                    }
                    if (CorrectFile(str[1]))
                    {
                        str[1] = FullPath(str[1]);
                    }
                    else
                    {
                        Error(str[0]);
                        return false;
                    }
                    break;
                case "move":
                    if (str.Length != 3)
                    {
                        Error(str[0]);
                        return false;
                    }
                    if (CorrectFile(str[1]))
                    {
                        str[1] = FullPath(str[1]);
                    }
                    else
                    {
                        Error(str[0]);
                        return false;
                    }
                    break;
                case "clear":
                    if (str.Length > 1)
                    {
                        Error(str[0]);
                        return false;
                    }
                    break;
                case "merge":
                    if (str.Length < 4)
                    {
                        Error(str[0]);
                        return false;
                    }
                    for (int i = 1; i < str.Length; i++)
                    {
                        if (str[i] != @"\w" && i == str.Length - 1)
                        {
                            if (str[i] != "UTF-8" && str[i] != "ASCII" && str[i] != "Unicode" && i == str.Length - 2 && str[str.Length - 1] == @"\w")
                            {
                                if (CorrectFile(str[i]) && CheckTxt(str[i]))
                                {
                                    str[i] = FullPath(str[i]);
                                }
                                else
                                {
                                    Error(str[0]);
                                    return false;
                                }
                            }
                        }
                    }
                    break;
                case "more":
                    if (!(str.Length == 3 || str.Length == 2))
                    {
                        Error(str[0]);
                        return false;
                    }
                    if (CorrectFile(str[1]) && CheckTxt(str[1]))
                    {
                        str[1] = FullPath(str[1]);
                    }
                    else
                    {
                        Error(str[0]);
                        return false;
                    }
                    if (str.Length == 3)
                    {
                        if (!CheckEncoding(str[2]))
                        {
                            Error(str[0]);
                            return false;
                        }
                    }
                    break;
                case "copy":
                    if (str.Length != 3)
                    {
                        Error(str[0]);
                        return false;
                    }
                    if (CorrectFile(str[1]) && CheckTxt(str[1]) && CorrectFile(str[2]) && CheckTxt(str[2]))
                    {
                        str[1] = FullPath(str[1]);
                        str[2] = FullPath(str[2]);
                    }
                    else
                    {
                        Error(str[0]);
                        return false;
                    }
                    break;
                case "mkdir":
                    if (str.Length != 2)
                    {
                        Error(str[0]);
                        return false;
                    }
                    str[1] = FullPath(str[1]);
                    break;
            }
            return true;
        }

        /// <summary>
        /// Проверяет на корректность введенную пользователем кодировку.
        /// </summary>
        /// <param name="en">Введенная пользователем кодировка.</param>
        /// <returns>true если данные корректны, иначе false.</returns>
        static bool CheckEncoding(string en)
        {
            return en == "UTF8" || en == "Unicode" || en == "ASCII";
        }

        /// <summary>
        /// Составляет полный путь к файлу по его имени.
        /// </summary>
        /// <param name="name">Имя файла.</param>
        /// <returns>Полный путь к файлу.</returns>
        static string FullPath(string name)
        {
            string[] str = name.Split('\\');
            if (str.Length == 1)
            {
                return Path.Combine(Directory.GetCurrentDirectory(), name);
            }
            else
            {
                return name;
            }
        }

        /// <summary>
        /// Проверяет является ли введенная строка файлом .txt.
        /// </summary>
        /// <param name="str">Имя файла.</param>
        /// <returns>true если данные корректны, иначе false.</returns>
        static bool CheckTxt(string str)
        {
            string[] s = str.Split('.');
            if (s[s.Length - 1] != "txt")
            {
                Console.WriteLine("Использован неверный тип файла");
            }
            return s[s.Length - 1] == "txt";
        }

        /// <summary>
        /// Проверяет является ли имя файла коррректным.
        /// </summary>
        /// <param name="str_">Название файла.</param>
        /// <returns>true если данные корректны, иначе false.</returns>
        static bool CorrectFile(string str_)
        {
            string[] str = str_.Split('\\');
            string s = str[str.Length - 1].Trim('\\', '/', ':', '*', '?', '"', '<', '>', '|', '+', '%', '!', '@');
            string[] s_ = str[str.Length - 1].Split('.');
            if (s_.Length != 2)
            {
                return false;
            }
            if (str.Length > 1)
            {
                string dir = "";
                for (int i = 0; i < str.Length - 1; i++)
                {
                    dir += str[i];
                    dir += "\\";
                }
                if (!Directory.Exists(dir))
                {
                    return false;
                }
            }
            return s == str[str.Length - 1];
        }

        /// <summary>
        /// Выводит сообщение об ошибке при неправильном исплоьзовании команды.
        /// </summary>
        /// <param name="command">Команда, которая неправильно используется пользователем.</param>
        static void Error(string command)
        {
            Console.WriteLine("Неправильное использование команды");
            Console.WriteLine("Используйте help " + command);
        }

        /// <summary>
        /// Находит в массиве кодировку.
        /// </summary>
        /// <param name="list">Массив входных данных.</param>
        /// <returns>Кодировку.</returns>
        static string FindEncoding(string[] list)
        {
            string ans = "";
            for (int i = 0; i < list.Length; i++)
            {
                if (CheckEncoding(list[i]))
                {
                    ans = list[i];
                }
            }
            return ans;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите help если вам нужна помощь"); 
            Console.WriteLine("!!! Обратите внимание, что команды реализованы для ТЕКСТОВЫХ файлов (т.е. типа *.txt)\n" +
                "               Спасибо за внимание !!!");
            bool alive = true;
            while (alive)
            {
                Console.Write(Directory.GetCurrentDirectory() + ">");
                string input_ = Console.ReadLine();
                string[] input = DeleteSpacesAndSplit(input_);
                input[0] = input[0].ToLower();
                if (Correction(input))
                {
                    switch (input[0])
                    {
                        case "help":
                            if (input.Length == 2)
                            {
                                Help(input[1]);
                            }
                            else Help();
                            break;
                        case "dir":
                            Dir_();
                            break;
                        case "wmic":
                            Wmic();
                            break;
                        case "cd":
                            if (input.Length == 1)
                            {
                                Cd();
                            }
                            else
                            {
                                Cd(input[1]);
                            }
                            break;
                        case "create":
                            if (input.Length == 2)
                            {
                                Create(input[1]);
                            }
                            else
                            {
                                Create(input[1], input[2]);
                            }
                            break;
                        case "delete":
                            Delete(input[1]);
                            break;
                        case "move":
                            Move(input[1], input[2]);
                            break;
                        case "clear":
                            Clear();
                            break;
                        case "merge":
                            Merge(input);
                            break;
                        case "more":
                            if (input.Length == 2)
                            {
                                More(input[1]);
                            }
                            else
                            {
                                More(input[1], input[2]);
                            }
                            break;
                        case "copy":
                            Copy(input[1], input[2]);
                            break;
                        case "close":
                            Close();
                            break;
                        case "mkdir":
                            Mkdir(input[1]);
                            break;
                        case "":
                            break;
                        default:
                            Console.WriteLine($"{input_} не является внутренней или внешненй командой");
                            break;
                    }
                }
            }
        }
    }
}
