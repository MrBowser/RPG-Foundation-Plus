using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using RPG.Movement;
using RPG.Combat;
using System;
using RPG.Core;

namespace RPG.Control
{

    public class PlayerControls : MonoBehaviour
    {



        Ray lastRay;

        PlayerInput playerInput;
        Health health;
        NavMeshAgent playerMesh;

        bool moveShouldContinue;
        bool moveHasStarted;
        //note this is a bool turned on with start of click that turns offs after interact with combat runsthrough
        bool attackCheck;


        void Awake()
        {
            //note the new playerinput means we are pulling from the player input action map
            playerInput = new PlayerInput();

            playerInput.PlayerControls.ClickMove.started += _ => OnClickMoveStarted();
            playerInput.PlayerControls.ClickMove.performed += _ => OnClickToMove();
            //bote canceled triggers when the action has ended in this context it seems
            playerInput.PlayerControls.ClickMove.canceled += _ => OnClickToMoveCanceled();
        }



        private void Start()
        {
            health = GetComponent<Health>();
            playerMesh = GetComponent<NavMeshAgent>();
        }

        private void OnEnable()
        {
            playerInput.PlayerControls.Enable();
        }

        private void OnDisable()
        {
            playerInput.PlayerControls.Disable();
        }

        private void Update()
        {
            
        }

        void LateUpdate()
        {
            if (health.IsDead()) { return; }
            
            if (InteractWithCombat())
            {
                return;
            }
            InteractWithMovement();
            
        }

        private void InteractWithMovement()
        {
            if (moveShouldContinue && moveHasStarted)
            {
                bool canMove = MoveToCursor();
                
                if (canMove == false)
                {
                    ///can ignore right now just allows us to do something if no ray is hit on click
                }
            }
        }

        private void OnClickMoveStarted()
        {
            moveShouldContinue = true;
            moveHasStarted = false;
            attackCheck = true;
        }

        private void OnClickToMove()
        {
            moveHasStarted = true;
            

        }

        //note I believe canceled is the equivalent to on mouse up for this instance with the new system so will toggle the follow cursor off
        private void OnClickToMoveCanceled()
        {
            moveShouldContinue = false;
            attackCheck = false;
        }


        private bool MoveToCursor()
        {
            RaycastHit hit;
            //note out means that we are passing in hit but I think it outputs hit and the associated info based on ray if it does so successfully returns true
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                GetComponent<Mover>().StartMoveAction(hit.point,1f);
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
               CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if(target == null) { continue; }

                
                //note continute means that we end this iteration of hit with the foreach loop and move on to the next hit in hits, 
                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) { continue; }

                if(attackCheck)
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                    //note I have moved the false attackCheck to when the mouse lifts up, this means I will attack when scrolling over an enemy
                    //the explicit click feels better but am keeping this way to match tutorial
                    //attackCheck = false;
                }
                return true;
            }
            //attackCheck = false;
            return false;
        }



    }

}

//the below line shows a drawn ray in the editor for testing was originonally in MoveToCursor()
//Debug.DrawRay(ray.origin, ray.direction * 100);

/* note, below is old code, this used to be in OnClickToMove()
           if(attackCheck == false)
           {
              MoveToCursor();
               print("this fired");                 
           }
           */