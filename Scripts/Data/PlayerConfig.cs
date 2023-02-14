using General;
using UnityEngine;

namespace Data {
	[CreateAssetMenu]
	public class PlayerConfig : CharacterConfig {
		[SerializeReference, SubclassSelector]
		public IInputHandler Input;
	}
}