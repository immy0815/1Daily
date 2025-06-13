using UnityEngine;

public static class Extensions
{
    // Extension: Transform
    public static T FindChildByName<T>(this Transform trans, string name) where T : Component
    {
        // 비활성화된 것까지 전부 
        T[] children = trans.GetComponentsInChildren<T>(true);
        foreach (T child in children)
        {
            if (child.name == name)
            {
                return child;
            }
        }
        return null;
    }
    
    // Extension: Canvas Group
    public static void SetAlpha(this CanvasGroup canvasGroup, bool active)
    {
        if (active)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
