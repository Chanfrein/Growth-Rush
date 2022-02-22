using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat.Stats;
using Core;
using System.Threading.Tasks;
using UnityEngine.AI;
using System.Threading;

namespace Control.AIControl
{
    public class AIController : MonoBehaviour
    {
        [Header("Patrol Settings")]
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float walkSpeedFraction = 0.5f;

        [Header("Aggro Settings")]
        [SerializeField] float aggroRange = 5f;
        [SerializeField] LayerMask enemiesLayer;

        Combatant combat;
        Mover mover;
        
        private int currentWaypointIndex = 0;
        private bool movingToWaypoint = false;
        private bool hasDied = false;


        void Start()
        {
            combat = GetComponent<Combatant>();
            mover = GetComponent<Mover>();

            StartCoroutine(BeginAwareness());
        }

        void Update()
        {
            if(hasDied) return;

            if(combat.currentState == CurrentState.Dead)
            {
                hasDied = true;
                return;
            }

            if(combat.combatTarget != null && combat.combatTarget.currentState != CurrentState.Dead) 
            {
                AttackBehaviour();
                return;
            }

            PatrolBehaviour();
        }

        private Combatant FindClosestEnemyInRange()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRange, enemiesLayer);
            Combatant closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach(Collider candidate in colliders)
            {
                float distanceFromTarget = Vector3.Distance(transform.position, candidate.transform.position);
                
                if(distanceFromTarget < closestDistance)
                {
                    closestDistance = distanceFromTarget;
                    closestTarget = candidate.GetComponent<Combatant>();
                }
            }

            return closestTarget;
        }

        private void PatrolBehaviour()
        {
            if(AtWaypoint())
            {
                CycleWaypoint();
                mover.AdjustSpeedWithFraction(walkSpeedFraction);
                mover.MoveTo(GetCurrentWaypoint());
                movingToWaypoint = true;
            }
            else if(!movingToWaypoint && !AtWaypoint())
            {
                mover.AdjustSpeedWithFraction(walkSpeedFraction);
                mover.MoveTo(GetCurrentWaypoint());
                movingToWaypoint = true;
            }
        }

        private void AttackBehaviour()
        {
            if(combat.CanAttackTarget())
            {
                mover.Stop();
                combat.Attack();
                return;
            }

            combat.ChaseTarget();
            movingToWaypoint = false;
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private IEnumerator BeginAwareness()
        {
            while(!hasDied)
            {
                Combatant potentialTarget = FindClosestEnemyInRange();
                if(potentialTarget != null)
                {
                    combat.combatTarget = potentialTarget;
                }
                yield return new WaitForSeconds(0.5f);
            }

        }

        public void SetPatrolPath(PatrolPath path)
        {
            patrolPath = path;
        }
    }
}
