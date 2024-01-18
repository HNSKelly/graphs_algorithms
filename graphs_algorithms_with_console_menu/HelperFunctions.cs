

using System.Threading.Tasks;

namespace CAB301_Assignment_3
{
    public class HelperFunctions
    {
        
        public static List<Tasks> ExtractTextFromTextFileToList()
        {

            string[] splitArray;
            List<Tasks> tasksResult = new List<Tasks>();
            Console.WriteLine("Please enter in a file name or path to a file containing the tasks: ");
            string? OSPath = Console.ReadLine();

            using (StreamReader reader = new StreamReader(OSPath))
            {
                string? textLine;

                while ((textLine = reader.ReadLine()) != null)
                {
                    splitArray = textLine.Split("\n");
                    for (int i = 0; i < splitArray.Length; i++)
                    {
                        string[] furtherSplitArray= splitArray[i].Split(",");
                        string taskName = furtherSplitArray[0].Trim();
                        Tasks newTask = new Tasks(taskName);
                        newTask.timeForCompletion = int.Parse(furtherSplitArray[1]);
                        if (furtherSplitArray.Length > 2)
                        {
                            for (int j = 2; j < furtherSplitArray.Length; j++)
                            {
                                newTask.Dependencies.Add(furtherSplitArray[j].Trim());

                            }
                        }
                    
                        tasksResult.Add(newTask);

                    }

                }

            }
            return tasksResult;
        }

        public static void ExportSequenceToTextFile(List<Tasks> list)
        {

            Console.WriteLine("Please enter in a file path to save the sequence: ");
            string? filePath = Console.ReadLine();

            using (StreamWriter writer = new StreamWriter(filePath + "\\" + "Sequence.txt" ))
            {
                foreach(Tasks tasks in list)
                {

                    string textOutPut = string.Format($"{tasks.nameOfTask},");
                    writer.Write(textOutPut);
                }
            }
        }

        public static void ExportEarliestTimesToTextFile(List<KeyValuePair<string, int>> list)
        {

            Console.WriteLine("Please enter in a file path to save the earliest completion times: ");
            string? filePath = Console.ReadLine();

            using (StreamWriter writer = new StreamWriter(filePath + "\\" + "EarliestTimes.txt"))
            {
                foreach (KeyValuePair<string, int> kvpair in list)
                {
                    writer.WriteLine(kvpair);
                }
            }
        }

        public static void ExportUpdatedTasks(List<Tasks> list)
        {

            Console.WriteLine("Please enter in a file path to the file, or file name to save the updated tasks: ");
            string? filePath = Console.ReadLine();

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Tasks tasks in list)
                {
                    string dep = "";
                    if (tasks.Dependencies.Count > 0)
                    {
                        dep = ", " + string.Join(",", tasks.Dependencies);
                    }
                    string textOutPut = string.Format($"{tasks.nameOfTask}, {tasks.timeForCompletion}{dep}");
                    writer.WriteLine(textOutPut);
                }
            }
        }

        public static Graph<Tasks> CreateGraph(List<Tasks> tasks)
        {
            
            Graph<Tasks> graph = new Graph<Tasks>();
            graph.graphList.Clear();

            foreach (Tasks task in tasks)
            {
                graph.InsertVertex(task);
            }
            foreach (Tasks task in tasks)
            {
                foreach (string dep in task.Dependencies)
                {

                    graph.AddEdgeToList(task, graph.LocateTask(tasks, dep.ToString().Trim()));
                }
            }
            return graph;
        }

        public static void PrintGraph(Graph<Tasks> graph)
        {
            foreach (Tasks task in graph.GetGraphList())
            {
                Console.WriteLine("Task Name: " + task.nameOfTask + "| Completion Time: " + task.timeForCompletion + "| Dependencies: " + string.Join(",", task.Dependencies));
            }

        }

        public static void PrintGraphAdjacencyList(Graph<Tasks> graph)
        {
            foreach (Tasks task in graph.GetGraphList())
            {
                Console.WriteLine(task.nameOfTask + "-->" + "{" + string.Join(",", task.Dependencies) + "}");
            }

        }

        public static void PrintListTopological(List<Tasks> list)
        {
            foreach (Tasks task in list)
            {
                Console.Write(task.nameOfTask + ", ");

            }

        }

        public static Graph<Tasks>? AddNewTask(Graph<Tasks> graph, string newTaskName, int newTime, string[] dep)
        {
            Tasks newTask = new Tasks(newTaskName);
            List<Tasks> gList = graph.GetGraphList();   
            newTask.timeForCompletion = newTime;
            if (!string.IsNullOrWhiteSpace(dep[0]))
            {
                foreach (string dp in dep)
                {
                    Tasks task = graph.LocateTask(gList, dp.ToString().Trim());
                    newTask.Dependencies.Add(task.nameOfTask);
                }

                graph.InsertVertex(newTask);

                foreach (string dp in newTask.Dependencies)
                {
                    Tasks dpTask = graph.LocateTask(gList, dp.ToString().Trim());
                    graph.AddEdgeToList(dpTask, newTask);
                }

                return graph;
            }

            graph.InsertVertex(newTask);
            return graph;
            
        }

        public static Graph<Tasks>? RemoveTask(Graph<Tasks> graph, string task)
        {
            List<Tasks> gList = graph.GetGraphList();
            Graph<Tasks> newGraph = graph;
            Tasks? requestedTaskToRemove = graph.LocateTask(gList, task);

            if(requestedTaskToRemove != null)
            {
                graph.RemoveGraphVertex(requestedTaskToRemove);

                foreach(Tasks tasks in gList)
                {
                    if (tasks.Dependencies.Contains(requestedTaskToRemove.nameOfTask.ToString()))
                    {
                        tasks.Dependencies.Remove(requestedTaskToRemove.nameOfTask);
                    }
                }
                
                return graph;
            }
            return null;
        }

        public static Graph<Tasks>? ChangeCompletionTime(Graph<Tasks> graph, string task, int newTime)
        {
            List<Tasks> tasks = graph.GetGraphList();
            Tasks? requestedTaskToEdit = graph.LocateTask(tasks, task.Trim());

            if(requestedTaskToEdit != null)
            {
                requestedTaskToEdit.timeForCompletion = newTime;
                return graph;
            }
            return null;
        }



        public static List<Tasks> TopoSortSequence(Graph<Tasks> g)
        {
            List<Tasks> resultingSequenceList = new List<Tasks>();
            HashSet<Tasks> checkedV = new HashSet<Tasks>();
            List<Tasks> gList = g.GetGraphList();

            foreach (Tasks v in gList)
            {
                if (!checkedV.Contains(v))
                {
                    TopoDepthFirst(v, gList, checkedV, resultingSequenceList);
                }
            }

            void TopoDepthFirst(Tasks task, List<Tasks> graph, HashSet<Tasks> chked, List<Tasks> result)
            {
                chked.Add(task);

                foreach (string dp in task.Dependencies)
                {
                    Tasks? dep = g.LocateTask(gList, dp.ToString().Trim());

                    if (!chked.Contains(dep))
                    {
                        TopoDepthFirst(dep, graph, chked, result);
                    }
                }

                result.Add(task);
            }

            return resultingSequenceList;
        }

        public static Dictionary<string, int> TopoSortEarliestCommencementTime(Graph<Tasks> g)
        {
            Dictionary<string, int> resultingDictionary = new Dictionary<string, int>();
            HashSet<Tasks> checkedV = new HashSet<Tasks>();
            List<Tasks> gList = g.GetGraphList();

            foreach (Tasks v in gList)
            {

                if (!checkedV.Contains(v))
                {
     
                    int time = TopoDepthFirst(v, checkedV, resultingDictionary, g);
                    if (!resultingDictionary.ContainsKey(v.nameOfTask)) 
                    {

                        resultingDictionary.Add(v.nameOfTask, time);
                    }
 
                }
 
            }

            int TopoDepthFirst(Tasks task, HashSet<Tasks> chked, Dictionary<string, int> resultingDictionary, Graph<Tasks> g)
            {
                if (chked.Contains(task))
                {
                    return resultingDictionary[task.nameOfTask];
                }

                chked.Add(task);

                int time2 = task.timeForCompletion; 
      
                foreach (string dp in task.Dependencies) 
                {

                    Tasks? dep = g.LocateTask(gList, dp.ToString().Trim()); 
                    int dpTime = TopoDepthFirst(dep, chked, resultingDictionary, g);
                    time2 = Math.Max(time2, dpTime + task.timeForCompletion);
                    int sex = Math.Min(task.timeForCompletion, time2);
                }



                resultingDictionary[task.nameOfTask] = time2;
                return time2;
            }
            if(gList.Count == resultingDictionary.Count)
            {

                Dictionary<string, int> resultingDictionaryofEarliestTimes = new Dictionary<string, int>(resultingDictionary);
                

                foreach (Tasks task in gList)
                {
                    int min = 0;
                    foreach (string dp in task.Dependencies)
                    {
                        if (resultingDictionary.ContainsKey(dp.Trim()))
                        {
                            int finTime = resultingDictionary[dp.Trim()];
                            min = Math.Max(min, finTime);
                        }
                    }
                    resultingDictionaryofEarliestTimes[task.nameOfTask] = min;
                }
                resultingDictionary = resultingDictionaryofEarliestTimes;
                resultingDictionary = resultingDictionary.OrderBy(resultingDictionary => resultingDictionary.Key).ToDictionary(resultingDictionary => 
                resultingDictionary.Key, resultingDictionary => resultingDictionary.Value);

            }
            return resultingDictionary;
        }
    }
}
