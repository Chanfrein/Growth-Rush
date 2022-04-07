using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    public class Merchant : MonoBehaviour
    {
        [SerializeField] public float interactionRange = 2f;
        [SerializeField] public float leaveInteractionRange = 5f;
        [SerializeField] GameObject shopWindowPrefab = null;
        [SerializeField] GameObject[] merchantUpgrades = null;

        private Transform player = null;

        private void Start() 
        {
            player = GameObject.FindWithTag("Player").transform;
            UpdateShopWindow();
        }

        public void Interract()
        {
            UpdateShopWindow();
            shopWindowPrefab.SetActive(true);
            StartCoroutine(CheckDistance());
        }

        public void CloseInterraction()
        {
            shopWindowPrefab.SetActive(false);
        }

        private IEnumerator CheckDistance()
        {
            while(Vector3.Distance(transform.position, player.position) <= leaveInteractionRange)
            {
                yield return new WaitForSeconds(0.5f);
            }

            CloseInterraction();
        }

        private void UpdateShopWindow()
        {
            Transform background = shopWindowPrefab.transform.GetChild(0);

            int i = 0;

            foreach(Transform child in background.transform)
            {
                if(i >= merchantUpgrades.Length) break;

                foreach (Transform variable in child.transform)
                {
                    Destroy(variable.gameObject);
                }

                Instantiate(merchantUpgrades[i], child);
                i++;
            }

        }
    }
}
