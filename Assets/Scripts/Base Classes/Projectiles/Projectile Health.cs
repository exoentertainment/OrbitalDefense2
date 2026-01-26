using System;
using UnityEngine;

public class ProjectileHealth : MonoBehaviour, IHealth
{
    #region Serialized Fields

    [SerializeField] ProjectileSO projectileSO;

    #endregion

    #region Variables

    int currentHealth;
    private bool isHit;

    #endregion

    private void Start()
    {
        currentHealth = projectileSO.maxHealth;
    }

    private void Update()
    {
        isHit = false;
    }

    public void TakeDamage(float damage)
    {
        if (!isHit)
        {
            isHit = true;

            currentHealth -= (int)damage;
            if (currentHealth <= 0)
            {
                Instantiate(projectileSO.impactPrefab, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
            }
        }
    }
}
