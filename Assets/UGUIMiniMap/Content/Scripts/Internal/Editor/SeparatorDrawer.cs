using UnityEditor;
using UnityEngine;

namespace UGUIMiniMap
{
    [CustomPropertyDrawer(typeof(SeparatorAttribute))]
    public class SeparatorDrawer : DecoratorDrawer
    {
        SeparatorAttribute separatorAttribute { get { return ((SeparatorAttribute)attribute); } }


        public override void OnGUI(Rect _position)
        {
            if (separatorAttribute.title == "")
            {
                _position.height = 1;
                _position.y += 19;
                GUI.Box(_position, "");
            }
            else
            {
                Vector2 textSize = GUI.skin.label.CalcSize(new GUIContent(separatorAttribute.title));
                textSize = textSize * 2;
                _position.y += 19;
                GUI.Box(new Rect(_position.xMin - 10, _position.yMin - 8.0f, _position.width + 15, 20), separatorAttribute.title, EditorStyles.toolbarButton);

            }
        }

        public override float GetHeight()
        {
            return 41.0f;
        }
    }
}