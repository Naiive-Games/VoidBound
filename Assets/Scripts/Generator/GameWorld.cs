using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extenssions;
using Game;
using Mirror;
using UnityEngine;
using Random = System.Random;

namespace Generator {
	public class GameWorld : MonoBehaviour {
		public struct GeneratorMessage : NetworkMessage {
			public int Seed;
		}
		
		private int seed = -1;

		[Min(0)]
		[SerializeField] private int viewRadius = 4;

		[SerializeField] private Chunk spawnerChunk;

		private Vector2Int currentChunkPosition;
		private int maxDistanceExistence;
		private readonly ChunkType[] chunkTypes = { ChunkType.Friendly, ChunkType.Danger };
		private readonly Dictionary<Vector2Int, ChunkData> chunksDataMap = new(1000);

		private void Awake() {
			maxDistanceExistence = (Chunk.LENGTH + Chunk.WIDTH) * viewRadius / 2;
			
			var data = new ChunkData(ChunkType.Spawner);
			data.SetObject(spawnerChunk.gameObject);
			chunksDataMap.Add(WorldToMatrixPosition(spawnerChunk.transform.position), data);
			
			NetworkClient.RegisterHandler<GeneratorMessage>(OnGenerateMessage);
		}

		private void OnGenerateMessage(GeneratorMessage message) {
			if (seed != -1) return;
			
			Debug.Log($"Generator was started with seed {message.Seed}");
			seed = message.Seed;
			StartCoroutine(Generate());
			StartCoroutine(CheckingCurrentChunkRoutine());
			StartCoroutine(FoundingDestroyableChunksRoutine());
		}

		#region Coroutines
		private IEnumerator Generate() {
			for (var z = currentChunkPosition.y - viewRadius; z < currentChunkPosition.y + viewRadius; z++) {
				for (var x = currentChunkPosition.x - viewRadius; x < currentChunkPosition.x + viewRadius; x++) {
					var chunkPosition = new Vector2Int(x, z);
					
					if (HasChunkInMemory(chunkPosition, out var chunkData)) {
						if (IsExistChunk(chunkData)) continue;
						
						SpawnChunk(chunkPosition, ref chunkData);
					} else {
						LoadChunk(chunkPosition);
					}

					yield return new WaitForSecondsRealtime(0.07f);
				}
			}
		}
		
		private IEnumerator CheckingCurrentChunkRoutine() {
			while (true) {
				if (GameManager.Camera.TargetPlayer == null) {
					yield return new WaitForSecondsRealtime(0.05f);
					continue;
				}
				
				var playerChunk = WorldToMatrixPosition(GameManager.Camera.TargetPlayer.position);
				if (currentChunkPosition != playerChunk) {
					currentChunkPosition = playerChunk;
					StartCoroutine(Generate());
				}

				yield return new WaitForSecondsRealtime(0.05f);
			}
		}

		private IEnumerator FoundingDestroyableChunksRoutine() {
			var existingChunksCount = viewRadius * 2;
			while (true) {
				if (chunksDataMap.Values.Count < existingChunksCount) {
					yield return new WaitForSecondsRealtime(0.05f);
					continue;
				}

				var existingChunks = chunksDataMap.Values.Where(IsExistChunk, existingChunksCount);
				if (existingChunks.Count == 0) yield break;
			
				var destroyableChunks = existingChunks.Where(IsOutOfRange).ToArray(existingChunks.Count);
			
				if (destroyableChunks.Length >= 10) {
					yield return StartCoroutine(DestroyChunksRoutine(destroyableChunks));
				}

				yield return new WaitForSecondsRealtime(0.5f);
			}
		}

		private static IEnumerator DestroyChunksRoutine(IEnumerable<ChunkData> destroyableChunks) {
			foreach (var destroyableChunk in destroyableChunks) {
				if (destroyableChunk.Object == null) continue;

				Destroy(destroyableChunk.Object);
				yield return new WaitForFixedUpdate();
			}
		}
		#endregion

		private void LoadChunk(in Vector2Int chunkPosition) {
			var random = new Random(GenerateSeed(chunkPosition));
			
			var randomTypeIndex = random.Next(0, chunkTypes.Length);
			var randomRotationY = 90f * random.Next(0, 4);
			
			var chunkRotation = Quaternion.Euler(0f, randomRotationY, 0f);

			var data = new ChunkData(chunkTypes[randomTypeIndex], chunkRotation);
			chunksDataMap.Add(chunkPosition, data);

			var chunk = SpawnChunk(chunkPosition, ref data);
			chunk.Init(random);
		}

		private Chunk SpawnChunk(in Vector2Int chunkPosition, ref ChunkData data) {
			var prefab = GeneralManager.Instance.Resources.GetChunkPrefab(data.Type);
			return SpawnChunk(prefab, chunkPosition, ref data);
		}

		private Chunk SpawnChunk(Chunk prefab, in Vector2Int chunkPosition, ref ChunkData data) {
			var worldPosition = new Vector3(chunkPosition.x * Chunk.LENGTH, 0, chunkPosition.y * Chunk.WIDTH);
			var chunk = Instantiate(prefab, worldPosition, data.Rotation, transform);
			data.SetObject(chunk.gameObject);

			return chunk;
		}

		private static bool IsExistChunk(ChunkData chunkData) => chunkData.IsDestroyed == false;

		private bool IsOutOfRange(ChunkData chunkData) {
			if (GameManager.Camera.TargetPlayer == null || chunkData.Object == null) return false;
			
			return chunkData.Object.transform.DistanceTo(GameManager.Camera.TargetPlayer) >= maxDistanceExistence;
		}

		private static Vector2Int WorldToMatrixPosition(in Vector3 pointWorldPosition) {
			var worldPositionInt = Vector3Int.FloorToInt(pointWorldPosition);
			return new Vector2Int(worldPositionInt.x / Chunk.LENGTH, worldPositionInt.z / Chunk.WIDTH);
		}

		private bool HasChunkInMemory(in Vector2Int positionInMatrix, out ChunkData data) {
			return chunksDataMap.TryGetValue(positionInMatrix, out data);
		}

		private int GenerateSeed(in Vector2Int chunkPositionInMatrix) {
			var x = chunkPositionInMatrix.x;
			var y = chunkPositionInMatrix.y;
			var n = (x + y) * (x + y + 1) / 2 + y;
			return n + seed;
		}
	}
}