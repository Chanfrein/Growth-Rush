using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] float maxSpeed = 5.66f;

        NavMeshAgent navMeshAgent;
        Animator animator;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        private void LateUpdate() 
        {
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            animator.SetFloat("forwardSpeed", speed);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = destination;
        }

        public void Stop()
        {
            navMeshAgent.isStopped = true;
        }

        public void AdjustSpeedWithFraction(float fraction)
        {
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(fraction);
        }

    }
}
