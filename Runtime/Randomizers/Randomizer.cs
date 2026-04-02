using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using Random = System.Random;

namespace Fsi.Gameplay.Randomizers
{
	[Serializable]
	public class Randomizer<TValue>
	{
		private Random _random;
		private Random Random => _random ??= new Random();

		[SerializeField]
		private List<RandomizerEntry<TValue>> entries = new();
		public List<RandomizerEntry<TValue>> Entries => entries; 
		
		public int TotalWeight => Entries.Sum(e => e.Weight);

		[Header("Debugging")]

		[SerializeField]
		private bool showLogs;
		public bool ShowLogs
		{
			get => showLogs;
			set => showLogs = value;
		}
		
		public TValue Randomize(Random random = null, HashSet<TValue> exclude = default)
		{
			if (Entries.Count == 0 || TotalWeight == 0)
			{
				return default;
			}

			if (random != null)
			{
				_random = random;
			}
			
			List<RandomizerEntry<TValue>> testEntries;
			int testWeight;
			
			if (exclude == default)
			{
				testEntries = Entries;
				testWeight = TotalWeight;
			}
			else
			{
				testEntries = Entries
				              .Where(e => !exclude.Contains(e.Value))
				              .ToList();
				testWeight = testEntries.Sum(e => e.Weight);

			}
			
			if (testEntries.Count == 0 || testWeight == 0)
			{
				Log("No entries to randomize.", LogLevel.Warn);
				return default;
			}
			
			int roll = Random.Next(0, testWeight);
			
			int test = 0;
			foreach (RandomizerEntry<TValue> entry in entries)
			{
				test += entry.Weight;
				if (roll < test)
				{
					return entry.Value;
				}
			}

			Debug.LogError($"Randomizer {typeof(TValue).Name} is out of range. Roll: {roll} - Total: {TotalWeight}.");
			return default;
		}

		public List<TValue> Randomize(int amount, bool repeats, Random random = null)
		{
			List<TValue> values = new();
			HashSet<TValue> exclude = new();
			
			for (int i = 0; i < amount; i++)
			{
				TValue selected = Randomize(random, exclude);
				values.Add(selected);

				if (!repeats)
				{
					exclude.Add(selected);
				}
			}

			return values;
		}

		public void Log(string message, LogLevel level)
		{
			if (!ShowLogs)
			{
				return;
			}
			
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