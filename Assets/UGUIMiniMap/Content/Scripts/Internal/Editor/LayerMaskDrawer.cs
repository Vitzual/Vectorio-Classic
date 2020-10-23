using UnityEngine;
using UnityEditor;

namespace UGUIMiniMap
{
    [CustomPropertyDrawer(typeof(LayerMaskAttribute))]
    public class LayerMaskDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.LayerField(position, new GUIContent(property.name, ""), property.intValue);
        }
    }
}