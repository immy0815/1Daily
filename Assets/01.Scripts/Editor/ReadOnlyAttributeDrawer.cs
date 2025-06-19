using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace _01.Scripts.Editor
{
  [CustomPropertyDrawer(typeof(ReadOnlyAttribute), true)]
  public class ReadOnlyAttributeDrawer : PropertyDrawer
  {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return EditorGUI.GetPropertyHeight(property, label, true);
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginProperty(position, label, property);
      GUI.enabled = !Application.isPlaying && ((ReadOnlyAttribute)attribute).runtimeOnly;
      EditorGUI.PropertyField(position, property, label, true);
      GUI.enabled = true;
      EditorGUI.EndProperty();
    }
  }
}