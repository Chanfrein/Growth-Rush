using UnityEngine;
using Core;

namespace Combat.Stats
{
    public class Combatant : MonoBehaviour
    {
        [SerializeField] float AttackRange = 2f;
        [SerializeField] float timeBetweenAttacks = 2f;

        public CurrentState currentState = CurrentState.CanAttack;
        public Combatant combatTarget;

        Animator animator;
        Mover mover;
        float timeSinceLastAttack = Mathf.Infinity;

        private void Start() 
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }

        private void Update() 
        {
            timeSinceLastAttack += Time.deltaTime;

            if(combatTarget == null) return;

            if(TargetIsInRange())
            {
                mover.Stop();
                Attack();
            }
        }

        public bool TargetIsInRange()
        {
            return Vector3.Distance(combatTarget.transform.position, transform.position) <= AttackRange;
        }

        public void ChaseTarget()
        {
            mover.MoveTo(combatTarget.transform.position);
        }

        public void Attack()
        {
            transform.LookAt(combatTarget.transform);
            if(timeBetweenAttacks < timeSinceLastAttack)
            {
                animator.SetTrigger("attack");
                //Hit() is called following this trigger
                timeSinceLastAttack = 0f;
            }
        }

        //This is an animation event
        void Hit()
        {

        }

        public void Stop()
        {
            animator.SetTrigger("stopAttack");
            combatTarget = null;
        }

    }

    public enum CurrentState
    {
        CanAttack,
        Waiting,
        Disabled
    }
}
