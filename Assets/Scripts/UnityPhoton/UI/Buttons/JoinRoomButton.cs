using UnityEngine;
using UnityEngine.UI;

namespace UnityPhoton {

    [RequireComponent(typeof(Button))]
    public class JoinRoomButton : MonoBehaviour {
        private Button button;
        
        private Button Button {
            get {
                return button ?? LoadButton();
            }
        }

        private Button LoadButton() {
            return button = GetComponent<Button>();
        }

        public void Init(string roomName, LobbyController lobbyController) {
            Button.onClick.RemoveAllListeners();

            Button.onClick.AddListener(() => {
                PhotonNetwork.JoinRoom(roomName);
                lobbyController.FlagAsJoiningRoom();
            });
        }
    }
}