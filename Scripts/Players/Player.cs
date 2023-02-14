using Game;
using Mirror;

namespace Players {
	public class Player : NetworkBehaviour {
		public static Player LocalPlayer { get; private set; }
		public static Player LocalTeammate { get; private set; }

		public override void OnStartLocalPlayer() {
			if (isLocalPlayer == false) {
				LocalTeammate = this;
			}

			LocalPlayer = this;
			GameManager.Instance.Camera.SetTarget(this);
		}
	}
}