using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class StageManager : Singleton<StageManager>
{
  public const int LastStageIndex = 3;
  [SerializeField, ReadOnly] private int stageIndex = 1;
  [SerializeField, ReadOnly] private Stage currentStage = null;
  
  /// <summary>
  /// 현재 진행중인 스테이지입니다.
  /// 스테이지가 진행중이 아닐경우 null을 반환합니다.
  /// </summary>
  public static Stage CurrentStage => Instance.currentStage;
  /// <summary>
  /// 스테이지 시작시 이벤트입니다.
  /// 인자로 시작하려는 스테이지를 넘겨줍니다.
  /// </summary>
  public UnityEvent<Stage> OnStageStart = new();
  
  /// <summary>
  /// 스테이지 종료 시 이벤트입니다.
  /// 해당 이벤트 호출 시점에는 currentStage가 null이 아닙닌다.
  /// </summary>
  public UnityEvent<StageFinishState> OnStageEnd = new();

  /// <summary>
  /// 스테이지를 시작할 수 있습니다.
  /// 게임씬을 로딩 후 시작시킵니다.
  /// </summary>
  /// <returns></returns>
  public void StartStage()
  {
    OnStageEnd.AddListener((state) =>
    {
      switch (state)
      {
        case StageFinishState.Cancel:
        {
          UIManager.Instance.EnterScene(SceneType.Start);
          break;
        }
        case StageFinishState.Clear:
        {
          if (stageIndex == LastStageIndex)
          {
            // 마지막 스테이지 클리어시
          }
          stageIndex++;
          break;
        }
        case StageFinishState.Failure:
        {
          // 죽었을 때 → 노이즈 → 인트로 (화면 전환 느낌)
          UIManager.Instance.onUpdateDeathAnimation.Invoke();
          break;
        }
      }
    });
    
    StartStage(stageIndex);
  }
  
  public static void StartStageStatic() => Instance.StartStage();
  public static void StartStageStatic(int stageIndex) => Instance.StartStage(stageIndex);
  
  /// <summary>
  /// 스테이지를 시작할 수 있습니다.
  /// 게임씬을 로딩 후 시작시킵니다.
  /// </summary>
  /// <param name="stageIndex">시작할 스테이지의 번호입니다.</param>
  /// <returns>시작한 스테이지를 </returns>
  public void StartStage(int stageIndex)
  {
    var sceneName = SceneManager.GetActiveScene().name;
    
    if (sceneName == "GameScene") StopStage();

    void OnSceneLoad(Scene scene, LoadSceneMode _)
    {
      if (scene.name != "GameScene") return;
      var origin = Addressables.LoadAssetAsync<GameObject>($"Stage{stageIndex}").WaitForCompletion();
      var obj = Object.Instantiate(origin);
      
      if (obj)
      {
        var stage = obj.GetComponent<Stage>();
        obj.transform.position = Vector3.zero;
        currentStage = stage;
        stage.StartStage();
        OnStageStart?.Invoke(stage);
        stage.OnStageEnd.AddListener(OnStageFinish);
        stage.OnStageEnd.AddListener((_) =>
        {
          Addressables.ReleaseInstance(origin);
        });

        var vCam = GameObject.Find("FirstPersonCamera").GetComponent<CinemachineVirtualCamera>();
        vCam.Follow = stage.Player.transform;
        vCam.LookAt = stage.Player.transform;
      }
      else
      {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
#if UNITY_EDITOR
        Debug.LogError("Stage not found");
#endif
      }
      
      SceneManager.sceneLoaded -= OnSceneLoad;
    };

    SceneManager.sceneLoaded += OnSceneLoad;
    
    UIManager.Instance.EnterScene(SceneType.Game);
  }

  /// <summary>
  /// 현재 진행중인 스테이지가 있다면 강제로 종료시킵니다.
  /// </summary>
  public void StopStage()
  {
    if(currentStage)
    {
      currentStage.StopStage();
      Object.Destroy(currentStage.gameObject);
      currentStage = null;
    }
  }

  private void OnStageFinish(StageFinishState state)
  {
    currentStage.OnStageEnd.RemoveListener(OnStageFinish);
    OnStageEnd?.Invoke(state);
  }
}
