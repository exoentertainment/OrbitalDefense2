using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private CinemachineCamera defaultCamera;
    [SerializeField] private float panSpeed;
    
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxZoom;

    #endregion

    #region Variables
    
    bool isPanningLeft;
    bool isPanningRight;
    bool isPanningUp;
    bool isPanningDown;

    int scrollDirection;
    private float currentZoom;
    
    #endregion

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentZoom = 0;
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
        
        if(scrollDirection != 0)
            AdjustZoom();
    }

    #region Movement Methods

    void PanCameraLeft()
    {
        Vector3 direction = Vector3.left;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.deltaTime * panSpeed), Space.World);
    }
    
    void PanCameraRight()
    {
        Vector3 direction = Vector3.right;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.deltaTime * panSpeed), Space.World);
    }
    
    void PanCameraDown()
    {
        Vector3 direction = Vector3.back;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.deltaTime * panSpeed), Space.World);
    }
    
    void PanCameraUp()
    {
        Vector3 direction = Vector3.forward;
        direction.y = 0;
        direction.Normalize();
        defaultCamera.transform.Translate(direction * (Time.deltaTime * panSpeed), Space.World);
    }

    void AdjustZoom()
    {
        if (scrollDirection > 0)
        {
            if (currentZoom < maxZoom)
            {
                defaultCamera.transform.position += defaultCamera.transform.forward * (Time.deltaTime * zoomSpeed);
                currentZoom += Time.deltaTime * zoomSpeed;
            }
        }
        else if (scrollDirection < 0)
        {
            if (currentZoom > 0)
            {
                defaultCamera.transform.position -= defaultCamera.transform.forward * (Time.deltaTime * zoomSpeed);
                currentZoom -= Time.deltaTime * zoomSpeed;
            }
        }
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

    public void ZoomControl(InputAction.CallbackContext context)
    {
        float scrollY = context.ReadValue<float>();

        if (scrollY > 0)
        {
            scrollDirection = 1;
        }    
        else if (scrollY < 0)
        {
            scrollDirection = -1;
        }
        else if (scrollY == 0)
        {
            scrollDirection = 0;
        }
    }
    
    #endregion
}
