using Fsi.Gameplay.Randomizers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Fsi.Gameplay.Randomizer
{
    [CustomPropertyDrawer(typeof(Randomizer<>))]
    public class RandomizerDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new();

            SerializedProperty entriesProp = property.FindPropertyRelative("entries");
            PropertyField entriesField = new(entriesProp){ label = property.displayName };
            root.Add(entriesField);

            return root;
        }
    }
}