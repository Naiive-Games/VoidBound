using UnityEngine;

namespace Data {
	[CreateAssetMenu]
	public class EnemyConfig : CharacterConfig {
		public float TriggerDistance;
		public float AttackDistance;
		public float VisionDistance;
	}
}