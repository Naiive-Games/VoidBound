using Extenssions;
using Mirror;
using Network;
using UnityEngine;

namespace Generator {
	public class Chunk : MonoBehaviour {
		public ChunkType Type;
		public CharacterType[] EnemiesType;
		
		public const int LENGTH = 100;
		public const int WIDTH = 100;

		public void Init() {
			
		}
		
		private void SpawnEnemy() {
			if (Type == ChunkType.Friendly) return;

			var message = new CreateCharacterMessage {
				CharacterType = EnemiesType.GetRandomItem(),
				Position = transform.position,
				IsPlayer = false,
			};
			
			NetworkClient.Send(message);
		}
	}
}