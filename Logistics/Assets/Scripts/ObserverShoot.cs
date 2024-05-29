using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class ObserverShoot : MonoBehaviour {

	public float shootingRange = 50.0f;			//观察者射击距离
	public AudioClip shootingAudio;				//射击音效
	public GameObject shootingEffect;			//射击时的粒子效果对象
	public Transform shootingEffectTransform;	//播放粒子效果的Transfrom属性

	private LineRenderer gunLine;		//线渲染器：射击时的激光射线效果
	private bool isShooting;			//观察者是否正在射击
	private Camera myCamera;			//摄像机组件

	private Ray ray;					
	private RaycastHit hitInfo;
	private GameObject instantiation;

	private static float LINE_RENDERER_START=0.02f;	//射线初始宽度
	private static float LINE_RENDERER_END=0.05f;   //射线末端宽度

    


    //初始化函数，获取组件
    void Start () {
		gunLine = GetComponent<LineRenderer> ();		//获取线渲染器组件
		if (gunLine != null) gunLine.enabled = false;	//在监控开始时禁用线渲染器组件
		//myCamera = GetComponentInParent<Camera> ();		//获取父对象的摄像机组件
        GameObject gameObject = GameObject.Find("Camera");
        myCamera = gameObject.GetComponent<Camera>();
        //Debug.Log(myCamera);

    }

	//每帧执行一次，在Update函数后调用，实现观察者射击行为
	void LateUpdate () {	
		isShooting=CrossPlatformInputManager.GetButtonDown("Fire1");	//获取观察者射击键的输入（鼠标左键）
		//获取观察者射击输入，若在监控进行中（Playing）且鼠标光标不可见则，调用射击函数
		if (isShooting && (ProjectManager.PM==null || ProjectManager.PM.projectState == ProjectManager.ProjectManagerState.Playing)&& !Cursor.visible) {
			Shoot ();
		} else if (gunLine != null)	//若射击条件未满足，表示未进行射击，禁用线渲染器
			gunLine.enabled = false;
	}

	//射击函数
	void Shoot()
	{
		AudioSource.PlayClipAtPoint (shootingAudio, transform.position);	//播放射击音效
		
		ray.origin = myCamera.transform.position;		//设置射线发射的原点：摄像机所在的位置
		ray.direction = myCamera.transform.forward;		//设置射线发射的方向：摄像机的正方向
		if (gunLine != null) {
			gunLine.enabled = true;							//进行射击时，启用线渲染器（激光射线效果）
			gunLine.SetPosition (0, transform.position);	//设置线渲染器（激光射线效果）第一个端点的位置：玩家枪口位置
		}
		//发射射线，射线有效长度为shootingRange，若射线击中任何对象，则返回true，否则返回false
		if (Physics.Raycast (ray, out hitInfo, shootingRange)) {
           
            //当被击中的对象标签为Case，表明射线击中箱子
            if (hitInfo.transform.gameObject.tag.Equals ("Case"))
            {
                //播放射中箱子的相关音效方法，首先获取箱子的CaseInfo实例
                CaseInfo caseinfo = hitInfo.transform.gameObject.GetComponent<CaseInfo>();
                caseinfo.beShoot();
                //获取击中物体信息组件
                

            }
            //当被击中的对象标签为Shuttle，表明射线击中穿梭车
            if (hitInfo.transform.gameObject.tag.Equals("Shuttle"))
            {
                //播放射中shuttle的相关音效方法，首先获取穿梭车的Shuttle01实例
                Shuttle01 shuttleinfo = hitInfo.transform.gameObject.GetComponent<Shuttle01>();
                shuttleinfo.beShoot();

                

            }
            //当被击中的对象标签为inLifter，表明射线击中inLifter
            if (hitInfo.transform.gameObject.tag.Equals("inLifter"))
            {
                //获取击中物体信息组件
                InUpMechine inUpmachine = hitInfo.transform.gameObject.GetComponent<InUpMechine>();
                inUpmachine.beShoot();
                


            }
            //当被击中的对象标签为outLifter，表明射线击中outLifter
            if (hitInfo.transform.gameObject.tag.Equals("outLifter"))
            {
                //获取击中物体信息组件
                
                OutUpMechine outUpmachine = hitInfo.transform.gameObject.GetComponent<OutUpMechine>();
                outUpmachine.beShoot();

            }

            if (gunLine != null) {
				gunLine.SetPosition (1, hitInfo.point);	//当射线击中对象时，设置线渲染器（激光射线效果）第二个端点的位置：击中对象的位置
				gunLine.SetWidth (LINE_RENDERER_START, 	//射线在射程内击中对象时，需要根据击中对象的位置动态调整线渲染器（激光射线效果）的宽度
					Mathf.Clamp ((hitInfo.point - ray.origin).magnitude / shootingRange, 
						LINE_RENDERER_START, LINE_RENDERER_END));
			}
		} else if (gunLine != null) {
			//当射线未击中对象时，设置线渲染器（激光射线效果）第二个端点的位置：射线射出后的极限位置
			gunLine.SetPosition (1, ray.origin + ray.direction * shootingRange);
			//射线在射程内未击中对象，直接设置射线的初始与末尾宽度
			gunLine.SetWidth (LINE_RENDERER_START, LINE_RENDERER_END);
		}
	}
}
