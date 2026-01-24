using System;
using UnityEngine;

public class ShieldHealth : MonoBehaviour, IHealth
{
    #region Serialized Fields

    [Header("Components")] 
    [SerializeField] private GameObject mainShield;
    [SerializeField] private GameObject lowShield;
    
    [SerializeField] BaseShipSO shipSO;

    #endregion

    #region Variables
    
    private float currentShield;
    bool isShieldRecharging;
    private bool isHit;
    private bool isLowShield;
    
    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentShield = shipSO.maxShield;
    }

    private void Update()
    {
        isHit = false;
        
        Recharge();
    }

    public void TakeDamage(float damage)
    {
        if (!isHit)
        {
            isHit = true;
        
            currentShield -= damage;
        
            if (currentShield > 0)
            {
                if (currentShield < shipSO.maxShield * shipSO.lowShieldPercentage && !isLowShield)
                {
                    isLowShield = true;
                    mainShield.GetComponent<MeshCollider>().enabled = false;
                    mainShield.SetActive(false);
                    lowShield.SetActive(true);
                }
            }
            else
            {
                lowShield.GetComponent<MeshCollider>().enabled = false;
                lowShield.SetActive(false);
            }
        }
    }

    void Recharge()
    {
        if (currentShield < shipSO.maxShield)
        {
            currentShield += shipSO.maxShield * (shipSO.shieldRechargeRate * Time.deltaTime);
        }

        if (currentShield > shipSO.maxShield * shipSO.lowShieldPercentage && isLowShield)
        {
            isLowShield = false;
            lowShield.SetActive(false);
            mainShield.SetActive(true);
        }
    }
}
