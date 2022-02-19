using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Combat.Stats
{
    public class DamageIndicator : MonoBehaviour
    {
        [SerializeField] float lifetimeInSeconds = 3f;
        [SerializeField] float upwardMovespeed = 0.05f;

        private Text text = null;
        private float myDamage = 0f;
        

        // Start is called before the first frame update
        private void Start()
        {
            text = transform.GetChild(0).GetComponent<Text>();
            text.text = myDamage.ToString();

            text.CrossFadeAlpha(0.0f, lifetimeInSeconds, false);

            Destroy(gameObject, lifetimeInSeconds);
        }

        private void Update()
        {
            transform.LookAt(Camera.main.transform.position);
            transform.Translate(Vector3.up * upwardMovespeed * Time.deltaTime);
        }

        public void SetDamage(float damage)
        {
            myDamage = damage;
        }
    }
}
