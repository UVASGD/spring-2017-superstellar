using UnityEngine;
using System.Collections;

public class OnJoinedInstantiate : MonoBehaviour
{
	public Transform SpawnPosition;
    private float PositionOffset = 30.0f;
    public GameObject[] PrefabsToInstantiate;   // set in inspector

    public void OnJoinedRoom()
    {
//		Debug.Log (PhotonNetwork.playerList.Length);

        if (this.PrefabsToInstantiate != null)
        {
            foreach (GameObject o in this.PrefabsToInstantiate)
            {
                Debug.Log("Instantiating: " + o.name);

				Vector3 spawnPos = Vector3.zero;
                if (this.SpawnPosition != null)
                {
                    spawnPos = this.SpawnPosition.position;
                }

                Vector3 random = Random.insideUnitSphere;
                random.y = 0;
                random = random.normalized;
                Vector3 itempos = spawnPos + this.PositionOffset * random;
				itempos.z = -10;
                PhotonNetwork.Instantiate(o.name, itempos, Quaternion.identity, 0);
            }
        }
    }
}
