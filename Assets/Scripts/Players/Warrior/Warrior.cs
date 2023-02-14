using Data;
using General;
using UnityEngine;

namespace Players.Warrior {
	public class Warrior : Character {
		private PlayerConfig config;

		protected override void OnInitialize() {
			config = GetConfig<PlayerConfig>();
			GeneralManager.Instance.Input.SetHandler(config.Input);
		}

		public void Move() {
			var inputDirection = InputHelper.GetInputDirection();
			var vectorMove = new Vector3(inputDirection.x * config.Speed, rigidbody.velocity.y, inputDirection.y * config.Speed);
			rigidbody.velocity = transform.rotation * vectorMove;
			
			UpdateAnimations(inputDirection);
		}
		
		private void UpdateAnimations(in Vector2 inputDirection) {
			animator.SetFloat("VelocityX", inputDirection.x);
			animator.SetFloat("VelocityZ", inputDirection.y);
		}
	}
}