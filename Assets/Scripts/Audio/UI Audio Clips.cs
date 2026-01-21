using UnityEngine;

[CreateAssetMenu(fileName = "UI Clips", menuName = "Audio SO/UI Clips")]
public class UIAudioClips : ScriptableObject
{
    public AudioClipSO selectSound;
    public AudioClipSO closeSound;
    public AudioClipSO errorSound;
}
