using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// CaseInfo类用于描述箱子内商品信息数量信息等
/// </summary>
public class CaseInfo : MonoBehaviour
{

    /// <summary>
    /// 箱子的相关数据库属性（需要显示的相关信息）在此处定义
    ///</summary>

    //容器号：xxxx
    public int CaseNum;
    //容器类型：RIC周转箱、carton原纸箱、订单周转箱
    public enum CaseType{ RIC,Carton,OrderRIC}

    //容器状态：满、少于一半、多于一半、空
    public enum CaseStatus { FULL,lessHalf,moreThanHalf,Empty}

    public CaseType casetype = CaseType.Carton;
    public CaseStatus casestatus = CaseStatus.moreThanHalf;
    //长宽高：用于从数据库获取从界面上显示
    public float length;
    public float width;
    public float height;
    //箱子位置
    public bool isInWH;//箱子是否在仓库中
    public string caseLocation;//箱子当前位置，此处采用碰撞检测触发位置信息改变

    public string targetWH;//入库时箱子的扫码器赋予仓库货位WH01   后台根据数据库查询结果返回目标位置

    ///<summary>
    /// unity中箱子的属性在此处定义
    /// </summary>
    public AudioClip beShootAudio;    //箱子被射线击中音效
    public GameObject CaseInfoCanvas; //提升机设备信息Canvas
    public string RayDeviceNum;//射中箱子的光电检测设备编号
    public GameObject InUpMechine;//入库提升机3D对象
    public GameObject OutUpMechine;//出库提升3D对象
    public InUpMechine inUpMechine;//绑定到提升机上的类组件

    public Shuttle01 shuttle01;//一层穿梭车类
    public GameObject Shuttle01;//一层穿梭车对象

    public bool InMove;//标志位，确定入库提升机上箱子是否移动

    public GameObject ObjSshuttle01;//一层小车对象

    ShipperShoot shippershoot;//光电检测中的类
    ShipperShoot shippershoot2;//三叉角处货物通行光电2
    ShipperShoot shippershoot3;//提升机处使用
    ShipperShoot CurrentShipperRay;//当前击中箱子的光电检测装置类
    private Collider collider;          //箱子的Collider组件
    private Rigidbody rigidbody;        //箱子的rigidbody组件
    public GameObject target;       //箱子移动的目标位置
    public GameObject[] nearCase;  //相邻箱子
    public float moveSpeed = 8.0f;  //箱子的移动速度
    public float minDist = 0.1f;    //移动距离距离，当箱子与目标的距离小于等于该值时，箱子不再向目标移动
    private bool ToShuttleMoveTag;
    int i = 1;
    //初始化，获取箱子的组件
    void Start()
    {
        InMove = false;
        ToShuttleMoveTag = false;
        //InUpMechine = GameObject.Find("InUpMechine");
        //OutUpMechine= GameObject.Find("OutUpMechine");


        RayDeviceNum = null;
        shippershoot = null;
        shippershoot3 = null;
        collider = GetComponent<Collider>();    //获取箱子的Collider组件
        rigidbody = GetComponent<Rigidbody>();  //获取箱子的Rigidbody组件
        //nearCase = GameObject.FindGameObjectsWithTag("Case");
        //foreach (var item in nearCase)
        //{
        //    //Debug.Log(item.name);
        //}
    }


    /// <summary>
    /// 项目运行中箱子的相关处理方法在此处定义
    /// </summary>
    //每帧执行一次，检测箱子的各种状态并执行相应操作
    //定义一个每一步检测变量caseStep来表示当前执行的步骤
    private int caseStep = 0;
    private void FixedUpdate()
    {

        beDetected(RayDeviceNum);//根据光电检测设备号调用不同处理方法
        //if (ToShuttleMoveTag)
        //{
           // MoveToShuttle("穿梭车100");
        //}
        

    }
    void Update()
    {
        

        //根据检测不同的箱子类型，判断不同的箱子状态，执行相应操作
        switch (casetype)
        {
            case CaseType.Carton:
                switch (casestatus)
                {
                    case CaseStatus.Empty:

                        break;
                    case CaseStatus.FULL:

                        break;
                    case CaseStatus.lessHalf:

                        break;
                    case CaseStatus.moreThanHalf:

                        break;
                }
                break;
            case CaseType.OrderRIC:
                
                break;
            case CaseType.RIC:
                
                break;
        }
    }


    //箱子对象创建
    public void createCase(Vector3 vector3)
    {
        //GameObject.CreatePrimitive.
    }
    //箱子对象销毁

    public void moveInLifterCase(bool InMove)
    {
        if (InMove)
        {
            this.MoveFoward();
        }
        
    }

    //箱子对象移动  
    public void caseMove()
    {




        //dist = Vector3.Distance(transform.position, nearCase.transform.position); //计算箱子与箱子之间的距离
        //当项目监控状态为进行（Playing）时
        //if (ProjectManager.PM == null || ProjectManager.PM.projectState == ProjectManager.ProjectManagerState.Playing)
        //{

        //if (dist < minDist) { }
           //当箱子与箱子的距离大于安全距离时

        
        
        //根据不同的步骤状态执行相应箱子操作
        switch (caseStep)
            {
                case 0:
                    target = GameObject.Find("target001_case");

                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    if (transform.position == target.transform.position)
                    {
                        caseStep = 1;
                        target = GameObject.Find("target002_case");
                    }
                    break;
                case 1:
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    if (transform.position == target.transform.position)
                    {
                        caseStep = 2;
                        target = GameObject.Find("target003_case");
                    }
                    break;
                case 2:
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    if (transform.position == target.transform.position)
                    {
                        caseStep = 3;
                        target = GameObject.Find("target004_case");
                    }
                    break;
                case 3:
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    if (transform.position == target.transform.position)
                    {
                        caseStep = 4;
                        target = GameObject.Find("target005_case");
                    }
                    break;
                case 4:
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    if (transform.position == target.transform.position)
                    {
                        caseStep = 5;
                        target = GameObject.Find("target006_case");
                    }
                    break;
                case 5:
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                if (transform.position == target.transform.position)
                {
                    caseStep = 6;
                    target = GameObject.Find("target007_case");
                }

                //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);

                    break;
                case 6:
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);

                if (transform.position == target.transform.position)
                {
                    //GameObject inLifter = GameObject.Find("InUpMechine");
                    //this.transform.parent = inLifter.transform;
                    caseStep = 7;
                    //target = GameObject.Find("target007");
                    break;
                }
                break;
            case 7:
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                if (transform.position == target.transform.position)
                {
                    caseStep = 8;
                    target = GameObject.Find("target007_case");
                }
                break;
            case 8:
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                if (transform.position == target.transform.position)
                {
                    caseStep = 9;
                    target = GameObject.Find("target008_case");
                }
                break;
            case 9:
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                if (transform.position == target.transform.position)
                {
                    
                    caseStep = 10;
                    target = GameObject.Find("target009_case");
                }
                break;
            case 10:
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                if (transform.position == target.transform.position)
                {
                    GameObject shuttle01 = GameObject.Find("Shuttle01");
                    this.transform.parent = shuttle01.transform;
                    caseStep = 20;
                    target = GameObject.Find("target006_case");
                }
                break;
            case 11:
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                if (transform.position == target.transform.position)
                {
                    caseStep = 5;
                    target = GameObject.Find("target006_case");
                }
                break;
            case 12:
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                if (transform.position == target.transform.position)
                {
                    caseStep = 5;
                    target = GameObject.Find("target006_case");
                }
                break;
        }

            //Vector3 Goal01 = new Vector3(-16.35f, -51.33281f, -72.89f);
            // transform.position = Vector3.MoveTowards(transform.position, Goal01, Time.deltaTime * moveSpeed);
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);

            //transform.position += transform.forward * moveSpeed * Time.deltaTime; //箱子以moveSpeed的速度向追踪目标靠近
       
        

        //}
    }

    //箱子移动到小车（小车取货逻辑）的动作
    public void MoveToShuttle(String shuttleName)
    {
        switch (shuttleName)
        {
            case "穿梭车100":
                target = GameObject.Find("CartonShuttle01Location");

                if (shuttle01.rightTake)
                {
                    if (transform.position != target.transform.position)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    }
                    else
                    {
                        shuttle01.rightTake = false;
                    }
                }
                if (shuttle01.leftTake)
                {
                    if (transform.position != target.transform.position)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    }
                    else
                    {
                        shuttle01.leftTake = false;
                    }
                }
                break;
        }
    }
    

    //箱子被射线击中时函数，用于ObserverShoot脚本中调用
    //当前函数主要功能为调用GUI控件显示
    public void beShoot()
    {
        //调用相关GUI控件逻辑显示箱子信息
        CaseInfoCanvas.SetActive(true);

        //在箱子位置处播放箱子被击中音效
        if (beShootAudio != null) 
            AudioSource.PlayClipAtPoint(beShootAudio, transform.position);

        if (true)
        {    
            //当箱子信息与实际信息不符时，可改变项目运行状态
            if (ProjectManager.PM != null)
            {
                ProjectManager.PM.projectState = ProjectManager.ProjectManagerState.Playing;//根据判断条件改变项目状态
            }
           
           
        }
    }
    //Vector3 moveh = new Vector3(82.41568f, 5.398286f, -101.6666f);

    
    /// <summary>
    /// 箱子被光电检测到后的运行逻辑
    /// </summary>
    /// <param name="RayDeviceNum"></param>
    public void beDetected(string RayDeviceNum)//RayDeviceNum为当前光电检测装置编号
    {
        
        OrderShipperSwitch(RayDeviceNum);
        SelectShipperSwitch(RayDeviceNum);
        OutShipperSwitch(RayDeviceNum);
        InShipperSwitch(RayDeviceNum);
        InStationShipperSwitch(RayDeviceNum);
        switch (RayDeviceNum)
        {

            case "Shuttle01_M_Ray":
                this.transform.parent = shuttle01.transform;
                shuttle01.middleDetected = true;
                shuttle01.rightTake = false;
                shuttle01.leftTake = false;
                shuttle01.onCase = this.transform.gameObject;
                //放货逻辑：该箱子变为carton子对象，并且该箱子

                //Shuttle01
                //Debug.Log("Shuttle01_M_Ray" + this.name);
                break;
            case "Shuttle01_R_Ray":
                if (shuttle01.isEndRow)
                {
                    shuttle01.rightDetected = true; //print("right");

                    ////取货逻辑：该箱子移动到穿梭车目标位置，并且成为穿梭车子对象
                    //target = GameObject.Find("CartonShuttle01Location");

                    //if (shuttle01.rightTake)
                    //{
                    //    if (transform.position != target.transform.position)
                    //    {
                    //        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    //        this.MoveLeft();
                    //    }
                    //    else
                    //    {
                    //        shuttle01.rightTake = false;
                    //    }
                    //}
                }

                //Debug.Log("Shuttle01_R_Ray"+this.name);
                break;
            case "Shuttle01_L_Ray":
                if (shuttle01.isEndRow) { shuttle01.leftDetected = true;//print("left");

                //ToShuttleMoveTag
                //取货逻辑：该箱子移动到穿梭车目标位置，并且成为穿梭车子对象
                //target = GameObject.Find("CartonShuttle01Location");
                //if (shuttle01.leftTake)
                //{
                //    //if (transform.position != target.transform.position)
                //    //{
                //        //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                //        this.MoveRight();
                //    //}
                //    //else
                //    //{
                //    //    shuttle01.leftTake = false;
                //    //}
                //}
                }
                //Debug.Log("Shuttle01_L_Ray" + this.name);
                break;




            //2.若箱子从输送移动到提升机，提升机上光电检测到箱子，此时更改提升机状态为任务占用状态（任务占用状态提升机需等待目标层参数）
            case "InUpRayDevice"://此处为提升机上光电检测装置编号
                inUpMechine.TaskStatus = "inTask";//提升机上有物体，任务状态为占用
                //print("2.任务中");
                inUpMechine.isOnCase = true;//设置提升机上有物体
                this.transform.parent = inUpMechine.transform;//提升机上箱子父对象为提升机

                //获取目标层动力站台上光电检测装置实例，提升机层数，判断是否检测到物体，提升机层数是否为同一层

                judgeRayDevice(inUpMechine.lifterDestination);
                if (inUpMechine.isEndLocation)//若到达目标层 &&inUpMechine.TaskStatus=="inTask"s
                {

                    if (shippershoot3 != null)
                    {
                        //print(shippershoot3.name);
                        if (shippershoot3.ShootCaseState)//下一个光电检测是否检测到箱子
                        {

                            //print("123");
                            break;
                        }
                        else//目标层光电未检测到箱子
                        {
                            //print("234");
                            MoveFoward();
                        }
                    }


                }
                break;
            case "OutUpRayDevice"://此处为提升机上光电检测装置编号
                inUpMechine.TaskStatus = "inTask";//提升机上有物体，任务状态为占用
                //print("2.任务中");
                inUpMechine.isOnCase = true;//设置提升机上有物体
                this.transform.parent = inUpMechine.transform;//提升机上箱子父对象为提升机

                //获取目标层动力站台上光电检测装置实例，提升机层数，判断是否检测到物体，提升机层数是否为同一层

                judgeRayDevice(inUpMechine.lifterDestination);
                if (inUpMechine.isEndLocation)//若到达目标层 &&inUpMechine.TaskStatus=="inTask"s
                {

                    if (shippershoot3 != null)
                    {
                        //print(shippershoot3.name);
                        if (shippershoot3.ShootCaseState)//下一个光电检测是否检测到箱子
                        {

                            //print("123");
                            break;
                        }
                        else//目标层光电未检测到箱子
                        {
                            //print("234");
                            MoveFoward();
                        }
                    }


                }
                break;
            case "RayStationLayer01_R_02":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("RayStationLayer01_R_01").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveBack();
                }
                break;
            case "RayStationLayer01_R_01":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("RayStationLayer01_R_02").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveBack();
                }
                break;
            case "OrderShipperRay0013":

                break;

            default:
                RayDeviceNum = null;
                break;
        }
    }

    /// <summary>
    /// 入库站台光电检测逻辑
    /// </summary>
    /// <param name="RayDeviceNum"></param>
    private void InStationShipperSwitch(string RayDeviceNum)
    {
        switch (RayDeviceNum)
        {
            case "RayStationLayer01_L_01":
                this.transform.parent = GameObject.Find("Carton").transform;
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("RayStationLayer01_L_02").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;
            //当目前光电检测到箱子的名称与提升机上名字相同，更改提升机上箱子为空
            case "RayStationLayer02_L_01":
                this.transform.parent = GameObject.Find("Carton").transform;
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("RayStationLayer02_L_02").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;

            case "RayStationLayer03_L_01":
                this.transform.parent = GameObject.Find("Carton").transform;
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("RayStationLayer03_L_02").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;
            case "RayStationLayer04_L_01":
                this.transform.parent = GameObject.Find("Carton").transform;
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("RayStationLayer04_L_02").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;
            case "RayStationLayer05_L_01":
                this.transform.parent = GameObject.Find("Carton").transform;
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("RayStationLayer05_L_02").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;
            case "RayStationLayer06_L_01":
                this.transform.parent = GameObject.Find("Carton").transform;
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("RayStationLayer06_L_02").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;
            case "RayStationLayer07_L_01":
                this.transform.parent = GameObject.Find("Carton").transform;
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("RayStationLayer07_L_02").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;
        }
    }

    /// <summary>
    /// 出库输送光电检测逻辑
    /// </summary>
    /// <param name="RayDeviceNum"></param>
    private void InShipperSwitch(string RayDeviceNum)
    {
        switch (RayDeviceNum)
        {
            case "InShipperRay001":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay002_01").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;

            case "InShipperRay002_01":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay002").GetComponent<ShipperShoot>();

                //入库输送正常逻辑
                if (!shippershoot.ShootCaseState)//若下一个光电检测没有检测到箱子
                {
                    MoveFoward();
                }
                break;

            case "InShipperRay002":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay003").GetComponent<ShipperShoot>();
                shippershoot2 = GameObject.Find("Select_RayShoot008").GetComponent<ShipperShoot>();

                //if (shippershoot2.ShootCaseState)//并行插口处光电二检测到箱子,则设置光电二检测状态阻塞
                //{

                //    shippershoot2.Intflag = false;

                //}

                //入库输送正常逻辑
                if (!shippershoot.ShootCaseState)//若下一个光电检测没有检测到箱子
                {
                    MoveFoward();
                }
                break;

            case "InShipperRay003":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay004").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;
            case "InShipperRay004":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay005").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;
            case "InShipperRay005":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay006").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;
            case "InShipperRay006":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay007").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;
            case "InShipperRay007":
                //获取下一个光电检测装置实例，判断是否检测到物体


                shippershoot = GameObject.Find("InShipperRay008").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    //MoveBack();
                    target = GameObject.Find("Select_In_target_001");
                    //print(target.transform.position);
                    if (transform.position == target.transform.position)
                    {
                        //print("箱子已到达射线检测"+ RayDeviceNum);
                        RayDeviceNum = null;

                    }
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                }
                break;
            case "InShipperRay008":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay009").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    //print("InShipperRay009检测到箱子");
                    break;
                }
                else
                {
                    MoveRight();
                }
                break;
            case "InShipperRay009":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay010").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {

                    break;
                }
                else
                {
                    MoveRight();
                }
                break;
            case "InShipperRay010":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay011").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveRight();
                }
                break;
            case "InShipperRay011":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay012").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveRight();
                }
                break;
            case "InShipperRay012":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay013").GetComponent<ShipperShoot>();
                //print(shippershoot.ShootCaseState);
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    //MoveBack();

                    target = GameObject.Find("Select_In_target_002");
                    //print(target.transform.position);
                    if (transform.position == target.transform.position)
                    {
                        //print("箱子已到达射线检测"+ RayDeviceNum);
                        RayDeviceNum = null;

                    }
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                }
                break;

            case "InShipperRay013":
                //获取下一个光电检测装置实例，判断是否检测到物体
                //print("InShipperRay013设备处理方法");
                shippershoot = GameObject.Find("InShipperRay014").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;

            case "InShipperRay014":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("InShipperRay015").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveFoward();
                }
                break;

            case "InShipperRay015"://此处为最后入库输送光电检测装置编号

                //获取提升机上光电检测装置实例，提升机层数，提升机任务状态，判断是否检测到物体，提升机层数是否为一层

                shippershoot = GameObject.Find("InUpRayDevice").GetComponent<ShipperShoot>();
                //print(shippershoot.ShootCaseState);
                //获取提升机当前层数
                inUpMechine = InUpMechine.GetComponent<InUpMechine>();
                //print(inUpMechine.TaskStatus.Equals("taskCompleted"));
                if (inUpMechine.TaskStatus.Equals("taskCompleted") && inUpMechine.currentLocation == 0)
                {
                    //print(shippershoot.ShootCaseState);
                    if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                    {
                        //print(shippershoot.ShootCaseState);
                        //inUpMechine.TaskStatus = "inTask";
                        break;
                    }
                    else
                    {
                        MoveFoward();
                    }
                }

                break;
        }

    }

    /// <summary>
    /// 出库输送光电检测逻辑
    /// </summary>
    /// <param name="RayDeviceNum"></param>
    private void OutShipperSwitch(string RayDeviceNum)
    {
        switch (RayDeviceNum)
        {
            case "OutShipperRay001":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OutShipperRay002").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveBack();
                }
                break;
            case "OutShipperRay002":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OutShipperRay003").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveBack();
                }
                break;
            case "OutShipperRay003":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OutShipperRay004").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveBack();
                }
                break;
            case "OutShipperRay004":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OutShipperRay005").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveBack();
                }
                break;
            case "OutShipperRay005":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OutShipperRay006").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveBack();
                }
                break;
            case "OutShipperRay006":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OutShipperRay007").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveBack();
                }
                break;
            case "OutShipperRay007":
                //获取下一个光电检测装置实例，判断是否检测到物体


                shippershoot = GameObject.Find("Select_RayShoot001").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    //MoveBack();
                    target = GameObject.Find("Select_Out_target_001");//位置纠正
                    if (transform.position == target.transform.position)
                    {
                        RayDeviceNum = null;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                }
                break;
        }

    }

    /// <summary>
    /// 拣选输送出库检测逻辑
    /// </summary>
    /// <param name="RayDeviceNum"></param>
    private void SelectShipperSwitch(string RayDeviceNum)
    {
        switch (RayDeviceNum)
        {
            case "Select_RayShoot001":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("Select_RayShoot002").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();
                }
                break;
            case "Select_RayShoot002":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("Select_RayShoot003").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();
                }
                break;
            case "Select_RayShoot003":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("Select_RayShoot004").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();
                }
                break;
            case "Select_RayShoot004":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("Select_RayShoot005").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();
                }
                break;
            case "Select_RayShoot005":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("Select_RayShoot006").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();
                }
                break;
            case "Select_RayShoot006":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("Select_RayShoot007").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();
                }
                break;
            case "Select_RayShoot007":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("Select_RayShoot008").GetComponent<ShipperShoot>();

                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();
                }
                break;
            case "Select_RayShoot008":
                //获取下一个光电检测装置实例，判断是否检测到物体

                CurrentShipperRay= GameObject.Find(RayDeviceNum).GetComponent<ShipperShoot>();//下一个光电
                shippershoot = GameObject.Find("InShipperRay003").GetComponent<ShipperShoot>();//下一个光电
                //获取并行光电装置实例，判断是否检查到物体
                //shippershoot2 = GameObject.Find("InShipperRay002").GetComponent<ShipperShoot>();//并行光电
                //print("光电InShipperRay002的标志位为"+ shippershoot2.Intflag);
                //if (this.CurrentShipperRay.Intflag)//并行处光电检测到箱子,则阻塞当前状态
                //{
                    if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                    {
                        RayDeviceNum = "InShipperRay003";
                        //break;
                    }
                    else
                    {

                        target = GameObject.Find("Select_Out_target_002");
                        //print(target.transform.position);
                        if ((transform.position.x - target.transform.position.x )<0.3)
                        {
                            print("箱子已到达射线检测" + RayDeviceNum);
                            

                        }else { transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed); }
                        

                    }
                //}
               

                break;
            case "Select_RayShoot009":
                //获取下一个光电检测装置实例，判断是否检测到物体

                break;

        }
    }

    /// <summary>
    /// 订单箱输送光电检测逻辑
    /// </summary>
    /// <param name="RayDeviceNum"></param>
    private void OrderShipperSwitch(string RayDeviceNum)
    {
        switch (RayDeviceNum)
        {
            case "OrderShipperRay001":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay002").GetComponent<ShipperShoot>();
                //print(shippershoot.ShootCaseState);
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {

                    //target = GameObject.Find("OrderShipper_target_Ray002");
                    //if (transform.position == target.transform.position)
                    //{
                    //    RayDeviceNum = null;
                    //}
                    MoveLeft();
                    //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                }
                break;
            case "OrderShipperRay002":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay003").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();

                }
                break;
            case "OrderShipperRay003":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay004").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();

                }
                break;
            case "OrderShipperRay004":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay005").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();

                }
                break;
            case "OrderShipperRay005":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay006").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();

                }
                break;
            case "OrderShipperRay006":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay007").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();

                }
                break;
            case "OrderShipperRay007":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay008").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();

                }
                break;

            case "OrderShipperRay008":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay009").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();

                }
                break;
            case "OrderShipperRay009":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay010").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();

                }
                break;
            case "OrderShipperRay010":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay011").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();

                }
                break;
            case "OrderShipperRay011":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay012").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();

                }
                break;
            case "OrderShipperRay012":
                //获取下一个光电检测装置实例，判断是否检测到物体
                shippershoot = GameObject.Find("OrderShipperRay013").GetComponent<ShipperShoot>();
                if (shippershoot.ShootCaseState)//判断下一个光电检测是否检测到箱子
                {
                    break;
                }
                else
                {
                    MoveLeft();

                }
                break;
        }
    }

    /// <summary>
    /// 判断是哪个目标层光电
    /// </summary>
    /// <param name="lifterDestination"></param>
    public void judgeRayDevice(int lifterDestination)
    {
        switch (lifterDestination)
        {
            case 1:
                shippershoot3 = GameObject.Find("RayStationLayer01_L_01").GetComponent<ShipperShoot>();
                //print("juge方法"+shippershoot.ShootCaseState);
                break;
            case 2:
                shippershoot3 = GameObject.Find("RayStationLayer02_L_01").GetComponent<ShipperShoot>();
                break;
            case 3:
                shippershoot3 = GameObject.Find("RayStationLayer03_L_01").GetComponent<ShipperShoot>();
                break;
            case 4:
                shippershoot3 = GameObject.Find("RayStationLayer04_L_01").GetComponent<ShipperShoot>();
                break;
            case 5:
                shippershoot3 = GameObject.Find("RayStationLayer05_L_01").GetComponent<ShipperShoot>();
                break;
            case 6:
                shippershoot3 = GameObject.Find("RayStationLayer06_L_01").GetComponent<ShipperShoot>();
                break;
            case 7:
                shippershoot3 = GameObject.Find("RayStationLayer07_L_01").GetComponent<ShipperShoot>();
                break;

        }
        //if (shippershoot3 != null) { print("123" + shippershoot3.name); }
       
    }


    public void MoveLeft()//箱子向左移动
    {
        transform.Translate(-Vector3.right * moveSpeed * Time.deltaTime, Space.World);
    }
    public void MoveRight()//箱子向右移动
    {
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
    }
    public void MoveFoward()//箱子向前移动
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
    }
    public void MoveBack()//箱子向后移动
    {
        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
    }

    //箱子对象消除的处理函数
    public void DestroyCase()
    {
        collider.enabled = false;           //禁用箱子的collider组件，使其不会与其他物体发生碰撞
        rigidbody.useGravity = false;       //因为箱子的collider组件被禁用，箱子会因重力穿过地形系统下落，取消箱子受到的重力可以避免该现象
        Destroy(gameObject, 3.0f);          //3秒后删除箱子对象
    }

}
