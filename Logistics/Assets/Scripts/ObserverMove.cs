using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class ObserverMove : MonoBehaviour {

	public float moveSpeed = 6.0f;		//观察者移动速度
	public float rotateSpeed = 3.0f;    //观察者转向速度

    private GameObject MainCamera;      //场景上空主摄像机
    private float miniMouseRotateX = -75.0f;		//摄像机旋转角度的最小值
	private float maxiMouseRotateX = 75.0f;			//摄像机旋转角度的最大值
	private float mouseRotateX;						//当前摄像机在X轴的旋转角度
	

	private Camera myCamera;					//观察者的摄像机子对象
	//private Rigidbody rigid;					//观察者刚体组件
	//private CapsuleCollider capsuleCollider;	//观察者的胶囊体碰撞体

	//初始化，获取组件，并计算相关值
	void Start () {
		myCamera = GetComponentInChildren<Camera> ();		//获取摄像机组件
		mouseRotateX = myCamera.transform.eulerAngles.x;	//将当前摄像机在X轴的旋转角度赋值给mouseRotateX
        MainCamera = GameObject.Find("MainCamera");
        //rigid = GetComponent<Rigidbody> ();					//获取刚体组件
        //capsuleCollider = GetComponent<CapsuleCollider> ();	//获取胶囊体碰撞体


    }

	//每个固定时间执行一次，用于物理模拟
	void FixedUpdate()
	{
			
	}


    //每帧执行一次，用于获取观察者输入并控制角色的行为
    //说明Input.GetKey(KeyCode.Space)只为电脑端使用，CrossPlatformInputManager为跨平台组件，可配置适配PC 手机
    void Update () {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (myCamera.GetComponent<Camera>().enabled == true)
            {
                myCamera.GetComponent<Camera>().enabled = false;
                MainCamera.GetComponent<Camera>().enabled = true;
            }
            else
            {
                myCamera.GetComponent<Camera>().enabled = true;
                MainCamera.GetComponent<Camera>().enabled = false;
            }
           
        }
        //空格键抬升高度
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
        //Ctrl键降低高度
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z);
        }

        //当监控状态为项目进行中时
        if (ProjectManager.PM == null || ProjectManager.PM.projectState == ProjectManager.ProjectManagerState.Playing) 
		{
			float h = CrossPlatformInputManager.GetAxisRaw ("Horizontal");	//获取观察者水平轴上的输入
			float v = CrossPlatformInputManager.GetAxisRaw ("Vertical");	//获取观察者垂直轴上的输入
			Move (h, v);	//根据观察者的输入控制角色移动

			float rv = CrossPlatformInputManager.GetAxisRaw ("Mouse X");	//获取观察者鼠标垂直轴上的移动
			float rh = CrossPlatformInputManager.GetAxisRaw ("Mouse Y");	//获取观察者鼠标水平轴上的移动
            if (!Cursor.visible)
            {
                Rotate(rh, rv);	//根据观察者的鼠标输入控制角色转向
            }
			
		}
	}

	//观察者移动函数
	void Move(float h,float v){
		//观察者以moveSpeed的速度进行平移
		transform.Translate ((Vector3.forward * v + Vector3.right * h) * moveSpeed * Time.deltaTime);
	}

	//角色转向函数
	void Rotate(float rh,float rv){
		transform.Rotate (0, rv * rotateSpeed, 0);	//鼠标水平轴上的移动控制角色左右转向
		mouseRotateX -= rh * rotateSpeed;			//计算当前摄像机的旋转角度
		mouseRotateX = Mathf.Clamp (mouseRotateX, miniMouseRotateX, maxiMouseRotateX);	//将旋转角度限制在miniMouseRotateX与MaxiMouseRotateY之间
		myCamera.transform.localEulerAngles = new Vector3 (mouseRotateX, 0.0f, 0.0f);	//设置摄像机的旋转角度
	}
}
