using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Fsi.Gameplay.Visuals
{
	public class CharacterVisuals : MonoBehaviour
	{
		[SerializeField]
		private float movementSpeed = 10;

		[Header("Animation")]
		
		[SerializeField]
		private Animator animator;
		public Animator Animator => animator;

		// Bones
		[Header("Bones")]
		[SerializeField]
		private List<BoneReference> boneReferences = new();

		[Header("Parameters")]
		
		[SerializeField]
		private string moveXParam = "MoveX";

		[SerializeField]
		private string moveZParam = "MoveZ";

		[SerializeField]
		private string groundedParam = "Grounded";

		[SerializeField]
		private string attackParam = "Attack";

		[SerializeField]
		private string hitParam = "Hit";

		[SerializeField]
		private string deadParam = "Dead";

		[SerializeField]
		private string victoryParam = "Victory";

		[Header("Renderer")]
		
		[SerializeField]
		protected List<Renderer> renderers;

		[Header("References")]
		
		[SerializeField]
		private NavMeshAgent agent;
		
		public Dictionary<string, Transform> Bones { get; private set; }
		public List<Renderer> Renderer => renderers;

		private void Awake()
		{
			Bones = new Dictionary<string, Transform>();
			foreach (BoneReference boneReference in boneReferences) Bones.Add(boneReference.tag, boneReference.bone);
		}

		private void Update()
		{
			if (GetComponent<Rigidbody>())
			{
				SetMovement(GetComponent<Rigidbody>().linearVelocity, false);
			} 
			else if (agent)
			{
				SetMovement(agent.velocity, false);
			}
		}

		public void SetMovement(Vector3 velocity, bool normalize = false)
		{
			Vector3 inverse = transform.InverseTransformDirection(velocity.normalized) *
			                  (velocity.magnitude / movementSpeed);

			if (normalize) inverse.Normalize();

			animator.SetFloat(moveXParam, inverse.x);
			animator.SetFloat(moveZParam, inverse.z);
		}

		public void SetGrounded(bool set)
		{
			animator.SetBool(groundedParam, set);
		}

		public void SetDead(bool set)
		{
			animator.SetBool(deadParam, set);
		}

		public void SetVictory(bool set)
		{
			animator.SetBool(victoryParam, set);
		}

		public void DoAttack()
		{
			animator.SetTrigger(attackParam);
		}

		public virtual void DoHit()
		{
			animator.SetTrigger(hitParam);
		}

		[Serializable]
		private struct BoneReference
		{
			public string tag;
			public Transform bone;
		}
	}
}