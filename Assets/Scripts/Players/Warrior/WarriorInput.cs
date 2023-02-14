using System;
using Game;
using General;
using UnityEngine;

namespace Players.Warrior {
	[Serializable]
	public class WarriorInput : IInputHandler {
		private Warrior warrior;

		public void Init() {
			warrior = GameManager.Instance.GetLocalPlayer<Warrior>();
		}

		public void FixedUpdate() {
			warrior.Move();

			if (Input.GetMouseButtonDown(0)) {
				warrior.Shoot();
			}
		}

		public void Update() { }
	}
}