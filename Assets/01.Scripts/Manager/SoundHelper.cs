using UnityEngine;
using UnityEngine.Serialization;

namespace Retronia.Core
{
  [ AddComponentMenu( "Audio/Audio Helper" )]
  public class SoundHelper : MonoBehaviour
  {
    #region State 
    #if UNITY_EDITOR
    
    [SerializeField, Range(0, 100)] public float masterVolume = 0;
    [SerializeField, Range(0, 100)] public float bgmVolume = 0;
    [SerializeField, Range(0, 100)] public float uiVolume = 0;
    [SerializeField, Range(0, 100)] public float sfxVolume = 0;
    
    #endif

    public float MasterVolume
    {
      get => SoundManager.MasterVolume;
      set => SoundManager.MasterVolume = value;
    }

    public float BgmVolume
    {
      get => SoundManager.BgmVolume;
      set => SoundManager.BgmVolume = value;
    }

    public float UIVolume
    {
      get => SoundManager.UIVolume;
      set => SoundManager.UIVolume = value;
    }

    public float SfxVolume
    {
      get => SoundManager.SfxVolume;
      set => SoundManager.SfxVolume = value;
    }
    
    #endregion
    
    #region Controller
    
    public void Play(AudioClip clip, AudioType type)
    {
      if (!clip || !SoundManager.Loaded) return;
      SoundManager.Play(clip, type);
    }

    public void PlayBgm(AudioClip clip)
    {
      if (!clip || !SoundManager.Loaded) return;
      SoundManager.Play(clip, AudioType.Bgm);
    }

    public void PlayBgm(string clipName) => SoundManager.Play(clipName, AudioType.Bgm);
    
    public void PlayUI(AudioClip clip)
    {
      if (!clip || !SoundManager.Loaded) return;
      SoundManager.Play(clip, AudioType.UI);
    }
    
    public void PlayUI(string clipName) => SoundManager.Play(clipName, AudioType.UI);

    public void Stop(AudioType type)
    {
      if (!SoundManager.Loaded) return;
      SoundManager.Stop(type);
    }
    
    #endregion
  }
}