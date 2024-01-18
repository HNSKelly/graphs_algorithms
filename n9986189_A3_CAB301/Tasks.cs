using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAB301_Assignment_3
{

    public class Tasks
    {
        private string task;
        private int time;
        private List<string> dependencies;

        public string nameOfTask { get { return task; } private set {  task = value; } }

        public int timeForCompletion { get { return time; } set {  time = value; } }

        public List<string> Dependencies { get { return dependencies; } set { dependencies = value; } }


        public Tasks(string taskID)
        {

            nameOfTask = taskID;
            timeForCompletion = 0;
            Dependencies = dependencies ?? new List<string>();
        }

    }
}
