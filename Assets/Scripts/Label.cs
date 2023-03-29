
using UnityEngine;
using System.Collections;
 
public class Label : MonoBehaviour
{
    //    public Transform cube;

    public Camera camera;

    public TrailRenderer trailRenderer;
    public float maxTrailWidth = 0.5f;
    public float widthCurveTime = 0f;

    bool isShowTip;
    public bool WindowShow = false;
    Rigidbody rb;
    Vector2 windowSize = new Vector2(300f, 170f);

    float radius;

    void Start()
    {
        if(this.name != "Sun") trailRenderer = transform.GetComponent<TrailRenderer>();
        isShowTip = true;
        radius = GetComponent<Collider>().bounds.size.x;
        if(this.name == "Sun") radius = radius + 100;
        rb = GetComponent<Rigidbody>();
		//npcHeight = GetComponent<Collider>().bounds.size.x ;
    }
    private void Update() {
        if(!camera) camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if(this.name == "Sun" || this.name == "Sun(Clone)") return;
        var dis = Vector3.Distance(camera.transform.position, transform.position) - radius;
        var width = Mathf.Min(dis / 100f, 1f);

        // 获取 TrailRenderer 的 widthCurve 属性
        if(!trailRenderer) return;
        AnimationCurve widthCurve = trailRenderer.widthCurve;

        // 查找时间为 widthCurveTime 的关键帧
        int keyframeIndex = -1;
        for (int i = 0; i < widthCurve.length; i++)
        {
            if (Mathf.Approximately(widthCurve.keys[i].time, widthCurveTime))
            {
                keyframeIndex = i;
                break;
            }
        }

        // 如果找到了关键帧，则将其值设为最大宽度
        if (keyframeIndex >= 0)
        {
            Keyframe keyframe = widthCurve.keys[keyframeIndex];
            keyframe.value = width;
            widthCurve.MoveKey(keyframeIndex, keyframe);
        }

        // 否则，创建一个新的关键帧，并将其值设为最大宽度
        else
        {
            float widthCurveValue = widthCurve.Evaluate(widthCurveTime);
            Keyframe keyframe = new Keyframe(widthCurveTime, Mathf.Max(widthCurveValue, width));
            widthCurve.AddKey(keyframe);
        }

        // 将修改后的曲线赋值给 TrailRenderer 的 widthCurve 属性
        trailRenderer.widthCurve = widthCurve;
    }
    void OnMouseEnter()
    {
        isShowTip = true;
 
    }
    void OnMouseExit()
    {
        isShowTip = true;
    }
    void OnGUI()
    {
        if (isShowTip)
        {
            //Debug.Log(this.name);
            RaycastHit hit;
            if (Physics.Linecast(transform.position, camera.transform.position, out hit, ~(1 << LayerMask.NameToLayer("Celestial")))){
                // Debug.Log("blocked:"+hit.transform.name);
                return;
            }

            Vector3 worldPosition = new Vector3 (transform.position.x, transform.position.y,transform.position.z);
            
            Vector2 position = camera.WorldToScreenPoint(worldPosition);

            var distance = Vector3.Distance(transform.position, camera.transform.position);

            //get 2d position

            var frustumHeight = 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var frustumWidth = frustumHeight * camera.aspect;
            var true_width = 1.0f * Screen.width * radius / frustumWidth;

            //Debug.Log(Screen.height);

            
            Vector2 labelSize = GUI.skin.label.CalcSize (new GUIContent(this.name));

            position = new Vector2 (position.x + true_width, Screen.height - position.y + (labelSize.y / 2.0f));
    
            
            /*
            if (this.tag == "green")
                    GUI.color = Color.green;
            else if(this.tag == "red")
                    GUI.color = Color.red;*/
            GUI.color = Color.white;
            GUI.skin.label.fontSize = 18;//font size
            
            GUI.Label(new Rect(position.x - (labelSize.x/2),position.y - labelSize.y ,labelSize.x,labelSize.y), this.name);
            /*GUIStyle style1= new GUIStyle();
            style1.fontSize = 30;
            style1.normal.textColor = Color.red;
            GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 400, 50),"Cube", style1);*/
 
        }
        if (WindowShow){
            RaycastHit hit;
            if (Physics.Linecast(transform.position, camera.transform.position, out hit, ~(1 << LayerMask.NameToLayer("Celestial")))){
                // Debug.Log("blocked:"+hit.transform.name);
                return;
            }

            Vector3 worldPosition = new Vector3 (transform.position.x, transform.position.y,transform.position.z);
            
            Vector2 position = camera.WorldToScreenPoint(worldPosition);

            var distance = Vector3.Distance(transform.position, camera.transform.position);

            //get 2d position

            var frustumHeight = 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var frustumWidth = frustumHeight * camera.aspect;
            var true_width = 1.0f * Screen.width * radius / frustumWidth;

            //Debug.Log(Screen.height);


            position = new Vector2 (Mathf.Min(position.x + true_width, Screen.width - windowSize.x), Mathf.Min(Screen.height - position.y + (50 / 2.0f), Screen.height - windowSize.y));
            GUI.Window(0, new Rect(position.x, position.y, windowSize.x, windowSize.y), MyWindow, this.name);
        }
    }
 
    //dialog
    void MyWindow(int WindowID)
    {
        //float maxwidth = 0f;
        GUILayout.Label("Position: " + (transform.position - GameObject.Find("Sun").transform.position));
        GUILayout.Label("Radius: " + transform.localScale.x * 2000f);
        GUILayout.Label("Mass(earth): " + rb.mass);
        GUILayout.Label("Velocity: " + rb.velocity.magnitude);

        if(GUILayout.Button("Close")){
            WindowShow = false;
        }
        /*
        // 获取每个 GUI 元素的矩形大小
        Rect positionLabelRect = GUILayoutUtility.GetLastRect();
        Rect radiusLabelRect = GUILayoutUtility.GetLastRect();
        Rect massLabelRect = GUILayoutUtility.GetLastRect();
        Rect velocityLabelRect = GUILayoutUtility.GetLastRect();

        // 计算能够包含这四个矩形的最小矩形大小
        Rect boundingBox = Rect.MinMaxRect(
            Mathf.Min(positionLabelRect.xMin, radiusLabelRect.xMin, massLabelRect.xMin, velocityLabelRect.xMin),
            Mathf.Min(positionLabelRect.yMin, radiusLabelRect.yMin, massLabelRect.yMin, velocityLabelRect.yMin),
            Mathf.Max(positionLabelRect.xMax, radiusLabelRect.xMax, massLabelRect.xMax, velocityLabelRect.xMax),
            Mathf.Max(positionLabelRect.yMax, radiusLabelRect.yMax, massLabelRect.yMax, velocityLabelRect.yMax)
        );

        // 动态调整窗口大小
        float windowWidth = boundingBox.width + GUI.skin.window.padding.horizontal;
        float windowHeight = boundingBox.height + GUI.skin.window.padding.vertical;
        
        windowSize = new Vector2(windowWidth, windowHeight);
        */
        
    }
    //鼠标点击事件
    void OnMouseDown()
    {
        if (WindowShow)
            WindowShow = false;
        else
            WindowShow = true;
    }
}