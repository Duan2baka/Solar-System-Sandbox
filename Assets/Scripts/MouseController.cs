using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public GameObject prefab;
    private bool chk = false;
    private void Update()
    {
        MouseClick();
    }
    void MouseClick()
    {
        bool flag = GameObject.Find("CircleOnPlane").GetComponent<CircleOnPlane>().addPlanet;
        if(flag){
            if (Input.GetMouseButtonDown(0)){
                chk = true;
            }
            if (Input.GetMouseButtonUp(0) && chk)
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Clicked on UI.");
                }
                else
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log("Clicked on " + hit.collider.gameObject.name + " in scene.");
                    }
                    else{
                        Vector3 normal = new Vector3(0f, 1f, 0f);
                        //Debug.Log(normal);
                        Vector3 mousePosition = Input.mousePosition;
                        float rayDistance;
                        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (new Plane(normal, GameObject.Find("Sun").transform.position).Raycast(ray, out rayDistance)){
                            Vector3 hitPoint = ray.GetPoint(rayDistance);
                            GameObject.Find("CircleOnPlane").GetComponent<CircleOnPlane>().addPlanet = false;
                            chk = false;
                            GameObject spawnedPrefab = Instantiate(prefab, hitPoint, Quaternion.identity);
                            spawnedPrefab.GetComponent<Label>().camera = GameObject.Find("Main Camera").GetComponent<Camera>();
                        }
                    }
                }
            }
            /*
            if (Input.touchCount > 0){
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                Touch touch = Input.GetTouch(0); // 获取第一个触摸点

                
                if (touch.phase == TouchPhase.Ended)
                {
                    Debug.Log("Long Press");
                    
                    Vector3 normal = new Vector3(0f, 1f, 0f);
                    //Debug.Log(normal);
                    Vector3 mousePosition = Input.mousePosition;
                    float rayDistance;
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (new Plane(normal, GameObject.Find("Sun").transform.position).Raycast(ray, out rayDistance)){
                        Vector3 hitPoint = ray.GetPoint(rayDistance);
                        GameObject.Find("CircleOnPlane").GetComponent<CircleOnPlane>().addPlanet = false;
                        GameObject spawnedPrefab = Instantiate(prefab, hitPoint, Quaternion.identity);
                        spawnedPrefab.GetComponent<Label>().camera = GameObject.Find("Main Camera").GetComponent<Camera>();
                    }
                }
            }*/
        }
    }
}