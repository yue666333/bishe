using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InUpMechine : MonoBehaviour
{

    /// <summary>
    /// 入库提升机的相关数据库属性（需要显示的相关信息）在此处定义
    ///</summary>
    //当前入库提升机编号
    public int InUpMechineNum;
    //主任务号
    //public int[] MainCommandNum;
    //子任务号
    //public int[] minorCommandNum;

    //起始层
    //public GameObject startLocation;
    //目的层
    //public GameObject endLocation;

    //计划数 //写入数//完成数    //条码

    //任务状态
    public bool isCompleted;
    //提升机运行状态枚举
    public enum lifterStatus { alarm , normal }
    //提升机任务状态枚举
    public enum lifterTaskStatus { inTask,taskCompleted}
    //接受任务到达任务层，正在运行中，到达目标层且站台无箱子，到达目标层前方站台有箱子，一次任务完成，任务占用中
    //public string sLiterTaskStatus;
    public int inUpMechineLayer;//提升机当前层，通过update函数实时监测当前层,判断提升机状态运行中，任务占用
    public string TaskStatus;//提升机任务状态:任务完成，任务进行中
    public string LifterStatus;//提升机当前运行状态
    public int currentLocation;//提升机当前层
    public ShipperShoot shippershoot;//光电检测装置的类
    public string caseinfoName;//目标层光电检测到的箱子名称
    public string CurrentTaskCaseInfo;//保存当前任务箱子
    //目的货位
    //public string destination;

    ///<summary>
    /// unity中提升机的属性在此处定义
    /// </summary>
    public AudioClip beShootAudio_lifter;    //提升机被射线击中音效
    //private Collider collider;          //提升机的Collider组件
    private Rigidbody rigidbody;        //提升机的rigidbody组件
    public GameObject target;       //提升机移动的目标位置对象
    public GameObject LifterInfoCanvas; //提升机设备信息Canvas
    public int lifterDestination=0;//设置提升机目标层
    public bool isOnCase;//判断提升机上光电是否检测到物体
    public bool isEndLocation;//判断是否达到目的地 
    private bool flag;//只运行一次的标志位
    public GameObject onCase;  //提升机上的箱子
    public float moveSpeed = 8.0f;  //提升机的移动速度

    //本部分为入库提升机设备UI界面控件变量
    public Text DeviceNameText;//提升机UI界面设备名

    //初始化，获取提升机的组件
    void Start()
    {
        caseinfoName = null;
        isOnCase = false;
        isEndLocation = false;
        lifterDestination = 0;
        TaskStatus = lifterTaskStatus.taskCompleted.ToString();
        currentLocation = 0;
        //collider = GetComponent<Collider>();    //获取提升机的Collider组件
        rigidbody = GetComponent<Rigidbody>();  //获取提升机的Rigidbody组件
        //LifterInfoCanvas.SetActive(false);       //提升机的Canvas设置为不可用
        //target.transform.TransformVector(0, 0, 0); //初始化target对象三维地址
        target = GameObject.Find("stationlayer01"); //随机设置一个目标位置
    }


    /// <summary>
    /// 项目运行中提升机的相关处理方法在此处定义
    /// </summary>
    //每帧执行一次，检测提升机的各种状态并执行相应操作

    //定义一个每一步检测变量caseStep来表示当前执行的步骤
    private int caseStep = 0;

    private void FixedUpdate()
    {
        if (lifterDestination >= 0 && lifterDestination <= 7)
        {
            lifterMove(lifterDestination);
        }
        detectCurrentLayer();//判断提升机当前层，若达到目标层，则设置目标层为0
        judgeTaskStatus();//判断提升机当前状态，根据不同状态执行不同逻辑
        //judgeEndLayerCaseName(lifterDestination);//判断目标层站台上光电检测到的箱子的名称
        //lifterMove(lifterDestination);
    }
    void Update()
    {
        
    }

    /// <summary>
    /// 判断目标层站台上光电检测到的箱子的名称
    /// </summary>
    //public void judgeEndLayerCaseName(int lifterDestination)
    //{
    //    judgeRayDevice(lifterDestination);
    //    if (shippershoot.ShootCaseState) {
    //        shippershoot.onCaseName = shippershoot.hitInfo.transform.gameObject.GetComponent<CaseInfo>().name;
    //    }

    //}



    /// <summary>
    /// 入库提升机移动方法
    /// </summary>
    /// <param name="destination"></param>
    public void lifterMove(int destination)//destination 要到达的目标层，用数字1、2、3等表示
    {
        //transform.TransformPoint( 9.129997, 12.5, 20.03 )

        //根据不同的步骤状态执行相应箱子操作（共7个目标位置代表7层）
        switch (destination)
            {
            case 0:
                target = GameObject.Find("target000_inLifter");
                if (transform.position != target.transform.position)
                {
                    //TaskStatus = lifterTaskStatus.inTask.ToString();
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    isEndLocation = false;
                }
                else
                {
                    LifterStatus = lifterTaskStatus.taskCompleted.ToString();
                    currentLocation = 0;
                    isEndLocation = true;

                    //print(currentLocation);
                }

                break;            
                case 1:
                target = GameObject.Find("target001_inLifter");
                if (transform.position != target.transform.position)
                {
                    //TaskStatus = lifterTaskStatus.inTask.ToString();
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    isEndLocation = false;
                }
                else
                {
                    LifterStatus = lifterTaskStatus.taskCompleted.ToString();
                    currentLocation = 1;
                    isEndLocation = true;
                    
                    //print(currentLocation);
                }
                break;
                case 2:
                target = GameObject.Find("target002_inLifter");
                if (transform.position != target.transform.position)
                {
                                      
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    isEndLocation = false;
                }
                else
                {
                    LifterStatus = lifterTaskStatus.taskCompleted.ToString();
                    currentLocation = 2;
                    isEndLocation = true;
                }
                    break;
                case 3:
                target = GameObject.Find("target003_inLifter");
                if (transform.position != target.transform.position)
                {
                   
                    
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    isEndLocation = false;
                }
                else
                {
                    LifterStatus = lifterTaskStatus.taskCompleted.ToString();
                    currentLocation = 3;
                    isEndLocation = true;
                }
                    break;
                case 4:
                target = GameObject.Find("target004_inLifter");
                if (transform.position != target.transform.position)
                {
                    
                    //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    isEndLocation = false;
                }
                else
                {
                    LifterStatus = lifterTaskStatus.taskCompleted.ToString();
                    currentLocation = 4;
                    isEndLocation = true;
                }
                    break;
                case 5:
                target = GameObject.Find("target005_inLifter");
                if (transform.position != target.transform.position)
                {
                    
                    //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    isEndLocation = false;
                }
                else
                {
                    LifterStatus = lifterTaskStatus.taskCompleted.ToString();
                    currentLocation = 5;
                    isEndLocation = true;
                }
                    break;
                case 6:
                target = GameObject.Find("target006_inLifter");
                if (transform.position != target.transform.position)
                {
                    
                    //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    isEndLocation = false;
                }
                else
                {
                    LifterStatus = lifterTaskStatus.taskCompleted.ToString();
                    currentLocation = 6;
                    isEndLocation = true;
                }
                break;
                case 7:
                target = GameObject.Find("target007_inLifter");
                if (transform.position != target.transform.position)
                {
                    
                    //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                    transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    isEndLocation = false;
                }
                else
                {
                    LifterStatus = lifterTaskStatus.taskCompleted.ToString();
                    currentLocation = 7;
                    isEndLocation = true;
                }
                break;
        }

            
        }

    /// <summary>
    /// 判断提升机当前层,若达到目标层，则设置目标层为0
    /// </summary>
    public void detectCurrentLayer()
    {
        //if (isEndLocation) { lifterDestination = 0; }
        if (System.Math.Abs((transform.position.y - GameObject.Find("target000_inLifter").transform.position.y)) < 0.3) { currentLocation = 0; }
        if (System.Math.Abs((transform.position.y - GameObject.Find("target001_inLifter").transform.position.y))<0.3) { currentLocation = 1; }
        if (System.Math.Abs((transform.position.y - GameObject.Find("target002_inLifter").transform.position.y)) < 0.3) { currentLocation = 2; }
        if (System.Math.Abs((transform.position.y - GameObject.Find("target003_inLifter").transform.position.y)) < 0.3) { currentLocation = 3; }
        if (System.Math.Abs((transform.position.y - GameObject.Find("target004_inLifter").transform.position.y)) < 0.3) { currentLocation = 4; }
        if (System.Math.Abs((transform.position.y - GameObject.Find("target005_inLifter").transform.position.y)) < 0.3) { currentLocation = 5; }
        if (System.Math.Abs((transform.position.y - GameObject.Find("target006_inLifter").transform.position.y)) < 0.3) { currentLocation = 6; }
        if (System.Math.Abs((transform.position.y - GameObject.Find("target007_inLifter").transform.position.y)) < 0.3) { currentLocation = 7; }

    }

    /// <summary>
    /// 判断提升机任务状态
    /// </summary>
    public void judgeTaskStatus()
    {
        //如果提升机上箱子为空，则设置任务完成
        if (transform.Find(CurrentTaskCaseInfo)==null)
        {
            TaskStatus = lifterTaskStatus.taskCompleted.ToString();
        }
        //1.开始时提升机任务为已完成，此时提升机上无箱子，若提升机不在一层，则提升机移动到一层接箱子 ok
        if (TaskStatus == lifterTaskStatus.taskCompleted.ToString() && this.isOnCase == false)
        {
            CurrentTaskCaseInfo = "1234567";
            lifterDestination = 0;

            //print("CurrentTaskCaseInfo 设置为null任务完成");
        }

        if (TaskStatus == lifterTaskStatus.taskCompleted.ToString() && this.isOnCase == true)
        {
            //if (flag) { CurrentTaskCaseInfo = onCase.name; flag=false; }
            CurrentTaskCaseInfo = onCase.name;
            //print("任务完成");
        }
        //print(this.isOnCase);
        //if (currentLocation == lifterDestination)//如果当前层到达目标层
        //{
        //    judgeRayDevice(lifterDestination);//根据目标层获取目标层的第一个光电检测ShipperShoot类
        //    //print(shippershoot.ShootCaseState);
        //    if (shippershoot.ShootCaseState && this.isOnCase == true)//到达目标层，目标层站台有箱子,并且提升机上有箱子
        //    {
        //        //TaskStatus=lifterTaskStatus.inTask.ToString();
        //        print("到达目标层，目标层站台有箱子,并且提升机上有箱子");
        //    }
        //    if (shippershoot.ShootCaseState && this.isOnCase == false) {
        //        //TaskStatus = lifterTaskStatus.taskCompleted.ToString();
        //    }
        //    if(!shippershoot.ShootCaseState)//如果到达目标层,目标站台无箱子
        //    {
        //        //print(this.isOnCase == true);
        //        print("到达目标层，目标层站台无箱子");
        //        if (TaskStatus.Equals(lifterTaskStatus.inTask.ToString())&&this.isOnCase==true)//提升机上有箱子，且提升机状态为任务占用，箱子移动
        //        {
        //            //print("TaskStatus");
        //            //提升机上箱子移动,知道下一个站台检测到箱子且提升机上无箱子
        //            //if (shippershoot.ShootCaseState)
        //            //{
        //                //onCase.GetComponent<CaseInfo>().InMove = true;
        //            //}

        //            //TaskStatus = lifterTaskStatus.taskCompleted.ToString();
        //        }
        //        if (TaskStatus.Equals(lifterTaskStatus.inTask.ToString()) && this.isOnCase == false)//提升机上无箱子，且为任务占用，
        //        {
        //            //print("TaskStatus");
        //            TaskStatus = lifterTaskStatus.taskCompleted.ToString();
        //        }
        //    }
        //}
        //else
        //{
        //    TaskStatus = lifterTaskStatus.inTask.ToString();
        //}
        //if (this.isOnCase == true)
        //{
        //    TaskStatus = lifterTaskStatus.inTask.ToString();
        //}
        ////print(TaskStatus);


        ////判断提升机上是否有箱子
        //if (onCase == null)
        //{

        //    isOnCase = false;
        //    //print(isOnCase);
        //}
        //else
        //{
        //    isOnCase = true;
        //}
        if (true)//如果当前层到达目标层则执行下列
        {

            //判断变量：提升机任务状态，提升机上是否有箱子，目标站台是否有箱子，提升机是否在目标层 ok
            //出现的几种情况：

            //若提升机在一层，则箱子从输送移动到提升机(RayDevice控制caseInfo下InShipperRay015)，（转2） ok

            //2.若箱子从输送移动到提升机，提升机上光电检测到箱子，此时更改提升机状态为任务占用状态（任务占用状态提升机需等待目标层参数） ok

            //3.任务占用状态下，（提升机上光电检测到箱子），箱子到达目标层，目标层光电无东西，箱子foward  ok

            //到达目标层，目标光电无物体，箱子移动，首先提升机光电检测到无物体，然后目标站台光电检测到物体
            //提升机到达且能找到子对象casename,则提升机在任务，若提升机找不到子对象箱子，则任务成功
            //transform.Find(onCase.name)!=null
            judgeRayDevice(lifterDestination, ref shippershoot);
            //if(shippershoot!=null) { print("tisheng" + shippershoot.name); }

            if (this.TaskStatus == lifterTaskStatus.inTask.ToString())
            {//到达目标层，且任务占用状态，当前箱子找不到子对象且提升机上无箱子
                //if (shippershoot.onCaseName != null) { }
                //print(CurrentTaskCaseInfo);
                if (!CurrentTaskCaseInfo.Equals("1234567"))
                {
                    //如果当前任务箱子不为空
                    if (transform.Find(CurrentTaskCaseInfo) != null)// ==shippershoot.onCaseName   //当前提升机上有箱子
                    {

                        //print("当前提升机箱子" + onCase.name + "目标站台箱子");//+ shippershoot.onCaseName
                        //this.TaskStatus = lifterTaskStatus.inTask.ToString();
                        //print("1.任务进行中");
                    }
                    if (transform.Find(CurrentTaskCaseInfo) == null)//当前提升机上无箱子
                    {
                        //print("inup作用只执行一次");
                        this.TaskStatus = lifterTaskStatus.taskCompleted.ToString();
                        //lifterDestination = 1;
                        isOnCase = false;
                    }


                }

            }
            if (this.TaskStatus == lifterTaskStatus.taskCompleted.ToString())
            {
                //print(CurrentTaskCaseInfo+"");
                //如果当前任务箱子不为空
                if (!CurrentTaskCaseInfo.Equals("1234567"))// transform.Find(CurrentTaskCaseInfo) != null ==shippershoot.onCaseName   //当前提升机上有箱子
                {

                    //print("当前提升机箱子" + onCase.name + "目标站台箱子");//+ shippershoot.onCaseName
                    //this.TaskStatus = lifterTaskStatus.inTask.ToString();
                    //print("1.任务进行中");
                }
                if (transform.Find(CurrentTaskCaseInfo) == null)//当前提升机上无箱子
                {
                    //print("inup作用只执行一次");
                    this.TaskStatus = lifterTaskStatus.taskCompleted.ToString();
                    //lifterDestination = 1;
                    isOnCase = false;
                }

                
            }
        }
    }

    /// <summary>
    /// 该方法根据目标层获取目标层站台上光电检测类
    /// </summary>
    /// <param name="lifterDestination"></param>
    public void judgeRayDevice(int lifterDestination, ref ShipperShoot shippershoot1)
    {
        switch (lifterDestination)
        {
            case 1:
                shippershoot1 = GameObject.Find("RayStationLayer01_L_01").GetComponent<ShipperShoot>();
                //print("juge方法"+shippershoot.ShootCaseState);
                break;
            case 2:
                shippershoot1 = GameObject.Find("RayStationLayer02_L_01").GetComponent<ShipperShoot>();
                break;
            case 3:
                shippershoot1 = GameObject.Find("RayStationLayer03_L_01").GetComponent<ShipperShoot>();
                break;
            case 4:
                shippershoot1 = GameObject.Find("RayStationLayer04_L_01").GetComponent<ShipperShoot>();
                break;
            case 5:
                shippershoot1 = GameObject.Find("RayStationLayer05_L_01").GetComponent<ShipperShoot>();
                break;
            case 6:
                shippershoot1 = GameObject.Find("RayStationLayer06_L_01").GetComponent<ShipperShoot>();
                break;
            case 7:
                shippershoot1 = GameObject.Find("RayStationLayer07_L_01").GetComponent<ShipperShoot>();
                break;
            default:


                break;

        }
    }

    
    /// <summary>
    /// 出库提升机移动方法
    /// </summary>
    /// <param name="destination"></param>
    public void lifterMove2(int destination)//destination 要到达的目标层，用数字1、2、3等表示
    {
        //transform.TransformPoint( 9.129997, 12.5, 20.03 )

        //根据不同的步骤状态执行相应箱子操作（共7个目标位置代表7层）
        switch (destination)
        {
            case 0:
                if (transform.position != target.transform.position) { 

                target = GameObject.Find("target000_outLifter");
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                    //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                }else
                {
                    currentLocation = 1;
                    lifterDestination = 0;
                }
                break;

            case 1:
                //if (transform.position != target.transform.position)
                //{
                target = GameObject.Find("target001_outLifter");
                //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                //}
                break;
            case 2:
                //if (transform.position != target.transform.position)
                //{
                target = GameObject.Find("target002_outLifter");
                //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                //}
                break;
            case 3:
                //if (transform.position != target.transform.position)
                //{
                target = GameObject.Find("target003_outLifter");
                //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                //}
                break;
            case 4:
                //if (transform.position != target.transform.position)
                //{
                target = GameObject.Find("target004_outLifter");
                //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                //}
                break;
            case 5:
                //if (transform.position != target.transform.position)
                //{
                target = GameObject.Find("target005_outLifter");
                //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                //}
                break;
            case 6:
                //if (transform.position != target.transform.position)
                //{
                target = GameObject.Find("target006_outLifter");
                //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                //}
                break;
            case 7:
                //if (transform.position != target.transform.position)
                //{
                target = GameObject.Find("target007_outLifter");
                //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                //}
                break;
        }


    }



    /// <summary>
    /// 提升机被射线击中时函数，用于ObserverShoot脚本中调用,当前函数主要功能为调用GUI控件显示
    /// </summary>
    public void beShoot()
    {
        //调用相关GUI控件逻辑显示箱子信息
        LifterInfoCanvas.SetActive(true);

        DeviceNameText.text =  "设备信息： 入库提升机 "+InUpMechineNum.ToString();

        //在入库提升机位置处播放被击中音效
        if (beShootAudio_lifter != null)
            AudioSource.PlayClipAtPoint(beShootAudio_lifter, transform.position);

        if (true)
        {
            //当箱子信息与实际信息不符时，可改变项目运行状态
            if (ProjectManager.PM.projectState == ProjectManager.ProjectManagerState.Playing)
            {
                //根据判断条件改变项目状态

            }


        }
    }
}