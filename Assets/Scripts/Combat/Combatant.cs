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

        [Header("Visuals")]
        [SerializeField] int timeStayDeadInSeconds = 5;
        [SerializeField] Canvas damagePopupPrefab = null;

        [Header("States")]
        [SerializeField] bool isPlayer = false;
        public CurrentState currentState = CurrentState.CanAttack;
        public Combatant combatTarget;

        Animator animator;
        Mover mover;

        float timeSinceLastAttack = Mathf.Infinity;
        public float currentHealth;


        private void Start() 
        {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();

            currentHealth = maxHealth;

            EquipWeapon(weapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        public void EquipWeapon(Weapon newWeapon)
        {
            newWeapon.SpawnWeapon(rightHandTransform, leftHandTransform, animator);
            weapon = newWeapon;
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
            if(combatTarget.currentState != CurrentState.Dead)
            {
                mover.AdjustSpeedWithFraction(1f);
                mover.MoveTo(combatTarget.transform.position);
            }
        }

        public Weapon GetWeapon()
        {
            return weapon;
        }

        public void Attack()
        {
            transform.LookAt(combatTarget.transform);
            if(weapon.GetWeaponSpeed() < timeSinceLastAttack)
            {
                //Hit() or Shoot() is called following this trigger
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }

        //This is an animation event for melee weapons
        private void Hit()
        {
            if(combatTarget == null) return;

            if(weapon.HasProjectile())
            {
                weapon.LaunchProjectile(rightHandTransform, leftHandTransform, combatTarget, isPlayer);
                return;
            }

            combatTarget.TakeDamage(weapon.GetWeaponDamage(), isPlayer);
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

        public void TakeDamage(float damage, bool shouldSpawntext)
        {
            if(shouldSpawntext)
            {
                DamageIndicator damagePopup = Instantiate(damagePopupPrefab, GetComponent<CapsuleCollider>().bounds.max, Quaternion.identity).GetComponent<DamageIndicator>();
                damagePopup.SetDamage(damage);
            }
            currentHealth -= damage;
            Mathf.Clamp(currentHealth, 0, maxHealth);
            if(currentHealth <= 0)
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
