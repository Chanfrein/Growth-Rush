using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TroopSpawning;

namespace Upgrades
{
    public class Upgrade_AddTroop : Upgrade_Base
    {
        [Header("Settings")]
        [SerializeField] TroopType troopType;
        [SerializeField] int troopAmountPurchase = 1;

        private TroopSpawner troopSpawnerRef;

        private void Start() 
        {
            troopSpawnerRef = GameObject.FindWithTag("TroopSpawner").GetComponent<TroopSpawner>();
            tradeWindowManager = transform.root.GetComponent<TradeWindowManager>();
        }

        public override void Purchase()
        {
            switch(troopType)
            {
                case TroopType.Swordman: 
                    troopSpawnerRef.nbOfSwordmenPlayer += troopAmountPurchase;
                    break;
                case TroopType.Archer:
                    troopSpawnerRef.nbOfArchersPlayer += troopAmountPurchase;
                    break;
            }
            tradeWindowManager.UpdateText($"Increased the number of {troopType.ToString()} spawned by {troopAmountPurchase} !", new Color(197, 149, 0));
        }

    }
}

enum TroopType
{
    Swordman,
    Archer
}
