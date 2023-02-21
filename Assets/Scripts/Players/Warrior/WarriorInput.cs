using System;
using General;
using UnityEngine;

namespace Players.Warrior {
	[Serializable]
	public class WarriorInput : IInputHandler {
		private Warrior warrior;

		public void Init() {
			warrior = Player.LocalPlayer as Warrior;
			
		}

		public void FixedUpdate() {
			warrior.Move();

			if (Input.GetButtonDown("Fire1")) {
				warrior.CmdUseWeapon();
			}
		}

		public void Update() { }
	}
}