using UnityEngine;

namespace _Scripts
{
	public class AttackState : FSMState
	{
		private float attackRange;
		float curRotSpeed = 100.0f;
		Transform target;
		float attackCooldown;
		float currentCooldown;
		GameObject bulletPrefab;
		Transform bulletSpawnPoint;

		public AttackState(float attackRange,float attackCooldown, Transform playerTransform, GameObject bullet,Transform bulletSpawnPoint)
		{
			stateID = FSMStateID.Attacking;
			this.attackRange = attackRange;
			this.attackCooldown = attackCooldown;
			this.target = playerTransform;
			currentCooldown = 0;
			bulletPrefab = bullet;
			this.bulletSpawnPoint = bulletSpawnPoint;
		}
		
		public override void CheckTransitionRules(Transform player, Transform npc)
		{
			if (Vector3.Distance(npc.position, player.position) >= attackCooldown) {
				NPCTankController npcTankController = npc.GetComponent<NPCTankController>();
				Debug.Log("Switch to Patrol State");
				if (npcTankController != null) {
					npc.GetComponent<NPCTankController>().SetTransition(Transition.LostPlayer);
				} else {
					Debug.LogError("NPCTankController not found in NPC");
				}
			}
		}

		public override void RunState(Transform player, Transform npc)
		{
			Quaternion targetRotation = Quaternion.FromToRotation(Vector3.forward, player.position - npc.position);
			npc.rotation = Quaternion.Slerp(npc.rotation, targetRotation, Time.deltaTime * curRotSpeed);
			
			currentCooldown -= Time.deltaTime;
			if (currentCooldown <= 0f)
			{
				currentCooldown = attackCooldown;
				GameObject.Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
			}
		}
	}
}