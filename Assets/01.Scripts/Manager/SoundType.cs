using System;

public enum SoundType
{
  Master,
  UI,
  Bgm,
  Sfx,
}

public static class SoundTypeExtension
{
  /// <summary>
  /// 타입에 맞는 볼륨을 조정합니다 <br/>
  /// 각 볼륨의 범위는 0 ~ 100입니다.
  /// </summary>
  /// <param name="type">값을 변경할 볼륨의 타입입니다.</param>
  /// <param name="volume">새로 설정할 값입니다. 범위는 0 ~ 100 입니다.</param>
  public static void SetVolume(this SoundType type, float volume)
  {
    switch (type)
    {
      case SoundType.Master:
        SoundManager.MasterVolume = volume;
        break;
      case SoundType.UI:
        SoundManager.UIVolume = volume;
        break;
      case SoundType.Bgm:
        SoundManager.BgmVolume = volume;
        break;
      case SoundType.Sfx:
        SoundManager.SfxVolume = volume;
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(type), type, null);
    }
  }

  /// <summary>
  /// 타입에 맞는 볼륨의 값을 가져옵니다. <br/>
  /// 범위는 0 ~ 100입니다.
  /// </summary>
  /// <param name="type">가져올 볼륨의 type입니다.</param>
  /// <returns>봄륨의 세기입니다. 범위는 0 ~ 100입니다.</returns>
  public static float GetVolume(this SoundType type) => type switch
  {
    SoundType.Master => SoundManager.MasterVolume,
    SoundType.UI => SoundManager.UIVolume,
    SoundType.Bgm => SoundManager.BgmVolume,
    SoundType.Sfx => SoundManager.SfxVolume,
    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
  };
}
