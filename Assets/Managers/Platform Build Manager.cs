using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlatformBuildManager : MonoBehaviour
{
    #region Serialized Fields
    
    [SerializeField] private GameObject buildLayer;
    [SerializeField] private GameObject unOccupiedmouseIcon;
    [SerializeField] private GameObject occupiedmouseIcon;
    [SerializeField] private GameObject platformSelectionWindow;

    [SerializeField] LayerMask buildPlatformLayerMask;
    [SerializeField] LayerMask platformLayerMask;

    [Tooltip("This needs to match the sphere collider size of the mouse icons")]
    [SerializeField] private int detectionRadius;
    
    #endregion

    #region Variables
    
    GameObject mouseIcon;
    public static PlatformBuildManager instance;
    
    private bool isActive;
    private bool isOpenSpot;

    PlatformSelectionWindow  platformSelection;
    
    #endregion

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
        platformSelection = platformSelectionWindow.GetComponent<PlatformSelectionWindow>();
    }

    private void Start()
    {
        mouseIcon = unOccupiedmouseIcon;
    }

    private void Update()
    {
        if(isActive)
            ShowPlacementCursor();
        else
            HidePlacementCursor();
    }

    public void ShowBuildArea(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShowBuildArea();
        }
        else
        {
            HideBuildArea();
        }
    }
    
    public void ShowBuildArea()
    {
        buildLayer.SetActive(true);
        isActive = true;
    }

    public void HideBuildArea()
    {
        buildLayer.SetActive(false);
        isActive = false;
    }

    //Activate the mouse icon; Move the icon to the mouse position if it's over the build area
    void ShowPlacementCursor()
    {
        //Check if there is an existing platform in the mouse cursor area. Switch to appropriate mouse icon and set isOpenSpot
        Collider[] colliders = Physics.OverlapSphere(mouseIcon.transform.position, detectionRadius, platformLayerMask);

        if (colliders.Length > 0)
        {
            unOccupiedmouseIcon.SetActive(false);
            occupiedmouseIcon.SetActive(true);
            mouseIcon = occupiedmouseIcon;

            isOpenSpot = false;
        }
        else
        {
            unOccupiedmouseIcon.SetActive(true);
            mouseIcon = unOccupiedmouseIcon;
            occupiedmouseIcon.SetActive(false);
            
            isOpenSpot = true;
        }

        Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
        RaycastHit hitInfo;

        if (Physics.Raycast(screenRay, out hitInfo, Mathf.Infinity, buildPlatformLayerMask))
        {
            mouseIcon.transform.position = hitInfo.point;
        }
    }

    void HidePlacementCursor()
    {
        unOccupiedmouseIcon.SetActive(false);
        occupiedmouseIcon.SetActive(false);
    }

    //Open the build window and pass along the mouse icon location
    public void SelectBuildLocation(InputAction.CallbackContext context)
    {
        if (context.performed && isActive)
        {
            //platform selection code goes here
            if (isOpenSpot)
            {
                Time.timeScale = 0;
                platformSelectionWindow.SetActive(true);
                platformSelection.GetBuildLocation(mouseIcon.transform.position);
            }
            else
            {
                //play an SFX that tells player it cant be done
            }
        }
    }
}
