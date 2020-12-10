using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory System/Items/Weapon")]
    public class WeaponObject : ItemObject
    {
        public int damage;
        public enum WeaponType { Melee, Ranged }
        public WeaponType _damageType;
    }
}