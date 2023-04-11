
using UnityEngine;
using System.Collections;
 
public class Label : MonoBehaviour
{
    //    public Transform cube;

    public Camera myCamera;

    public TrailRenderer trailRenderer;
    public float maxTrailWidth = 0.5f;
    public float widthCurveTime = 0f;

    bool isShowTip;
    public bool WindowShow = false;
    Rigidbody rb;
    Vector2 windowSize = new Vector2(100f, 150f);
    private string massString = "";
    private string radiusString = "";
    private string velocityString = "";
    float radius;
    private bool messageShow = false;
    private string message = "";
    private float newMass, newVelocity, newRadius;
    public float scale = 2.0f;

    void Start()
    {
        if(this.name != "Sun") trailRenderer = transform.GetComponent<TrailRenderer>();
        isShowTip = true;
        radius = GetComponent<Collider>().bounds.size.x;
        if(this.name == "Sun" || this.name == "Planet1" || this.name == "Planet2" || this.name == "Sun(Clone)") radius = radius + 100;
        rb = GetComponent<Rigidbody>();
        messageShow = false;
        windowSize = Vector2.Scale(windowSize, new Vector2(Screen.width / 800f, Screen.height / 600f));
		//npcHeight = GetComponent<Collider>().bounds.size.x ;
    }
    private void Update() {
        fix_input();
        if(!myCamera) myCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        if(this.name == "Sun" || this.name == "Sun(Clone)") return;
        var dis = Vector3.Distance(myCamera.transform.position, transform.position) - radius;
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
        float scaledWidth = Screen.width * scale;
        float scaledHeight = Screen.height * scale;
        // 计算缩放后的位置，使窗口居中
        float xPos = (Screen.width - scaledWidth) / 2;
        float yPos = (Screen.height - scaledHeight) / 2;

        // 缩放窗口及其内容
        GUI.matrix = Matrix4x4.TRS(new Vector3(xPos, yPos, 0), Quaternion.identity, new Vector3(scale, scale, 1));
        if (isShowTip)
        {
            //Debug.Log(this.name);
            RaycastHit hit;
            if (Physics.Linecast(transform.position, myCamera.transform.position, out hit, ~(1 << LayerMask.NameToLayer("Celestial")))){
                // Debug.Log("blocked:"+hit.transform.name);
                return;
            }

            Vector3 worldPosition = new Vector3 (transform.position.x, transform.position.y,transform.position.z);
            
            Vector2 position = myCamera.WorldToScreenPoint(worldPosition);

            var distance = Vector3.Distance(transform.position, myCamera.transform.position);

            //get 2d position

            var frustumHeight = 2.0f * distance * Mathf.Tan(myCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var frustumWidth = frustumHeight * myCamera.aspect;
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
            if (Physics.Linecast(transform.position, myCamera.transform.position, out hit, ~(1 << LayerMask.NameToLayer("Celestial")))){
                // Debug.Log("blocked:"+hit.transform.name);
                return;
            }

            Vector3 worldPosition = new Vector3 (transform.position.x, transform.position.y,transform.position.z);
            
            Vector2 position = myCamera.WorldToScreenPoint(worldPosition);

            var distance = Vector3.Distance(transform.position, myCamera.transform.position);

            //get 2d position

            var frustumHeight = 2.0f * distance * Mathf.Tan(myCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var frustumWidth = frustumHeight * myCamera.aspect;
            var true_width = 1.0f * Screen.width * radius / frustumWidth;

            //Debug.Log(Screen.height);


            position = new Vector2 (Mathf.Min(position.x + true_width, Screen.width - windowSize.x), Mathf.Min(Screen.height - position.y + (50 / 2.0f), Screen.height - windowSize.y));
            GUI.Window(0, new Rect(position.x, position.y, windowSize.x, windowSize.y), MyWindow, this.name);
        }
        
        if (messageShow){
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            float windowWidth = 100 * (Screen.width / 800f);
            float windowHeight = 60 * (Screen.height / 600f);
            float windowX = (screenWidth - windowWidth) / 2;
            float windowY = (screenHeight - windowHeight) / 2;

            // Set window position
            GUI.Window(0, new Rect(windowX, windowY, windowWidth, windowHeight), (int id) => {
                // Draw messages
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(message);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                // Draw back button
                if (GUILayout.Button("Back")) {
                    messageShow = false;
                }
            }, "Info Window");
        }
    }
    private bool check_input(ref float mass, ref float velocity, ref float radius){
        return float.TryParse(massString, out mass) && float.TryParse(velocityString, out velocity) 
        && float.TryParse(radiusString, out radius);
    }
    //dialog
    void MyWindow(int WindowID)
    {
        //float maxwidth = 0f;
        GUILayout.Label("Position: " + (transform.position - GameObject.Find("Sun").transform.position));
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Radius: ", GUILayout.ExpandWidth(true));
        radiusString = GUILayout.TextField(radiusString, GUILayout.MaxWidth(40 * (Screen.width / 800f)));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Mass(earth): ", GUILayout.ExpandWidth(true));
        massString = GUILayout.TextField(massString, GUILayout.MaxWidth(40 * (Screen.width / 800f)));
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Velocity: ", GUILayout.ExpandWidth(true));
        velocityString = GUILayout.TextField(velocityString, GUILayout.MaxWidth(40 * (Screen.width / 800f)));
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Apply")){
            if(!check_input(ref newMass, ref newVelocity, ref newRadius)){
                WindowShow = false;
                messageShow = true;
                message = "Input error!";
            }
            else{
                WindowShow = false;
                // Debug.Log("" + newVelocity + " " + rb.velocity.magnitude);
                if(rb.velocity.magnitude != 0f){
                    rb.velocity = rb.velocity * (newVelocity / rb.velocity.magnitude);
                }
                rb.mass = newMass;
                transform.localScale *= ((newRadius / 2000f) / transform.localScale.x);
                messageShow = true;
                message = "Applied successfully!";
            }
        }
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

    void fix_input(){
        if (!System.Text.RegularExpressions.Regex.IsMatch(massString, "^[0-9.]*$")){
            // Remove non-numeric characters
            massString = System.Text.RegularExpressions.Regex.Replace(massString, "[^0-9.]", "");
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(radiusString, "^[0-9.]*$")){
            // Remove non-numeric characters
            radiusString = System.Text.RegularExpressions.Regex.Replace(radiusString, "[^0-9.]", "");
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(velocityString, "^[0-9.]*$")){
            // Remove non-numeric characters
            velocityString = System.Text.RegularExpressions.Regex.Replace(velocityString, "[^0-9.]", "");
        }
    }

    //鼠标点击事件
    void OnMouseDown()
    {
        if (WindowShow)
            WindowShow = false;
        else{
            WindowShow = true;
            massString = "" + rb.mass;
            radiusString = "" + transform.localScale.x * 2000f;
            velocityString = "" + rb.velocity.magnitude;
        }
    }
}