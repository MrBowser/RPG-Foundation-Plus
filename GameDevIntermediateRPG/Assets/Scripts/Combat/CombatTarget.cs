using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Attributes;
using RPG.Control;
using UnityEngine.InputSystem.HID;

namespace RPG.Combat 
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRayCast(PlayerControls callingController)
        {
            //note continue means that we end this iteration of hit with the foreach loop and move on to the next hit in hits, 
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject)) { return false; }

            if (callingController.GetAttackCheck ==true)
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
                //note I have moved the false attackCheck to when the mouse lifts up, this means I will attack when scrolling over an enemy
                //the explicit click feels better but am keeping this way to match tutorial
                //attackCheck = false;
            }
            
            return true;
        }

    }
}

