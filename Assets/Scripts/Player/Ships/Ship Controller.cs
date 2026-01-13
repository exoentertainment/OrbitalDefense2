using UnityEngine;

public class ShipController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private GameObject selectedShipVFX;

    #endregion

    
    public void ShipSelected()
    {
        selectedShipVFX.SetActive(true);
        //turn on hp bar
        //send ship data to UI
        //play selection SFX
    }

    public void ShipDeselected()
    {
        selectedShipVFX.SetActive(false);
    }

    public void SetTargetPos(Vector3 pos)
    {
        GetComponent<ShipMovement>().SetTargetPos(pos);
        //Play acknowledgement SFX
    }
}
