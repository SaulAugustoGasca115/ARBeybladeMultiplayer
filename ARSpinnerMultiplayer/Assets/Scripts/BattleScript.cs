using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class BattleScript : MonoBehaviourPun
{

    public Spinner spinnerScript;

    private float startSpinPeed;
    private float currentSpinSpeed;
    public Image spinSpeedBarImage;
    public TextMeshProUGUI spinSpeedRatioText;

    public float commonDamageCoefficient = 0.04f;

    public bool isAttacker;
    public bool isDefender;
    private bool isDead = false;

    [Header("Player Type Damage Coefficients")]
    public float doDamageCoefficientAttacker = 10.0f; //do more damagwe - avantage
    public float getDamagedCoefficientAttacker = 1.2f;// gets more damage - disavantage
    public float doDamageCoefficientDefender = 0.75f; // do less damage - disavantage
    public float getDamagedCoefficientDefender = 0.2f; // gets less damage - avantage

    Rigidbody rb;
    public GameObject ui3DGameObject;
    public GameObject deathPanelUIPrefab;
    private GameObject deathPanelUIGameobject;

    private void Awake()
    {
        startSpinPeed = spinnerScript.SpinSpeed;
        currentSpinSpeed = spinnerScript.SpinSpeed;

        spinSpeedBarImage.fillAmount = currentSpinSpeed / startSpinPeed;
    }


    void CheckPlayerType()
    {
        if(gameObject.name.Contains("Attacker"))
        {
            isAttacker = true;
            isDefender = false;

        }else if(gameObject.name.Contains("Defender"))
        {
            isDefender = true;
            isAttacker = false;

            spinnerScript.SpinSpeed = 4400.0f;
            startSpinPeed = spinnerScript.SpinSpeed;
            currentSpinSpeed = spinnerScript.SpinSpeed;

            spinSpeedRatioText.text = currentSpinSpeed + "/" + startSpinPeed;

        }
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


                float defaultDamageAmount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600.0f * commonDamageCoefficient;

                if (isAttacker)
                {

                    defaultDamageAmount *= doDamageCoefficientAttacker;

                }
                else if (isDefender)
                {
                    defaultDamageAmount *= doDamageCoefficientDefender;
                }

                //Apply damage to the slower player
                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, defaultDamageAmount);
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

        if(!isDead)
        {
            if (isAttacker)
            {
                damageAmount *= getDamagedCoefficientAttacker;

                if(damageAmount > 1000)
                {
                    damageAmount = 400.0f;
                }


            }
            else if (isDefender)
            {
                damageAmount *= getDamagedCoefficientDefender;
            }


            spinnerScript.SpinSpeed -= damageAmount;
            currentSpinSpeed = spinnerScript.SpinSpeed;

            spinSpeedBarImage.fillAmount = currentSpinSpeed / startSpinPeed;
            spinSpeedRatioText.text = currentSpinSpeed.ToString("F0") + "/" + startSpinPeed;

            if (currentSpinSpeed < 100)
            {
                //Die
                Die();

            }
        }
  

    }

    void Die()
    {

        isDead = true;

        GetComponent<MovementController>().enabled = false;
        rb.freezeRotation = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        spinnerScript.SpinSpeed = 0f;

        ui3DGameObject.SetActive(false);

        if(photonView.IsMine)
        {
            //countdown for respawn
            StartCoroutine(Respawn());
        }

    }

    IEnumerator Respawn()
    {
        GameObject canvas = GameObject.Find("Canvas");

        if(deathPanelUIGameobject == null)
        {
            deathPanelUIGameobject = Instantiate(deathPanelUIPrefab, canvas.transform);

        }
        else
        {
            deathPanelUIGameobject.SetActive(true);
        }
        
        Text respawnTimeText = deathPanelUIGameobject.transform.Find("RespawnTimeText").GetComponent<Text>();

        float respawnTime = 8.0f;

        respawnTimeText.text = respawnTime.ToString(".00");

        while(respawnTime > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime -= 1.0f;
            respawnTimeText.text = respawnTime.ToString(".00");

            GetComponent<MovementController>().enabled = false;

        }

        deathPanelUIGameobject.SetActive(false);

        GetComponent<MovementController>().enabled = true;

        photonView.RPC("ReBorn",RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void ReBorn()
    {
        spinnerScript.SpinSpeed = startSpinPeed;
        currentSpinSpeed = spinnerScript.SpinSpeed;

        spinSpeedBarImage.fillAmount = currentSpinSpeed / startSpinPeed;
        spinSpeedRatioText.text = currentSpinSpeed + "/" + startSpinPeed;

        rb.freezeRotation = true;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        ui3DGameObject.SetActive(true);

        isDead = false;

    }


    // Start is called before the first frame update
    void Start()
    {
        CheckPlayerType();

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
