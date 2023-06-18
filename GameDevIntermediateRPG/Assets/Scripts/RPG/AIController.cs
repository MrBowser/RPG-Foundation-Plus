using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggravatedCDTime = 4f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wayPointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 2f;
        [Range(0f, 1f)][SerializeField] float patrolSpeedFraction = .5f;
        [SerializeField] float shoutDistance = 5f;

        Fighter fighter;
        GameObject player;
        Health health;
        Mover mover;

        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer =Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggravated = Mathf.Infinity;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            player = GameObject.FindGameObjectWithTag("Player");
            mover = GetComponent<Mover>();

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }

        private Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) { return; }

            if (IsAggravated() && fighter.CanAttack(player))
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }
            UpdateTimers();
        }

        
        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggravated += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition.value;

            if(patrolPath!= null)
            {
                if(AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if(timeSinceArrivedAtWaypoint > waypointDwellTime )
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < wayPointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWayPoint(currentWaypointIndex);
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggravateNearbyEnemies();
        }

        private void AggravateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if(ai == null) { continue; }

                ai.Aggravate();

            }
        }

        public void Aggravate()
        {
            timeSinceAggravated = 0;
        }


        private bool IsAggravated()
        {

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggravated < aggravatedCDTime ;
        }

        //this is a special method that is called by Unity like start or update, shows gizmos when object is selected
        private void OnDrawGizmosSelected()
        {
            //this draws the chase distance on an enemy
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
