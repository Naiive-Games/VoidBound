using System;
using Mirror;
using UnityEngine;

namespace Weapons {
	public class Bullet : NetworkBehaviour {
		private BulletConfig config;
		private new Rigidbody rigidbody;

		private void Awake() {
			rigidbody = GetComponent<Rigidbody>();
		}

		public void Initialize(BulletConfig config) {
			this.config = config;
			Move();
		}

		private void Move() {
			rigidbody.AddForce(Vector3.forward * config.Velocity, ForceMode.Impulse);
		}
	}
	
	[Serializable]
	public struct BulletConfig {
		public float Velocity;
	}
}