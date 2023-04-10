using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    readonly float G = 100f;
    GameObject[] celestials;
    public bool freeze = true;


    // Start is called before the first frame update
    void Start()
    {
        celestials = GameObject.FindGameObjectsWithTag("Celestial");

        InitialVelocity();
    }

    // Update is called once per frame
    void Update(){
    }

    private void FixedUpdate() {
        Gravity();
        if(freeze) GameObject.Find("Sun").GetComponent<Rigidbody>().velocity=Vector3.zero;
    }

    public void GetVelocity(){
        GameObject a = GameObject.Find("Sun");
        foreach(GameObject b in celestials){
            if(!a.Equals(b)){
                float m2 = b.GetComponent<Rigidbody>().mass;
                float r = Vector3.Distance(a.transform.position, b.transform.position);
                a.transform.LookAt(b.transform);
                a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((G * m2) / r);
            }
        }
    }

    public void GetVelocityFor(GameObject a){
        foreach(GameObject b in celestials){
            if(!a.Equals(b)){
                float m2 = b.GetComponent<Rigidbody>().mass;
                float r = Vector3.Distance(a.transform.position, b.transform.position);
                a.transform.LookAt(b.transform);

                a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((G * m2) / r);
            }
        }
    }

    void Gravity(){
        celestials = GameObject.FindGameObjectsWithTag("Celestial");
        foreach(GameObject a in celestials){
            if(a.name == "Sun" && freeze) continue;
            foreach(GameObject b in celestials){
                if(!a.Equals(b)){
                    float m1 = a.GetComponent<Rigidbody>().mass;
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);
                    a.GetComponent<Rigidbody>().AddForce((b.transform.position - a.transform.position).normalized * 
                    (G * (m1 * m2) / (r * r)));
                }
            }
        }
    }

    void InitialVelocity(){
        foreach(GameObject a in celestials){
            foreach(GameObject b in celestials){
                if(!a.Equals(b) || !freeze){
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);
                    a.transform.LookAt(b.transform);

                    a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((G * m2) / r);
                }
            }
        }
    }
}
