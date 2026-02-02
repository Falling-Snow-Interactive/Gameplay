using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Fsi.Gameplay.Healths
{
    [CustomPropertyDrawer(typeof(Health))]
    public class HealthDrawer : PropertyDrawer
    {
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
    }
}