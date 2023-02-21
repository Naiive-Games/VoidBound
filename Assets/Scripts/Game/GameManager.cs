using General;
using Generator;
using Managers;
using Mirror;
using Network;
using Players;
using UnityEngine;
using Random = System.Random;

namespace Game {
	public class GameManager : MonoBehaviour {
		
		public static GameCamera Camera { get; private set; }

		private static Player[] players;

		public static Player[] Players {
			get {
				if (players == null || players.Length == 0) {
					players = FindObjectsOfType<Player>();
					return players;
				}

				foreach (var player in players) {
					if (player == null) {
						players = FindObjectsOfType<Player>();
						break;
					}
				}

				return players;
			}
		}

		private void Awake() {
			Camera = UnityEngine.Camera.main!.GetComponent<GameCamera>();
		}

		[Server]
		public static void StartGenerateWorld() {
			if (NetworkServer.connections.Count < 1) return;

			NetworkServer.SendToAll(new GameWorld.GeneratorMessage { Seed = new Random().Next() });
		}

		[Server]
		public static void ApplyDamage(Character target, float damage) {
			var damageForHealthAndProtection = CalculateDamage(target.Protection, damage);
			
			target.Health -= damageForHealthAndProtection.Item1;
			target.Protection -= damageForHealthAndProtection.Item2;
			Debug.Log($"По объекту {target.name} был нанесен урон {damage}\n" +
			          $"Health - {target.Health}\n" +
			          $"Protection - {target.Protection}\n");

			if (target.Health > 0) return;
			
			if (ResourceManager.IsPlayerType(target.Type)) {
				RespawnPlayer(target);
				return;
			}

			CharacterFactory.DestroyCharacter(target);
		}

		private static void RespawnPlayer(Character character) {
			var message = new CreateCharacterMessage {
				CharacterType = character.Type,
				Position = new Vector3(0f, 3f, 0f),
				IsPlayer = true,
			};
			
			CharacterFactory.RespawnPlayer(character, message);
		}

		private static (float, float) CalculateDamage(in float protection, float damage) {
			float resultDamage = damage;
			var damageForProtection = 0f;
			if (protection > 0) {
				var dividor = 2f;
				if (protection > 50) {
					dividor = UnityEngine.Random.Range(2f, 3f);
				}

				resultDamage = damage / dividor;
				damageForProtection = resultDamage * (dividor - 1);
			}

			var damageForHealth = resultDamage;
			return (damageForHealth, damageForProtection);
		}
	}
}