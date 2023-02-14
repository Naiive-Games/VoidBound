using Data;

namespace Enemies {
	public class AttackState : IEnemyState {
		private EnemyConfig config;
		
		public const string NAME = "Attack";
		
		public void Init(EnemyConfig config) {
			this.config = config;
		}

		public void FixedUpdate() {
			
		}

		public void Enter() {
			
		}

		public void Exit() {
			
		}
	}
}