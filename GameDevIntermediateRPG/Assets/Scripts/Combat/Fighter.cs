using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{

    public class Fighter : MonoBehaviour, IAction
    {

        [SerializeField] float weaponRange = 2f;
        Transform target;
        

        void Update()
        {
            if(target == null) { return; }
            
            //note this is a short circuit function
            
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }



        }

        private void AttackBehavior()
        {
            //note should hash in an ideal world
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) <= weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;

            
        }

        public void Cancel()
        {
            target = null;
        }

        //this is an Animation Event necessary for the unarmed attack, called within the animator
        void Hit()
        {

        }



    }
}

