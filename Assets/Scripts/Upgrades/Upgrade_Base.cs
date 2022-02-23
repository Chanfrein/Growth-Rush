using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Upgrades
{
    public abstract class Upgrade_Base : MonoBehaviour
    {
        [SerializeField] protected float cost;

        public abstract void Purchase();

    }
}
