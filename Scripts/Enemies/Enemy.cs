using System.Collections;
using System.Collections.Generic;
using Data;
using Extenssions;
using General;
using Mirror;
using Players;
using UnityEngine;
using Weapons;

namespace Enemies {
	public class Enemy : Character {
		private Transform closestPlayer;
		private Dictionary<string, IEnemyState> states;
		private IEnemyState currentState;
		private Weapon currentWeapon;
		private EnemyConfig config;
		
		private void Awake() {
			states = new Dictionary<string, IEnemyState> {
				{ IdleState.NAME, new IdleState() }, { AttackState.NAME, new AttackState() }, 
			};
			
		}

		[Command]
		protected override void OnInitialize() {
			config = GetConfig<EnemyConfig>();
			foreach (var state in states.Values) {
				state.Init(config);
			}
			
			SetState(IdleState.NAME);
			StartCoroutine(CheckingDistanceToPlayer());
		}

		private void FixedUpdate() {
			currentState?.FixedUpdate();
		}

		private IEnumerator CheckingDistanceToPlayer() {
			var players = new[] { Player.LocalPlayer.transform, Player.LocalTeammate.transform };
			while (true) {
				if (closestPlayer == null) {
					var newClosestPlayer = players[Random.Range(0, players.Length)];
					
					if (DistanceToPlayer(newClosestPlayer.transform) >= config.VisionDistance) {
						SetState(IdleState.NAME);
						yield return new WaitForSecondsRealtime(0.5f);
						continue;
					}
					
					closestPlayer = newClosestPlayer.transform;
				}
				
				if (DistanceToPlayer(closestPlayer) <= config.AttackDistance) {
					SetState(AttackState.NAME);
				}
			}
		}

		private float DistanceToPlayer(Transform player) {
			if (player == null) return float.MaxValue;
			
			return transform.DistanceTo(player);
		}

		private void SetState(string nameState) {
			if (states.TryGetValue(nameState, out var state) == false) {
				Debug.LogError($"State with name {nameState} not exist!");
				return;
			}

			if (currentState == state) return;
			
			currentState?.Exit();
			currentState = state;
			currentState.Enter();
		}
	}
}