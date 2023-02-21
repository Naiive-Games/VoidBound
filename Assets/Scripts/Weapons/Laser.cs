using System.Collections;
using Game;
using General;
using Mirror;
using Network;
using UnityEngine;

namespace Weapons {
	[RequireComponent(typeof(LineRenderer))]
	public class Laser : Weapon {
		[SerializeField] private float damage = 5f;
		[SerializeField] private float maxLength = 15f;
		[SerializeField] private float lifeTime = 1f;
		[SerializeField] private float shootDelay = 1f;
		[SerializeField] private LayerMask enemyMask;

		private bool isActive;
		private float lastShotTime;
		private float timeToDamage;
		private Vector3 firstPoint;
		private Vector3 secondPoint;
		private LineRenderer laser;
		private NetworkIdentity owner;

		private void Awake() {
			laser = GetComponent<LineRenderer>();
			owner = GetComponentInParent<NetworkIdentity>();
		}

		[Server]
		public override void Use() {
			if (isActive) return;
			
			if (Time.time > lastShotTime + shootDelay + lifeTime) {
				StartCoroutine(FireRoutine());
			}
		}

		private IEnumerator FireRoutine() {
			isActive = true;
			var time = 0f;
			var currentDamage = damage;
			while (time < lifeTime) {
				firstPoint = transform.position;
				RaycastLaser(currentDamage);
				laser.SetPosition(0, firstPoint);
				laser.SetPosition(1, secondPoint);

				var message = new LaserShootMessage {
					LaserSender = owner,
					Origin = firstPoint,
					EndPoint = secondPoint,
				};

				NetworkServer.SendToAll(message);

				time += Time.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
			}
			
			lastShotTime = Time.time;
			isActive = false;
		}

		private void RaycastLaser(float damage) {
			if (Physics.Raycast(transform.position, transform.forward, out var hit, maxLength, enemyMask)) {
				timeToDamage += Time.fixedDeltaTime;
				if (timeToDamage >= 0.1f) {
					HandleHit(hit, damage);
					timeToDamage = 0f;
				}

				secondPoint = hit.point;
				return;
			}

			secondPoint = firstPoint;
		}

		private void HandleHit(RaycastHit hit, float damage) {
			if (hit.transform.TryGetComponent(out Character character) == false) return;

			GameManager.ApplyDamage(character, damage);
			if (character.Health <= 0) {
				firstPoint = transform.position;
				secondPoint = firstPoint;
			}
		}
	}
}