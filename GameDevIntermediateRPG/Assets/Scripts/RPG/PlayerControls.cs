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
using RPG.Attributes;
using UnityEngine.EventSystems;
using RPG.Control;

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

        public bool GetMoveShouldContinue { get { return moveShouldContinue; } }
        public bool GetAttackCheck { get { return attackCheck; } }

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNavMeshProjectionDistance = 1;
        [SerializeField] float rayCastRadius = .5f;


        void Awake()
        {
            //note the new playerinput means we are pulling from the player input action map
            playerInput = new PlayerInput();

            playerInput.PlayerControls.ClickMove.started += _ => OnClickMoveStarted();
            playerInput.PlayerControls.ClickMove.performed += _ => OnClickToMove();
            //bote canceled triggers when the action has ended in this context it seems
            playerInput.PlayerControls.ClickMove.canceled += _ => OnClickToMoveCanceled();

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

        void LateUpdate()
        {
            if(InteractWithUI()) { return; }
            if (health.IsDead()) { SetCursor(CursorType.None); return; }
            if(InteractWithComponent()) { return; }

            InteractWithMovement();
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRayCast(this))
                    {
                        SetCursor(raycastable.GetCursorType());

                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(),1);
            
            float[] distances = new float[hits.Length];
            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
            return hits; 
        }

        private bool InteractWithUI()
        {
            //note this refers to only ui systems
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private void InteractWithMovement()
        {
            if (moveShouldContinue && moveHasStarted)
            {
                bool canMove = MoveToCursor();
               
                if (canMove == false)
                {
                    ///can ignore right now just allows us to do something if no ray is hit on click
                    SetCursor(CursorType.NoMove);
                }
                else
                {
                    SetCursor(CursorType.Movement);           
                }
            }
            else
            {
                SetCursor(CursorType.None);
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
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if(GetComponent<Mover>().CanMoveTo(target) == false)
                {
                    return false;
                }

                GetComponent<Mover>().StartMoveAction(target,1f);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            //note out means that we are passing in hit but I think it outputs hit and the associated info based on ray if it does so successfully returns true
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if(!hasHit) { return false; }

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh =  NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);

            if(!hasCastToNavMesh) { return false; }
            target = navMeshHit.position;

            return true;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private void SetCursor(CursorType cursorType)
        {
            CursorMapping mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == cursorType)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
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



/*
private bool InteractWithCombat()
{
    RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
    foreach (RaycastHit hit in hits)
    {

    }

    //attackCheck = false;
    return false;
}
*/

/* Late Update
if (InteractWithCombat())
{
    return;
}
*/