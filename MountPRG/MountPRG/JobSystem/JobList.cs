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

        public void Update()
        {
            for (int i = 0; i < Count; i++)
                if (Get(i).JobState == JobState.COMPLETED)
                    Remove(i);
        }

        public void Add(Job j)
        {
            jobList.Add(j);
        }

        public Job Get(int i)
        {
            if (i >= Count)
                return null;

            return jobList[i];
        }

        public void Remove(int i)
        {
            jobList.Remove(jobList[i]);
        }

        public void Remove(Job job)
        {
            jobList.Remove(job);
        }

    }
}
