using UnityEngine;

[CreateAssetMenu(fileName = "Platform SO", menuName = "Platform SO/ Platform SO")]
public class PlatformScriptableObject : ScriptableObject
{
    public int maxHealth;
    public int resourceCost;
    public GameObject platformPrefab;
    public GameObject explosionPrefab;
    public float explosionFrequency;
    public int numExplosions;
    public string platformDescription;
    public AudioClipSO explosionSFX;
}