using UnityEngine;
using UnityUtils.EventUtils;

namespace UnityPhoton {
    public class ConnectionStateObserver : MonoBehaviour {
        private string lastConnectionState;
        private Event<string> newConnectionStateEvent;

        public Event<string> NewConnectionStateEvent {
            get {
                return newConnectionStateEvent ?? LoadNewConnectionStateEvent();
            }
        }

        private Event<string> LoadNewConnectionStateEvent() {
            return newConnectionStateEvent = new Event<string>();
        }

        private void Start() {
            lastConnectionState = "";
        }

        private void Update() {
            CheckNewConnectionState();
        }

        private void CheckNewConnectionState() {
            string currentConnectionState = PhotonNetwork.connectionStateDetailed.ToString();

            if (currentConnectionState != lastConnectionState) {
                HandleNewConnectionState(currentConnectionState);
            }
        }

        private void HandleNewConnectionState(string newConnectionState) {
            NewConnectionStateEvent.Trigger(newConnectionState);
            lastConnectionState = newConnectionState;
        }
    }
}