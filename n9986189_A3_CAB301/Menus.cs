
namespace CAB301_Assignment_3
{
    public class Menus
    {
        private static Graph<Tasks> graph;
        private static List<Tasks> tasks;
        private static List<Tasks> topologicalSorted;

        public static void Menu()
        {

            bool valid = false;
            
            while (!valid)
            {
                Console.WriteLine("1. Create new graph from task list.");
                Console.WriteLine("2. Display current tasks.");
                Console.WriteLine("3. Show Adjacency List.");
                Console.WriteLine("4. Show topological sorted list.");
                Console.WriteLine("5. Remove task from graph.");
                Console.WriteLine("6. Add a task.");
                Console.WriteLine("7. Change completion time of task.");
                Console.WriteLine("8. Save updated and export graph.");
                Console.WriteLine("9. Find task sequence and export.");
                Console.WriteLine("10. Find earliest task times and export.");
                Console.WriteLine("0. Exit.");
                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        tasks = HelperFunctions.ExtractTextFromTextFileToList();
                        graph = HelperFunctions.CreateGraph(tasks);
                        Console.WriteLine("Graph Created!");
                        ReturnToMain();
                        valid = true;
                        break;
                    case "2":
                        HelperFunctions.PrintGraph(graph);
                        ReturnToMain();
                        valid = true;
                        break;
                    case "3":
                        HelperFunctions.PrintGraphAdjacencyList(graph);
                        ReturnToMain();
                        valid = true;
                        break;
                    case "4":
                        topologicalSorted = HelperFunctions.TopoSortSequence(graph);
                        HelperFunctions.PrintListTopological(topologicalSorted);
                        ReturnToMain();
                        valid = true;
                        break;
                    case "5":
                        TaskToRemove();
                        valid = true;
                        break;
                    case "6":
                        TaskToAdd();
                        valid = true;
                        break;
                    case "7":
                        ChangeCompTime();
                        valid = true;
                        break;
                    case "8":
                        SaveAndExportUpdatedTasks(graph);
                        valid = true;
                        break;
                    case "9":
                        SaveSequenceAndExport(graph);
                        valid = true;
                        break;
                    case "10":
                        SaveEarliestTimesAndExport(graph);
                        valid = true;
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        ReturnToMain();
                        break;
                }
            }
            


        }
        public static void Return()
        {
            Console.Clear();
            Menu();
        }

        public static void ReturnToMain()
        {
            bool valid = false;
            while (!valid)
            {
                Console.WriteLine();
                Console.WriteLine("1. Return to main menu.");
                int option = int.Parse(Console.ReadLine());
                Console.WriteLine();
                switch (option)
                {
                    case 1:
                        valid = true;
                        Return();
                        break;
                    default:
                        Console.WriteLine("Invalid option, please choose again.");
                        break;
                }
            }
        }

        public static void SaveSequenceAndExport(Graph<Tasks> graph)
        {
            List<Tasks> tasks = HelperFunctions.TopoSortSequence(graph);
            HelperFunctions.ExportSequenceToTextFile(tasks);
            ReturnToMain() ;

        }

        public static void SaveEarliestTimesAndExport(Graph<Tasks> graph)
        {
            Dictionary<string, int> newDict = HelperFunctions.TopoSortEarliestCommencementTime(graph);
            List<KeyValuePair<string, int>> exportList = newDict.ToList();
            HelperFunctions.ExportEarliestTimesToTextFile(exportList);
            ReturnToMain();
        }

        public static void SaveAndExportUpdatedTasks(Graph<Tasks> graph)
        {
            List<Tasks> tasks = graph.GetGraphList();
            HelperFunctions.ExportUpdatedTasks(tasks);
            ReturnToMain();
        }

        public static void TaskToRemove()
        {
            Graph<Tasks> newGraph = graph;
            Console.WriteLine();
            Console.WriteLine("Please enter the name of a task to remove");
            string task = Console.ReadLine().Trim();
            Console.WriteLine();
            newGraph = HelperFunctions.RemoveTask(newGraph, task);
            graph = newGraph;
            Console.WriteLine("Task Removed.");
            HelperFunctions.PrintGraph(newGraph);
            ReturnToMain();

            

        }

        public static void TaskToAdd()
        {
            string[] depArray;
            Graph<Tasks> newGraph = graph;
            Console.WriteLine();
            Console.WriteLine("Please enter the name of a task to Add: ");
            string task = Console.ReadLine().Trim();
            Console.WriteLine();
            Console.WriteLine("Please enter in a completion time for this task: ");
            int compTime = int.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine("Please enter in the new task's dependencies separated by ',': ");
            Console.WriteLine();
            depArray = Console.ReadLine().Split(",");
            HelperFunctions.AddNewTask(graph, task, compTime, depArray);
            graph = newGraph;
            Console.WriteLine("Task Added.");
            HelperFunctions.PrintGraph(graph);
            ReturnToMain();



        }

        public static void ChangeCompTime()
        {
            Graph<Tasks> newGraph = graph;
            Console.WriteLine();
            Console.WriteLine("Please enter the name of a task to to update: ");
            string task = Console.ReadLine().Trim();
            Console.WriteLine();
            Console.WriteLine("Please enter in a new completion time for this task: ");
            int compTime = int.Parse(Console.ReadLine());
            Console.WriteLine();
            newGraph = HelperFunctions.ChangeCompletionTime(graph, task, compTime);
            graph = newGraph;
            HelperFunctions.PrintGraph(graph);
            Console.Write($"Completion time has been changed.");
            ReturnToMain();




        }


    }
}
