using Players;
using UnityEngine;

namespace General {
	public class GameCamera : MonoBehaviour {
		[SerializeField] private Vector3 offset = new(0f, 10f, -8f);
		[SerializeField] private float speed = 0.5f;
		[SerializeField] private Player target;

		private Transform weapon;
		private new Camera camera;

		public Transform TargetPlayer => target != null ? target.transform : null;

		private void Awake() {
			camera = Camera.main;
		}

		public void SetTarget(Player targetPlayer) {
			target = targetPlayer;
		}

		public void SetWeapon(Transform targetWeapon) {
			weapon = targetWeapon;
		}

		private void FixedUpdate() {
			if (target == null) return;
			
			Follow();
			
			if (target.isOwned == false) return;

			var mouse = camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(mouse, out var hit, 50f)) {
				RotatePlayer(hit.point);
			}
		}

		private void Follow() {
			var newPosition = TargetPlayer.position + offset;
			transform.position = Vector3.Lerp(transform.position, newPosition, speed);
		}

		private void RotatePlayer(in Vector3 point) {
			TargetPlayer.LookAt(point);
			NormalizeRotation(TargetPlayer);
			RotateWeapon(point);
		}
		
		private void RotateWeapon(in Vector3 point) {
			if (weapon == null) return;
			
			weapon.LookAt(point);
			NormalizeRotation(weapon);
		}

		private void NormalizeRotation(Transform transform) {
			var transformRotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
			transform.rotation = transformRotation;
		}
	}
}