using System;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    private GameObject selectedShip;

    public static ShipManager instance;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
    
        instance = this;
    }

    //Input manager calls this method and passes along the clicked on ship. Manager stores the passed object locally
    public void SetSelectedShip(GameObject ship)
    {
        if(selectedShip != ship && selectedShip != null)
            selectedShip.GetComponent<ShipController>().ShipDeselected();
        
        selectedShip = ship;
        
        //Send ship info to UI
        //Turn on circular selected VFX around ship
        selectedShip.GetComponent<ShipController>().ShipSelected();
    }
    
    //Send movement coordinates to selected ship
    public void SetMovePos(Vector3 movePos)
    {
        if (selectedShip != null)
        {
            //Send move coordinates to selected ship
            selectedShip.GetComponent<ShipController>().SetDestinationPos(movePos);
        }
    }
    
    public bool IsShipSelected()
    {
        if (selectedShip == null)
            return false;
        else
            return true;
    }

    public void SetTarget(GameObject target)
    {
        selectedShip.GetComponent<ShipController>().SetTarget(target);
    }
}
