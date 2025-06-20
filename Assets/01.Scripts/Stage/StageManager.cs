using Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
  public const int LastStageIndex = 3;
  [SerializeField, ReadOnly] private Stage currentStage = null;
  [SerializeField, ReadOnly] private GameObject clearObj;
  
  /// <summary>
  /// 현재 진행중인 스테이지입니다.
  /// 스테이지가 진행중이 아닐경우 null을 반환합니다.
  /// </summary>
  public static Stage CurrentStage => Instance.currentStage;
  
  public static GameObject ClearOrb => Instance.clearObj;
  public static int StageIndex { get; private set; } = 1;
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

  private void Awake()
  {
    Cursor.lockState = CursorLockMode.Locked;
    clearObj = Addressables.LoadAssetAsync<GameObject>("ClearOrb").WaitForCompletion();
  }

  private void OnDestroy()
  {
    if(clearObj)
      Addressables.Release(clearObj);
    
    Cursor.lockState = CursorLockMode.None;
  }
  
  /// <summary>
  /// 스테이지를 시작할 수 있습니다.
  /// 게임씬을 로딩 후 시작시킵니다.
  /// </summary>
  /// <returns></returns>
  public static void StartStageStatic()
  {
    var sceneName = SceneManager.GetActiveScene().name;
    
    if (sceneName != "GameScene")
    {
      UIManager.Instance.EnterScene(SceneType.Game);
      SceneManager.sceneLoaded += OnSceneLoaded;
    }
    else
    {
      Instance.StartStage(StageIndex);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      Instance.OnStageEnd.AddListener((state) =>
      {
        switch (state)
        {
          case StageFinishState.Cancel:
          {
            // Destroy(gameObject);
            UIManager.Instance.EnterScene(SceneType.Start);
            break;
          }
          case StageFinishState.Clear:
          {
            break;
          }
          case StageFinishState.Failure:
          {
            // Destroy(gameObject);
            // 죽었을 때 → 노이즈 → 인트로 (화면 전환 느낌)
            UIManager.Instance.EnterScene(SceneType.Start, true);
            break;
          }
        }
      });
    
      SceneManager.sceneLoaded -= OnSceneLoaded;
    }
  }
    

  /// <summary>
  /// StageClearTrigger(ClearOrb) 상호작용 용도 메서드
  /// </summary>
  public void StartNextStage()
  {
    if(StageIndex != LastStageIndex)
    {
      StageIndex++;
      StartStage(StageIndex);
    }
    else
    {
      StageIndex = 1;
      currentStage = null;
      Cursor.lockState = CursorLockMode.None;
      
      UIManager.Instance.EnterScene(SceneType.Credit);
    }
  }
  
  public static void StartNextStageStatic() => Instance.StartNextStage();
  
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
    StageIndex = stageIndex;
    
    if (sceneName == "GameScene")
    {
      StopStage();
      
      if (ResourceManager.Instance.InstantiateStage($"Stage{StageIndex}", out var obj))
      {
        var stage = obj.GetComponent<Stage>();
        obj.transform.position = Vector3.zero;
        currentStage = stage;
        stage.StartStage();
        OnStageStart?.Invoke(stage);
        stage.OnStageEnd.AddListener(OnStageFinish);

        var vCam = GameObject.Find("FirstPersonCamera").GetComponent<CinemachineVirtualCamera>();
        vCam.Follow = stage.Player.transform;
        vCam.LookAt = stage.Player.transform;
      }
    }
    else UIManager.Instance.EnterScene(SceneType.Game);
  }

  private void Start()
  {
    if (ResourceManager.Instance.InstantiateStage($"Stage{StageIndex}", out var obj))
    {
      var stage = obj.GetComponent<Stage>();
      obj.transform.position = Vector3.zero;
      currentStage = stage;
      stage.StartStage();
      OnStageStart?.Invoke(stage);
      stage.OnStageEnd.AddListener(OnStageFinish);

      var vCam = GameObject.Find("FirstPersonCamera").GetComponent<CinemachineVirtualCamera>();
      vCam.Follow = stage.Player.transform;
      vCam.LookAt = stage.Player.transform;
    }
  }

  /// <summary>
  /// 현재 진행중인 스테이지가 있다면 강제로 종료시킵니다.
  /// </summary>
  public void StopStage()
  {
    if(currentStage)
    {
      currentStage.StopStage();
      Destroy(currentStage.gameObject);
      currentStage = null;
    }
  }

  private void OnStageFinish(StageFinishState state)
  {
    currentStage.OnStageEnd.RemoveListener(OnStageFinish);
    OnStageEnd?.Invoke(state);
  }
}
