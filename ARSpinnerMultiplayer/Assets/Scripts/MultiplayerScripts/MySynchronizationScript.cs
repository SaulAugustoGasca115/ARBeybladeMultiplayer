using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MySynchronizationScript : MonoBehaviour, IPunObservable
{

    Rigidbody rb;
    PhotonView photonView;

    Vector3 networkedPosition;
    Quaternion networkedRotation;

    public bool syncronizeVelocity = true;
    public bool syncronizeAngularVelocity = true;
    public bool isTeleportEnable = true;
    public float telepoortIfDistanceIsGreaterThan = 1.0f;

    float distance;
    float angle;

    // Start is called before the first frame update
    void Awake()
    {
        

        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();

        networkedPosition = new Vector3();
        networkedRotation = new Quaternion();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(!photonView.IsMine)
        {
            rb.position = Vector3.MoveTowards(rb.position, networkedPosition, distance*(1.0f / PhotonNetwork.SerializationRate));

            //rb.position = Vector3.MoveTowards(rb.position, networkedPosition, Time.fixedDeltaTime));
            //rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, Time.fixedDeltaTime * 100.0f);

            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkedRotation, angle * (1.0f / PhotonNetwork.SerializationRate));
        }

        
    }

    public void OnPhotonSerializeView(PhotonStream stream,PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            //Then, the photonview is mine and I am the one who controls this player.
            //should send position, velocity,etc... data to the other players
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);

            if(syncronizeVelocity)
            {
                stream.SendNext(rb.velocity);
            }

            if(syncronizeAngularVelocity)
            {
                stream.SendNext(rb.angularVelocity);
            }


        }
        else if(stream.IsReading)
        {
            //Called on my player gameObject that exists in remote player's game
            networkedPosition = (Vector3)stream.ReceiveNext();
            networkedRotation = (Quaternion)stream.ReceiveNext();


            if(isTeleportEnable)
            {
                if(Vector3.Distance(rb.position,networkedPosition) > telepoortIfDistanceIsGreaterThan)
                {
                    rb.position = networkedPosition;
                }
            }


            if(syncronizeAngularVelocity || syncronizeVelocity)
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                if(syncronizeVelocity)
                {
                    rb.velocity = (Vector3)stream.ReceiveNext();

                    networkedPosition += rb.velocity * lag;

                    distance = Vector3.Distance(rb.position,networkedPosition);
                }

                if(syncronizeAngularVelocity)
                {
                    rb.angularVelocity = (Vector3)stream.ReceiveNext();

                    networkedRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkedRotation;

                    angle = Quaternion.Angle(rb.rotation,networkedRotation);
                }

               
            }

        }
    }
}
