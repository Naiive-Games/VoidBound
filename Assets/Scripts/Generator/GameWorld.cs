using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extenssions;
using Game;
using General;
using Mirror;
using UnityEngine;
using Random = System.Random;

namespace Generator {
	public class GameWorld : MonoBehaviour {
		public struct GeneratorMessage : NetworkMessage {
			public int Seed;
		}
		
		private static int seed = -1;

		[Min(0)]
		[SerializeField] private int viewRadius = 4;
		[SerializeField] private Chunk spawnerChunk;
		
		private GameCamera gameCamera;
		private Vector2Int currentChunkPosition;
		private int maxDistanceExistence;
		private readonly ChunkType[] chunkTypes = { ChunkType.Friendly, ChunkType.Danger };
		private readonly Dictionary<Vector2Int, ChunkData> chunksDataMap = new(1000);

		private void Awake() {
			maxDistanceExistence = (Chunk.LENGTH + Chunk.WIDTH) * viewRadius / 2;

			var chunkPosition = new Vector2Int(0, 0);
			var data = new ChunkData(ChunkType.Spawner);
			SpawnChunk(spawnerChunk, chunkPosition, ref data);
			chunksDataMap.Add(chunkPosition, data);
			
			NetworkClient.RegisterHandler<GeneratorMessage>(OnGenerateMessage);
		}

		private void Start() {
			gameCamera = GameManager.Instance.Camera;
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
				if (gameCamera.TargetPlayer == null) {
					yield return new WaitForSecondsRealtime(0.05f);
					continue;
				}
				
				var playerChunk = GetPlayerChunkPosition();
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

			SpawnChunk(chunkPosition, ref data);
		}

		private void SpawnChunk(in Vector2Int chunkPosition, ref ChunkData data) {
			var prefab = GeneralManager.Instance.Resources.GetChunkPrefab(data.Type);
			SpawnChunk(prefab, chunkPosition, ref data);
		}

		private void SpawnChunk(Chunk prefab, in Vector2Int chunkPosition, ref ChunkData data) {
			var worldPosition = new Vector3(chunkPosition.x * Chunk.LENGTH, 0, chunkPosition.y * Chunk.WIDTH);
			var chunk = Instantiate(prefab, worldPosition, data.Rotation, transform);
			data.SetObject(chunk.gameObject);
		}

		private static bool IsExistChunk(ChunkData chunkData) => chunkData.IsDestroyed == false;

		private bool IsOutOfRange(ChunkData chunkData) {
			if (gameCamera.TargetPlayer == null || chunkData.Object == null) return false;
			
			return chunkData.Object.transform.DistanceTo(gameCamera.TargetPlayer) >= maxDistanceExistence;
		}

		private Vector2Int GetPlayerChunkPosition() {
			var playerWorldPosition = Vector3Int.FloorToInt(gameCamera.TargetPlayer.position);
			return GetChunkAtPosition(playerWorldPosition);
		}
		
		private static Vector2Int GetChunkAtPosition(in Vector3Int pointWorldPosition) {
			return new Vector2Int(pointWorldPosition.x / Chunk.LENGTH, pointWorldPosition.z / Chunk.WIDTH);
		}

		private bool HasChunkInMemory(in Vector2Int positionInMatrix, out ChunkData data) {
			return chunksDataMap.TryGetValue(positionInMatrix, out data);
		}

		private static int GenerateSeed(in Vector2Int chunkPositionInMatrix) {
			var x = chunkPositionInMatrix.x;
			var y = chunkPositionInMatrix.y;
			var n = (x + y) * (x + y + 1) / 2 + y;
			return n + seed;
		}
	}
}