using System.Text.Json;

namespace StudentScoreManager
{
    struct ScoreBoard
    {
        public int Id { get; set; }
        public double mathematics { get; set; }
        public double english { get; set; }
        public double programming { get; set; }

        //public ScoreBoard(string Id, double mathematics, double english, double programming)
        //{
        //    this.Id = Id;
        //    this.mathematics = mathematics;
        //    this.english = english;
        //    this.programming = programming;
        //}
        public static bool IsIdContained(List<ScoreBoard> board, int Id)
        {
            foreach (ScoreBoard s in board)
            {
                if (s.Id == Id) return true;
            }
            return false;
        }
        public static int IndexById(List<ScoreBoard> board, int Id)
        {
            int res = 0;
            foreach(ScoreBoard s in board)
            {
                if (s.Id != Id) res++;
                else return res;
            }
            return -1;
        }
        //public static void SortById(List<ScoreBoard> board)
        //{
        //    if (board.Count <= 1) return;
        //    QuickSort(board, 0, board.Count - 1);
        //}
        //static void QuickSort(List<ScoreBoard> boards, int left, int right)
        //{
        //    if (left < right)
        //    {
        //        int pivotIndex = Partition(boards, left, right);
        //        QuickSort(boards, left, pivotIndex - 1);
        //        QuickSort(boards, pivotIndex + 1, right);
        //    }
        //}
        //static int Partition(List<ScoreBoard> boards, int left, int right)
        //{
        //    ScoreBoard pivot = boards[right];
        //    int i = left - 1;
        //    for (int j = left; j < right; j++)
        //    {
        //        if(boards[j].Id < pivot.Id)
        //        {
        //            i++;
        //            Swap(boards, i, j);
        //        }
        //    }
        //    Swap(boards, i + 1, right);
        //    return i + 1;
        //}
        //static void Swap(List<ScoreBoard> boards, int i, int j)
        //{
        //    var tmp = boards[i]; boards[i] = boards[j]; boards[j] = tmp;
        //}
    }
    class Program
    {
        static string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data.json");
        static string tmpPath = Path.Combine(Directory.GetCurrentDirectory(), "tmp.json");
        static void ShowBy(string sortBy)
        {
            List<ScoreBoard> scores;

            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
            {
                Console.WriteLine("Look like there is no scoreboard. Try to add anew by typing \"ssm new\"");
                return;
            }
            else
            {
                string json = File.ReadAllText(filePath);
                scores = JsonSerializer.Deserialize<List<ScoreBoard>>(json) ?? new List<ScoreBoard>();
            }
            List<ScoreBoard> sorted = sortBy switch
            {
                "id" => scores.OrderBy(s => s.Id).ToList(),
                "math" => scores.OrderBy(s => s.mathematics).ToList(),
                "eng" => scores.OrderBy(s => s.english).ToList(),
                "prog" => scores.OrderBy(s => s.programming).ToList(),
                _ => throw new ArgumentException($"Sort by \"{sortBy}\" is invalid.")
            };
            Console.WriteLine("-Begin-");
            foreach (ScoreBoard board in sorted)
            {
                Console.WriteLine($"Id = {board.Id}, Mathematics = {board.mathematics}, English = {board.english}, Programming = {board.programming}");
            }
            Console.Write("-End-");
        }
        static void Show()
        {
            Console.WriteLine("Typing \"ssm show <sort by>\". The program only supports sorting by ascending order.");
            Console.WriteLine("Here are the options: ");
            Console.WriteLine("\n\tid : Sort by Id");
            Console.WriteLine("\n\tmath : Sort by Mathematics score.");
            Console.WriteLine("\n\teng : Sort by English score.");
            Console.WriteLine("\n\tprog : Sort by Programming score.");
        }
        static void New()
        {
            List<ScoreBoard> scores;
            string json = "";

            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
            {
                scores = new List<ScoreBoard>();
            }
            else
            {
                json = File.ReadAllText(filePath);
                scores = JsonSerializer.Deserialize<List<ScoreBoard>>(json) ?? new List<ScoreBoard>();
            }

            Console.Write("Id = ");
            int id = Convert.ToInt32(Console.ReadLine());
            if (ScoreBoard.IsIdContained(scores, id))
            {
                Console.WriteLine("Id is already existed.");
                return;
            }
            Console.Write("Mathematics = ");
            if (!double.TryParse(Console.ReadLine(), out double mathematics) || mathematics < 0 || mathematics > 10)
            {
                ScoreInputError(); return;
            }
            Console.Write("English = ");
            if (!double.TryParse(Console.ReadLine(), out double english) || english < 0 || english > 10)
            {
                ScoreInputError(); return;
            }
            Console.Write("Programming = ");
            if (!double.TryParse(Console.ReadLine(), out double programming) || programming < 0 || programming > 10)
            {
                ScoreInputError(); return;
            }
            ScoreBoard sb = new ScoreBoard
            {
                Id = id,
                mathematics = mathematics,
                english = english,
                programming = programming
            };

            scores.Add(sb);

            WriteToPath(scores);
            Console.WriteLine("Successfully creat new scoreboard.");
        }
        static void Update()
        {
            string json = File.ReadAllText(filePath);
            List<ScoreBoard> scores = JsonSerializer.Deserialize<List<ScoreBoard>>(json) ?? new List<ScoreBoard>();
            Console.Write("Type Id\n>");
            int index;
            if (int.TryParse(Console.ReadLine(), out int id) && (index = ScoreBoard.IndexById(scores, id)) != -1)
            {
                Console.Write("Mathematics = ");
                if (!double.TryParse(Console.ReadLine(), out double mathematics) || mathematics < 0 || mathematics > 10)
                {
                    ScoreInputError(); return;
                }
                Console.Write("English = ");
                if (!double.TryParse(Console.ReadLine(), out double english) || english < 0 || english > 10)
                {
                    ScoreInputError(); return;
                }
                Console.Write("Programming = ");
                if (!double.TryParse(Console.ReadLine(), out double programming) || programming < 0 || programming > 10)
                {
                    ScoreInputError(); return;
                }
                scores[index] = new ScoreBoard { Id = id, mathematics = mathematics, english = english, programming = programming };
                WriteToPath(scores);
                Console.WriteLine($"\"{id}\" scoreboard has been updated.");
            }
            else IdInputError();
        }
        static void Search()
        {
            Console.Write("Type Id.\n>");
            int id = Convert.ToInt32(Console.ReadLine());
            string json = File.ReadAllText(filePath);
            List<ScoreBoard> scores = JsonSerializer.Deserialize<List<ScoreBoard>>(json) ?? new List<ScoreBoard>();

            foreach (ScoreBoard board in scores)
            {
                if (board.Id == id)
                {
                    Console.WriteLine($"Id = {board.Id}, Mathematics = {board.mathematics}, English = {board.english}, Programming = {board.programming}");
                    return;
                }
            }
            Console.WriteLine("Can't find the scoreboard with the same Id.");
        }
        static void Delete()
        {
            Console.Write("Type Id.\n>");
            int id = Convert.ToInt32(Console.ReadLine());
            string json = File.ReadAllText(filePath);
            List<ScoreBoard> scores = JsonSerializer.Deserialize<List<ScoreBoard>>(json) ?? new List<ScoreBoard>();
            int index = ScoreBoard.IndexById(scores, id);
            if (index != -1)
            {
                Console.WriteLine($"Sure to delete \"{id}\" scoreboard?");
                Console.Write("[Y]: Yes, [N]: No.\n>");
                string? choose = Console.ReadLine();
                if (choose != null && choose.ToUpper() == "Y")
                {
                    scores.RemoveAt(index);
                    WriteToPath(scores);
                    Console.WriteLine($"\"{id}\" scoreboard has been deleted.");
                }
            }
            else IdInputError();
        }
        static void Clear()
        {
            Console.WriteLine("Are you sure to clear all scoreboards?");
            Console.Write("[Y]: Yes, [N]: No.\n>");
            string? choose = Console.ReadLine();
            if (choose != null && choose.ToUpper() == "Y")
            {
                File.WriteAllText(filePath, "");
                Console.WriteLine("The scoreboards have been successfully emptied.");
                Console.WriteLine("If you want to recover the data, please type \"ssm restore\" before make any change, otherwise, the data could be deleted forever.");
            }
            else
            {
                Console.WriteLine("The clear command has been denied. Nothing was change.");
            }
        }
        static void Restore()
        {
            string tmp = File.ReadAllText(tmpPath);
            string data = File.ReadAllText(filePath);
            File.WriteAllText(filePath, tmp);
            File.WriteAllText(tmpPath, data);
            Console.WriteLine("Data have been successfully restore.");
        }
        static void About()
        {
            Console.WriteLine("Develop by Tran Thien Long, using C#.");
            Console.WriteLine("Student Score Manager(SSM) is used for storing, editing scores of student(mainly Computer Science Student).");
            Console.WriteLine("In the first ever version, there are only 3 subject which are \"Mathematics\", \"English\" and \"Programming\"");
            Console.WriteLine("It also could be my first ever usable application. Really cost a lot of work cus I make it alone.");
            Console.WriteLine("Feel free to use it!");
        }
        static void Start()
        {
            Console.WriteLine("--------Welcome to the Student Score Manager--------");
            Console.WriteLine("\nUsing this application by typing command: \"ssm <command>\" on your Terminal.");
            Console.WriteLine("\nHere is the command list:");
            Console.WriteLine("\n\tshow : Show all the score board.");
            Console.WriteLine("\n\tnew : Add new scoreboard.");
            Console.WriteLine("\n\tupdate : Update score of a student by Id");
            Console.WriteLine("\n\tsearch : Search for a scoreboard.");
            Console.WriteLine("\n\tdelete : Delete a scoreboard.");
            Console.WriteLine("\n\tclear : Delete all scoreboards.");
            Console.WriteLine("\n\trestore : Restore the last edit.");
            Console.WriteLine("\n\tabout : Information of Student Score Manager.");
            Console.WriteLine("\nFor example, try: ssm about");
        }
        private static void WriteToPath(List<ScoreBoard> scores)
        {
            SaveToTmp(File.ReadAllText(filePath));
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            string updatedContent = JsonSerializer.Serialize(scores, options);
            File.WriteAllText(filePath, updatedContent);
        }
        private static void SaveToTmp(string json)
        {
            File.WriteAllText(tmpPath, json);
        }
        private static void ScoreInputError() { Console.Write("Input is invalid or different data type."); }
        private static void IdInputError() { Console.Write("Id is invalid or non existed"); }
        static void Main(string[] args)
        {
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists) { File.Create(filePath); }
            fi = new FileInfo(tmpPath);
            if (!fi.Exists) { File.Create(tmpPath); }
            if (args.Length == 0) { Start(); return; }
            switch (args[0].ToLower())
            {
                case "show":
                    if (args.Length == 1) Show();
                    else
                    {
                        try { ShowBy(args[1].ToLower()); }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                    }
                    break;
                case "new":
                    New(); break;
                case "update":
                    Update(); break;
                case "search":
                    Search(); break;
                case "delete":
                    Delete(); break;
                case "clear":
                    Clear(); break;
                case "restore":
                    Restore(); break;
                case "about":
                    About(); break;
                default:
                    Console.Write($"Invalid commmand, please look it up on \"ssm\".");
                    break;
            }
        }
    }
}