using Game;
using General;
using UnityEngine;

namespace Managers {
	public class InputManager : Manager {
		private IInputHandler input;
		
		public void SetHandler(IInputHandler handler) {
			input = handler;
			input?.Init();
		}

		private void FixedUpdate() {
			input?.FixedUpdate();
		}

		private void Update() {
			input?.Update();
			
			if (Input.GetKeyDown(KeyCode.F)) {
				GameManager.Instance?.StartGenerateWorld();
			}
		}
	}
}