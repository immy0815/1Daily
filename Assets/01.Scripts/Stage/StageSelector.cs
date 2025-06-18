using UnityEngine;

public class StageSelector : MonoBehaviour 
{
    public void Start()
    {
        StageManager.StartStageStatic(1);
    }
}
