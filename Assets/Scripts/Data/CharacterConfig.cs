using General;
using UnityEngine;

namespace Data {
	public abstract class CharacterConfig : ScriptableObject {
		public Character Prefab;
		public float Health;
		public float Protection;
		public float Speed;
		public CharacterType Type;
	}
}