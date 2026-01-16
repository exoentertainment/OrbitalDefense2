using UnityEngine;
using UnityEngine.Events;

public class TurretController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private UnityEvent<GameObject> turretListeners;

    #endregion

    //Called by ship controller. Sends the passed target to any listening turret
    public void SetTarget(GameObject target)
    {
        turretListeners?.Invoke(target);
    }
}
