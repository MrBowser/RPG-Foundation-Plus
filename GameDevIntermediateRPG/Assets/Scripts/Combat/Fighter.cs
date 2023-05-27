using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    //important note, this is nifty since we are able to use this for both the player controls and AI controls, this is due to the modular nature of the code
    public class Fighter : MonoBehaviour, IAction
    {

        
        [SerializeField] float timeBetweenAttacks = 1f;
        
        [SerializeField] Weapon weapon = null;
        
        [SerializeField] Transform handTransform = null;
       
        
        
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;



        private void Start()
        {
            if(gameObject.tag == "Player")
            {
                SpawnWeapon();
            }
            
        }

        

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) { return; }
            if(target.IsDead()) { return; }


            //note this is a short circuit function
            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position,.9f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void SpawnWeapon()
        {
            if(weapon == null) { return; }
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(handTransform, animator);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) { return false; }

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();

            
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                TriggerAttack();
                timeSinceLastAttack = 0f;

            }

        }

        private void TriggerAttack()
        {
            //note should hash in an ideal world, attach will trigger the Hit() event.
            //note we need to reset trigger for stopAttack since sometime when we cancel an attack the trigger may not have been consumed
            //and if this is live at the start of an animation it can cause errors
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //this is an Animation Event necessary for the unarmed attack, called within the animator
        private void Hit()
        {
            if (target == null) { return; }

            target.TakeDamage(weapon.GetWeaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) <= weapon.GetWeaponRange;
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

    }
}

