using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{

    public class Fighter : MonoBehaviour
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
                GetComponent<Mover>().Stop();
            }


        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) <= weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;

            Debug.Log("takethat");
        }

        public void Cancel()
        {
            target = null;
        }



    }
}

