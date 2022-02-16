using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Combat.Stats
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 15f;

        private float bowDamage = 1f;
        Combatant target = null;

        private void Update()
        {
            if (target == null) return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, GetAimLocation()) < 0.2 ) HitTarget();
        }

        public void SetTarget(Combatant arrowTarget, float damage)
        {
            target = arrowTarget;
            bowDamage = damage;
        }

        private void HitTarget()
        {
            target.TakeDamage(bowDamage);
            Destroy(gameObject);
        }

        private Vector3 GetAimLocation()
        {
            return target.transform.position + Vector3.up;
        }
    }
}
