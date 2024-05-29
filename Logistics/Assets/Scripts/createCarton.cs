using UnityEngine;
using System.Collections;

public class createCarton : MonoBehaviour
{
    //只有设置为public才可以被Button调用，函数作用：在世界坐标系原点创建一个cube
    public void AutoCreateCarton()
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = Vector3.zero;

       
    }
}

