using DG.Tweening;
using Fsi.StateMachines;
using UnityEngine;

namespace Fsi.Gameplay.Sample.Gameplay.Players.StateMachines.States
{
	public class SampleIdleState : MonoState
	{
		[SerializeField]
		private float minTime;

		private bool canExit;

		public override void OnEnter()
		{
			base.OnEnter();
			canExit = false;
		    
			Sequence s = DOTween.Sequence();
			s.AppendInterval(minTime)
			 .AppendCallback(() => canExit = true);

			s.Play();
		}

		public override void OnExit()
		{
			base.OnExit();
			canExit = false;
		}

		public override bool CanTransitionIn() => true;

		public override bool CanTransitionOut() => canExit;
	}
}
