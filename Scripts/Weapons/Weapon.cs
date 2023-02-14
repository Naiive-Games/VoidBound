using Mirror;
using Players;
using UnityEngine;

namespace Weapons {
	public class Weapon : NetworkBehaviour {
		[SerializeField] private BulletConfig bulletConfig;
		[SerializeField] private Bullet bulletPrefab;
		[SerializeField] private Transform shootPoint;
		[SerializeField] private new string name;
		
		[Command]
		public void CmdShoot() {
			var bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
			bullet.Initialize(bulletConfig);
			NetworkServer.Spawn(bullet.gameObject, Player.LocalPlayer.gameObject);
		}
	}
}