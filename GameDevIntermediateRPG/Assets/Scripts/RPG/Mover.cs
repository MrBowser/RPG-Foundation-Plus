
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using UnityEngine.UIElements;
using RPG.Attributes;

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

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }


        public object CaptureState()
        {
           MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            //note serializable vector 3 is a class script in saving
            return data;
            
        }

        public void RestoreState(object state)
        {

            MoverSaveData data = (MoverSaveData)state;


            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();

            GetComponent<NavMeshAgent>().enabled = true;

        }
    }


}

