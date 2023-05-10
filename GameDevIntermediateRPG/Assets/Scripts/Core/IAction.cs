using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core 
{
    public interface IAction
    {
        //note everything in an interface is public and an interface can only be methods and properties
        void Cancel();
    }

}


