using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace PhoneNumberDuplicateSearch
{
    class Program
    {
        //global variables
        static bool globalRestart = false;
        static bool globalRemoved = false;
        static IEnumerable<string> nonDupes;
        static string name;
        static int globalCount = 0;
        static int globalAfterCount = 0;
        static int globalRemoveCount = 0;

        /// <summary>
        /// Prints the result screen. Uses some global variables
        /// </summary>
        public static void printResults() {
            Console.Clear();
            Console.WriteLine("\n**********************************************");
            Console.WriteLine(@"                            .__   __          ");
            Console.WriteLine(@"_______   ____   ________ __|  |_/  |_  ______");
            Console.WriteLine(@"\_  __ \_/ __ \ /  ___/  |  \  |\   __\/  ___/");
            Console.WriteLine(@" |  | \/\  ___/ \___ \|  |  /  |_|  |  \___ \ ");
            Console.WriteLine(@" |__|    \___  >____  >____/|____/__| /____  >");
            Console.WriteLine(@"             \/     \/                     \/ ");
            Console.WriteLine("\n\n---Original entries: " + globalCount + "---");
            Console.WriteLine("---Duplicates found: " + globalAfterCount + "---");
            if (globalRemoved) {
                globalCount = globalCount - globalAfterCount;
                Console.WriteLine("---After removing the duplicates: " + globalCount + "---");
            }
            Console.WriteLine("\n**********************************************");
            Console.WriteLine("\nDone");
            Console.ReadLine();
            return;
        }
        /// <summary>
        /// Creates the file with a custom name with numbers that are not duplicates
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="duplicates"></param>
        /// <param name="newNumbers"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async Task createFile(List<string> lines, List<string> duplicates, List<string> newNumbers, string name) {
            globalRemoved = true;
            int count = 0;
            string yn;
            bool restart = false;
            Console.WriteLine("\nCreating file");
            nonDupes = lines.Except(duplicates);
            foreach(string value in nonDupes) {
                newNumbers.Add(value);
                count++;
                if(count == 1) {
                    Console.WriteLine("Wrote 1 line");
                }
                else {
                    Console.WriteLine("Wrote " + count + " lines");
                }
            }
            for (int i = 0; i < duplicates.Count; i++) { //restores duplicates in single form
                newNumbers.Add(duplicates[i]);
                count++;
                Console.WriteLine("Wrote " + count + " lines");
            }
            try {
                await File.WriteAllLinesAsync(name, newNumbers);
                Console.WriteLine("\nFile created successfully");
                if (globalRestart) {
                    Console.WriteLine("Press [ENTER] to continue...");
                    Console.ReadLine();
                    printResults();
                }
                do {
                    Console.WriteLine("\nWould you like to scan the new file? Y / N");
                    yn = Console.ReadLine();
                    if (yn == "yes".ToLower() || yn == "y".ToLower()) {
                        restart = false;
                        globalRestart = true;
                    }
                    else if (yn == "no".ToLower() || yn == "n".ToLower()) {
                        restart = false;
                        printResults();
                    }
                    else {
                        Console.WriteLine("\nInvalid input");
                        restart = true;
                    }
                } while (restart);
            }
            catch {
                Console.WriteLine("Something went wrong");
            }
        }
        /// <summary>
        /// Prompts the user if they want to create a new file. 
        /// </summary>
        /// <param name="duplicates"></param>
        /// <param name="lines"></param>
        /// <param name="newNumbers"></param>
        private static void createFileQuestion(List<string> duplicates, List<string> lines, List<string> newNumbers) {
            string yn;
            bool restart;
            do {
                Console.WriteLine("Would you like to create a new file without duplicates? Y / N");
                yn = Console.ReadLine();
                if (yn == "yes".ToLower() || yn == "y".ToLower()) {
                    Console.WriteLine("\nName for new file [make sure to include the .txt extension]: ");
                    name = Console.ReadLine();
                    restart = false;
                    _ = createFile(lines, duplicates, newNumbers, name);
                }
                else if (yn == "no".ToLower() || yn == "n".ToLower()) {
                    restart = false;
                    printResults();
                }
                else {
                    Console.WriteLine("\nInvalid input");
                    restart = true;
                }
            } while (restart);
        }
        /// <summary>
        /// Checks for duplicates in the 'lines' list
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="duplicates"></param>
        /// <param name="newNumbers"></param>
        /// <param name="count"></param>
        private static void checkForDuplicates(List<string> lines, List<string> duplicates, List<string> newNumbers, int count) {
            //check for duplicates
            globalAfterCount = 0;
            globalRemoveCount = 0;
            if (lines.Distinct().Count() != lines.Count()) {
                Console.WriteLine("\nA duplicate(s) was found\n");
                Console.WriteLine("**************");
                for (int i = 0; i < lines.Count; i++) {
                    for (int j = i + 1; j < lines.Count; j++) {
                        if ((lines[i].Equals(lines[j])) && (i != j)) {
                            duplicates.Add(lines[j]);
                            count++;
                            globalAfterCount++;
                            globalRemoveCount++;
                            Console.WriteLine(lines[j] + "*");
                        }
                    }
                }
                Console.WriteLine("**************");
                Console.WriteLine("\nTotal duplicates found: " + count + "\n");
                createFileQuestion(duplicates, lines, newNumbers);
            }
            else {
                Console.WriteLine("No duplicates were found");
                Console.WriteLine("Press [ENTER] to continue...");
                Console.ReadLine();
                printResults();
            }
        }
        /// <summary>
        /// Prints out that the file was found and prints the phone numbers
        /// </summary>
        /// <param name="list"></param>
        /// <param name="fullPath"></param>
        /// <param name="fileName"></param>
        private static void numbersOverview(List<string> list, string fullPath, string fileName) {
            globalCount = 0;
            for(int i = 0; i < list.Count; i++) {
                Console.WriteLine(list[i]);
                globalCount++;
            }
            Console.WriteLine("\nFile found");
            Console.WriteLine("[" + globalCount + " entries found]");
            Console.WriteLine("\nPress [ENTER] to continue...");
            Console.ReadLine();
        }
        /// <summary>
        /// main method
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //define misc variables
            int count = 0;
            bool restart;

            //define file information variables
            string filePath = "";
            string fileName = "";
            string fullPath = "";

            //define lists
            List<string> lines = new List<string>();
            List<string> duplicates = new List<String>();
            List<string> newNumbers = new List<String>();

            Console.WriteLine(@"________               .__  .__               __  .__                      ");
            Console.WriteLine(@"\______ \  __ ________ |  | |__| ____ _____ _/  |_|__| ____   ____   ______");
            Console.WriteLine(@" |    |  \|  |  \____ \|  | |  _/ ___\\__  \\   __|  |/  _ \ /    \ /  ___/");
            Console.WriteLine(@" |    `   |  |  |  |_> |  |_|  \  \___ / __ \|  | |  (  <_> |   |  \\___ \ ");
            Console.WriteLine(@"/_______  |____/|   __/|____|__|\___  (____  |__| |__|\____/|___|  /____  >");
            Console.WriteLine(@"        \/      |__|                \/     \/                    \/     \/ ");
            Console.WriteLine("\n\n\n-----NOTE-----\n\nThis only reads .txt files and not .docx[Word] files.");
            Console.WriteLine("Copy and paste all of the numbers from the Word file into a new .txt file.");
            Console.WriteLine("This program will slightly switch the positions for some of the phone numbers\nif there are duplicates. This will not change the actual phone numbers themselves.");
            Console.WriteLine("You can also use this program to compare other items in a text file\nas long as you have each new entry on a new line.");

            globalRemoved = false;

            //get the file path and the file name
            do {
                try {
                    restart = false;
                    Console.WriteLine("\nCopy and paste the file directory that the phone numbers are in");
                    Console.WriteLine("For example:");
                    Console.WriteLine(@"C:\Users\zacth\Desktop");
                    Console.WriteLine();
                    Console.WriteLine("[Right-click in the terminal to paste]");
                    Console.WriteLine("Then hit [ENTER]");
                    Console.WriteLine("\n");
                    filePath = Console.ReadLine();
                    Console.WriteLine("\nWhat is the exact filename? [including the file extension]");
                    Console.WriteLine("For example:");
                    Console.WriteLine("NewPhoneNumbers.txt\n");
                    fileName = Console.ReadLine();

                    //combine the filepath with the filename and assign each line in the file to a string array
                    fullPath = String.Concat(filePath, @"\", fileName);
                    lines = File.ReadAllLines(fullPath).ToList();
                    lines = lines.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                }
                catch {
                    Console.Clear();
                    Console.WriteLine("\nSomething went wrong. Maybe invalid file path or invalid name?");
                    restart = true;
                }
            } while (restart);

            numbersOverview(lines, fullPath, fileName);
            checkForDuplicates(lines, duplicates, newNumbers, count);

            if (globalRestart) {
                Console.WriteLine("\nChecking the new file");
                fullPath = String.Concat(filePath, @"\", name);
                duplicates.Clear();
                lines = newNumbers;
                newNumbers.Clear();
                count = 0;
                checkForDuplicates(lines, duplicates, newNumbers, count);
            }
        }
    }
}
