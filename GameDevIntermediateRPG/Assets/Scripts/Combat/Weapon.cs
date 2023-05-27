using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] bool isRightHanded = true;

        public void Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            if(equippedPrefab!= null)
            {
                Transform handTransform;
                if(isRightHanded) 
                {
                    handTransform = rightHandTransform; 
                }
                else
                {
                    handTransform = leftHandTransform; 
                }

                Instantiate(equippedPrefab, handTransform);               
            }
            if(animatorOverride != null) 
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            


        }

       public float GetWeaponRange { get { return weaponRange; } }
       public float GetWeaponDamage { get { return weaponDamage; } }
        
    }


}
