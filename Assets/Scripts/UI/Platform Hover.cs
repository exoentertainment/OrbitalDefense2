using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Serialized Fields

    [Header("Scriptable Object")]
    [SerializeField] PlatformScriptableObject platformScriptableObject;
    
    [Header("Components")] 
    [SerializeField] private GameObject descriptionWindow;
    [SerializeField] TextMeshProUGUI resourceText;
    [SerializeField] TextMeshProUGUI descriptionText;

    #endregion


    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionWindow.SetActive(true);
        SetResourceText();
        SetDescriptionText();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionWindow.SetActive(false);
    }

    void SetResourceText()
    {
        resourceText.text = platformScriptableObject.resourceCost.ToString();
    }

    void SetDescriptionText()
    {
        descriptionText.text = platformScriptableObject.platformDescription;
    }
}