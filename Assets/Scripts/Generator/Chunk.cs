using UnityEngine;

namespace Generator {
	public class Chunk : MonoBehaviour {
		public ChunkType Type;
		public CharacterType[] EnemiesType;
		
		public const int LENGTH = 100;
		public const int WIDTH = 100;
	}
}