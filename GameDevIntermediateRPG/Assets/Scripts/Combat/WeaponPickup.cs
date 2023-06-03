using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace RPG.Combat
{
    
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                //Destroy(gameObject);
                StartCoroutine(HideForSeconds(respawnTime));
            }

        }


        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);

            yield return new WaitForSeconds(seconds);

            ShowPickup(true);
        }


        private void ShowPickup(bool pickUpStatus)
        {
            this.GetComponent<Collider>().enabled = pickUpStatus;

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(pickUpStatus);
            }




        }


    }

}
