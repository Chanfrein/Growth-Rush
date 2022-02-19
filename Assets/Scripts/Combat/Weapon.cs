using UnityEngine;

namespace Combat.Stats
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [Header("References")]
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] Projectile projectile = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;

        [Header("Stats")]
        [SerializeField] public float weaponDamage = 20f;
        [SerializeField] float attackRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1.5f;
        [SerializeField] bool isRightHanded = true;


        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator)
        {
            Transform handTransform = GetHandTransform(rightHand, leftHand);

            Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = animatorOverride;
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded) handTransform = rightHand;
            else handTransform = leftHand;
            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Combatant target, bool comesFromPlayer)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage, comesFromPlayer);
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetWeaponRange()
        {
            return attackRange;
        }

        public float GetWeaponSpeed()
        {
            return timeBetweenAttacks;
        }

    }
}
