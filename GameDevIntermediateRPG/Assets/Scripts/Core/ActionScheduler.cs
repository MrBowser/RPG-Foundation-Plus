using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{

    //note I believe this is going to help with a dependency cycle

    public class ActionScheduler : MonoBehaviour
    {
        IAction currentAction;

        public void StartAction(IAction action)
        {
            if(currentAction == action) { return; }
            if(currentAction != null)
            {
                currentAction.Cancel();
                
            }
            currentAction = action;



        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }

}

