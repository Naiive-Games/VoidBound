using General;
using Mirror;
using Network;
using UnityEngine;

namespace Managers {
	public class NetManager : NetworkManager {
		public override void Awake() {
			NetworkClient.RegisterHandler<LaserShootMessage>(OnLaserShoot);
		}

		public override void OnStartServer() {
			NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);
		}

		public override void OnClientConnect() {
			base.OnClientConnect();
			
			var message = new CreateCharacterMessage {
				CharacterType = CharacterType.Warrior,
				Position = new Vector3(0f, 3f, 0f),
				IsPlayer = true,
			};
			
			NetworkClient.Send(message);
		}

		public override void OnClientDisconnect() {
			GeneralManager.Instance.Input.SetHandler(null);
		}

		[Server]
		private static void OnCreateCharacter(NetworkConnectionToClient connection, CreateCharacterMessage message) {
			if (message.IsPlayer) {
				CharacterFactory.SpawnPlayer(connection, message);
				return;
			}
			
			CharacterFactory.SpawnEnemy(message);
		}
		
		[Client]
		private static void OnLaserShoot(LaserShootMessage message) {
			if (message.LaserSender == null) return;
			var laser = message.LaserSender.GetComponentInChildren<LineRenderer>();
			if (laser == null) return;
			
			laser.SetPosition(0, message.Origin);
			laser.SetPosition(1, message.EndPoint);
		}
	}
}