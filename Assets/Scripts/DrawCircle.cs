using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public float radius = 1000f;
    public int segments = 32;
    public Color color = Color.white;
    public float lineWidth = 100f;

    private LineRenderer lineRenderer;

    void Start()
    {
        // 创建一个新的 LineRenderer 组件
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // 设置 LineRenderer 的参数
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // 生成圆上的点，并将它们赋给 LineRenderer 的位置数组
        Vector3[] positions = new Vector3[segments + 1];
        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.PI * 2f * i / segments;
            float x = Mathf.Sin(angle) * radius;
            float y = Mathf.Cos(angle) * radius;
            positions[i] = new Vector3(x, y, 0f);
        }
        lineRenderer.SetPositions(positions);

        // 设置 LineRenderer 的材质和颜色
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material.color = color;
    }
}