using UnityEngine;
using UnityEngine.UI;

namespace UnityPhoton {
    public class RoomDisplay : MonoBehaviour, IListDisplayElement<RoomInfo> {

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private JoinRoomButton joinButton;

        void IListDisplayElement<RoomInfo>.Init(RoomInfo roomInfo) {
            nameText.text = roomInfo.name;
        }

        public void Init(LobbyController lobbyController) {
            joinButton.Init(nameText.text, lobbyController);
        }
    }
}