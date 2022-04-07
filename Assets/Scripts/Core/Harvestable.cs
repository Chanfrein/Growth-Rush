using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Resources
{
    public class Harvestable : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] Transform player;
        [SerializeField] GameObject baseTreeAsset;
        [SerializeField] GameObject cutTreeAsset;
        [SerializeField] Image progressBar;

        [Header("Harvesting")]
        [SerializeField] float harvestTime = 2f;
        [SerializeField] float harvestRange = 5f;
        [SerializeField] float respawnTime = 120f;

        [Header("Asset fading")]
        [SerializeField] float transitionTimeInSeconds = 3f;
        [SerializeField] float updatesPerSeconds = 6f;

        public IEnumerator Harvest()
        {
            if (isPlayerInHarvestRange())
            {
                progressBar.enabled = true;
                progressBar.fillAmount = 100f;

                while (progressBar.fillAmount > 0)
                {
                    progressBar.fillAmount--;
                    yield return new WaitForSeconds(harvestTime / 100f);
                }

                if (isPlayerInHarvestRange())
                {
                    StartCoroutine(AssetfadeOut(baseTreeAsset));
                    StartCoroutine(AssetfadeIn(cutTreeAsset));
                    GetComponent<CapsuleCollider>().enabled = false;
                    StartCoroutine(RespawnDelay());
                }
            }
        }

        private IEnumerator RespawnDelay()
        {
            Debug.Log("Respawn timer has started");
            yield return new WaitForSeconds(respawnTime);
            StartCoroutine(AssetfadeOut(cutTreeAsset));
            yield return StartCoroutine(AssetfadeIn(baseTreeAsset));
            GetComponent<Collider>().enabled = true;
            Debug.Log("Respawn complete");

        }

        private bool isPlayerInHarvestRange()
        {
            return (Vector3.Distance(player.position, transform.position) <= harvestRange);
        }

        private IEnumerator AssetfadeOut(GameObject asset)
        {
            List<Material> assetMaterials = asset.GetComponent<Renderer>().materials.ToList();

            float timeIncrements = updatesPerSeconds * transitionTimeInSeconds;
            float fadeIncrements = 1f / timeIncrements;
            float counter = 0f;

            while (counter <= timeIncrements)
            {
                foreach (Material mat in assetMaterials)
                {
                    Color current = mat.color;
                    current.a = Mathf.Clamp01(current.a - fadeIncrements);
                    mat.color = current;
                }
                counter++;
                yield return new WaitForSeconds((transitionTimeInSeconds) / timeIncrements);
            }
        }

        private IEnumerator AssetfadeIn(GameObject asset)
        {
            List<Material> assetMaterials = asset.GetComponent<Renderer>().materials.ToList();

            float timeIncrements = updatesPerSeconds * transitionTimeInSeconds;
            float fadeIncrements = 1f / timeIncrements;
            float counter = 0f;

            while (counter <= timeIncrements)
            {
                foreach (Material mat in assetMaterials)
                {
                    Color current = mat.color;
                    current.a = Mathf.Clamp01(current.a + fadeIncrements);
                    mat.color = current;
                }
                counter++;
                yield return new WaitForSeconds((transitionTimeInSeconds) / timeIncrements);
            }
        }
    }
}
