using UnityEngine;

namespace Generator {
	public struct ChunkData {
		public readonly ChunkType Type;
		public readonly Quaternion Rotation;
		public GameObject Object { get; private set; }

		public bool IsDestroyed => Object == null;

		public ChunkData(ChunkType type) {
			Type = type;
			Rotation = Quaternion.identity;
			Object = null;
		}
		
		public ChunkData(ChunkType type, in Quaternion rotation) {
			Type = type;
			Rotation = rotation;
			Object = null;
		}

		public void SetObject(GameObject chunkObject) {
			if (IsDestroyed == false || Object == chunkObject) {
				Debug.LogWarning($"Object {Object.name} already was set!");
				return;
			}
			
			Object = chunkObject;
		}
	}
}