using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat.Stats;
using Core;
using System;
using UnityEngine.AI;

namespace Control.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        NavMeshAgent navMeshAgent;
        Combatant playerCombat;
        Transform currentTarget;
        Animator animator;
        Mover mover;

        void Start()
        {
            playerCombat = GetComponent<Combatant>();
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
        }

        void Update()
        {
            if (playerCombat.combatTarget != null)
            {
                if(playerCombat.CanAttackTarget())
                {
                    mover.Stop();
                    playerCombat.Attack();
                }
                else
                {
                    playerCombat.ChaseTarget();
                }
            }

            if(Input.GetMouseButtonDown(0))
            {
                if(InterractWithNPC()) return;
                if(InterractWithMovement()) return;
            }
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool InterractWithNPC()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach(RaycastHit hit in hits)
            {
                Combatant target = hit.collider.GetComponent<Combatant>();
                if(target == null || target == playerCombat) continue;

                currentTarget = target.transform;
                playerCombat.combatTarget = target;

                return true;
            }
            return false;
        }

        private bool InterractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            playerCombat.combatTarget = null;

            if(hasHit && playerCombat.currentState != CurrentState.Disabled)
            {
                currentTarget = null;
                playerCombat.combatTarget = null;
                
                mover.MoveTo(hit.point);
                return true;
            }
            return false;
        }


    }
}

