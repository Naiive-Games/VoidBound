using System.Collections.Generic;
using Data;
using Generator;
using UnityEngine;

namespace Managers {
	public class ResourceManager : Manager {
		[SerializeField] private GameData gameData;
		private readonly Dictionary<ChunkType, Chunk> chunksPrefabMap = new(20);
		private readonly Dictionary<CharacterType, CharacterConfig> charactersConfigMap = new(20);

		private void Awake() {
			foreach (var data in gameData.CharactersConfig) {
				charactersConfigMap.Add(data.Type, data);
			}
            
			foreach (var chunkPrefab in gameData.ChunksPrefab) {
				chunksPrefabMap.Add(chunkPrefab.Type, chunkPrefab);
			}
		}

		public Chunk GetChunkPrefab(ChunkType type) {
			if (chunksPrefabMap.TryGetValue(type, out var prefab)) {
				return prefab;
			}
			
			Debug.LogError($"Chunk with type {type} not exist!");
			return null;
		}

		public CharacterConfig GetCharacterConfig(CharacterType type) {
			return charactersConfigMap[type];
		}
	}
}