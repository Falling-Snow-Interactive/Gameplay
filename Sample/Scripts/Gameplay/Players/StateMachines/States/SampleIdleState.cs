using Fsi.StateMachines;

namespace Fsi.Gameplay.Sample.Gameplay.Players.StateMachines.States
{
	public class SampleIdleState : MonoState
	{
		public override bool CanTransitionIn() => true;
		public override bool CanTransitionOut() => true;
	}
}
