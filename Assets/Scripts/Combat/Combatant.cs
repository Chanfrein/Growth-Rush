using UnityEngine;
using Core;
using System.Threading.Tasks;

namespace Combat.Stats
{
    public class Combatant : MonoBehaviour
    {
        /* Stats */
        [Header("Combat Stats")]
        [SerializeField] float AttackRange = 2f;
        [SerializeField] float timeBetweenAttacks = 2f;
        [SerializeField] float maxHealth = 100f;
        [SerializeField] float attackDamage = 25f;

        [Header("Faction")]
        [SerializeField] Alignment alignment;

        [Header("Animation")]
        [SerializeField] int timeStayDeadInSeconds = 5;

        public CurrentState currentState = CurrentState.CanAttack;
        public Combatant combatTarget;

        Animator animator;
        Mover mover;

        float timeSinceLastAttack = Mathf.Infinity;
        float currentHealth;


        private void Start() 
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();

            currentHealth = maxHealth;
        }

        private void Update() 
        {
            timeSinceLastAttack += Time.deltaTime;

            if(combatTarget == null) return;

            if(TargetIsInRange() && combatTarget.currentState != CurrentState.Dead)
            {
                mover.Stop();
                Attack();
                return;
            }

            ChaseTarget();
        }

        public bool TargetIsInRange()
        {
            return Vector3.Distance(combatTarget.transform.position, transform.position) <= AttackRange;
        }

        public void ChaseTarget()
        {
            mover.AdjustSpeedWithFraction(1f);
            mover.MoveTo(combatTarget.transform.position);
        }

        public void Attack()
        {
            transform.LookAt(combatTarget.transform);
            if(timeBetweenAttacks < timeSinceLastAttack)
            {
                //Hit() is called following this trigger
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        //This is an animation event
        void Hit()
        {
            if(combatTarget != null)
            {
                combatTarget.TakeDamage(attackDamage);
            }
        }

        public void Stop()
        {
            animator.SetTrigger("stopAttack");
            combatTarget = null;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth);
            if(currentHealth == 0)
            {
                Die();
            }
        }

        private async void Die()
        {
            mover.Stop();
            animator.SetTrigger("death");
            currentState = CurrentState.Dead;
            GetComponent<Collider>().enabled = false;

            await Task.Delay(timeStayDeadInSeconds * 1000);
            
            Destroy(this.gameObject);
        }

        public int OppositeFactionLayerInt()
        {
            return alignment == Alignment.East ? 6 : 7;
        }
    }

    public enum CurrentState
    {
        CanAttack,
        Waiting,
        Disabled,
        Dead
    }

    public enum Alignment
    {
        East,
        West
    }
}
