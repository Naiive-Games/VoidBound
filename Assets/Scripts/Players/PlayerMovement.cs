using UnityEngine;
using General;

namespace Players {
	public class PlayerMovement {
		private Rigidbody rigidbody;
		private Vector3 vectorMove;
		
		public PlayerMovement(Rigidbody rigidbody) {
		     this.rigidbody = rigidbody;
		}
		
		public void Move(in float speed) {
		     if (rigidbody == null) return;
             			
             var inputDirection = InputHelper.GetInputDirection().normalized;
			
             vectorMove = new Vector3(inputDirection.x * speed, rigidbody.velocity.y, inputDirection.y * speed);
             rigidbody.velocity = vectorMove;
		}
		
		public Vector3 GetLocalDirection() {
            return rigidbody.transform.InverseTransformDirection(vectorMove);
        }
	}
}