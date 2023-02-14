using Mirror;
using UnityEngine;

namespace Network {
	public struct CreateCharacterMessage : NetworkMessage {
		public CharacterType CharacterType;
		public Vector3 Position;
		public bool IsPlayer;
	}
}