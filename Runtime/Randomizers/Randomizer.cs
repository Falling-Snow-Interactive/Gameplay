using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fsi.Gameplay.Randomizers
{
	[Serializable]
	public class Randomizer<TValue, TEntry>
		where TEntry : RandomizerEntry<TValue>
	{
		[SerializeField]
		private List<TEntry> entries = new();
		public List<TEntry> Entries => entries; 

		public int TotalWeight => GetWeight(Entries);

		public void Add(TEntry value)
		{
			Entries.Add(value);
		}

		public void Remove(TEntry value)
		{
			Entries.Remove(value);
		}

		public int GetWeight(List<TEntry> entries)
		{
			int weight = 0;
			foreach (TEntry entry in entries)
			{
				weight += entry.Weight;
			}

			return weight;
		}
		
		// ReSharper disable Unity.PerformanceAnalysis
		public TValue Randomize(List<TEntry> entries, int totalWeight)
		{
			if (entries.Count == 0 || totalWeight == 0)
			{
				Debug.LogWarning("There is nothing to randomize.");
				return default;
			}
			
			int roll = Random.Range(0, totalWeight);
			int weight = 0;
			foreach (TEntry entry in entries)
			{
				weight += entry.Weight;
				if (roll < weight) return entry.Value;
			}

			Debug.LogError($"Randomizer {typeof(TValue).Name} is out of range. Roll: {roll} - Total: {TotalWeight}.");
			return default;
		}

		public TValue Randomize()
		{
			if (Entries.Count == 0 || TotalWeight == 0)
			{
				return default;
			}
			return Randomize(Entries, TotalWeight);
		}

		public List<TValue> Randomize(int amount, bool repeats)
		{
			if (Entries.Count == 0 || TotalWeight == 0　|| amount == 0)
			{
				return new List<TValue>();
			}
			
			if (repeats)
			{
				List<TValue> e = new();
				for (int i = 0; i < amount; i++)
				{
					e.Add(Randomize());
				}

				return e;
			}

			List<TValue> randomize = new();
			for (int i = 0; i < amount; i++)
			{
				List<TEntry> adjusted = new();
				foreach (TEntry e in Entries)
				{
					if (!randomize.Contains(e.Value))
					{
						adjusted.Add(e);
					}
				}

				if (adjusted.Count == 0)
				{
					return randomize;
				}

				int weight = GetWeight(adjusted);
				TValue selected = Randomize(adjusted, weight);
				randomize.Add(selected);
			}

			return randomize;
		}
	}
}