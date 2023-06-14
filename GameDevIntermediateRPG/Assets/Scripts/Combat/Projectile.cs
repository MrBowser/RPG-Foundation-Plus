using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.Events;


namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float projectileSpeed = 1f;
        [SerializeField] bool StraightLine = false;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifetime = 10f;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2f;
        [SerializeField] UnityEvent onHit;

        Health target = null;
        GameObject instigator = null;
        float damage = 0f;





        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }


        void Update()
        {
            if (target == null) { return; }

            if (StraightLine == false && !target.IsDead())
            {
                transform.LookAt(GetAimLocation());
            }
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);


        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.instigator = instigator;

            this.target = target;

            this.damage = damage;

            Destroy(gameObject, maxLifetime);
        }

        private Vector3 GetAimLocation()
        {


            CapsuleCollider targetCapsuleCollider = target.GetComponent<CapsuleCollider>();
            if (targetCapsuleCollider == null) { return target.transform.position; }



            return target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.GetComponent<Health>() != target) { return; }
            if (target.IsDead()) { return; }

            target.TakeDamage(instigator,damage);

            projectileSpeed = 0f;

            onHit.Invoke();

            if (hitEffect != null)
            {
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach (GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }


    }
}

