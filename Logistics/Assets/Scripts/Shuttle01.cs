using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// 本方法主要是穿梭车的控制方法，包括移动，取放货，接受数据执行命令，光电检测（检查箱子是否为子对象）
/// </summary>
public class Shuttle01 : MonoBehaviour
{

    /// <summary>
    /// 穿梭车的相关数据库属性（需要显示的相关信息）在此处定义
    ///</summary>
    //当前穿梭车编号
    public int ShuttleNum;
    public string ShuttleName;
    //主任务号
    public int[] MainCommandNum;
    //子任务号
    public int[] minorCommandNum;

    //起始层
    //public GameObject startLocation;
    //目的层
    //public GameObject endLocation;

    //计划数 //写入数//完成数    //条码

    
    //穿梭车状态枚举
    public enum ShuttleStatus { alarm, normal }
    public enum ShuttleTaskStatus { inTask, taskCompleted}

    ShipperShoot shipperShoot;//小车上中央的射线检测

    /// <summary>
    /// UI面板相关属性在此处命名
    /// </summary>
    public int CurrentRow;//当前穿梭车排
    public int CurrentLayer;//当前穿梭车层
    public int endRow;//穿梭车目标排
    public bool isEndRow;//穿梭车是否到达目标排（任务状态）
    public string deviceStatus;//穿梭车设备状态(正常，报警，手动)
    public string taskStatus;//穿梭车任务状态（完成，进行中）

    //穿梭车上的三个光电检测状态（是否检测到箱子）
    public bool leftDetected;
    public bool middleDetected;
    public bool rightDetected;
    //小车取货标志位
    public bool leftTake;
    public bool rightTake;
    public bool leftPlace;
    public bool rightPlace;


    //public int ShuttleDestination = 0;//设置穿梭车目标排

    //目的货位
    //public string destination;

    ///<summary>
    /// unity中穿梭车的属性在此处定义
    /// </summary>
    public AudioClip beShootAudio_lifter;    //穿梭车被射线击中音效

    private Collider collider;          //穿梭车的Collider组件
    private Rigidbody rigidbody;        //穿梭车的rigidbody组件
    public GameObject target;       //穿梭车移动的目标位置对象

    public GameObject ShuttleInfoCanvas; //穿梭车设备信息Canvas


    public GameObject onLeftCase;  //穿梭车左边的箱子
    public GameObject onRightCase;//穿梭车右边的箱子

    public GameObject onCase;//穿梭车中间的箱子

    public string onLeftCaseName;//穿梭车左边箱子名称
    public string onRightCaseName;//穿梭车右边箱子名称
    

    public bool isOnCase;//穿梭车上是否有箱子
    public float moveSpeed = 8.0f;  //穿梭车的移动速度
    private float process;
    //本部分为穿梭车设备UI界面控件变量
    public Text DeviceNameText;//穿梭车UI界面设备名

    //初始化，获取穿梭车的组件
    void Start()
    {
        leftDetected = false;
        middleDetected = false;
        rightDetected = false;
        leftTake = false;
        rightTake = false;
        leftPlace = false;
        rightPlace = false;
        deviceStatus = ShuttleStatus.normal.ToString();
        shipperShoot = GameObject.Find("Shuttle01_M_Ray").GetComponent<ShipperShoot>();
        endRow = 0;
        isEndRow = false;
        collider = GetComponent<Collider>();    //获取穿梭车的Collider组件
        rigidbody = GetComponent<Rigidbody>();  //获取穿梭车的Rigidbody组件
        //ShuttleInfoCanvas.SetActive(false);       //穿梭车的Canvas设置为不可用
        //target.transform.TransformVector(0, 0, 0); //初始化target对象三维地址
        target = GameObject.Find("stationlayer01"); //随机设置一个目标位置
    }


    /// <summary>
    /// 项目运行中穿梭车的相关处理方法在此处定义
    /// </summary>
    //每帧执行一次，检测穿梭车的各种状态并执行相应操作

    //定义一个每一步检测变量Step来表示当前执行的步骤
    private int Step = 0;

    private void FixedUpdate()
    {
        //ShuttleMove(lifterDestination);
        detectCurrentRow();
        ShuttleMove(endRow);
        detectCurrentStatus();
    }

    private void detectCurrentStatus()
    {
        if (leftDetected)
        {
            if (leftTake)
            {
                TakeGoodsFromLeft();
            }
            
        }
        if(rightDetected)
        {//onRightCase!=null
            if (rightTake)
            {
                //print("123");
                TakeGoodsFromRight();
            }
        }
        if (middleDetected)
        {
            if (leftPlace)
            {
                PlaceGoodsToLeft();
            }
        }
        if (middleDetected)
        {
            if (rightPlace)
            {
                PlaceGoodsToRight();
            }
        }

    }

    void Update()
    {

        
        //ShuttleMove(1);

        //if (ShuttleDestination >= 0 && ShuttleDestination <= 7)
        //{
        //    ShuttleMove(ShuttleDestination);
        //}

    }

    public void TakeGoodsFromLeft()
    {
        target = GameObject.Find("CartonShuttle01Location");

        if (this.leftTake)
        {
            if (!shipperShoot.ShootCaseState)//transform.position != target.transform.position
            {
                //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                if (onLeftCase!=null) { onLeftCase.GetComponent<CaseInfo>().MoveRight(); }
                
                //transform.position = Vector3.Lerp(transform.position, 5f, process); onCase

                //this.leftTake = false;
            }
            else
            {
                this.rightTake = false;
            }
        }

    }
    public void PlaceGoodsToLeft()
    {
        target = findLeftTarget(CurrentRow);
        if (onCase!=null)
        {
            //onCase.transform.position = Vector3.MoveTowards(onCase.transform.position, target.transform.position, Time.deltaTime * moveSpeed);
            onCase.transform.position = target.transform.position;
        }
        else { leftPlace = false; }
        //}else
        //{
        //    onCase.transform.parent = GameObject.Find("Carton").transform;
        //}
    }

    private GameObject findLeftTarget(int CurrentRow)//返回每层放货目标
    {
        switch (CurrentRow)
        {
            case 0:
                //target = GameObject.Find("target_layer01_R_0");
                break;
            case 1:
                target = GameObject.Find("target_layer01_L_1");
                //target = GameObject.Find("target_layer01_R_0");
                return target;
                break;
            case 2:
                target = GameObject.Find("target_layer01_L_2");
                return target;
                break;
            case 3:
                target = GameObject.Find("target_layer01_L_3");
                return target;
                break;
            case 4:
                target = GameObject.Find("target_layer01_L_4");
                return target;
                break;
            case 5:
                target = GameObject.Find("target_layer01_L_5");
                return target;
                break;
            case 6:
                target = GameObject.Find("target_layer01_L_6");
                return target;
                break;
            case 7:
                target = GameObject.Find("target_layer01_L_7");
                return target;
                break;

        }
        return target;

    }

    public void TakeGoodsFromRight()
    {
        //Vector3 dest = new Vector3(this.transform.position.x + 1.2f, this.transform.position.y, 0);
        //取货逻辑：该箱子移动到穿梭车目标位置，并且成为穿梭车子对象
        target = GameObject.Find("CartonShuttle01Location");

        if (this.rightTake)
        {
            if (!shipperShoot.ShootCaseState)//如果没有击中箱子
            {
                //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                if (onRightCase != null)
                { onRightCase.GetComponent<CaseInfo>().MoveLeft(); }
            }
            else
            {
                this.rightTake = false;
            }
        }

    }
    public void PlaceGoodsToRight()
    {
        target = findRightTarget(CurrentRow);
        if (onCase != null)
        {
            //onCase.transform.position = Vector3.MoveTowards(onCase.transform.position, target.transform.position, Time.deltaTime * moveSpeed);
            onCase.transform.position = target.transform.position;
        }
        else { rightPlace = false; }
    }

    private GameObject findRightTarget(int currentRow)
    {
        switch (CurrentRow)
        {
            case 0:
                target = GameObject.Find("target_layer01_R_0");
                break;
            case 1:
                target = GameObject.Find("target_layer01_R_1");
                //target = GameObject.Find("target_layer01_R_0");
                return target;
                break;
            case 2:
                target = GameObject.Find("target_layer01_R_2");
                return target;
                break;
            case 3:
                target = GameObject.Find("target_layer01_R_3");
                return target;
                break;
            case 4:
                target = GameObject.Find("target_layer01_R_4");
                return target;
                break;
            case 5:
                target = GameObject.Find("target_layer01_R_5");
                return target;
                break;
            case 6:
                target = GameObject.Find("target_layer01_R_6");
                return target;
                break;
            case 7:
                target = GameObject.Find("target_layer01_R_7");
                return target;
                break;

        }
        return target;
    }

    //一层穿梭车移动  
    public void ShuttleMove(int destination)//destination 要到达的目标层，用数字1、2、3等表示
    {
        //transform.TransformPoint( 9.129997, 12.5, 20.03 )

        //根据不同的步骤状态执行相应箱子操作（共7个目标位置代表7层）
        switch (destination)
        {
            case 0:


                target = GameObject.Find("target000_Shuttle01");
                toTarget();//移动到目标排
                break;

            case 1:
             
                target = GameObject.Find("target001_Shuttle01");

                toTarget();//移动到目标排

                break;
            case 2:
                target = GameObject.Find("target002_Shuttle01");
                toTarget();//移动到目标排
                break;
            case 3:
                target = GameObject.Find("target003_Shuttle01");
                toTarget();//移动到目标排
                break;
            case 4:
                target = GameObject.Find("target004_Shuttle01");
                toTarget();//移动到目标排
                break;
            case 5:
                target = GameObject.Find("target005_Shuttle01");
                toTarget();//移动到目标排
                break;
            case 6:
                target = GameObject.Find("target006_Shuttle01");
                toTarget();//移动到目标排
                break;
            case 7:
                target = GameObject.Find("target007_Shuttle01");
                toTarget();//移动到目标排
                break;

        }


    }

    /// <summary>
    /// 该方法执行移动到目标排的相关逻辑
    /// </summary>
    private void toTarget()
    {
        if (transform.position != target.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
            isEndRow = false;
            taskStatus = ShuttleTaskStatus.inTask.ToString();
        }
        else
        {
            isEndRow = true;
            taskStatus = ShuttleTaskStatus.taskCompleted.ToString();

        }
    }

    //二层穿梭车移动
    public void ShuttleMove2(int destination)//destination 要到达的目标层，用数字1、2、3等表示
    {
        //transform.TransformPoint( 9.129997, 12.5, 20.03 )

        //根据不同的步骤状态执行相应箱子操作（共7个目标位置代表7层）
        switch (destination)
        {
            case 0:
                //if (transform.position != target.transform.position) { 

                target = GameObject.Find("target000_outLifter");
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * moveSpeed);
                //this.transform.Translate(Vector3.Normalize(target.transform.position - transform.position) * (Vector3.Distance(transform.position, target.transform.position) / (10000 * Time.deltaTime)));
                // }
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
    /// 判断穿梭车当前排,若达到目标排
    /// </summary>
    public void detectCurrentRow()
    {
        ////if (isEndLocation) { lifterDestination = 0; }CurrentRow
        if (System.Math.Abs((transform.position.z - GameObject.Find("target000_Shuttle01").transform.position.z)) < 0.3) { CurrentRow = 0; }
        if (System.Math.Abs((transform.position.z - GameObject.Find("target001_Shuttle01").transform.position.z)) < 0.3) { CurrentRow = 1; }
        if (System.Math.Abs((transform.position.z - GameObject.Find("target002_Shuttle01").transform.position.z)) < 0.3) { CurrentRow = 2; }
        if (System.Math.Abs((transform.position.z - GameObject.Find("target003_Shuttle01").transform.position.z)) < 0.3) { CurrentRow = 3; }
        if (System.Math.Abs((transform.position.z - GameObject.Find("target004_Shuttle01").transform.position.z)) < 0.3) { CurrentRow = 4; }
        if (System.Math.Abs((transform.position.z - GameObject.Find("target005_Shuttle01").transform.position.z)) < 0.3) { CurrentRow = 5; }
        if (System.Math.Abs((transform.position.z - GameObject.Find("target006_Shuttle01").transform.position.z)) < 0.3) { CurrentRow = 6; }
        if (System.Math.Abs((transform.position.z - GameObject.Find("target007_Shuttle01").transform.position.z)) < 0.3) { CurrentRow = 7; }

    }


    //穿梭车被射线击中时函数，用于ObserverShoot脚本中调用
    //当前函数主要功能为调用GUI控件显示
    public void beShoot()
    {
        //调用相关GUI控件逻辑显示箱子信息
        ShuttleInfoCanvas.SetActive(true);

        DeviceNameText.text = "设备信息： 穿梭车 " + ShuttleNum.ToString();

        //在穿梭车位置处播放被击中音效
        if (beShootAudio_lifter != null)
            AudioSource.PlayClipAtPoint(beShootAudio_lifter, transform.position);

        if (true)
        {
            //当穿梭车信息与实际信息不符时，可改变项目运行状态
            if (ProjectManager.PM.projectState == ProjectManager.ProjectManagerState.Playing)
            {
                //根据判断条件改变项目状态

            }


        }
    }
}