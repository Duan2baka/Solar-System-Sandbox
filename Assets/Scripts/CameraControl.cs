using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform pivot;                      // 手动添加：被跟踪的对象：pivot——以什么为轴
    public Vector3 pivotOffset = Vector3.zero; // 与目标的偏移量
    public Transform target;                     // 像一个被选中的对象(用于检查cam和target之间的对象)
    public float distance = 10.0f;     // 距目标距离(使用变焦)
    public float minDistance = 2f;        //最小距离
    public float maxDistance = 200000;       //最大距离
    public float zoomSpeed = 100f;        //速度倍率
    public float xSpeed = 250.0f;    //x速度
    public float ySpeed = 120.0f;    //y速度
    public bool allowYTilt = true;      //允许Y轴倾斜
    public bool free = false;
    public float speed = 5000f;
    public float yMinLimit = -90f;      //相机向下最大角度
    public float yMaxLimit = 90f;       //相机向上最大角度
    private float x = 0.0f;      //x变量
    private float y = 0.0f;      //y变量
    private float targetX = 0f;        //目标x
    private float targetY = 0f;        //目标y
    private float targetDistance = 0f;        //目标距离
    private float xVelocity = 1f;        //x速度
    private float yVelocity = 1f;        //y速度
    private float zoomVelocity = 1f;        //速度倍率


    void Start()
    {
        var angles = transform.eulerAngles;                          //当前的欧拉角
        targetX = x = angles.x;                                   //给x，与目标x赋值
        targetY = y = ClampAngle(angles.y, yMinLimit, yMaxLimit); //限定相机的向上，与下之间的值，返回给：y与目标y
        targetDistance = distance;                                       //初始距离数据为10；
    }

    public void setTargetDistance(float f){
        targetDistance = f;
    }

    void LateUpdate()
    {
        if (pivot) //如果存在设定的目标
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float tmp = distance - target.GetComponent<Collider>().bounds.size.x;
            zoomSpeed = Mathf.Clamp(tmp * 0.1f,10f,10000f);
            if (Input.touchCount == 2){
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                targetDistance += deltaMagnitudeDiff * zoomSpeed;
                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
            }

            if (scroll > 0.0f) targetDistance -= zoomSpeed;  
            else if (scroll < 0.0f) targetDistance +=zoomSpeed;
            minDistance = pivot.gameObject.GetComponent<SphereCollider>().radius * 2.0f;
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);      
            if (Input.GetMouseButton(1) || Input.GetMouseButton(0) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))){
                targetX += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                if (allowYTilt){
                    targetY -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                    targetY = ClampAngle(targetY, yMinLimit, yMaxLimit);
                }
            }
            #region 使用了平滑插值
            x = Mathf.SmoothDampAngle(x, targetX, ref xVelocity, 0.3f);
            if (allowYTilt) y = Mathf.SmoothDampAngle(y, targetY, ref yVelocity, 0.3f);
            else y = targetY;
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, 0.5f);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + pivot.position + pivotOffset;
            transform.rotation = rotation;
            transform.position = position;
            #endregion

        }
    }
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}