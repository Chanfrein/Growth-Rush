using UnityEngine;
using Core;
using System.Threading.Tasks;
using System;
using System.Collections;

namespace Combat.Stats
{
    public class Combatant : MonoBehaviour
    {
        [Header("Combat Stats")]
        [SerializeField] float maxHealth = 100f;

        [Header("Weapon")]
        [SerializeField] Weapon weapon = null;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;


        [Header("Animation")]
        [SerializeField] int timeStayDeadInSeconds = 5;

        [Header("States")]
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

            EquipWeapon(weapon);
        }

        public void EquipWeapon(Weapon weapon)
        {
            weapon.SpawnWeapon(rightHandTransform, leftHandTransform, animator);
        }

        private void Update() 
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public bool CanAttackTarget()
        {
            return TargetIsInRange() && combatTarget.currentState != CurrentState.Dead;
        }

        public bool TargetIsInRange()
        {
            return Vector3.Distance(combatTarget.transform.position, transform.position) < weapon.GetWeaponRange();
        }

        public void ChaseTarget()
        {
            if (combatTarget.currentState != CurrentState.Dead)
            {
                mover.AdjustSpeedWithFraction(1f);
                mover.MoveTo(combatTarget.transform.position);
            }
        }

        public void Attack()
        {
            transform.LookAt(combatTarget.transform);
            if (weapon.GetWeaponSpeed() < timeSinceLastAttack)
            {
                //Hit() or Shoot() is called following this trigger
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        //This is an animation event for melee weapons
        private void Hit()
        {
            if (combatTarget == null) return;

            if (weapon.HasProjectile())
            {
                weapon.LaunchProjectile(rightHandTransform, leftHandTransform, combatTarget);
                return;
            }

            combatTarget.TakeDamage(weapon.GetWeaponDamage());
        }

        //This is an animation event for the bow
        private void Shoot()
        {
            Hit();
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
            if (currentHealth == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            mover.Stop();
            combatTarget = null;
            animator.SetTrigger("death");
            currentState = CurrentState.Dead;
            GetComponent<Collider>().enabled = false;
            GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

            Destroy(gameObject, timeStayDeadInSeconds);
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
