using System;
using Data;
using Game;
using General;
using UnityEngine;

namespace Players.Warrior {
	[Serializable]
	public class WarriorInput : IInputHandler {
		private Warrior warrior;
		private CharacterConfig config;

		public void Init() {
			warrior = GameManager.Instance.GetPlayer<Warrior>();
			config = warrior.GetConfig();
		}

		public void FixedUpdate() {
			ReadMove();

			if (Input.GetMouseButtonDown(0)) {
				warrior.Shoot();
			}
		}

		public void Update() { }

		private void ReadMove() {
			warrior.Move(config.Speed);
		}
	}
}