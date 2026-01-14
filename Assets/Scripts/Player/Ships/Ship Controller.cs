using UnityEngine;

public class ShipController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private GameObject selectedShipVFX;
    
    [SerializeField] ShipMovement shipMovement;

    #endregion

    //When the ship is selected, turn on the selection VFX, the health bar, send ship data to the UI, play selection SFX
    public void ShipSelected()
    {
        selectedShipVFX.SetActive(true);
        //turn on hp bar
        //send ship data to UI
        //play selection SFX
    }

    //When the ship is no longer selected, turn off the selection VFX, health bar
    public void ShipDeselected()
    {
        selectedShipVFX.SetActive(false);
        //turn off hp bar
    }

    public void SetDestinationPos(Vector3 pos)
    {
        shipMovement.SetDestinationPos(pos);
        //Play acknowledgement SFX
    }

    //Receive target info from input manager and pass it along to the currently selected ship movement and weapon components
    public void SetTarget(GameObject target)
    {
        shipMovement.SetTarget(target);
    }
}
