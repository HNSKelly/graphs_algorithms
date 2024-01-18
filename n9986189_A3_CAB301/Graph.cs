using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAB301_Assignment_3
{
    public class Graph<T>
    {

        public Dictionary<T, List<T>> graphList;

        public Graph()
        {
            graphList = new Dictionary<T, List<T>>();

        }

        private bool CheckForVertex(T v)
        {
            if(v == null)
            {
                return false;
            }
            return graphList.ContainsKey(v);
        }

        public void InsertVertex(T v)
        {
            if(!CheckForVertex(v))
            {
                graphList[v] = new List<T>();
            }
        }

        public void AddEdgeToList(T d, T s) {
            
            if(CheckForVertex(s) && CheckForVertex(d))
            {
                graphList[s].Add(d);
            }

            
        }

        public void RemoveGraphVertex(T v)
        {
            graphList.Remove(v);
        }
        public void RemoveEdgeFromList(T s, T d)
        {
            if (graphList.ContainsKey(s))
            {
                graphList[s].Remove(d); 
            }
        }

        public List<T> GetGraphList()
        {
            return new List<T>(graphList.Keys);
        }

        public Tasks? LocateTask(List<Tasks> list, string taskName)
        {
     
            foreach(Tasks task in list)
            {
                if(taskName == task.nameOfTask)
                {
                    return task;
                }
            }
            return null;
        }

        public bool IsEmpty() 
        { 
            return graphList == null || graphList.Count == 0;
        }


        
    }
}
