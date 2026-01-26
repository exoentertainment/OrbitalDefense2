using Forge3D;
using MystifyFX;
using MystifyFX.Demo;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] LayerMask leftMouseClickMask;
    [SerializeField] private LayerMask rightMouseClickMask;
    [SerializeField] LayerMask movementPlaneMask;

    [SerializeField] private GameObject movementPlane;
    
    [SerializeField] GameObject targetEffect;
    
    #endregion
    
    //Left mouse click selects the ship at mouse cursor and sends to ship manager to store
    public void PrimaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            RaycastHit hitInfo;
            if (Physics.Raycast(screenRay, out hitInfo, Mathf.Infinity, leftMouseClickMask))
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
            if (Physics.Raycast(screenRay, out hitInfo, Mathf.Infinity, rightMouseClickMask))
            {
                if (hitInfo.collider.CompareTag("Movement Layer") && ShipManager.instance.IsShipSelected())
                {
                    movementPlane.GetComponent<InteractionLayer>().ActivateMovementEffect(hitInfo.point);
                    ShipManager.instance.SetMovePos(hitInfo.point);
                }
                else
                {
                    Ray movementPlaneRay = new Ray(hitInfo.collider.gameObject.transform.position, -hitInfo.collider.gameObject.transform.up);
                    RaycastHit movementPlaneHit;

                    if (Physics.Raycast(movementPlaneRay, out movementPlaneHit, Mathf.Infinity, movementPlaneMask) &&
                        ShipManager.instance.IsShipSelected())
                    {
                        movementPlane.GetComponent<InteractionLayer>().ActivateTargetEffect(movementPlaneHit.point);
                        ShipManager.instance.SetTarget(hitInfo.collider.gameObject);
                    }
                }
            }
        }
    }
}
