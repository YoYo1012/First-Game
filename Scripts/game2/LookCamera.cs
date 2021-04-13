using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 selfToCameraVector = Camera.main.transform.position - transform.position;//相機和角色間向量
        Quaternion vectorRotation = Quaternion.LookRotation(selfToCameraVector);//相機和角色間旋轉量
        transform.rotation = new Quaternion(vectorRotation.x , vectorRotation.y, 0, vectorRotation.w);//只轉xy軸
    }
}
