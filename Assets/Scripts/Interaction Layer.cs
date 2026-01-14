using MystifyFX;
using UnityEngine;

public class InteractionLayer : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] MystifyEffect mystifyEffect; 
    [SerializeField] MystifyEffectProfile movementProfile;
    [SerializeField] MystifyEffectProfile targetProfile;

    #endregion

    
    //Called by input manager. Changes the Mystify component preset and activates it
    public void ActivateMovementEffect(Vector3 hitPosition)
    {
        mystifyEffect.profile = movementProfile;
        mystifyEffect.HitFX(hitPosition);
    }

    //Called by input manager. Changes the Mystify component preset and activates it
    public void ActivateTargetEffect(Vector3 hitPosition)
    {
        mystifyEffect.profile = targetProfile;
        mystifyEffect.HitFX(hitPosition);
    }
}
