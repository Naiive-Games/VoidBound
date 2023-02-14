using System.Collections.Generic;
using General;
using Generator;
using Mirror;
using UnityEngine;

namespace Game {
	public class GameManager : MonoBehaviour {
		[SerializeField] private GameWorld world;
		
		public GameCamera Camera { get; private set; }
		public static GameManager Instance { get; private set; }
		
		private readonly List<Character> playersMap = new(2);

		private void Awake() {
			Instance = this;
			Camera = UnityEngine.Camera.main!.GetComponent<GameCamera>();
		}

		[Server]
		public void StartGenerateWorld() {
			if (NetworkServer.connections.Count < 1) return;
			
			world.SendSeed();
		}

		public void AddPlayer(Character character) {
			playersMap.Add(character);
		}

		public T GetPlayer<T>() where T : Character {
			foreach (var player in playersMap) {
				if (player is T character) {
					return character;
				}
			}

			return null;
		}
	}
}