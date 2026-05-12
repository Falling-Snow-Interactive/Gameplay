using Fsi.Gameplay.Randomizers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Fsi.Gameplay.Randomizer
{
    [CustomPropertyDrawer(typeof(Randomizer<>))]
    public class RandomizerDrawer : PropertyDrawer
    {
        private const string EntriesProp = "entries";
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            SerializedProperty entriesProp = property.FindPropertyRelative(EntriesProp);
            PropertyField entriesField = new(entriesProp){ label = property.displayName };
            root.Add(entriesField);

            return root;
        }
        
        #region IMGUI
        
        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
        {
            SerializedProperty entriesProp = property.FindPropertyRelative(EntriesProp);
            EditorGUI.PropertyField(position, entriesProp, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label)
        {
            SerializedProperty entriesProp = property.FindPropertyRelative(EntriesProp);
            return EditorGUI.GetPropertyHeight(entriesProp, label, true);
        }
        
        #endregion
    }
}