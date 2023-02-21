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
		
		[SerializeField] protected Weapon weapon;
		protected PlayerConfig config;
		protected PlayerMovement movement;

		protected virtual void Start() {
			if (isLocalPlayer == false) {
				print($"Your teammate - {LocalTeammate.name}");
				LocalTeammate = this;
			}
			
			LocalPlayer = this;
		}

		[Server]
		protected override void OnServerInitialize() {
			config = LoadConfig();
			
			RpcInitialize();
			RpcSetWeapon();
		}

		[TargetRpc]
		private void RpcInitialize() {
			config ??= LoadConfig();
			
			movement = new PlayerMovement(rigidbody);
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

		private PlayerConfig LoadConfig() {
			return GeneralManager.Instance.Resources.GetCharacterConfig(Type) as PlayerConfig;
		}
		
		public virtual void Move() {
		    movement.Move(config.Speed);
		    UpdateMoveAnimations(movement.GetLocalDirection());
		}
		
		protected virtual void UpdateMoveAnimations(in Vector3 localMoveDirection) {
            animator.SetFloat("VelocityX", localMoveDirection.x, 0.1f, Time.fixedDeltaTime);
            animator.SetFloat("VelocityZ", localMoveDirection.z, 0.1f, Time.fixedDeltaTime);
        }
	}
}