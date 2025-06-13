using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

  /// <summary>
  /// 싱글톤보다 정적 클래스가 접근하기 편할 것 같아 정적 클래스로 구현했습니다.
  /// 각 값은 SoundManager 초기화시 자동으로 불러오며, 값을 설정시 해당 값을 자동으로 저장합니다.
  /// </summary>
  public static class SoundManager
  {
    public static bool Loaded { get; private set; } = false;
    public const string Label = "Audio";
    public static readonly Dictionary<string, AudioClip> Clips = new();

    private static Camera mainCamera;
    private static AudioSource uiSource = null, bgmSource = null;
    private static AudioMixerGroup uiGroup, bgmGroup, sfxGroup;
    
    static SoundManager()
    {
      InitScene();

      SceneManager.activeSceneChanged += (_, _) =>
      {
        InitScene();
      };
    }

    private static void InitScene()
    {
      mainCamera = Camera.main;
      uiSource = null;
      bgmSource = null;
        
      UISource?.Stop();
      BgmSource?.Stop();
    }

    #region Volumes

    private static AudioMixer mixer;
    public static AudioMixer Mixer
    {
      get => mixer;
      set
      {
        mixer = value;
        if (mixer)
        {
          MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 80);
          BgmVolume = PlayerPrefs.GetFloat("BgmVolume", 80);
          UIVolume = PlayerPrefs.GetFloat("UIVolume", 80);
          SfxVolume = PlayerPrefs.GetFloat("SfxVolume", 80);
        
          uiGroup = Mixer.FindMatchingGroups("UI")[0];
          bgmGroup = Mixer.FindMatchingGroups("Bgm")[0];
          sfxGroup = Mixer.FindMatchingGroups("Sfx")[0];
        }
      }
    }

    /// <summary>
    /// 각 볼륨을 총괄하는 마스터 볼륨입니다.
    /// 해당 볼륨을 변경시 다른 모든 볼륨이 영향받습니다.
    /// 값은 0~100 사이로 설정할 수 있고, 미만 혹은 초과시 자동으로 포맷팅됩니다.
    /// </summary>
    public static float MasterVolume
    {
      get => PlayerPrefs.GetFloat("MasterVolume", 80);
      set
      {
        var input = Math.Max(0, Math.Min(100, value));

        if(UISource) UISource.volume = (input / 100) * (BgmVolume / 100);
        if(BgmSource) BgmSource.volume = (input / 100) * (UIVolume / 100);
        PlayerPrefs.SetFloat("MasterVolume", input);
      }
    }

    /// <summary>
    /// 배경 볼륨입니다.
    /// 값은 0~100 사이로 설정할 수 있고, 미만 혹은 초과시 자동으로 포맷팅됩니다.
    /// </summary>
    public static float BgmVolume
    {
      get => PlayerPrefs.GetFloat("BgmVolume", 80);
      set
      {
        var input = Math.Max(0, Math.Min(100, value));

        if(BgmSource) BgmSource.volume = (input / 100) * (MasterVolume / 100);
        PlayerPrefs.SetFloat("BgmVolume", input);
      }
    }

    /// <summary>
    /// 효과음 볼륨입니다.
    /// 값은 0~100 사이로 설정할 수 있고, 미만 혹은 초과시 자동으로 포맷팅됩니다.
    /// </summary>
    public static float UIVolume
    {
      get => PlayerPrefs.GetFloat("UIVolume", 80);
      set
      {
        var input = Math.Max(0, Math.Min(100, value));
        
        if(UISource) UISource.volume = (input / 100) * (MasterVolume / 100);
        PlayerPrefs.SetFloat("UIVolume", input);
      }
    }
    
    /// <summary>
    /// 오브젝트 볼륨입니다.
    /// 값은 0~100 사이로 설정할 수 있고, 미만 혹은 초과시 자동으로 포맷팅됩니다.
    /// </summary>
    public static float SfxVolume
    {
      get => Mixer && Mixer.GetFloat("Sfx", out var value) ? value + 80 : PlayerPrefs.GetFloat("ObjectVolume", 80);
      set
      {
        var input = Math.Max(0, Math.Min(100, value));

        if(Mixer) Mixer.SetFloat("Sfx", input - 80);
        PlayerPrefs.SetFloat("SfxVolume", input);
      }
    }

    public static AudioSource UISource
    {
      get
      {
        if (mainCamera == null) return null;
        
        if (uiSource && uiSource.gameObject == mainCamera.gameObject)
          return uiSource;
        
        uiSource = mainCamera.GetComponents<AudioSource>().FirstOrDefault(s => s.outputAudioMixerGroup == uiGroup);

        if (uiSource) return uiSource;
        
        uiSource = mainCamera.gameObject.AddComponent<AudioSource>();
        uiSource.outputAudioMixerGroup = uiGroup;
        uiSource.volume = (UIVolume / 100) * (MasterVolume / 100);

        return uiSource;
      }
    }
    
    public static AudioSource BgmSource
    {
      get
      {
        if (!mainCamera) return null;
        
        if (bgmSource && bgmSource.gameObject == mainCamera.gameObject)
          return bgmSource;
        
        bgmSource = mainCamera.GetComponents<AudioSource>().FirstOrDefault(s => s.outputAudioMixerGroup == bgmGroup);

        if (bgmSource) return bgmSource;
        
        bgmSource = mainCamera.gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.outputAudioMixerGroup = bgmGroup;
        bgmSource.volume = (BgmVolume / 100) * (MasterVolume / 100);

        return bgmSource;
      }
    }

    #endregion

    #region Controller
    
    public static void Play(AudioClip clip, GameObject obj)
    {
      if(!Mixer) return;

      if (obj.TryGetComponent(out AudioSource source)) {}
      else source = obj.AddComponent<AudioSource>();
      
      source.clip = clip;
      source.outputAudioMixerGroup = sfxGroup;
      source.Play();
    }

    public static void Play(string clipName, GameObject obj)
    {
      if(!Loaded) return;
      
      if(Clips.TryGetValue(clipName, out var clip)) Play(clip, obj);
      #if UNITY_EDITOR
      else Debug.LogWarning($"AudioClip {clipName} is not found.");
      #endif
    }

    public static void Play(AudioClip clip, AudioType type = AudioType.UI)
    {
      if(!Mixer) return;
      
      var source = type switch
      {
        AudioType.Bgm => BgmSource,
        AudioType.UI => UISource,
        _ => UISource
      };
      
      source.clip = clip;
      source.outputAudioMixerGroup = type == AudioType.Bgm ? bgmGroup : uiGroup;
      source.Play();
    }
    
    public static void Play(string clipName, AudioType type = AudioType.UI)
    {
      if(!Loaded) return;
      
      if(Clips.TryGetValue(clipName, out var clip)) Play(clip, type);
      #if UNITY_EDITOR
      else Debug.LogWarning($"AudioClip {clipName} is not found.");
      #endif
    }

    public static void Stop(AudioType type = AudioType.Bgm)
    {
      if(!Mixer) return;
      
      var source = type switch
      {
        AudioType.Bgm => BgmSource,
        AudioType.UI => UISource,
        _ => UISource
      };
      
      source.Stop();
    }
    
    #endregion
  }

  public enum AudioType
  {
    Bgm,
    UI
  }
