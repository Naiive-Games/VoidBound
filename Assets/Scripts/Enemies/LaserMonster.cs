using UnityEngine;
using Weapons;

namespace Enemies {
	public class LaserMonster : Enemy {
		[SerializeField] private Weapon weapon;

		protected override void OnIdleState() { }

		protected override void OnTriggeredState() {
			MoveToPlayer();
		}

		protected override void OnAttackState() {
			OnTriggeredState();
			UseWeapon();
		}
		
		private void UseWeapon() {
			if (weapon == null) return;
			
			weapon.Use();
		}
		
		private void MoveToPlayer() {
			transform.LookAt(closestPlayer);
			var transformRotation = transform.rotation;
			transformRotation.x = 0f;
			transformRotation.z = 0f;
			transform.rotation = transformRotation;

			if (DistanceToPlayer(closestPlayer) > 2f) {
				rigidBody.AddForce(transform.forward * config.Speed, ForceMode.Acceleration);
			}
		}
	}
}