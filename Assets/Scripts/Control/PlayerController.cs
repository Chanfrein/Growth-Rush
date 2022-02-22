using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat.Stats;
using Core;
using System;
using UnityEngine.AI;
using Upgrades;

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
            if(playerCombat.combatTarget != null)
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
                if(IntereactWithUI()) return;
                if(InterractWithNPC()) return;
                if(InterractWithMovement()) return;
            }
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        private bool IntereactWithUI()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if(hasHit && hit.collider.gameObject.layer == 5)
            {
                Debug.Log("Hit UI!!");
                return true;
            }
            return false;
        }

        private bool InterractWithNPC()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach(RaycastHit hit in hits)
            {
                Combatant target = hit.collider.GetComponent<Combatant>();
                if(target != null && target != playerCombat)
                {
                    currentTarget = target.transform;
                    playerCombat.combatTarget = target;

                    return true;
                }
                
                Merchant merchant = hit.collider.GetComponent<Merchant>();
                if(merchant != null)
                {
                    float distanceFromMerchant = Vector3.Distance(transform.position, merchant.transform.position);
                    if(distanceFromMerchant <= merchant.interactionRange)
                    {
                        mover.Stop();
                        merchant.Interract();
                        return true;
                    }
                    StartCoroutine(InterractWithMovement(merchant));
                    return true;
                }
            }
            return false;
        }

        private bool InterractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            playerCombat.combatTarget = null;


            if (hasHit && playerCombat.currentState != CurrentState.Disabled)
            {
                currentTarget = null;
                playerCombat.combatTarget = null;
                
                mover.MoveTo(hit.point);
                return true;
            }
            return false;
        }

        private IEnumerator InterractWithMovement(Merchant target)
        {
            Vector3 destination = target.GetComponent<CapsuleCollider>().ClosestPoint(transform.position);
            mover.MoveTo(destination);

            while(Vector3.Distance(transform.position, destination) >= 1f)
            {
                yield return null;
            }

            target.Interract();
        }


    }
}

