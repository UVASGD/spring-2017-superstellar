using UnityEngine;
using UnityEngine.UI;

namespace UnityPhoton {
    public class PlayerDisplay : MonoBehaviour, IListDisplayElement<PhotonPlayer> {

        [SerializeField]
        private Text playerNameText;

        void IListDisplayElement<PhotonPlayer>.Init(PhotonPlayer player) {
            playerNameText.text = "name: " + player.name;
        }
    }
}