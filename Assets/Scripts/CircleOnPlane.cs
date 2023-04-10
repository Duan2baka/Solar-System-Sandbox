using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleOnPlane : MonoBehaviour
{

    public float radius = 1f;
    public int segments = 32;
    public Color color = Color.red;
    public float lineWidth = 10f;
    public Transform cameraTransform;
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;

    private Vector3 center;
    private Vector3[] positions;
    private LineRenderer lineRenderer;

    private Vector3 normal;
    private Vector3 planePoint;

    public bool addPlanet = false;

    void Start()
    {
        addPlanet = false;
        //pointA = GameObject.Find("Sun").transform;
        //pointB = GameObject.Find("Earth").transform;
        //pointC = GameObject.Find("Venus").transform;
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        planePoint = pointA.position;

        lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.useWorldSpace = true;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        center = Vector3.zero;
        positions = new Vector3[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.PI * 2f * i / segments;
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;
            positions[i] = new Vector3(x, y, 0f);
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = color;
    }

    void Update()
    {
        if(!addPlanet){
            lineRenderer.enabled = false;
            return;
        }
        lineRenderer.enabled = true;
        //normal = Vector3.Cross(pointC.position - pointA.position, pointB.position - pointA.position).normalized;
        normal = new Vector3(0.0f, 1.0f, 0.0f);
        //Debug.Log(normal);
        Vector3 mousePosition = Input.mousePosition;
        float rayDistance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (new Plane(normal, planePoint).Raycast(ray, out rayDistance))
        {
            Vector3 hitPoint = ray.GetPoint(rayDistance);
            radius = Vector3.Distance(hitPoint, pointA.position);
            Vector3 u, v;
            GetBasisVectors(normal, out u, out v);
            
            for (int i = 0; i <= segments; i++){
                float angle = Mathf.PI * 2f * i / segments;
                Vector3 pointOnCircle = pointA.position + radius * (Mathf.Cos(angle) * u + Mathf.Sin(angle) * v);
                positions[i] = pointOnCircle;
            }

            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);

            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.material.color = color;
        }
    }

    private void OnGUI() {
        if(!addPlanet) return;
        GUIStyle style1= new GUIStyle();
        style1.fontSize = 30;
        style1.normal.textColor = Color.red;
        GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 400, 50),"Radius: " + radius, style1);
    }

    private Vector3 GetCircleCenter(Vector3 mousePos, Vector3 a, Vector3 b, Vector3 c)
    {

        Vector3 rayDirection = cameraTransform.position - mousePos;
        Vector3 rayStart = mousePos;

        float rayDistance;
        Ray ray = new Ray(rayStart, rayDirection);
        if (new Plane(normal, planePoint).Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);
        }

        return Vector3.zero;
    }

    private void GetBasisVectors(Vector3 normal, out Vector3 u, out Vector3 v)
    {
        if (normal == Vector3.up || normal == Vector3.down)
        {
            u = Vector3.right;
            v = Vector3.forward;
        }
        else
        {
            u = Vector3.Cross(normal, Vector3.up).normalized;
            v = Vector3.Cross(normal, u).normalized;
        }
    }

}