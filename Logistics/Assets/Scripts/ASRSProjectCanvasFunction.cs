using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/**
 * 本方法主要是界面UI中信息显示以及按钮控制方法
 *
 **/
public class ASRSProjectCanvasFunction : MonoBehaviour
{

    /// <summary>
    /// 提升机信息UI面板控制方法
    /// </summary>

    public GameObject LifterInfoCanvas;//提升机信息UI面板
    public InputField FromLayerInputField;   //提升机输入框组件FromLayerInputField
    public InputField ToLayerInputField; // 提升机输入框组件ToLayerInputField
    public Text CurrentLayerText;//入库提升机当前层
    public Text PlaceGoodsLayerText;//入库提升机目标层
    public Text ProcessModeText;//入库提升机任务状态
    public Text DeviceStateText;//入库提升机运行状态




    public Text LifterDeviceNameText;//提升机UI界面设备名
    public GameObject InUpMechine; // 需要操作的入库提升机对象
    public GameObject OutUpMechine; // 需要操作的出库提升机对象
    InUpMechine inUpMechine;//入库提升机上的类对象

    /// <summary>
    /// 穿梭车UI面板控制方法
    /// </summary>
    public GameObject ShuttleInfoCanvas;//穿梭车信息面板
    public InputField FromRowsInputField;   //穿梭车输入框组件FromRowsInputField
    public InputField ToRowsInputField; // 穿梭车输入框组件ToRowsInputField
    public Text ShuttleDeviceNameText;//穿梭车UI界面设备名
    public GameObject Shuttle01; // 需要操作的穿梭车对象
    public GameObject Shuttle02; // 需要操作的穿梭车对象

    public Text CurrentRowText;//穿梭车当前排信息
    public Text endRowText;//穿梭车目标排
    public Text ShuttleTaskStatusText;//入库提升机任务状态
    public Text ShuttleDeviceStatusText;//入库提升机运行状态

    Shuttle01 shuttle01;//穿梭车上的类


    public GameObject CaseInfoCanvas;//货物箱子信息面板
    public GameObject ShipperInfoCanvas;//输送信息面板

    public GameObject Observer;//观察者




    //初始化函数
    void Start()
    {
        //获取各设备对象的scripts组件以便修改相关属性执行相关方法
        inUpMechine = InUpMechine.GetComponent<InUpMechine>();//获取入库提升机的相关类
        //ActiveInitPanel();  //调用ActiveInitPanel函数，启用初始面板，禁用其他面板
        shuttle01 = Shuttle01.GetComponent<Shuttle01>();//获取一层穿梭车的


    }

    void Update()
    {
        //执行过程中显示各UI控件的值（数据更新）
        flashInLifterInfoCanvas();//入库提升机
        flashShuttleInfoCanvas();//小车

    }



    
    //面板取消框按钮调用的函数
    public void ExitPanel()
    {
        LifterInfoCanvas.SetActive(false);
        ShuttleInfoCanvas.SetActive(false);
        CaseInfoCanvas.SetActive(false);
        ShipperInfoCanvas.SetActive(false);
    }
    //更新提升机面板上数据
    public void flashInLifterInfoCanvas()
    {
        CurrentLayerText.text= "当 前 层 ："+inUpMechine.currentLocation.ToString();
        PlaceGoodsLayerText.text= "目 标 层 ：" + inUpMechine.lifterDestination.ToString();
        if (inUpMechine.TaskStatus.Equals("inTask"))
        {
            ProcessModeText.text = "任务状态: 任务中";
        }
        else
        {
            ProcessModeText.text = "任务状态: 已完成";
        }
        
        
        //print(inUpMechine.TaskStatus);

    }
    //更新穿梭车面板上数据
    public void flashShuttleInfoCanvas()
    {
        CurrentRowText.text = "当 前 排 ：" + shuttle01.CurrentRow.ToString(); 
        endRowText.text = "目 标 排 ：" + shuttle01.endRow.ToString();
        if (shuttle01.taskStatus.Equals("inTask"))
        {
            ShuttleTaskStatusText.text = "任务状态: 任务中";
        }
        else
        {
            ShuttleTaskStatusText.text = "任务状态: 已完成";
        }
        if (shuttle01.deviceStatus.Equals("alarm"))
        {
            ShuttleDeviceStatusText.text = "设备状态 ：报警";
        }else if(shuttle01.deviceStatus.Equals("normal"))
        {
            ShuttleDeviceStatusText.text = "设备状态 ：正常";
        }
        //ShuttleDeviceStatusText
        //print(inUpMechine.TaskStatus);

    }
    //点击提升机面板执行按钮时执行下列逻辑，获取面板中到达层，并执行移动逻辑
    public void LifterExcute()
    {

        //var FromLayer = int.Parse(FromLayerInputField.text);
        int ToLayer = int.Parse(ToLayerInputField.text);
        //FromLayer>=0&& FromLayer<=7 &&
        if (ToLayer >= 0 && ToLayer <= 7)
        {
            //InUpMechine.GetComponent<InUpMechine>().lifterMove(FromLayer);
            //InUpMechine.GetComponent<InUpMechine>().lifterMove(ToLayer);
            string DeviceNum = split(LifterDeviceNameText.text);

            switch (int.Parse(DeviceNum))
            {
                case 100:
                    InUpMechine.GetComponent<InUpMechine>().lifterDestination = ToLayer;
                    break;
                case 200:
                    OutUpMechine.GetComponent<OutUpMechine>().lifterDestination = ToLayer;
                    break;


            }
        }
    }
    //从当前设备面板的设备或箱子信息中提取数字编号
    
  
    private string split(string str)
    {
        string numstr = "";

        for (int i = 0; i < str.Length; i++)
            {
                if (str[i] <= '9' & str[i] >= '0')
                    numstr += str[i];
            }
        return numstr;
    }
    
    //点击穿梭车面板执行按钮时执行下列逻辑，获取面板中到达层，并执行移动逻辑
    public void ShuttleExcute()
    {

        //var FromRows = int.Parse(FromRowsInputField.text);
        int ToRows = int.Parse(ToRowsInputField.text);
        
        if (ToRows >= 0 && ToRows <= 7)
        {
            Shuttle01.GetComponent<Shuttle01>().endRow = ToRows;
            Shuttle01.GetComponent<Shuttle01>().ShuttleMove(ToRows);

            //此处判断穿梭车编号使穿梭车移动
            switch (100)
            {
                case 100:
                    Shuttle01.GetComponent<Shuttle01>().endRow = ToRows;
                    break;
                case 200:
                    //Shuttle02.GetComponent<Shuttle01>().ShuttleDestination = ToRows;
                    break;


            }
        }


    }

    /// <summary>
    /// 一层小车左取货
    /// </summary>
    public void ShuttleLeftTake()
    {
        
        shuttle01.leftTake = true;
    }

    /// <summary>
    /// 一层小车右取货
    /// </summary>
    public void ShuttleRightTake()
    {

        shuttle01.rightTake = true;
    }

    /// <summary>
    /// 一层小车右放货
    /// </summary>
    public void ShuttleRightPlace()
    {

        shuttle01.rightPlace = true;
    }
    /// <summary>
    /// 一层小车左放货
    /// </summary>
    public void ShuttleLeftPlace()
    {

        shuttle01.leftPlace = true;
    }
}

    

