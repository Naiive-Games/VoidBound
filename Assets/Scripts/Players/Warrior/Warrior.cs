using General;
using UnityEngine;

namespace Players.Warrior {
	public class Warrior : Player {
		public void Move() {
			if (rigidBody == null) return;
			
			var inputDirection = InputHelper.GetInputDirection().normalized;
			var vectorMove = new Vector3(inputDirection.x * config.Speed, rigidBody.velocity.y, inputDirection.y * config.Speed);
			rigidBody.velocity = transform.rotation * vectorMove;
			
			UpdateMoveAnimations(inputDirection);
		}
	}
}