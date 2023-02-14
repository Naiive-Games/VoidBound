using Windows.HelperElements;
using System.Collections;
using System.Linq;
using System;
using UnityEngine;
using System.Collections.Generic;
using Windows;

namespace Managers {
    public class WindowsManager : Manager {
        [Header("Windows")]
        [SerializeField] private Window defaultWindowPrefab;

        [SerializeField] private Window[] windowPrefabs;
        [SerializeField] private Window[] popupPrefabs;

        [Header("For Windows")]
        [SerializeField] private Background darkBackgroundPrefab;
        [SerializeField] private Background transparentBackgroundPrefab;
        [SerializeField] private PopupHolder popupHolderPrefab;
        [SerializeField] private Transform parentTransform;
        
        private Window currentMainWindow;
        private readonly Dictionary<WindowName, Window> windowPrefabsMap = new(10);
        private readonly Dictionary<WindowName, Window> popupsPrefabsMap = new(10);

        private void Awake() {
            foreach (var prefab in windowPrefabs) {
                windowPrefabsMap.Add(prefab.Name, prefab);
            }

            foreach (var prefab in popupPrefabs) {
                popupsPrefabsMap.Add(prefab.Name, prefab);
            }
        }

        public override void Init() {
            currentMainWindow = Open<Window>(defaultWindowPrefab.Name);
        }

        public T Open<T>(WindowName windowName, bool mayClosePopups = false) where T: Window {
            if (currentMainWindow != null) {
                if (windowName == currentMainWindow.Name) {
                    return (T)currentMainWindow;
                }
                
                CloseMainWindow(mayClosePopups);
            }
            
            var prefab = GetWindow<T>(windowName);
            currentMainWindow = InstantiateWindow(prefab.gameObject, parentTransform);
            return (T)currentMainWindow;
        }
        
        public T OpenPopup<T>(T windowPrefabComponent, Background backgroundPrefabComponent = null, 
            float? lifeTimeInSeconds = null) where T: Window {
            var popup = FindOpenPopup(windowPrefabComponent);
            if (popup != null) return (T)popup;
            
            var popupHolder = Instantiate(popupHolderPrefab, parentTransform);

            var backgroundPrefab = backgroundPrefabComponent ?? transparentBackgroundPrefab;
            popupHolder.BeforePopupOpening(backgroundPrefab.gameObject);
            
            popup = InstantiateWindow(windowPrefabComponent.gameObject, popupHolder.PopupContainerTransform);
            popup.WindowClosing += popupHolder.OnPopupClosing;

            if (lifeTimeInSeconds != null) {
                if (lifeTimeInSeconds <= 0) {
                    throw new Exception("Продолжительность жизни всплывающего окна должна быть больше нуля!");
                }

                StartCoroutine(DestroyPopupAfterTime(popup, (float)lifeTimeInSeconds));
            }
            
            return (T)popup;
        }
        
        public void CloseMainWindow(bool mayClosePopups = false) {
            if (mayClosePopups)
                CloseAllPopups();
            
            DestroyWindow(currentMainWindow);
            currentMainWindow = null;
        }

        public void ClosePopup(Window window) {
            if (FindOpenPopup(window) != null) {
                DestroyWindow(window);
            }
        }
        
        public IEnumerable<Window> GetAllPopups() {
            var popupHolder = GameObject.FindGameObjectsWithTag("PopupHolder");
            return popupHolder.Select(holder => holder.GetComponentInChildren<Window>());
        }

        public Window FindOpenPopup(Window popup) {
            return GetAllPopups().FirstOrDefault(item => item.Name == popup.Name);
        }
        
        private void CloseAllPopups() {
            foreach (var window in GetAllPopups()) {
                DestroyWindow(window);
            }
        }

        private T GetWindow<T>(WindowName windowName) where T: Window {
            if (windowPrefabsMap.TryGetValue(windowName, out var window)) {
                return (T)window;
            }

            return null;
        }

        private Window InstantiateWindow(GameObject windowObject, Transform parent) {
            var window = Instantiate(windowObject, parent).GetComponent<Window>();
            window.OnOpen();
            return window;
        }
        
        private void DestroyWindow(Window window) {
            window.OnClose();
            Destroy(window.gameObject);
        }
        
        private IEnumerator DestroyPopupAfterTime(Window window, float time) {
            yield return new WaitForSeconds(time);
            if (window != null) DestroyWindow(window);
        }
    }
}
