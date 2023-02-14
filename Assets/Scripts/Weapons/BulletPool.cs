using UnityEngine;
using System.Collections.Generic;
using Mirror;

namespace Weapons {
	public class BulletPool : MonoBehaviour {
		public static BulletPool Instance { get; private set; }
		
		[SerializeField] private Bullet prefab;
		[SerializeField] private int capacity = 50;
		
		private List<Bullet> pool;

		private void Start() {
			Instance = this;
			pool = new List<Bullet>(capacity);
			for (var i = 0; i < capacity; i++) {
				CreateObject();
			}
		}

		public Bullet Get(Transform shootingPoint) {
			var bullet = HasFreeBullet(out var freeBullet) ? freeBullet : CreateObject();

			var bulletTransform = bullet.transform;
			bulletTransform.position = shootingPoint.position;
			bulletTransform.rotation = shootingPoint.rotation;
			bullet.gameObject.SetActive(true);
			NetworkServer.Spawn(bullet.gameObject);
			return bullet;
		}

		public void Return(Bullet bullet) {
			bullet.gameObject.SetActive(false);
			NetworkServer.UnSpawn(bullet.gameObject);
		}
		
		private Bullet CreateObject() {
			var bullet = Instantiate(prefab, transform);
			bullet.gameObject.SetActive(false);
			pool.Add(bullet);
			return bullet;
		}
		
		private bool HasFreeBullet(out Bullet freeBullet) {
			foreach (var bullet in pool) {
				if (bullet.gameObject.activeInHierarchy) continue;
				
				freeBullet = bullet;
				return true;
			}
			
			freeBullet = null;
			return false;
		}
	}
}