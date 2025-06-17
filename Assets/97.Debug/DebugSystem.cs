using UnityEditor;
using UnityEngine;

public class DebugSystem : EditorWindow  
{
    [MenuItem("Window/My Debug Tool")]
    public static void ShowWindow()    
    {
        GetWindow<DebugSystem>("Debug Tool");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Spawn Map"))
        {
            // ResourceManager.Instance.InstantiateStage("Stage1");
        }

        if (GUILayout.Button("Effect UI Text"))
        {
            UIManager.Instance.PlayEffectText();
        }
    }
}