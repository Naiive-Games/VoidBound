using Data;
using Game;
using General;
using Mirror;
using UnityEngine;
using Weapons;

namespace Players {
	public abstract class Player : Character {
		public static Player LocalPlayer { get; private set; }
		public static Player LocalTeammate { get; private set; }
		
		protected PlayerConfig config;
		[SerializeField] protected Weapon weapon;
		
		public override void OnStartLocalPlayer() {
			/*if (isLocalPlayer == false) {
				LocalTeammate = this;
				print($"Your teammate - {LocalTeammate.name}");
				return;
			}*/

			LocalPlayer = this;
		}
		
		[Server]
		protected override void OnServerInitialize() {
			config = LoadConfig();
			
			RpcSetWeapon();
			RpcInitialize();
		}

		[TargetRpc]
		private void RpcInitialize() {
			config ??= LoadConfig();
			
			GameManager.Camera.SetTarget(this);
			GeneralManager.Instance.Input.SetHandler(config.Input);
		}

		[Command]
		public void CmdUseWeapon() {
			if (weapon == null) return;
			
			weapon.Use();
		}
		
		[TargetRpc]
		private void RpcSetWeapon() {
			GameManager.Camera.SetWeapon(weapon.transform);
		}

		protected void UpdateMoveAnimations(in Vector2 inputDirection) {
			animator.SetFloat("VelocityX", inputDirection.x);
			animator.SetFloat("VelocityZ", inputDirection.y);
		}

		private PlayerConfig LoadConfig() {
			return GeneralManager.Instance.Resources.GetCharacterConfig(Type) as PlayerConfig;
		}
	}
}