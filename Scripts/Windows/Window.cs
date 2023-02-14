using System;
using UnityEngine;

namespace Windows {
	[RequireComponent(typeof(CanvasGroup))] 
	public abstract class Window : MonoBehaviour {
		public WindowName Name;
		public CanvasGroup CanvasGroup { get; private set; }
		public event Action WindowClosing;

		protected virtual void Awake() {
			CanvasGroup = GetComponent<CanvasGroup>();
		}
		
		public virtual void OnOpen() { }
		
		public virtual void OnClose() {
			WindowClosing?.Invoke();
		}
	}
}