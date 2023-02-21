using Data;
using Mirror;
using Network;
using UnityEngine;
using Weapons;
using Object = UnityEngine.Object;

namespace General {
	public class CharacterFactory {
		[Server]
		public static void SpawnPlayer(NetworkConnectionToClient connection, CreateCharacterMessage message) {
			var character = StartCreateCharacter(message, out var config);
			NetworkServer.AddPlayerForConnection(connection, character.gameObject);
			character.Initialize(config);
		}

		[Server]
		public static void SpawnEnemy(CreateCharacterMessage message) {
			var character = StartCreateCharacter(message, out var config);
			NetworkServer.Spawn(character.gameObject);
			character.Initialize(config);
		}

		private static Character StartCreateCharacter(CreateCharacterMessage message, out CharacterConfig config) {
			config = GeneralManager.Instance.Resources.GetCharacterConfig(message.CharacterType);
			return Object.Instantiate(config.Prefab, message.Position, Quaternion.identity);
		}
		
		[Server]
		public static Weapon SpawnWeapon(Character character, Weapon weaponPrefab, Vector3 position) {
			var weapon = Object.Instantiate(weaponPrefab, character.transform);
			weapon.transform.localPosition = position;
			NetworkServer.Spawn(weapon.gameObject, character.gameObject);
			return weapon;
		}

		[Server]
		public static void RespawnPlayer(Character player, CreateCharacterMessage message) {
			NetworkServer.RemovePlayerForConnection(player.connectionToClient, true);
			SpawnPlayer(player.connectionToClient, message);
		}

		[Server]
		public static void DestroyCharacter(Character character) {
			NetworkServer.Destroy(character.gameObject);
		}
	}
}