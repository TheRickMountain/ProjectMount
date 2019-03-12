using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountPRG
{
    public class JobQueue
    {

        private Queue<Job> jobQueue;
        
        public int Count
        {
            get { return jobQueue.Count; }
        }

        public JobQueue()
        {
            jobQueue = new Queue<Job>();
        }

        public void Enqueue(Job j)
        {
            jobQueue.Enqueue(j);
        }

        public Job Dequeue()
        {
            if (jobQueue.Count == 0)
                return null;

            return jobQueue.Dequeue();
        }

        

    }
}
