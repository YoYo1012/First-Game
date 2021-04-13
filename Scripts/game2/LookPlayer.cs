using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 selfToCameraVector = Camera.main.transform.position - transform.position;//相機和角色間向量
        Quaternion vectorRotation = Quaternion.LookRotation(selfToCameraVector);//相機和角色間旋轉量
        transform.rotation = new Quaternion(0, vectorRotation.y, 0, vectorRotation.w);
    }
}
