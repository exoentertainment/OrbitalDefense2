using MystifyFX;
using UnityEngine;

public class InteractionLayer : MonoBehaviour
{
    [SerializeField] MystifyEffect effect; 
    
    public void Interact(Vector3 hitPosition)
    {
        effect.HitFX(hitPosition);
    }
}
