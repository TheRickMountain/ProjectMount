using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class JobList
    {
        private List<Job> jobList;
        
        public int Count
        {
            get { return jobList.Count; }
        }

        public JobList()
        {
            jobList = new List<Job>();
        }

        public void Add(Job j)
        {
            jobList.Add(j);
        }

        public Job Get(int i)
        {
            return jobList[i];
        }

        public void Remove(int i)
        {
            jobList.Remove(jobList[i]);
        }

    }
}
