using Game;
using General;
using Mirror;
using UnityEngine;

namespace Weapons {
	public class Rifle : Weapon {
		[SerializeField] private BulletConfig config;
		[SerializeField] private Transform shootingPoint;
		[SerializeField] private float shootDelay = 1f;
		
		private float lastShotTime;

		[Server]
		public override void Use() {
			if (Time.time > lastShotTime + shootDelay) {
				Shoot();
			}
		}

		private void Shoot() {
			var fireEffect = Instantiate(config.FireEffect, shootingPoint);
			fireEffect.Play();
			Destroy(fireEffect.gameObject, 1f);

			if (Physics.Raycast(shootingPoint.position, shootingPoint.forward, out var hit, config.MaxDistanceExistence, config.EnemyLayer)) {
				var hitEffect = Instantiate(config.HitEffect, hit.point, Quaternion.identity);
				hitEffect.Play();
				var target = hit.transform.GetComponent<Character>();
				if (target != null) {
					GameManager.ApplyDamage(target, config.Damage);
					return;
				}
				Destroy(hitEffect.gameObject, 0.5f);
			}
			lastShotTime = Time.time;
		}
	}
	
	[System.Serializable]
	public struct BulletConfig {
		public float Damage;
		public float MaxDistanceExistence;
		public LayerMask EnemyLayer;
		public ParticleSystem HitEffect;
		public ParticleSystem FireEffect;
	}
}