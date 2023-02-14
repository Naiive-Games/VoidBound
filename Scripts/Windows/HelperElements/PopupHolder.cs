using UnityEngine;

namespace Windows.HelperElements {
	public class PopupHolder : MonoBehaviour {
		[SerializeField] private Transform popupContainer;
		
		public Transform PopupContainerTransform => popupContainer;
		
		public void BeforePopupOpening(GameObject backgroundPrefab) {
			Instantiate(backgroundPrefab, transform);
		}
		
		public void OnPopupClosing() {
			Destroy(gameObject);
		}
	}
}