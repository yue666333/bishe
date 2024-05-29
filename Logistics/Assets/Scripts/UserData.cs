using UnityEngine;
using System.Collections;

public class UserData{

	public string username;//登录用户名
    public string date;//用户登录日期
	public float time;//用户登录时间

	public UserData(){
		username = "";
		time = -1;
	}

	public UserData(string _username,float _time){
		time = _time;
	}

	public UserData(string s){
        username = s;
		//time = 
	}

	public string DataToString(){
		return username + " "  + time.ToString ();
	}

	
}
