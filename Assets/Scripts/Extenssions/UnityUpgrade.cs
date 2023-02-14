using General;
using UnityEngine;

namespace Extenssions {
	public static class UnityUpgrade {
		public static float DistanceTo(this Vector3 vector, Vector3 target) {
			return Vector3.Distance(vector, target);
		}
		public static float DistanceTo(this Transform transform, Transform target) {
			if (transform == null || target == null) return 0f;
			
			return DistanceTo(transform.position, target.position);
		}
		public static float DistanceTo(this Transform transform, Vector3 target) {
			if (transform == null) return 0f;
			
			return DistanceTo(transform.position, target);
		}
	}
}