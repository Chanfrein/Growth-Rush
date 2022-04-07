using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;

namespace Upgrades
{
    public class TradeWindowManager : MonoBehaviour
    {
        [SerializeField] GameObject textZoneRef;

        private Text textZoneObject;

        void Start()
        {
            textZoneObject = textZoneRef.GetComponent<Text>();
        }

        public void UpdateText(string textUpdate, Color coloration)
        {
            textZoneObject.text = null;
            textZoneObject.text = textUpdate;
            textZoneObject.color = coloration;
        }
    }
}
