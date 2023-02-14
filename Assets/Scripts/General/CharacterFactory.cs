using Data;
using Game;
using Mirror;
using Network;
using UnityEngine;
using Weapons;
using Object = UnityEngine.Object;

namespace General {
	public static class CharacterFactory {
		[Server]
		public static void SpawnPlayer(NetworkConnectionToClient connection, CreateCharacterMessage message) {
			var character = StartCreateCharacter(message, out var config);
			GameManager.Instance.AddPlayer(character);
			NetworkServer.AddPlayerForConnection(connection, character.gameObject);
			FinalizeCreateCharacter(character, config);
		}

		[Server]
		public static void SpawnEnemy(CreateCharacterMessage message) {
			var character = StartCreateCharacter(message, out var config);
			NetworkServer.Spawn(character.gameObject);
			FinalizeCreateCharacter(character, config);
		}

		private static Character StartCreateCharacter(CreateCharacterMessage message, out CharacterConfig config) {
			config = GeneralManager.Instance.Resources.GetCharacterConfig(message.CharacterType);
			return Object.Instantiate(config.Prefab, message.Position, Quaternion.identity);
		}

		private static void FinalizeCreateCharacter(Character character, CharacterConfig config) {
			if (config.DefaultWeapon != null) {
				var weapon = SpawnWeapon(character, config);
				character.RpcSetWeapon(weapon);
			}

			character.Initialize(config);
		}

		public static Weapon SpawnWeapon(Character character, CharacterConfig config) {
			var weapon = Object.Instantiate(config.DefaultWeapon, character.transform);
			NetworkServer.Spawn(weapon.gameObject, character.gameObject);
			var transform = weapon.transform;
			transform.localPosition = config.WeaponPosition;
			transform.localRotation = config.WeaponRotation;

			return weapon;
		}

		[Server]
		public static Character RespawnPlayer(Character player) {
			NetworkServer.ReplacePlayerForConnection(player.connectionToClient, player.gameObject);
			return null;
		}

		[Server]
		public static void DestroyCharacter(Character character) {
			NetworkServer.Destroy(character.gameObject);
		}
	}
}