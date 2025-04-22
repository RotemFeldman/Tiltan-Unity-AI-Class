using UnityEngine;

namespace _Scripts
{
	public class ChaseState : FSMState
	{
		private float curSpeed = 100.0f;
		private float curRotSpeed = 100.0f;
		private Transform playerTransform;
		private float detectionRadius;
		private float attackRange;

		public ChaseState(float attackRange, float detectionRadius, Transform playerTransform)
		{
			stateID = FSMStateID.Chasing;
			this.attackRange = attackRange;
			this.detectionRadius = detectionRadius;
			this.playerTransform = playerTransform;
		}
		public override void CheckTransitionRules(Transform player, Transform npc)
		{
			if (Vector3.Distance(npc.position, player.position) >= detectionRadius)
			{
				NPCTankController npcTankController = npc.GetComponent<NPCTankController>();
				Debug.Log("Switch to Chase Patrol State");
				if (npcTankController != null) {
					npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);
				} else {
					Debug.LogError("NPCTankController not found in NPC");
				}
			}
			else if (Vector3.Distance(npc.position, player.position) <= attackRange )
			{
				NPCTankController npcTankController = npc.GetComponent<NPCTankController>();
				Debug.Log("Switch to Chase Attack State");
				if (npcTankController != null) {
					npc.GetComponent<NPCTankController>().SetTransition(Transition.ReachPlayer);
				} else {
					Debug.LogError("NPCTankController not found in NPC");
				}
			}
		}

		public override void RunState(Transform player, Transform npc)
		{
			Quaternion targetRotation = Quaternion.FromToRotation(Vector3.forward, playerTransform.position - npc.position);
			npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);

			//Go Forward
			npc.Translate(Vector3.forward * Time.deltaTime * curSpeed);
		}
	}
}