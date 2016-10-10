using UnityEngine;

namespace UnityPhoton {
    public class CharacterSpawner : MonoBehaviour {

        [SerializeField]
        private Vector2 spawnPosition;

        public void SpawnCharacter() {
            const int GROUP = 0;

            PhotonNetwork.Instantiate(
                "Character",
                spawnPosition,
                Quaternion.identity,
                GROUP);
        }
    }
}