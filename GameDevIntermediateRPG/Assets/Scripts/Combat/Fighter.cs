using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;


namespace RPG.Combat
{
    //important note, this is nifty since we are able to use this for both the player controls and AI controls, this is due to the modular nature of the code
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {

        
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Weapon defaultweapon = null;

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        //[SerializeField] string defaultWeaponName = "Unarmed";

        Weapon currentWeapon = null;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;



        private void Start()
        {
            //note I think this is special and directly links to a Resources folder // Resources folders are treated uniquely in unity
            //Weapon weapon = Resources.Load<Weapon>(defaultWeaponName);
            
            if(currentWeapon== null)
            {
                EquipWeapon(defaultweapon);
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

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon= weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return target;
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

            if(currentWeapon.HasProjectile() == true)
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target, gameObject);
            }
            else
            {
                target.TakeDamage(gameObject ,currentWeapon.GetWeaponDamage);
            }

            

        }

        private void Shoot()
        {
            if (target == null) { return;}
            Hit();
        }

        private bool GetIsInRange()
        {
            
            return Vector3.Distance(transform.position, target.transform.position) <= currentWeapon.GetWeaponRange;
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

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}

