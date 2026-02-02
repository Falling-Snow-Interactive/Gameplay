using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Fsi.Gameplay.Randomizers
{
	[Serializable]
	public class RandomizerEntry<T> : ISerializationCallbackReceiver
	{
		[HideInInspector, UsedImplicitly]
		[SerializeField]
		private string name = "";
		
		[SerializeField]
		private T value;
		public T Value
		{
			get => value;
			set => this.value = value;
		}
		
		[SerializeField]
		private int weight = 1;
		public int Weight
		{
			get => weight;
			set => weight = value;
		}

		public override string ToString() => $"{value} ({weight})";
		
		public void OnBeforeSerialize()
		{
			name = ToString();
		}

		public void OnAfterDeserialize() { }
	}
}