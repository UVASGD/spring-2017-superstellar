using UnityEngine;
using UnityUtils.ConfigUtils;

namespace UnityPhoton {
    public class NetworkConfig : Config<NetworkConfig> {

        [SerializeField]
        private string gameVersion;

        // Static members

        public static string GameVersion {
            get {
                return Instance.gameVersion;
            }
        }
    }
}