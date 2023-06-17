using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Combat
{
    //note this was renamed weaponConfig in gamedev lectures

    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] WeaponCore equippedPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float percentageDamageBonus = 0;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        public float GetWeaponRange { get { return weaponRange; } }
        public float GetWeaponDamage { get { return weaponDamage; } }
        public float GetPercentageDamageBonus { get { return percentageDamageBonus; } }

        const string weaponName = "Weapon";

        public WeaponCore Spawn(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {

            DestroyOldWeapon(rightHandTransform,leftHandTransform);

            WeaponCore weapon = null;
            if(equippedPrefab!= null)
            {
                Transform handTransform = GetTransform(rightHandTransform, leftHandTransform);

                weapon = Instantiate(equippedPrefab, handTransform);
                weapon.gameObject.name= weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (animatorOverride != null) 
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null) { return; }

            oldWeapon.name = "Destroying";

            Destroy(oldWeapon.gameObject);
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand,leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        private Transform GetTransform(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHandTransform;
            }
            else
            {
                handTransform = leftHandTransform;
            }
            return handTransform;
        }

    }


}
