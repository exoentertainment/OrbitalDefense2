using UnityEngine;

[CreateAssetMenu(fileName = "Base Turret SO", menuName = "Turret SO/Base Turret")]
public class TurretSO : ScriptableObject
{
    #region --Attack Variables--

    public ProjectileSO projectileSO;
    //public AudioClipSO fireSFX;
    public float fireRate;
    public float barrelFireDelay;
    public LayerMask targetLayers;
    
    #endregion
}
