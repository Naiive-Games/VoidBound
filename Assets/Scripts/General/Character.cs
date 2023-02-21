using Data;
using Mirror;
using UnityEngine;

namespace General {
	public abstract class Character : NetworkBehaviour {
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

		[SyncVar]
		public CharacterType Type;

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

			Type = config.Type;

			OnServerInitialize();
		}

		protected virtual void OnServerInitialize() { }
	}
}