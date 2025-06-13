using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance 
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }
    
    public Dictionary<SoundType, Action<float>> OnVolumeChangedByType = new();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public void SetVolume(SoundType type, float volume)
    {
        if (OnVolumeChangedByType.TryGetValue(type, out var callback))
        {
            callback?.Invoke(volume);
        }
    }
}
