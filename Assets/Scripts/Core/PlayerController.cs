using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Core.Stats;
using System;

namespace Core.PlayerControl
{
    public class PlayerController : MonoBehaviour
    {   
        NavMeshAgent navMeshAgent;
        CombatStats playerStats;
        Animator animator;
        

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            playerStats = GetComponent<CombatStats>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(InterractWithCombat()) return;
                if(InterractWithMovement()) return;
            }
        }

        void LateUpdate()
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

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool InterractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach(RaycastHit hit in hits)
            {
                CombatStats target = hit.collider.GetComponent<CombatStats>();
                if(target == null) continue;

                //attack

                return true;
            }

            return false;
        }

        private bool InterractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if(hasHit)
            {
                navMeshAgent.destination = hit.point;
                return true;
            }
            return false;
        }
    }
}

