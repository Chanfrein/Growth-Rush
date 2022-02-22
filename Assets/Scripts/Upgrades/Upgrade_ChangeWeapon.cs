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

            button.onClick.AddListener(delegate {Purchase();});
        }

        public override void Purchase()
        {
            playerCombat.EquipWeapon(newWeapon);
        }
    }
}
