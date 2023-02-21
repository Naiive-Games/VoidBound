using System.Collections;
using Data;
using Extenssions;
using Game;
using General;
using Players;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies {
	public abstract class Enemy : Character {
		protected Transform closestPlayer;
		protected EnemyConfig config;

		protected override void OnServerInitialize() {
			config = GeneralManager.Instance.Resources.GetCharacterConfig(Type) as EnemyConfig;
			
			StartCoroutine(CheckingDistanceToPlayer());
		}

		private IEnumerator CheckingDistanceToPlayer() {
			while (true) {
				if (Player.LocalPlayer == null) {
					yield return new WaitForFixedUpdate();
					continue;
				}
				
				if (closestPlayer == null) {
					var players = GameManager.Players;
					var newClosestPlayer = players[Random.Range(0, players.Length)];

					if (DistanceToPlayer(newClosestPlayer.transform) >= config.VisionDistance) {
						OnIdleState();
						yield return new WaitForFixedUpdate();
						continue;
					}
					
					closestPlayer = newClosestPlayer.transform;
				}

				if (DistanceToPlayer(closestPlayer) > config.TriggerDistance) {
					yield return new WaitForFixedUpdate();
					continue;
				}

				if (DistanceToPlayer(closestPlayer) <= config.AttackDistance) {
					OnAttackState();
				} else {
					OnTriggeredState();
				}
				
				yield return new WaitForFixedUpdate();
			}
		}

		protected float DistanceToPlayer(Transform player) {
			if (player == null) return float.MaxValue;
			
			return transform.DistanceTo(player);
		}

		protected abstract void OnIdleState();
		protected abstract void OnTriggeredState();
		protected abstract void OnAttackState();
	}
}