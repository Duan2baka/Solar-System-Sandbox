using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class celestialController : MonoBehaviour
{
    // Start is called before the first frame update
    public float v;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void showInfo(){

    }
}
