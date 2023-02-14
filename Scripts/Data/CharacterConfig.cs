using General;
using UnityEngine;
using Weapons;

namespace Data {
	public abstract class CharacterConfig : ScriptableObject {
		public Character Prefab;
		public float Health;
		public float Protection;
		public float Speed;
		public CharacterType Type;
		public Weapon DefaultWeapon;
	}
}