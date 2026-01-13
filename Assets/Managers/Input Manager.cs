using Forge3D;
using MystifyFX;
using MystifyFX.Demo;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] LayerMask shipLayerMask;

    #endregion
    
    //Left mouse click selects the ship at mouse cursor and sends to ship manager to store
    public void PrimaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit hitInfo;
            if (Physics.Raycast(screenRay, out hitInfo, Mathf.Infinity, shipLayerMask))
            {
                ShipManager.instance.SetSelectedShip(hitInfo.collider.gameObject);
            }
        }
    }
    
    //Right mouse click sends coordinates of mouse click to ship manager to send to selected ship
    public void SecondaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit hitInfo;
            if (Physics.Raycast(screenRay, out hitInfo))
            {
                if (hitInfo.collider.CompareTag("Movement Layer"))
                {
                    hitInfo.collider.GetComponent<InteractionLayer>().Interact(hitInfo.point);
                    ShipManager.instance.SetMovePos(hitInfo.point);
                }
            }
        }
    }
}
