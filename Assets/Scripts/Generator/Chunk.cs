using System.Collections;
using Mirror;
using Network;
using UnityEngine;
using Random = System.Random;

namespace Generator {
	public class Chunk : MonoBehaviour {
		[SerializeField] private int minEnemyCount = 3;
		[SerializeField] private int maxEnemyCount = 15;
		public ChunkType Type;
		public CharacterType[] EnemiesType;
		
		public const int LENGTH = 100;
		public const int WIDTH = 100;

		private Random random;

		public void Init(Random random) {
			if (!NetworkClient.activeHost) return;
			
			this.random = random;
			if (Type is not ChunkType.Friendly and not ChunkType.Spawner) {
				StartCoroutine(SpawnEnemiesRoutine());
			}
		}

		private IEnumerator SpawnEnemiesRoutine() {
			var randomCount = random.Next(minEnemyCount, maxEnemyCount + 1);
			for (int i = 0; i < randomCount; i++) {
				SpawnEnemy();
				yield return null;
			}
		}
		
		private void SpawnEnemy() {
			var message = new CreateCharacterMessage {
				CharacterType = GetRandomEnemy(),
				Position = GetEnemyPosition(),
			};
			
			NetworkClient.Send(message);
		}

		private CharacterType GetRandomEnemy() {
			var index = random.Next(0, EnemiesType.Length);
			return EnemiesType[index];
		}

		private Vector3 GetEnemyPosition() {
			var rX = GetRandomValue(transform.position.x, LENGTH);
			var rZ = GetRandomValue(transform.position.z, WIDTH);
			return new Vector3(rX, 2f, rZ);
		}

		private int GetRandomValue(in float axis, in int offset) {
			return random.Next((int)axis, (int)axis + offset);
		}
	}
}