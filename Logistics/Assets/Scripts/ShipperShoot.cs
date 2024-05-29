using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class ShipperShoot : MonoBehaviour
{
    public enum  EnumShipperRayNum
    {   OrderShipperRay001,
        OrderShipperRay002,
        OrderShipperRay003,
        OrderShipperRay004,
        OrderShipperRay005,
        OrderShipperRay006,
        OrderShipperRay007,
        OrderShipperRay008,
    };
    public string ShipperRayNum;//光电检测装置编号


    public float shootingRange = 3.0f;         //输送射线检测射击距离
    public AudioClip shootingAudio;             //射击音效
    private bool isShooting;            //光电检测是否正在发射
    //public GameObject RayGun;          //光电检测组件
    private LineRenderer gunLine1;		//线渲染器：射击时的激光射线效果
    public bool ShootCaseState;//光电检测装置是否检测到物体
    public bool Intflag;//并行三岔口检测标志位
    public string onCaseName;//光电击中的箱子名称

    Shuttle01 shuttle01;//一层小车上的类
    private Ray ray;
    public RaycastHit hitInfo;
    private GameObject instantiation;
    private float LINE_RENDERER_START = 0.02f;   //射线初始宽度
    private float LINE_RENDERER_END = 0.05f;   //射线末端宽度
    float m_timer = 0;//延时计时器
    CaseInfo caseinfo;//箱子类


    //初始化函数，获取组件
    void Start()
    {
        shuttle01 = GameObject.Find("Shuttle01").GetComponent<Shuttle01>();
        Intflag = false;
        shootingRange = 4f;//设置所有光电检测装置检测距离
        if (this.name== "InShipperRay003")
        {
            this.shootingRange = 1.5f;
        }
        if (this.name == "Shuttle01_R_Ray")
        {
            this.shootingRange = 30f;
        }
        if (this.name== "Shuttle01_L_Ray")//小车上光电射程
        {
            this.shootingRange = 30f;
        }
        
        onCaseName = "初始化站台处箱子名称";
        ShipperRayNum = this.name;
        caseinfo = null;
        ShootCaseState = false;//光电检测是否检测到箱子
        gunLine1 = GetComponent<LineRenderer>();     //获取线渲染器组件
        if (gunLine1 != null) gunLine1.enabled = false;	//在监控开始时禁用线渲染器组件
       
       
        //RayGun = GameObject.Find("RayShoot007");
        isShooting = true;
        //Debug.Log(RayGun);

        //遍历此.CS文件挂的物体下每个子物体
        //foreach (Transform RayDevice in GetComponentsInChildren<Transform>())
        //{
          //  print("name:" + RayDevice.name + "\n");
            //if (RayDevice.GetComponent<ShipperShoot> ().ShipperRayNum!=null)//判断每个子物体是否有组件ShipperRayNum
            //{
                //获取每个物体的ShipperShoot组件，给每个RayGun赋予各自的值
              //  this.ShipperRayNum = RayDevice.name;

            // RayDevice.GetComponent<ShipperShoot>().ShipperRayNum = RayDevice.name;
            // RayDevice.GetComponent<ShipperShoot>().RayGun= GameObject.Find(RayDevice.name);


                //RayDevice.GetComponent<ShipperShoot>().enabled = false;//判断每个子物体是否有MeshRenderer组件，如果有，使其透明
            //}
       // }

    }

    //每帧执行一次，在Update函数后调用，实现观察者射击行为
   
    void FixedUpdate()
    {
        //m_timer += Time.time;//延时执行
        //if (m_timer >= 20)
        //{
        //    RayShootFun();//光电检测方法
        //    //RayDeviceLayer02_L_01();//RayDeviceLayer02_L_01 射线检测装置逻辑
        //    m_timer = 0;
        //}
        RayShootFun();

    }

    void RayShootFun()
    {
        //CrossPlatformInputManager.GetButtonDown("Fire1");  //获取观察者射击键的输入（鼠标左键）
        //获取光电传感器射击输入，若在监控进行中（Playing）则，调用射击函数
        if (isShooting && (ProjectManager.PM == null || ProjectManager.PM.projectState == ProjectManager.ProjectManagerState.Playing))
        {
            //if (ShipperRayNum == EnumShipperRayNum.OrderShipperRay001.ToString())
            //{
            /*if (this.name!= "Shuttle01_R_Ray"||this.name!= "Shuttle01_L_Ray") {Shoot();}*/
            Shoot(); 
            //else
            //{
            //    if (shuttle01.isEndRow) { Shoot(); }
            //}
                
            //}
            
        }
    }    

    //光电检测装置射击函数
    void Shoot()
    {
        ray.origin = transform.position;       //设置射线发射的原点：光电所在的位置
        
        ray.direction = transform.up;     //设置射线发射的方向：光电的正方向
                                          //设置入库提升机上检测箱子(如果当前光电检测是入库提升机上的)

        //发射射线，射线有效长度为shootingRange，若射线击中任何对象，则返回true，否则返回false
        InpuMechineRayLogic();//提升机射线检测装置逻辑
        Shuttle01RayLogic();//穿梭车射线检测装置逻辑
        if (Physics.Raycast(ray, out hitInfo, shootingRange))// && hitInfo.transform.gameObject.tag.Equals("Case")
        {
            AllRayLogic();//所有射线检测通用方法
            
            
        }
        else { ShootCaseState = false; }
        
    }

    /// <summary>
    /// RayDeviceLayer02_L_01 射线检测装置逻辑
    /// </summary>
    private void RayDeviceLayer02_L_01()
    {
        if (this.name == "RayStationLayer02_L_01")
        {
            //如果该光电击中箱子为提升机任务箱子，则改变提升机任务状态
            

            //if (this.onCaseName != null) { print(onCaseName); }
            //print("设备"+ ShipperRayNum+"击中箱子"+"");
            //    if (hitInfo.transform.gameObject.tag.Equals("Case"))
            //    {
            //        caseinfo = hitInfo.transform.gameObject.GetComponent<CaseInfo>();
            //        if (caseinfo.name== GameObject.Find("InUpRayDevice").GetComponent<ShipperShoot>().hitInfo)
            //        {

            //        }
            //        GameObject.Find("InUpMechine").GetComponent<InUpMechine>().onCase = hitInfo.transform.gameObject;
            //        GameObject.Find("InUpMechine").GetComponent<InUpMechine>().isOnCase = true;
            //        print("入库提升机击中物体");
            //    }
        }
    }

    /// <summary>
    /// 所有射线检测装置通用方法
    /// </summary>
    private void AllRayLogic()
    {
        //Debug.Log("射线检测成功");
        //AudioSource.PlayClipAtPoint(shootingAudio, transform.position); //播放射击音效
        //当被击中的对象标签为Case，表明射线击中箱子
        if (hitInfo.transform.gameObject.tag.Equals("Case"))
        {
            ShootCaseState = true;//设置该光电击中箱子状态
            //运行射中箱子的相关处理方法，首先获取箱子的CaseInfo实例
            caseinfo = hitInfo.transform.gameObject.GetComponent<CaseInfo>();

            caseinfo.RayDeviceNum = ShipperRayNum;//设置箱子类的击中箱子的光电检测设备编号

            this.onCaseName = caseinfo.name;//设置当前光电检测到的箱子编号
           
            


            //caseinfo.beDetected(ShipperRayNum);
            //print("设备"+ ShipperRayNum+"击中箱子"+""+caseinfo.name);
            //获取击中物体信息组件

            //Debug.Log("箱子移动");

        }
    }

    /// <summary>
    /// 提升机射线检测处理逻辑
    /// </summary>
    private void InpuMechineRayLogic()
    {
        if (this.name == "InUpRayDevice")
        {
            if (Physics.Raycast(ray, out hitInfo, shootingRange))
            {
                if (hitInfo.transform.gameObject.tag.Equals("Case"))
                {
                    //将击中箱子对象给入库提升机
                    GameObject.Find("InUpMechine").GetComponent<InUpMechine>().onCase = hitInfo.transform.gameObject;
                    GameObject.Find("InUpMechine").GetComponent<InUpMechine>().isOnCase = true;
                    //print("入库提升机击中物体");
                }
            }
            else
            {
                //print("zai");
                //将入库提升机的onCase对象设置为空
                GameObject.Find("InUpMechine").GetComponent<InUpMechine>().onCase = null;
                GameObject.Find("InUpMechine").GetComponent<InUpMechine>().isOnCase = false;
            }
        }
    }
    /// <summary>
    /// 提升机射线检测处理逻辑
    /// </summary>
    private void Shuttle01RayLogic()
    {
        if (this.name == "Shuttle01_R_Ray")//
        {
            if (Physics.Raycast(ray, out hitInfo, shootingRange))
            {
                
                if (hitInfo.transform.gameObject.tag.Equals("Case"))
                {
                    //将击中箱子对象给入库提升机
                    shuttle01.onRightCase = hitInfo.transform.gameObject;
                    //print("jizhong");
                    //shuttle01.onRightCaseName = hitInfo.transform.gameObject.GetComponent<CaseInfo>().CaseNum.ToString();

                    //GameObject.Find("Shuttle01").GetComponent<Shuttle01>().isOnCase = true;
                    //print("入库提升机击中物体");
                }
            }
            else
            {
                shuttle01.onRightCase = null;
                //GameObject.Find("Shuttle01").GetComponent<InUpMechine>().isOnCase = false;
            }
        }
        if (this.name == "Shuttle01_M_Ray")//
        {
            if (Physics.Raycast(ray, out hitInfo, shootingRange))
            {

                if (hitInfo.transform.gameObject.tag.Equals("Case"))
                {
                    //将击中箱子对象给入库提升机
                    shuttle01.onCase = hitInfo.transform.gameObject;
                    //print("jizhong");
                    //shuttle01.onRightCaseName = hitInfo.transform.gameObject.GetComponent<CaseInfo>().CaseNum.ToString();

                    //GameObject.Find("Shuttle01").GetComponent<Shuttle01>().isOnCase = true;
                    //print("入库提升机击中物体");
                }
            }
            else
            {
                if (shuttle01.onCase != null) { shuttle01.onCase.transform.parent = GameObject.Find("Carton").transform; }
                
                shuttle01.onCase = null;
                //GameObject.Find("Shuttle01").GetComponent<InUpMechine>().isOnCase = false;
            }
        }
        if (this.name == "Shuttle01_L_Ray")//
        {
            if (Physics.Raycast(ray, out hitInfo, shootingRange))
            {
                if (hitInfo.transform.gameObject.tag.Equals("Case"))
                {
                    //将击中箱子对象给入库提升机
                    shuttle01.onLeftCase = hitInfo.transform.gameObject;

                    //shuttle01.onLeftCaseName = hitInfo.transform.gameObject.GetComponent<CaseInfo>().CaseNum.ToString();

                    //GameObject.Find("Shuttle01").GetComponent<Shuttle01>().isOnCase = true;
                    
                }
            }
            else
            {
                shuttle01.onLeftCase = null;
                //GameObject.Find("Shuttle01").GetComponent<InUpMechine>().isOnCase = false;
            }
        }
    }
}
        

