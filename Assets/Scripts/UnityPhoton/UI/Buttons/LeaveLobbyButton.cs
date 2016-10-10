using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityPhoton {

    [RequireComponent(typeof(Button))]
    public class LeaveLobbyButton : MonoBehaviour {
        private void Start() {
            GetComponent<Button>().onClick.AddListener(() => {
                if (!PhotonNetwork.LeaveLobby()) {
                    throw new Exception("Failed to leave lobby");
                }
            });
        }
    }
}