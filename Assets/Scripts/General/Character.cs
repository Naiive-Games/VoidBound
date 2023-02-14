using Data;
using Mirror;
using UnityEngine;
using Weapons;

namespace General {
	public abstract class Character : NetworkBehaviour, ICanBeDamaged {
		[SyncVar, SerializeField] private float health;
		public float Health {
			get => health;
			set => health = Mathf.Clamp(value, 0f, maxHealth);
		}
		[SyncVar]
		private float maxHealth;
		
		[SyncVar, SerializeField] private float protection;
		public float Protection {
			get => protection;
			set => protection = Mathf.Clamp(value, 0f, maxProtection);
		}
		[SyncVar]
		private float maxProtection;

		private CharacterConfig config;
		
		protected Weapon currentWeapon;
		protected Animator animator;
		protected new Rigidbody rigidbody;

		private void Awake() {
			animator = GetComponent<Animator>();
			rigidbody = GetComponent<Rigidbody>();
		}
		
		[Server]
		public void Initialize(CharacterConfig config) {
			maxHealth = config.Health;
			maxProtection = config.Protection;
			
			health = maxHealth;
			protection = maxProtection;
			
			RpcInitialize(config.Type);
		}
		
		[TargetRpc]
		private void RpcInitialize(CharacterType type) {
			config = GeneralManager.Instance.Resources.GetCharacterConfig(type);

			OnInitialize();
		}

		protected virtual void OnInitialize() { }

		protected T GetConfig<T>() where T : CharacterConfig {
			return (T)config;
		}

		public void Shoot() {
			if (currentWeapon == null) return;
			
			currentWeapon.Shoot();
		}

		[TargetRpc]
		public void RpcSetWeapon(Weapon weapon) {
			currentWeapon = weapon;
		}
	}
}