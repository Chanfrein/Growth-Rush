using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat.Stats;
using UnityEngine.UI;


namespace Upgrades
{
    public class Upgrade_ChangeWeapon : Upgrade_Base
    {
        [SerializeField] Weapon newWeapon = null;

        private Combatant playerCombat;
        
        void Start()
        {
            Button button = GetComponent<Button>();
            playerCombat = GameObject.FindWithTag("Player").GetComponent<Combatant>();
            tradeWindowManager = transform.root.GetComponent<TradeWindowManager>();
        }

        public override void Purchase()
        {
            if (playerCombat.GetWeapon() == newWeapon)
            {
                tradeWindowManager.UpdateText("Weapon is already equipped!", new Color(197, 0, 0));
                return;
            }

            playerCombat.EquipWeapon(newWeapon);
            tradeWindowManager.UpdateText("Weapon has been equipped!", new Color(197, 149, 0));
            Destroy(gameObject);
        }
    }
}
