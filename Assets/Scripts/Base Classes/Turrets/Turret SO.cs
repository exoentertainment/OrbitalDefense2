using UnityEngine;

[CreateAssetMenu(fileName = "Base Turret SO", menuName = "Turret SO/Base Turret")]
public class TurretSO : ScriptableObject
{
    #region --Attack Variables--

    public ProjectileSO projectileSO;
    public AudioClipSO fireSFX;
    public int engageRange;
    public float fireRate;
    public float barrelFireDelay;
    public LayerMask targetLayers;
    
    #endregion

    #region Rotation Variables

    public TurretTrackingType TrackingType;
    public float HeadingTrackingSpeed = 2f;
    public float ElevationTrackingSpeed = 2f;
    public Vector2 HeadingLimit;
    public Vector2 ElevationLimit;

    public enum TurretTrackingType
    {
        Step,
        Smooth,
    }

    #endregion
}
