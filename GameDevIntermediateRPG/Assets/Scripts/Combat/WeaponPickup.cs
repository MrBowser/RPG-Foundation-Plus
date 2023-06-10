using RPG.Control;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace RPG.Combat
{
    
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;

        

        private void Awake()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }

        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            //Destroy(gameObject);
            StartCoroutine(HideForSeconds(respawnTime));
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

        public bool HandleRayCast( PlayerControls playerControls)
        {
            if(playerControls.GetMoveShouldContinue)
            {
                //below equips weapon on click
                //Pickup(playerControls.GetComponent<Fighter>());

                //below line is not in the course, this is added cause I hate the click to pick up versus collider method
                playerControls.GetComponent<Mover>().StartMoveAction(this.transform.position,1);
            }

            

            return true;
            
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }

}
