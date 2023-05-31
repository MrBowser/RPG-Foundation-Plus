using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] bool StraightLine = false;
    Health target = null;
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

    public void SetTarget(Health target, float damage)
    {

        this.target = target;
        
        this.damage = damage;
    }

    private Vector3 GetAimLocation()
    {
       
        
       CapsuleCollider targetCapsuleCollider = target.GetComponent<CapsuleCollider>();
       if(targetCapsuleCollider == null ) { return target.transform.position; }

        

        return target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.GetComponent<Health>() != target) { return; }
        if (target.IsDead()) { return; }

        target.TakeDamage(damage);
        
        Destroy(gameObject);
    }

    
}
