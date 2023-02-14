using General;
using Mirror;
using Network;
using UnityEngine;

namespace Managers {
	public class NetManager : NetworkManager {
		public override void OnClientDisconnect() {
			GeneralManager.Instance.Input.SetHandler(null);
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

		private static void OnCreateCharacter(NetworkConnectionToClient connection, CreateCharacterMessage message) {
			if (message.IsPlayer) {
				CharacterFactory.SpawnPlayer(connection, message);
				return;
			}
			
			CharacterFactory.SpawnEnemy(message);
		}
	}
}