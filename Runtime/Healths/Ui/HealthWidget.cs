using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fsi.Gameplay.Healths.Ui
{
    public class HealthWidget : MonoBehaviour
    {
        #region Runtime Variables

        private Health health;
		
        #endregion
        
        #region Inspector Properties
		
        [Header("Properties")]

        [SerializeField]
        private Gradient gradient = new();

        [SerializeField]
        private bool showMaxHealth;

        [Header("References")]
		
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private Image fillImage;

        [SerializeField]
        private TMP_Text text;
		
        #endregion
        
        #region MonoBehaviour

        private void Awake()
        {
            slider.interactable = false;
        }

        private void OnEnable()
        {
            if (health != null)
            {
                health.Changed += OnHealthChanged;
            }
        }

        private void OnDisable()
        {
            if (health != null)
            {
                health.Changed -= OnHealthChanged;
            }
        }
        
        #endregion
        
        #region Initialize

        public void Initialize(Health health)
        {
            this.health = health;

            health.Changed += OnHealthChanged;
            Refresh();
        }
        
        #endregion
		
        #region Event Callbacks

        private void OnHealthChanged()
        {
            Refresh();
        }
		
        #endregion
        
        #region Visual Controls
		
        // ReSharper disable Unity.PerformanceAnalysis
        private void Refresh()
        {
            slider?.SetValueWithoutNotify(health.Normalized);

            if (fillImage)
            {
                fillImage.color = gradient.Evaluate(health.Normalized);
            }

            if (text)
            {
                text.text = showMaxHealth ? $"{health.Current}/{health.Max}" : $"{health.Current}";
            }
        }
		
        #endregion
    }
}
