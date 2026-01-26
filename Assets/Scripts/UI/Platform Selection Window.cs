using System;
using UnityEngine;

public class PlatformSelectionWindow : MonoBehaviour
{
    [SerializeField] private GameObject turret;

    private Vector3 buildPosition;
    
    public void GetBuildLocation(Vector3  position)
    {
        buildPosition = position;
    }

    public void PlacePlatform()
    {
        Instantiate(turret, buildPosition, Quaternion.identity);
        
        CloseWindow();
    }

    public void CloseWindow()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
