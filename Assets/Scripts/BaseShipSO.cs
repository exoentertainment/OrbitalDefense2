using UnityEngine;

[CreateAssetMenu(fileName = "Base Ship SO", menuName = "Ships SO/Base Ship")]
public class BaseShipSO : ScriptableObject
{
    #region Health Variables

    public int maxHealth;
    public int maxShield;
    public float lowShieldPercentage;
    public float shieldRechargeRate;
    
    #endregion

    #region Movement Variables

    public int moveSpeed;
    public float turnSpeed;
    public int minOrbitRadius;
    public int maxOrbitRadius;
    public int lookAheadDistance;
    public LayerMask obstacleLayerMask;

    #endregion

    #region SFX Variables



    #endregion

    #region Explosion  Variables

    public GameObject explosionPrefab;
    public GameObject[] debrisPrefab;
    public float explosionFrequency;
    public int numExplosions;

    #endregion
}
