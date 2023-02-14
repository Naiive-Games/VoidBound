using System;
using General;
using Mirror;
using UnityEngine;
using Weapons;
using Object = UnityEngine.Object;

namespace Managers {
	public static class CharacterFactory {
		public static Character CreateCharacter(CharacterType type, Vector3 position, bool isPlayer = false) {
			if (NetworkClient.activeHost == false) throw new Exception("Only server can be create characters!");
			
			var config = GeneralManager.Instance.Resources.GetCharacterConfig(type);
			var character = Object.Instantiate(config.Prefab, position, Quaternion.identity);

			if (config.DefaultWeapon != null) {
				var weapon = SpawnWeapon(character, config.DefaultWeapon, character.WeaponPosition);
				character.SetWeapon(weapon);
			}

			if (isPlayer == false) {
				NetworkServer.Spawn(character.gameObject); 
				character.Initialize(config);
			}

			return character;
		}

		public static Weapon SpawnWeapon(Character character, Weapon weaponPrefab, Vector3 position) {
			if (NetworkClient.activeHost == false) throw new Exception("Only server can be spawn weapons!");
			
			var weapon = Object.Instantiate(weaponPrefab, character.transform);
			NetworkServer.Spawn(weapon.gameObject, character.gameObject);
			weapon.transform.localPosition = position;

			return weapon;
		}
	}
}