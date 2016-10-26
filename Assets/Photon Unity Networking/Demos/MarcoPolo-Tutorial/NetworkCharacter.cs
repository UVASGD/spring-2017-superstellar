using UnityEngine;

public class NetworkCharacter : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
	private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
	private Vector3 correctPlayerScale = Vector3.zero; // We lerp towards this
    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
		{
			Debug.Log (this.correctPlayerPos);
			Debug.Log (this.correctPlayerRot);
			Debug.Log (this.correctPlayerScale);
//			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
//            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
			transform.position = this.correctPlayerPos;
			transform.rotation = this.correctPlayerRot;
			transform.localScale = this.correctPlayerScale;
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
		Debug.Log ("Serializing");
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);	
            stream.SendNext(transform.rotation);
			stream.SendNext (transform.localScale);
//            myThirdPersonController myC = GetComponent<myThirdPersonController>();
//            stream.SendNext((int)myC._characterState);
        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			this.correctPlayerScale = (Vector3)stream.ReceiveNext();
//            myThirdPersonController myC = GetComponent<myThirdPersonController>();
//            myC._characterState = (CharacterState)stream.ReceiveNext();
        }
    }
}
