using UnityEngine;
using Arkanor.Characters;

namespace Arkanor.Inventory
{
    [CreateAssetMenu(
        fileName = "NewHealEffect",
        menuName = "Arkanor/Inventory/Effects/Heal"
    )]
    public class HealEffect : ItemUseEffect
    {
        [SerializeField] private int healAmount = 25;

        public override bool Use(GameObject user)
        {
            if (user == null)
                return false;

            Health health = user.GetComponent<Health>();

            if (health == null)
                return false;

            if (health.IsDead)
                return false;

            if (health.CurrentHealth >= health.MaxHealth)
                return false;

            health.Heal(healAmount);

            return true;
        }
    }
}