using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

  /// <summary>
  /// 싱글톤보다 정적 클래스가 접근하기 편할 것 같아 정적 클래스로 구현했습니다.
  /// 각 값은 SoundManager 초기화시 자동으로 불러오며, 값을 설정시 해당 값을 자동으로 저장합니다.
  /// </summary>
  public static class SoundManager
  {
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
    /// <summary>
    /// 현재 활성화된 AudioMixer를 가져올 수 있습니다.
    /// Temp) 임시로 Addressable를 통해서 AudioMixer를 가져옵니다.
    /// </summary>
    public static AudioMixer Mixer
    {
      get
      {
        if(!mixer)
        {
          mixer = Addressables.LoadAssetAsync<AudioMixer>("AudioMixer").WaitForCompletion();
          
          MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 80);
          BgmVolume = PlayerPrefs.GetFloat("BgmVolume", 80);
          UIVolume = PlayerPrefs.GetFloat("UIVolume", 80);
          SfxVolume = PlayerPrefs.GetFloat("SfxVolume", 80);
        
          uiGroup = mixer.FindMatchingGroups("UI")[0];
          bgmGroup = mixer.FindMatchingGroups("Bgm")[0];
          sfxGroup = mixer.FindMatchingGroups("Sfx")[0];
        }

        return mixer;
      }
      set
      {
        mixer = value;
        if (!mixer) return;
        
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 80);
        BgmVolume = PlayerPrefs.GetFloat("BgmVolume", 80);
        UIVolume = PlayerPrefs.GetFloat("UIVolume", 80);
        SfxVolume = PlayerPrefs.GetFloat("SfxVolume", 80);
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

    /// <summary>
    /// 현재 활성화된 UI AudioSource를 가져옵니다.
    /// </summary>
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
    
    /// <summary>
    /// 현재 활성화된 Bgm AudioSource를 가져옵니다.
    /// </summary>
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
    
    /// <summary>
    /// 오브젝트에서 clip을 재생합니다.
    /// </summary>
    /// <param name="clip">재생할 AudioClip입니다.</param>
    /// <param name="obj">AudioClip이 재생될 게임오브젝트입니다.</param>
    public static void Play(AudioClip clip, GameObject obj)
    {
      if(!Mixer) return;

      if (obj.TryGetComponent(out AudioSource source)) {}
      else source = obj.AddComponent<AudioSource>();
      
      source.clip = clip;
      source.outputAudioMixerGroup = sfxGroup;
      source.Play();
    }

    /// <summary>
    /// SoundManager.Clips에서 clipId에 맞는 AudioClip를 가져와 obj에서 재생합니다.
    /// </summary>
    /// <param name="clipId">재생할 클립의 Clips에 등록된 Id입니다.</param>
    /// <param name="obj">AudioClip이 재생될 게임오브젝트입니다.</param>
    public static void Play(string clipId, GameObject obj)
    {
      if(Clips.TryGetValue(clipId, out var clip)) Play(clip, obj);
      #if UNITY_EDITOR
      else Debug.LogWarning($"AudioClip {clipId} is not found.");
      #endif
    }

    /// <summary>
    /// type에 맞는 방식으로 AudioClip을 재생합니다.
    /// </summary>
    /// <param name="clip">재생할 AudioClip입니다.</param>
    /// <param name="type">음원을 재생할 방식입니다.</param>
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
    
    /// <summary>
    /// SoundManager.Clips에서 clipId에 맞는 AudioClip를 가져와 type의 방식으로 재생합니다.
    /// </summary>
    /// <param name="clipId"></param>
    /// <param name="type"></param>
    public static void Play(string clipId, AudioType type = AudioType.UI)
    {
      if(Clips.TryGetValue(clipId, out var clip)) Play(clip, type);
      #if UNITY_EDITOR
      else Debug.LogWarning($"AudioClip {clipId} is not found.");
      #endif
    }

    /// <summary>
    /// 타입에 맞는 음원의 재생을 중지시킵니다.
    /// </summary>
    /// <param name="type">중지할 음원의 타입입니다.</param>
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
