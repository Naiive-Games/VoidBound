using System.Collections;
using Data;
using Extenssions;
using General;
using Players;
using UnityEngine;

namespace Enemies {
	public class Enemy : Character {
		private Transform closestPlayer;
		private EnemyConfig config;

		protected override void OnInitialize() {
			config = GetConfig<EnemyConfig>();
			StartCoroutine(CheckingDistanceToPlayer());
		}

		private IEnumerator CheckingDistanceToPlayer() {
			var players = new[] { Player.LocalPlayer.transform, Player.LocalTeammate.transform };
			while (true) {
				if (closestPlayer == null) {
					var newClosestPlayer = players[Random.Range(0, players.Length)];
					
					if (DistanceToPlayer(newClosestPlayer.transform) >= config.VisionDistance) {
						
						yield return new WaitForSecondsRealtime(0.5f);
						continue;
					}
					
					closestPlayer = newClosestPlayer.transform;
				}
				
				if (DistanceToPlayer(closestPlayer) <= config.AttackDistance) {
					
				}
			}
		}

		private float DistanceToPlayer(Transform player) {
			if (player == null) return float.MaxValue;
			
			return transform.DistanceTo(player);
		}

		private void Attack() {
	
		}

		private void MoveToPlayer() {
			
		}
	}
}