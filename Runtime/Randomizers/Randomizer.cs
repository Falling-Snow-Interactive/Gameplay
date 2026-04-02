using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using Random = System.Random;

namespace Fsi.Gameplay.Randomizers
{
	[Serializable]
	public class Randomizer<TValue>
		// where TEntry : RandomizerEntry<TValue>
	{
		private Random _random;
		private Random Random => _random ??= new Random();

		[SerializeField]
		private List<RandomizerEntry<TValue>> entries = new();
		public List<RandomizerEntry<TValue>> Entries => entries; 
		
		public int TotalWeight => GetWeight(Entries);

		[Header("Debugging")]

		[SerializeField]
		private bool showLogs;
		public bool ShowLogs
		{
			get => showLogs;
			set => showLogs = value;
		}

		public void Add(RandomizerEntry<TValue> value)
		{
			Entries.Add(value);
		}

		public void Remove(RandomizerEntry<TValue> value)
		{
			Entries.Remove(value);
		}

		public int GetWeight(List<RandomizerEntry<TValue>> entries)
		{
			int weight = 0;
			foreach (RandomizerEntry<TValue> entry in entries)
			{
				weight += entry.Weight;
			}

			return weight;
		}
		
		// ReSharper disable Unity.PerformanceAnalysis
		public TValue Randomize(List<RandomizerEntry<TValue>> entries, int totalWeight, Random random)
		{
			if (entries.Count == 0 || totalWeight == 0)
			{
				Debug.LogWarning("There is nothing to randomize.");
				return default;
			}
			
			int roll = random.Next(0, totalWeight);
			int weight = 0;
			foreach (RandomizerEntry<TValue> entry in entries)
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

			return Randomize(Entries, TotalWeight, Random);
		}

		public TValue Randomize(Random random)
		{
			return Randomize(Entries, TotalWeight, random);
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
				List<RandomizerEntry<TValue>> adjusted = new();
				foreach (RandomizerEntry<TValue> e in Entries)
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
				TValue selected = Randomize(adjusted, weight, Random);
				randomize.Add(selected);
			}

			return randomize;
		}

		public void Log(string message, LogLevel level)
		{
			switch (level)
			{
				case LogLevel.Error:
					Debug.LogError($"Randomizer | {message}");
					break;
				case LogLevel.Warn:
					Debug.LogWarning($"Randomizer | {message}");
					break;
				case LogLevel.Info:
					Debug.Log($"Randomizer | {message}");
					break;
				case LogLevel.Verbose:
					Debug.Log($"Randomizer | {message}");
					break;
				case LogLevel.Debug:
					Debug.Log($"Randomizer | {message}");
					break;
				case LogLevel.Silly:
					Debug.Log($"Randomizer | {message}");
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(level), level, null);
			}
		}
	}
}