using System;
using UnityEngine;

public class simplemove : MonoBehaviour
{
    public float HitRadius = 0.1f;
    public float Dirt = 1f;
    public float Burn = 1f;
    public float Heat = 1f;
    public float Clip = 0.7f;
    public Transform ImpactFX;
    public float ImpactSize = 0.3f;
    
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position +  transform.forward * (Time.deltaTime * 50f));
    }

    private void OnCollisionEnter(Collision other)
    {
        var dfx = other.gameObject.GetComponent<DamageFX>();

        if (dfx != null)
        {
            Debug.Log("dfx");
            dfx.Hit(dfx.transform.InverseTransformPoint(other.GetContact(0).point), HitRadius, Dirt, Burn, Heat, Clip);
            var fx = Instantiate(ImpactFX, other.GetContact(0).point, Quaternion.LookRotation(other.GetContact(0).normal));
            fx.localScale = Vector3.one * HitRadius + Vector3.one * ImpactSize;
        }

        Destroy(gameObject);
    }
}
