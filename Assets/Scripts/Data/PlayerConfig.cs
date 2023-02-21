using General;
using UnityEngine;
using Weapons;

namespace Data {
	[CreateAssetMenu]
	public class PlayerConfig : CharacterConfig {
		[SerializeReference, SubclassSelector]
		public IInputHandler Input;
		public Weapon DefaultWeapon;
		public Vector3 WeaponPosition;
	}
}