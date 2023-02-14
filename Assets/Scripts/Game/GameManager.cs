using System.Collections.Generic;
using General;
using Generator;
using Mirror;
using Network;
using UnityEngine;
using Random = System.Random;

namespace Game {
	public class GameManager : MonoBehaviour {
		public GameCamera Camera { get; private set; }
		private readonly List<Character> players = new(2);
		
		public static GameManager Instance { get; private set; }

		private void Awake() {
			Instance = this;
			Camera = UnityEngine.Camera.main!.GetComponent<GameCamera>();
			NetworkServer.RegisterHandler<DamageMessage>(OnApplyDamage);
		}

		[Server]
		public void StartGenerateWorld() {
			if (NetworkServer.connections.Count < 1) return;
			
			NetworkServer.SendToAll(new GameWorld.GeneratorMessage { Seed = new Random().Next() });
		}

		public void AddPlayer(Character character) {
			players.Add(character);
		}

		public T GetLocalPlayer<T>() where T : Character {
			foreach (var player in players) {
				if (player is not T character) continue;
				
				if (player.isLocalPlayer) {
					return character;
				}
				
				Debug.LogError($"Player {player} is not local player!");
			}

			return null;
		}

		public void ApplyDamage(NetworkIdentity targetConnection, ICanBeDamaged target, float damage) {
			float resultDamage = damage;
			float health = target.Health;
			float protection = target.Protection;
			
			if (protection > 0) {
				var dividor = 2f;
				if (protection > 50) {
					dividor = UnityEngine.Random.Range(2f, 3f);
				}

				resultDamage = damage / dividor;
				protection -= resultDamage * (dividor - 1);
			}

			health -= resultDamage;
			var message = new DamageMessage {
				Target = targetConnection,
				NewHealth = health,
				NewProtection = protection,
			};
			
			NetworkClient.Send(message);
		}
		
		[Server]
		private void OnApplyDamage(NetworkConnectionToClient connection, DamageMessage message) {
			var character = message.Target.GetComponent<ICanBeDamaged>();
			character.Health = message.NewHealth;
			character.Protection = message.NewProtection;
		}

		private void SpawnEnemy() {
			var message = new CreateCharacterMessage {
				CharacterType = CharacterType.VoidsMonster,
				Position = transform.position,
				IsPlayer = false,
			};
			
			NetworkClient.Send(message);
		}
	}

	public struct DamageMessage : NetworkMessage {
		public NetworkIdentity Target;
		public float NewHealth;
		public float NewProtection;
	}
}