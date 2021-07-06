using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [Header("Spinner Attributes")]
    public float SpinSpeed = 3600.0f;
    public bool bIsSpin = false;
    Rigidbody rb;
    public GameObject playerGraphics;
    private float timeToSpin = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsSpin == false)
        {
            timeToSpin += Time.deltaTime;
            //Debug.Log("TIME TO SPIN: " + timeToSpin);
        }

        if (timeToSpin >= 1.4f)
        {
            bIsSpin = true;
        }
    }

    private void FixedUpdate()
    {
        if(bIsSpin)
        {
            playerGraphics.transform.Rotate(new Vector3(0,SpinSpeed * Time.deltaTime,0));
        }
    }
}
