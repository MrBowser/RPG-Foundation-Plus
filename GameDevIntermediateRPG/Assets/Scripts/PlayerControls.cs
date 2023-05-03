using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    [SerializeField] Transform target;

    Ray lastRay;
    
    PlayerInput playerInput;

    NavMeshAgent playerMesh;

    bool isRightClickedPressed;

     
    void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.PlayerControls.ClickMove.performed += _ => MoveBool();
        


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

    void Update()
    {
        PlayerActionToTake();
    }


    private void PlayerActionToTake()
    {
        if(isRightClickedPressed)
        {

            MoveToCursor();
                
            
            isRightClickedPressed= false;
        }
        
    }

    private void MoveBool()
    {
        isRightClickedPressed = true;
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
}
