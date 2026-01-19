using System;
using UnityEngine;

public class ShieldPassThrough : MonoBehaviour, IHealth
{
    ShieldHealth shieldHealth;

    private void Awake()
    {
        shieldHealth = GetComponentInParent<ShieldHealth>();
    }

    public void TakeDamage(float damage)
    {
        shieldHealth.TakeDamage(damage);
    }
}
