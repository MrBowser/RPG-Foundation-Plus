using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    

    Ray lastRay;
    
    PlayerInput playerInput;

    NavMeshAgent playerMesh;

    bool moveShouldContinue;
    bool moveHasStarted;

     
    void Awake()
    {
        //note the new playerinput means we are pulling from the player input action map
        playerInput = new PlayerInput();

        playerInput.PlayerControls.ClickMove.started += _ => OnClickMoveStarted();
        playerInput.PlayerControls.ClickMove.performed += _ => OnClickToMove();

        //bote canceled triggers when the action has ended in this context it seems
        playerInput.PlayerControls.ClickMove.canceled += _ => OnClickToMoveCanceled();


        //playerInput.PlayerControls.ClickMove. += _ => NoMoveBool();



    }

    private void Start()
    {
        playerMesh= GetComponent<NavMeshAgent>();
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
        if(moveShouldContinue && moveHasStarted)        
        {
            MoveToCursor();
        }

        
        UpdateAnimator();
    }



    private void OnClickMoveStarted()
    {
        moveShouldContinue= true;
        moveHasStarted= false;
    }

    private void OnClickToMove()
    {
        moveHasStarted = true;
        MoveToCursor();

    }

    //note I believe canceled is the equivalent to on mouse up for this instance with the new system so will toggle the follow cursor off
    private void OnClickToMoveCanceled()
    {
        moveShouldContinue= false;
    }




    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        //note out means that we are passing in hit but I think it outputs hit and the associated info based on ray if it does so successfully returns true
       bool hasHit = Physics.Raycast(ray, out hit);

        if(hasHit)
        {
            playerMesh.destination = hit.point;
        }

        //the below line shows a drawn ray in the editor for testing
        //Debug.DrawRay(ray.origin, ray.direction * 100);
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = playerMesh.velocity;
        //we convert from global to local because we need to grab the global coordinates/velocity within worldspace but animator just needs to know
        //if we are just going forward, so this translates it at rate of change versus getting us coordinates of change / global values (z is the player forward)
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        


    }
}
