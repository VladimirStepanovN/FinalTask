using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask;

internal class Program
{
    internal static void Main(string[] args)
    {
        if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
        {
            try
            {
                string directorryPath = Path.Combine(new string[] { Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Students" });
                if (!Directory.Exists(directorryPath))
                {
                    Directory.CreateDirectory(directorryPath);
                }
                BinaryFormatter formatter = new();
                using (var fileStream = new FileStream(args[0], FileMode.OpenOrCreate))
                {
                    var groups = ((Student[])formatter.Deserialize(fileStream)).GroupBy(e => e.Group);
                    foreach (var group in groups)
                    {
                        string newFilePath = Path.Combine(new string[] { directorryPath, $"{group.First().Group}.txt" });
                        if (!File.Exists(newFilePath))
                        {
                            File.Create(newFilePath).Close();
                        }
                        using (StreamWriter writer = File.CreateText(newFilePath))
                        {
                            foreach (Student student in group)
                            {
                                writer.WriteLine($"{student.Name}, {student.DateOfBirth}");
                            }
                        }
                    }
                }
                Console.WriteLine("Объекты успешно десериализованы.");
                Console.WriteLine("Файлы созданы.");
                Console.ReadLine();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файл {args[0]} не найден.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Путь к файлу или папке содержит недопустимые символы.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Не удаётся определить путь до каталога рабочего стола.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Недопустимое действие или недостаточно прав на чтение или запись файлов.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}