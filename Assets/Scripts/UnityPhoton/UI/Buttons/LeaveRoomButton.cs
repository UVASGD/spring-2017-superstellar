using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityPhoton {

    [RequireComponent(typeof(Button))]
    public class LeaveRoomButton : MonoBehaviour {
        private void Start() {
            GetComponent<Button>().onClick.AddListener(() => {
                if (!PhotonNetwork.LeaveRoom()) {
                    throw new Exception("Failed to leave room");
                }
            });
        }
    }
}