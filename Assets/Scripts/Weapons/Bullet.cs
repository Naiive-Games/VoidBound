using System;
using System.Collections;
using Extenssions;
using Game;
using General;
using Mirror;
using UnityEngine;

namespace Weapons {
	public class Bullet : NetworkBehaviour {
		[SerializeField] private float maxDistanceExistence = 100f; 
		private BulletConfig config;
		private new Rigidbody rigidbody;
		private Coroutine calculateDistanceRoutine;
		private bool isInited;

		private void Awake() {
			rigidbody = GetComponent<Rigidbody>();
		}
		
		private void OnCollisionEnter(Collision collision) {
			if (isInited == false) return;
			
			if (collision.gameObject.layer == config.EnemyMask) {
				var target = collision.gameObject.GetComponent<Character>();
				GameManager.Instance.ApplyDamage(target.netIdentity, target, config.Damage);
				Debug.Log($"Bullet hit in target {target.name} with damage {config.Damage}");
			}
			
			DestroySelf();
		}

		public void Initialize(BulletConfig config) {
			this.config = config;
			gameObject.layer = config.BulletMask;
			isInited = true;
			rigidbody.AddForce(transform.forward * config.Velocity, ForceMode.Impulse);
			calculateDistanceRoutine = StartCoroutine(CalculateDistanceRoutine());
		}
		
		private IEnumerator CalculateDistanceRoutine() {
			var startPosition = transform.position;
			while (startPosition.DistanceTo(transform.position) < maxDistanceExistence) {
				yield return new WaitForFixedUpdate();
			}
			
			DestroySelf();
		}

		private void DestroySelf() {
			if (calculateDistanceRoutine != null) {
				StopCoroutine(calculateDistanceRoutine);
			}

			isInited = false;
			BulletPool.Instance.Return(this);
		}
	}
	
	[Serializable]
	public struct BulletConfig {
		public float Velocity;
		public float Damage;
		public LayerMask EnemyMask;
		public LayerMask BulletMask;
	}
}