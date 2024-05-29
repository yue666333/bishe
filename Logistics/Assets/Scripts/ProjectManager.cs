using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/**
 * ProjectManager类
 * 场景中唯一的项目管理器实例
 * 
 * 
 * */
public class ProjectManager : MonoBehaviour {

	static public ProjectManager PM;		//静态项目管理器，场景中唯一的项目管理器实例

	public GameObject Observer;			//观察者对象
	

	//项目监控状态枚举，分别表示项目正常进行（Playing）、维修（Pause）、故障停止（Stop）
    //初步设想根据当前项目不同状态给观察者创建UI遮罩空间显示不同颜色，并在页面上显示项目状态文本
	public enum ProjectManagerState {Playing, Pause, Stop };	
	public ProjectManagerState projectState;            //项目监控状态变量

    public int ProjectStateChange;  //场景中物体出现故障时，可通过改变该值反映不同变化，初步设想使用错误代码方式{1：“”；2：“”；3：“”}
    public GameObject player;			//监控主角
    public GameObject playingCanvas;    //监控进行时Canvas

                                        //箱子信息Canvas
                                        //拣选台信息框Canvas
                                        //发货处信息框Canvas
    public Text userText1;               //当前登录用户名Text组件
    public Text timeText;               //当前用户登陆时间Text组件
    public Image warningImage;		    //Image组件，用于系统发生警告时的屏幕变红效果
    public Text ProjectStateText;   //GUI控件，用于显示当前项目监控状态的文本信息

    public AudioClip playingAudio;              //监控进行时音效
    public GameObject gameResultCanvas;         //退出监控Canvas
    public GameObject mobileControlRigCanvas;   //移动端控制UI

    public GameObject firstUserText;    //日期排名第一的用户log信息
    public GameObject secondUserText;   //日期排名第二的用户log信息
    public GameObject thirdUserText;    //日期排名第三的用户log信息
    public GameObject userText;         //本次登录用户信息
    public Text logMessage;            //监控退出结果信息，提示用户是否退出监控


    private float startTime;            //场景加载的时刻
    private float currentTime;          //从场景加载到现在所花的时间

    private bool cursor;                    //鼠标光标是否显示
    private AudioListener audioListener;    //摄像机的AudioListener组件
    private Color flashColor = new Color(1.0f, 0.0f, 0.0f, 0.3f);   //场景切换时，warningImage的颜色
    private float flashSpeed = 2.0f;                                //warningImage颜色的渐变速度

    private UserData firstUserData;     //日期排名第一的用户log的相关数据
    private UserData secondUserData;    //日期排名第二的用户log的相关数据
    private UserData thirdUserData;     //日期排名第三的用户log的相关数据
    private UserData currentUserData;   //当前用户的log相关数据
    private UserData[] userDataArray = new UserData[4];

    private bool isGameOver = false;		//标识，保证监控结束时的相关行为只执行一次


    //初始化，获取相关组件，并初始化变量
    void Start () {
        Cursor.visible = false; //禁用鼠标光标
        if (PM == null)         //静态项目监控管理器初始化
            PM = GetComponent<ProjectManager>();

        if (player == null)     //获取场景中的监控者observer
            player = GameObject.FindGameObjectWithTag("Observer");

        //获取场景中的AudioListener组件
        //audioListener = GameObject.FindGameObjectWithTag("Camera").GetComponent<AudioListener>();

        PM.projectState = ProjectManagerState.Playing;//正常监控状态设置项目监控状态为监控正常进行中
        startTime = Time.time;              //记录场景加载的时刻

        playingCanvas.SetActive(true);      //启用监控进行中Canvas
        //gameResultCanvas.SetActive(false);  //禁用退出监控Canvas


        //从本地保存的数据中获取之前登陆的三名用户信息
        if (PlayerPrefs.GetString("FirstUser") != "")
        {
            firstUserData = new UserData(PlayerPrefs.GetString("FirstUser"));
        }
        else
            firstUserData = new UserData();
        if (PlayerPrefs.GetString("SecondUser") != "")
        {
            secondUserData = new UserData(PlayerPrefs.GetString("SecondUser"));
        }
        else
            secondUserData = new UserData();
        if (PlayerPrefs.GetString("ThirdUser") != "")
        {
            thirdUserData = new UserData(PlayerPrefs.GetString("ThirdUser"));
        }
        else
            thirdUserData = new UserData();
        //根据GameStart场景中的声音设置，控制本场景中AudioListener的启用与禁用
        //audioListener.enabled = (PlayerPrefs.GetInt("SoundOff") != 1);

		}

	//每帧执行一次，用于项目状态的检测与切换，以及处理当前项目状态需要执行的语句
	void Update () {
        //更新warningImage的颜色（线性插值）
        warningImage.color = Color.Lerp(
            warningImage.color,
            Color.clear,
            flashSpeed * Time.deltaTime
        );

        //根据当前项目状态来决定要执行的语句
        switch (projectState) {	

		//当项目状态为监控进行中（Playing）状态时
		case ProjectManagerState.Playing:
                ProjectStateText.text = "项目状态:" + projectState.ToString();  //将显示当前项目监控的文本信息更改为“项目状态:状态” 
                if (Input.GetKeyDown(KeyCode.E))       //输入E键，控制鼠标光标的可见性
                    Cursor.visible = !Cursor.visible;
                if (ProjectStateChange == 0)          //若场景中物体出现故障时，项目监控状态切换到监控暂停
                    PM.projectState = ProjectManagerState.Pause;
                else if (ProjectStateChange == 0)//若场景中物体出现故障时，项目监控状态切换到监控停止
                {
                    PM.projectState = ProjectManagerState.Stop;
                }
                //否则，当前项目监控状态还是监控正常进行时状态
                else
                {
                    userText1.text = "当前登录用户： " + PlayerPrefs.GetString("Username");

                    currentTime = Time.time - startTime;                //根据当前时刻与场景加载时刻计算监控场景运行的时间
                    timeText.text = "监控运行时间： " + currentTime.ToString("0.00");    //显示已用时间
                    //if (mobileControlRigCanvas != null)                 //启用移动端控制Canvas
                    //    mobileControlRigCanvas.SetActive(true);
                }
                break;
		
		//当项目状态为维修暂停（Pause）状态时
		case ProjectManagerState.Pause:
                if (!isGameOver)
                {
                    AudioSource.PlayClipAtPoint(playingAudio, player.transform.position);   //播放音效
                    Cursor.visible = true;                  //将鼠标光标显示
                    playingCanvas.SetActive(false);     //禁用监控进行中Canvas
                    //gameResultCanvas.SetActive(true);       //启用退出监控Canvas
                    //if (mobileControlRigCanvas != null)     //禁用移动端控制Canvas
                    //    mobileControlRigCanvas.SetActive(false);
                    isGameOver = true;
                    //EditGameOverCanvas();   //编辑结束Canvas中的排行榜
                }
                break;

        //当项目状态为故障停止（Stop）状态时
        case ProjectManagerState.Stop:
                if (!isGameOver)
                {
                    AudioSource.PlayClipAtPoint(playingAudio, player.transform.position);  //播放退出监控音效
                    Cursor.visible = true;                  //将鼠标光标显示
                    playingCanvas.SetActive(false);     //禁用监控进行中Canvas
                    //gameResultCanvas.SetActive(true);       //启用监控退出Canvas
                    //if (mobileControlRigCanvas != null)     //禁用移动端控制Canvas
                    //    mobileControlRigCanvas.SetActive(false);
                    isGameOver = true;
                    
                }
                break;

        }

    }

   


    //将用户log信息显示在对应的text中
    void LeaderBoardChange(Text[] texts, UserData data)
    {
        texts[0].text = data.username;

        texts[1].text = data.time.ToString();
    }



}
