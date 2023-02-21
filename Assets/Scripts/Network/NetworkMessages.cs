using Mirror;
using UnityEngine;

namespace Network {
	public struct CreateCharacterMessage : NetworkMessage {
		public CharacterType CharacterType;
		public Vector3 Position;
		public bool IsPlayer;
	}

	public struct LaserShootMessage : NetworkMessage {
		public NetworkIdentity LaserSender;
		public Vector3 Origin;
		public Vector3 EndPoint;
	}
}