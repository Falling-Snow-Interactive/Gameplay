using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Fsi.Gameplay.Healths
{
    [CustomPropertyDrawer(typeof(Health))]
    public class HealthDrawer : PropertyDrawer
    {
        #region IMGUI
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty currentProp = property.FindPropertyRelative("current");
            SerializedProperty maxProp = property.FindPropertyRelative("max");

            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginProperty(position, label, property);

            const float dividerWidth = 18f;
            const float spacing = 4f;

            float fieldWidth = (position.width - dividerWidth - spacing * 2f) * 0.5f;

            Rect currentRect = new(position.x, position.y, fieldWidth, position.height);
            Rect dividerRect = new(currentRect.xMax + spacing, position.y, dividerWidth, position.height);
            Rect maxRect = new(dividerRect.xMax + spacing, position.y, fieldWidth, position.height);

            EditorGUI.PropertyField(currentRect, currentProp, GUIContent.none);
            EditorGUI.LabelField(dividerRect, "/");
            EditorGUI.PropertyField(maxRect, maxProp, GUIContent.none);

            EditorGUI.EndProperty();
        }
        
        #endregion
        
        #region UI Toolkit

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new() { style = { flexDirection = FlexDirection.Row } };

            SerializedProperty currentProp = property.FindPropertyRelative("current");
            SerializedProperty maxProp = property.FindPropertyRelative("max");

            PropertyField currentField = new(currentProp) { label = property.displayName, style = { flexGrow = 1 } };
            PropertyField maxField = new(maxProp){label = "", style = { flexGrow = 1 }};

            Label div = new("/")
                        {
                            style =
                            {
                                paddingLeft = 10,
                            }
                        };
            
            root.Add(currentField);
            root.Add(div);
            root.Add(maxField);
            
            return root;
        }
        
        #endregion
    }
}