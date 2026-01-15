using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private CinemachineCamera defaultCamera;
    [SerializeField] private float panSpeed;

    #endregion

    #region Variables

    bool isPanningLeft;
    bool isPanningRight;
    bool isPanningUp;
    bool isPanningDown;

    #endregion

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        if (isPanningLeft)
        {
            PanCameraLeft();
        }

        if (isPanningRight)
        {
            PanCameraRight();
        }

        if (isPanningUp)
        {
            PanCameraUp();
        }

        if (isPanningDown)
        {
            PanCameraDown();
        }
    }

    #region Movement Methods

    void PanCameraLeft()
    {
        Vector3 direction = defaultCamera.transform.right;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.unscaledDeltaTime * -panSpeed), Space.World);
    }
    
    void PanCameraRight()
    {
        Vector3 direction = defaultCamera.transform.right;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.unscaledDeltaTime * panSpeed), Space.World);
    }
    
    void PanCameraDown()
    {
        defaultCamera.transform.position += -defaultCamera.transform.forward * (Time.deltaTime * panSpeed);
        // defaultCamera.transform.position += Vector3.down * (Time.deltaTime * panSpeed);
    }
    
    void PanCameraUp()
    {
        defaultCamera.transform.position += defaultCamera.transform.forward * (Time.deltaTime * panSpeed);
        // defaultCamera.transform.position += Vector3.up * (Time.deltaTime * panSpeed);
    }

    #endregion

    #region Input Methods

    public void PanLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningLeft = true;
        }
        else if (context.canceled)
        {
            isPanningLeft = false;
        }
    }
    
    public void PanRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningRight = true;
        }
        else if (context.canceled)
        {
            isPanningRight = false;
        }
    }
    
    public void PanUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningUp = true;
        }
        else if (context.canceled)
        {
            isPanningUp = false;
        }
    }
    
    public void PanDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPanningDown = true;
        }
        else if (context.canceled)
        {
            isPanningDown = false;
        }
    }

    #endregion
}
