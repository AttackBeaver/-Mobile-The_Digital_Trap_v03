using Crew.Components.Health;
using Crew.Definitions;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    [SerializeField] private int _hpDelta;

    public void ApplyHealth(GameObject target)
    {
        var healthComponent = target.GetComponent<HealthComponent>();
        var maxHealth = DefsFacade.I.PLayer.MaxHealth;

        // Проверяем, превысит ли здоровье после применения _hpDelta максимальное значение
        int finalHealth = healthComponent.Health + _hpDelta;
        if (finalHealth > maxHealth)
        {
            // Если превысит, ограничиваем _hpDelta, чтобы здоровье не превышало максимальное
            _hpDelta = maxHealth - healthComponent.Health;
        }

        // Применяем здоровье
        healthComponent.ModifyHealth(_hpDelta);
    }
}