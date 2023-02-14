using Data;

namespace Enemies {
	public interface IEnemyState {
		void Init(EnemyConfig config);
		void FixedUpdate();
		void Enter();
		void Exit();
	}
}