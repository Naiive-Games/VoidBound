using Players;
using UnityEngine;

namespace General {
	public class GameCamera : MonoBehaviour {
		[SerializeField] private Vector3 offset = new(0f, 10f, -8f);
		[SerializeField] private float speed = 0.5f;
		[SerializeField] private Player target;
		private new Camera camera;

		public Transform TargetPlayer => target != null ? target.transform : null;

		private void Awake() {
			camera = Camera.main;
		}

		public void SetTarget(Player targetPlayer) {
			target = targetPlayer;
		}

		private void FixedUpdate() {
			if (target == null) return;

			Move();

			if (target.isLocalPlayer == false) return;

			RotatePlayer();
		}

		private void Move() {
			var newPosition = target.transform.position + offset;
			transform.position = Vector3.Lerp(transform.position, newPosition, speed);
		}

		private void RotatePlayer() {
			var mouse = camera.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(mouse, out var hit, 50f) == false) return;
			
			TargetPlayer.LookAt(hit.point);
			var playerRotation = TargetPlayer.rotation;
			playerRotation.x = 0f;
			playerRotation.z = 0f;
			TargetPlayer.rotation = playerRotation;
		}
	}
}