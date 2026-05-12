using System;
using UnityEngine;

namespace Fsi.Gameplay.Healths
{
	/// <summary>
	/// Represents a simple health container with current and maximum values,
	/// providing events for when health changes or reaches zero.
	/// </summary>
	[Serializable]
	public class Health
	{
		#region Events
		
		/// <summary>
		/// Invoked whenever the health value changes (damage, healing, or initialization).
		/// </summary>
		public event Action Changed;
		
		/// <summary>
		/// Invoked when health reaches zero for the first time.
		/// </summary>
		public event Action Died;
		
		#endregion

		#region Inspector Properties
		
		[Tooltip("The current health value.")]
		[Min(0)]
		[SerializeField]
		private int current;

		[Tooltip("The maximum health value.")]
		[Min(0)]
		[SerializeField]
		private int max;
		
		#endregion
		
		#region Public Properties

		/// <summary>
		/// Gets the current health value.
		/// </summary>
		public int Current => current;

		/// <summary>
		/// Gets the maximum health value.
		/// </summary>
		public int Max => max;
		
		/// <summary>
		/// Returns true if the current health is greater than zero.
		/// </summary>
		public bool IsAlive => current > 0;
		
		/// <summary>
		/// Returns true if the current health is zero or below.
		/// </summary>
		public bool IsDead => current <= 0;

		/// <summary>
		/// Gets the health ratio between 0 and 1.
		/// </summary>
		public float Normalized => (float)current / max;
		
		#endregion

		/// <summary>
		/// Creates a new health container with the specified initial value.
		/// </summary>
		/// <param name="health">The initial and maximum health.</param>
		public Health(int health)
		{
			current = health;
			max = health;
		}

		public Health(int current, int max)
		{
			this.current = current;
			this.max = max;
		}

		/// <summary>
		/// Sets both current and maximum health to the given value.
		/// </summary>
		/// <param name="maxHealth">The new maximum and starting health value.</param>
		/// <param name="notify">Whether to notify listeners of the change.</param>
		public void Initialize(int maxHealth, bool notify = true)
		{
			max = maxHealth;
			current = maxHealth;

			if (notify)
			{
				Changed?.Invoke();
			}
		}

		// ReSharper disable Unity.PerformanceAnalysis
		/// <summary>
		/// Reduces the current health by the specified amount.
		/// </summary>
		/// <param name="damage">Amount of damage to apply.</param>
		/// <param name="notify">Whether to notify listeners of the change.</param>
		/// <returns>The actual damage applied.</returns>
		public int Damage(int damage, bool notify = true)
		{
			if (IsDead)
			{
				return 0;
			}

			int damaged = Mathf.Clamp(damage, 0, current);
			current -= damaged;
			
			if (notify)
			{
				Changed?.Invoke();
			}

			if (IsDead)
			{
				Died?.Invoke();
			}

			return damaged;
		}

		// ReSharper disable Unity.PerformanceAnalysis
		/// <summary>
		/// Increases the current health by the specified amount, up to the maximum.
		/// </summary>
		/// <param name="heal">Amount of healing to apply.</param>
		/// <param name="notify">Whether to notify listeners of the change.</param>
		/// <returns>The actual healing applied.</returns>
		public int Heal(int heal, bool notify = true)
		{
			if (IsDead)
			{
				return 0;
			}

			int healed = Mathf.Clamp(heal, 0, max - current);
			current += healed;
			
			if (notify)
			{
				Changed?.Invoke();
			}

			return healed;
		}

		public int SetCurrent(int newCurrent, bool notifiy = true)
		{
			if (IsDead)
			{
				return 0;
			}

			int dif = newCurrent - current;
			return dif switch
			       {
				       > 0 => Heal(dif, notifiy),
				       < 0 => Damage(dif, notifiy),
				       _ => 0,
			       };
		}

		/// <summary>
		/// Updates the maximum health value and clamps current health if needed.
		/// </summary>
		/// <param name="max">New maximum health.</param>
		/// <param name="notify">Whether to notify listeners.</param>
		public void SetMax(int max, bool notify = true)
		{
			this.max = max;
			current = Mathf.Clamp(current, 0, max);

			if (notify)
			{
				Changed?.Invoke();
			}
		}
	}
}