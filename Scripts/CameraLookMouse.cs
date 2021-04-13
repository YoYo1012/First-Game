using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookMouse : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 cameraRoation;
    [SerializeField] GameObject character;//背後空白物件
    public float mouseSensitiaity;//靈敏
    public Vector2 angle;//限制角度
    public Transform lookAt;//弓箭射出方向空白物件

    Vector3 currentVelocity = Vector3.zero;     
    [SerializeField] float maxSpeed = 0.5f;
    [SerializeField] float smoothTime = 5f;
    // Start is called before the first frame update

    void Start()
    {
        cameraTransform =GameObject.Find("Main Camera").GetComponent<Transform>();
        lookAt.forward = cameraTransform.forward;
       
        
    }

    private void FixedUpdate()
    {
        var tmp_MouseX = Input.GetAxis("Mouse X");
        var tmp_MouseY = Input.GetAxis("Mouse Y");
        cameraRoation.x -= tmp_MouseY * mouseSensitiaity;//相機上下旋轉跟滑鼠Y值相反
        cameraRoation.y += tmp_MouseX * mouseSensitiaity;//相機左右旋轉

        cameraRoation.x = Mathf.Clamp(cameraRoation.x, angle.x, angle.y);//限制角度在angle(XY)之間

        cameraTransform.rotation = Quaternion.Euler(cameraRoation.x, cameraRoation.y, 0);
        lookAt.transform.rotation = cameraTransform.rotation;
        moveCamera();
    }
    void moveCamera() {//將相機移動到背後空白物件
        cameraTransform.position  = Vector3.SmoothDamp(cameraTransform.position, character.transform.position, ref currentVelocity, smoothTime, maxSpeed);
    }
}
