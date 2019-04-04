using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MountPRG
{
    public class SleepJob : Job
    {

        public SleepJob() : base(null, JobType.SLEEP)
        {

        }



        public override void DoJob(SettlerControllerCmp settler, GameTime gameTime)
        {
            switch(CurrentTask.TaskType)
            {
                case TaskType.SLEEP:
                    {
                        settler.Sprite.CurrentAnimation = AnimationKey.Sleep;
                        settler.AnimationState = AnimationState.MOVE;
                    }
                    break;
            }
        }

        public override void CheckJob(SettlerControllerCmp settler)
        {
            settler.SettlerState = SettlerState.WORKING;

            Tasks.Add(new Task(TaskType.SLEEP, null, 0));
            CurrentTask = Tasks[0];
        }

    }
}
