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
						// todo типа ходит туда сюда. либо пока просто стоит на месте. оставь так просто
						yield return new WaitForSecondsRealtime(0.5f);
						continue;
					}
					
					closestPlayer = newClosestPlayer.transform;
				}
				
				if (DistanceToPlayer(closestPlayer) <= config.AttackDistance) {
					// todo attack (отдельный метод)
				}
			}
		}

		private float DistanceToPlayer(Transform player) {
			if (player == null) return float.MaxValue;
			
			return transform.DistanceTo(player);
		}

		private void Attack() {
			// у нас в базовом классе будет ссылка на оружие, пишешь currentWeapon и вызываешь Shoot когда надо
			// и типа когда игрок отходит нужно чтоб мы еще подошли поближе. в config есть AttackDistance.
			// это дистанция с которой мы будем стрелять по нему.
			// также есть TriggerDistance. если мы подходим на это расстояние то просто тригерримся на игрока
			// в той корутине выше добавь проверку на эту дистанцию (есть готовый метод выше)
			// и если условие выполняется то делаем MoveToPlayer()
		}

		private void MoveToPlayer() {
			// здесь мы должны повернуть себя на игрока и идти на него, в базовом классе есть rigidbody
		}
	}
}