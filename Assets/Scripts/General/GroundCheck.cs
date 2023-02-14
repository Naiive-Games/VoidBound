using UnityEngine;

namespace General {
	public sealed class GroundCheck : MonoBehaviour {
		[Min(0f)]
		[SerializeField] private float _radius;
		public float Radius => _radius;

		public Vector3 Position => transform.position;
	}
}