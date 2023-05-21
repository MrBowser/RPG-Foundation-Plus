
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement 
{

    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed =6f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake()
        {
            navMeshAgent= GetComponent<NavMeshAgent>();
            
        }

        private void Start()
        {
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();

            UpdateAnimator();
        }



        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = Mathf.Clamp01(speedFraction) * maxSpeed;
            navMeshAgent.isStopped = false;

        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }


        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            //we convert from global to local because we need to grab the global coordinates/velocity within worldspace but animator just needs to know
            //if we are just going forward, so this translates it at rate of change versus getting us coordinates of change / global values (z is the player forward)
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);

        }

        public object CaptureState()
        {
            //note serializable vector 3 is a class script in saving
            return new SerializableVector3(transform.position);
            
        }

        public void RestoreState(object state)
        {
           SerializableVector3 position = (SerializableVector3)state;


            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;

        }
    }


}

