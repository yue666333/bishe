using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OutUpMechine : MonoBehaviour
{

    /// <summary>
    /// 出库提升机的相关数据库属性（需要显示的相关信息）在此处定义
    ///</summary>
    //当前出库提升机编号
    public int OutUpMechineNum;
    //主任务号
    public int[] MainCommandNum;
    //子任务号
    public int[] minorCommandNum;


    //任务状态
    public bool isCompleted;
    //提升机运行状态枚举
    public enum lifterStatus { alarm, normal }
    //提升机任务状态枚举
    public enum lifterTaskStatus { inTask, taskCompleted }
    //接受任务到达任务层，正在运行中，到达目标层且站台无箱子，到达目标层前方站台有箱子，一次任务完成，任务占用中
    //public string sLiterTaskStatus;
    public int inUpMechineLayer;//提升机当前层，通过update函数实时监测当前层,判断提升机状态运行中，任务占用
    public string TaskStatus;//提升机任务状态:任务完成，任务进行中
    public string LifterStatus;//提升机当前运行状态
    public int currentLocation;//提升机当前层
    public ShipperShoot shippershoot;//光电检测装置的类
    public string caseinfoName;//目标层光电检测到的箱子名称
    public string CurrentTaskCaseInfo;//保存当前任务箱子

    ///<summary>
    /// unity中提升机的属性在此处定义
    /// </summary>
    public AudioClip beShootAudio_lifter;    //提升机被射线击中音效

    private Collider collider;          //提升机的Collider组件
    private Rigidbody rigidbody;        //提升机的rigidbody组件
    public GameObject target;       //提升机移动的目标位置对象

    public GameObject LifterInfoCanvas; //提升机设备信息Canvas
    public int lifterDestination = -1;//设置提升机目标层

    public GameObject onCase;  //提升机上的箱子
    public float moveSpeed = 8.0f;  //提升机的移动速度

    //本部分为出库提升机设备UI界面控件变量
    public Text DeviceNameText;//提升机UI界面设备名

    //初始化，获取提升机的组件
    void Start()
    {
        collider = GetComponent<Collider>();    //获取提升机的Collider组件
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
        //lifterMove(lifterDestination);
    }
    void Update()
    {

       

        if (lifterDestination >= 0 && lifterDestination <= 7)
        {
            lifterMove(lifterDestination);
        }
        
    }


    //箱子对象创建

    //箱子对象销毁



    

    //提升机移动  
    public void lifterMove(int destination)//destination 要到达的目标层，用数字1、2、3等表示
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




    //提升机被射线击中时函数，用于ObserverShoot脚本中调用
    //当前函数主要功能为调用GUI控件显示
    public void beShoot()
    {
        //调用相关GUI控件逻辑显示箱子信息
        LifterInfoCanvas.SetActive(true);

        DeviceNameText.text = "设备信息： 出库提升机 " + OutUpMechineNum.ToString();

        //在出库提升机位置处播放被击中音效
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