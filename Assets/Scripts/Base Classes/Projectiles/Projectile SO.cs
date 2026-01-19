using UnityEngine;

[CreateAssetMenu(fileName = "Base Projectile SO", menuName = "Projectile SO/Base Projectile SO")]
public class ProjectileSO : ScriptableObject
{
    public GameObject projectilePrefab;
    public GameObject dischargePrefab;
    public GameObject impactPrefab;
    public int moveSpeed;
    public int lifetime;
}
