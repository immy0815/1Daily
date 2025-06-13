using UnityEngine;
using UnityEngine.UI;

public class DebugTool : MonoBehaviour
{
    [SerializeField] private Button _loadSceneButton;

    private void Start()
    {
        _loadSceneButton.onClick.AddListener(OnClickSwitchScene);
    }

    private void OnClickSwitchScene()
    {
        Debug.Log("[DebugTool] SwitchScene button clicked.");
        ResourceManager.Instance.SwitchScene(SceneName.Game);
    }
}
