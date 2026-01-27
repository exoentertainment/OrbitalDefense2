using System;
using UnityEngine;

public class PlatformSelectionWindow : MonoBehaviour
{
    private Vector3 buildPosition;
    
    public void GetBuildLocation(Vector3  position)
    {
        buildPosition = position;
    }

    public void PlacePlatform(PlatformScriptableObject platform)
    {
        Instantiate(platform.platformPrefab, buildPosition, Quaternion.identity);
        //Decrease resources
        
        CloseWindow();
    }

    public void CloseWindow()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
