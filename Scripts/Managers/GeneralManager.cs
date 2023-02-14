using System.Collections.Generic;
using Managers;
using UnityEngine;

public class GeneralManager : MonoBehaviour {
	public NetManager Network;
	public InputManager Input;
	public ResourceManager Resources;
	public WindowsManager Windows;

	public static GeneralManager Instance;

	private void Awake() {
	    Instance = this;
	    DontDestroyOnLoad(transform.parent);
	    InitManagers(new Manager[] { Input, Resources, });
	}

	private static void InitManagers(IEnumerable<Manager> managers) {
		foreach (var manager in managers) {
			manager.Init();
		}
	}
}
