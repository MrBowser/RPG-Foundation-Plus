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
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;
        
        
        Health target;
        float timeSinceLastAttack = 0f;

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) { return; }
            if(target.IsDead()) { return; }


            //note this is a short circuit function
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }



        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack > timeBetweenAttacks) 
            {
                //note should hash in an ideal world, attach will trigger the Hit() event.
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack= 0f;
                
            }
            
        }

        //this is an Animation Event necessary for the unarmed attack, called within the animator
        void Hit()
        {
            if(target == null) { return; }

            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();

            
        }

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }

       



    }
}

