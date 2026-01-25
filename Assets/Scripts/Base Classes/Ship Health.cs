using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class ShipHealth : MonoBehaviour, IHealth
{
    #region Serialized Fields

    [SerializeField] private BaseShipSO shipSO;
    [SerializeField] UnityEvent onShipDeath;

    #endregion

    #region Variables

    private float currentHealth;
    private bool isHit;
    private bool isDead;

    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = shipSO.maxHealth;
    }

    private void Update()
    {
        isHit = false;
    }

    public void TakeDamage(float damage)
    {
        if(!isHit)
        {
            isHit = true;
            currentHealth -= damage;

            if (currentHealth <= 0 && !isDead)
            {
                isDead = true;
                onShipDeath?.Invoke();
                StartCoroutine(ExplodeRoutine());
            }
        }
    }

    IEnumerator ExplodeRoutine()
    {
        Collider shipCollider = gameObject.GetComponent<Collider>();
        
        for (int x = 0; x < shipSO.numExplosions; x++)
        {
            yield return new WaitForSeconds(shipSO.explosionFrequency);
            
            Vector3 randomSpot = new Vector3(
                Random.Range(shipCollider.bounds.center.x - shipCollider.bounds.size.x / 2,
                    shipCollider.bounds.center.x + shipCollider.bounds.size.x / 2),
                Random.Range(shipCollider.bounds.center.y - shipCollider.bounds.size.y / 2,
                    shipCollider.bounds.center.y + shipCollider.bounds.size.y / 2),
                Random.Range(shipCollider.bounds.center.z - shipCollider.bounds.size.z / 2,
                    shipCollider.bounds.center.z + shipCollider.bounds.size.z / 2));
            
            Instantiate(shipSO.explosionPrefab,  randomSpot, Quaternion.identity);
        }
        
        Instantiate(shipSO.debrisPrefab[Random.Range(0, shipSO.debrisPrefab.Length)], transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
