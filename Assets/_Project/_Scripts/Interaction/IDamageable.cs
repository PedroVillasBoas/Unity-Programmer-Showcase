using UnityEngine;

namespace GoodVillageGames.Core.Interfaces
{
    /// <summary>
    /// A universal contract for any object in the game that can receive damage.
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(DamageInfo damageInfo);
    }

    /// <summary>
    /// A data container for information about a single instance of damage.
    /// This makes the system easily extendable with different damage types or critical hits. (If I ever want to implement, of course)
    /// </summary>
    public struct DamageInfo
    {
        public float damageAmount;
        public Vector3 hitPoint;
    }
}
