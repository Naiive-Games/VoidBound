using Mirror;
using UnityEngine;

namespace Weapons {
	public class Weapon : NetworkBehaviour {
		[SerializeField] private BulletConfig bulletConfig;
		[SerializeField] private Transform shootingPoint;
		
		public void Shoot() {
			var bullet = BulletPool.Instance.Get(shootingPoint);
			bullet.Initialize(bulletConfig);
		}
	}
}