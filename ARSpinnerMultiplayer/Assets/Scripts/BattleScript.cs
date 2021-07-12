using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class BattleScript : MonoBehaviour
{

    public Spinner spinnerScript;

    private float startSpinPeed;
    private float currentSpinSpeed;
    public Image spinSpeedBarImage;
    public TextMeshProUGUI spinSpeedRatioText;


    private void Awake()
    {
        startSpinPeed = spinnerScript.SpinSpeed;
        currentSpinSpeed = spinnerScript.SpinSpeed;

        spinSpeedBarImage.fillAmount = currentSpinSpeed / startSpinPeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //Comparing the speeds of the SPinnerTop
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            Debug.Log("MY SPEED: " + mySpeed + " ---Other Player Speed " + otherPlayerSpeed);


            if(mySpeed > otherPlayerSpeed)
            {
                Debug.Log("<color=green> You Damage the other player </color>");
                
                
                //Apply damage to the slower player
                if(collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, 400.0f);
                }

                

            }
            else
            {
                Debug.Log("<color=red> You get DAMAGE </color>");
            }

        }
    }

    [PunRPC]
    public void DoDamage(float damageAmount)
    {
        spinnerScript.SpinSpeed -= damageAmount;
        currentSpinSpeed = spinnerScript.SpinSpeed;

        spinSpeedBarImage.fillAmount = currentSpinSpeed / startSpinPeed;
        spinSpeedRatioText.text = currentSpinSpeed + "/" + startSpinPeed;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
