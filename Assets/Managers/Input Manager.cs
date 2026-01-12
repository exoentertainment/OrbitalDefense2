using Forge3D;
using MystifyFX;
using MystifyFX.Demo;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public void PrimaryAction(InputAction.CallbackContext context)
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
                }
            }
        }
    }
}
