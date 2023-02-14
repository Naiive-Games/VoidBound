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

		public void Move(float speed) {
			var inputDirection = InputHelper.GetInputDirection();
			var vectorMove = new Vector3(inputDirection.x * speed, rigidbody.velocity.y, inputDirection.y * speed);
			rigidbody.velocity = transform.rotation * vectorMove;
			
			animator.SetFloat("VelocityX", inputDirection.x);
			animator.SetFloat("VelocityZ", inputDirection.y);
		}
	}
}