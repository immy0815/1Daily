using UnityEngine;

namespace Retronia.Core
{
  [ AddComponentMenu( "Audio/Audio Helper" )]
  public class SoundHelper : MonoBehaviour
  {
    #region State 
    #if UNITY_EDITOR
    /// <summary>
    /// TODO 이후 GetSet 애트리뷰트를 추가하면 구현될 기능입니다.
    /// </summary>
    
    [Tooltip("아직 미지원되는 기능입니다.")]
    [SerializeField, Range(0, 100)] public float masterVolume = 0;
    [SerializeField, Range(0, 100)] public float bgmVolume = 0;
    [SerializeField, Range(0, 100)] public float uiVolume = 0;
    [SerializeField, Range(0, 100)] public float sfxVolume = 0;
    
    #endif

    /// <summary>
    /// Master 볼률을 조정할 수 있습니다. <br/>
    /// 범위는 0 ~ 100 입니다.
    /// </summary>
    public float MasterVolume
    {
      get => SoundManager.MasterVolume;
      set => SoundManager.MasterVolume = value;
    }

    /// <summary>
    /// Bgm 볼률을 조정할 수 있습니다. <br/>
    /// 범위는 0 ~ 100 입니다.
    /// </summary>
    public float BgmVolume
    {
      get => SoundManager.BgmVolume;
      set => SoundManager.BgmVolume = value;
    }

    /// <summary>
    /// UI 볼률을 조정할 수 있습니다. <br/>
    /// 범위는 0 ~ 100 입니다.
    /// </summary>
    public float UIVolume
    {
      get => SoundManager.UIVolume;
      set => SoundManager.UIVolume = value;
    }

    /// <summary>
    /// Sfx 볼률을 조정할 수 있습니다. <br/>
    /// 범위는 0 ~ 100 입니다.
    /// </summary>
    public float SfxVolume
    {
      get => SoundManager.SfxVolume;
      set => SoundManager.SfxVolume = value;
    }
    
    #endregion
    
    #region Controller
    
    /// <param name="clip">재생할 AudioClip입니다.</param>
    /// <param name="type">재생 방식입니다.</param>
    public void Play(AudioClip clip, AudioType type)
    {
      if (!clip) return;
      SoundManager.Play(clip, type);
    }

    /// <summary>
    /// 클립을 Bgm으로 재생합니다.
    /// </summary>
    /// <param name="clip">재생할 AudioClip입니다.</param>
    public void PlayBgm(AudioClip clip)
    {
      if (!clip) return;
      SoundManager.Play(clip, AudioType.Bgm);
    }

    /// <summary>
    /// SoundManager의 Clips에서 해당 이름의 클립을 받아와서 Bgm으로 재생합니다.
    /// </summary>
    /// <param name="clipId">재생할 AudioClip의 이름입니다.</param>
    public void PlayBgm(string clipId) => SoundManager.Play(clipId, AudioType.Bgm);
    
    /// <summary>
    /// 클립을 UI로 재생합니다.
    /// </summary>
    /// <param name="clip">재생할 AudioClip입니다.</param>
    public void PlayUI(AudioClip clip)
    {
      if (!clip) return;
      SoundManager.Play(clip, AudioType.UI);
    }
    
    /// <summary>
    /// SoundManager의 Clips에서 해당 이름의 클립을 받아와서 UI로 재생합니다.
    /// </summary>
    /// <param name="clipId">재생할 AudioClip의 이름입니다.</param>
    public void PlayUI(string clipId) => SoundManager.Play(clipId, AudioType.UI);

    /// <summary>
    /// 타입에 맞는 음원의 재생을 중지시킵니다.
    /// </summary>
    /// <param name="type">중지할 음원의 타입입니다.</param>
    public void Stop(AudioType type)
    {
      SoundManager.Stop(type);
    }
    
    #endregion
  }
}