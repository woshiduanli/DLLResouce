using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZWFlyingObject : MonoBehaviour 
{
	public Dictionary<string,object> Parameters=new Dictionary<string,object>();
	
	public void SetParmeters(string key,object value)
	{
		if(!Parameters.ContainsKey(key))
		{
			return;
		}
		Parameters[key]=value;
	}
	
	public object GetParmeters(string key)
	{
        object result = new object();
        Parameters.TryGetValue(key, out result);
		return result;
	}
	
	public void AddParmeters(string key,object value)
	{
        if (Parameters.ContainsKey(key))
        {
            return;
        }
        Parameters.Add(key,value);
        
	}

	protected float pointataxis(Vector3 axis,Vector3 point1,Vector3 point2)//求点在轴上的投影，返回投影到目标的距离，用来做碰撞检测
	{
		Vector3 xie=point1-point2;
		float a=Vector3.Dot(xie.normalized,(-axis));
		a*=xie.magnitude;
		return a;
	}
	protected Vector3 getright(Vector3 vec)//得到轴的右方向
	{
		return Vector3.Cross(vec,new Vector3(0.0f,1.0f,0.0f));
	}
	protected Vector3 getup(Vector3 vec)//得到轴的上方向
	{
		return Vector3.Cross(new Vector3(0.0f,0.0f,1.0f),vec);
	}
	protected Vector3 getup2(Vector3 vec)//得到轴的上方向
	{
		return Vector3.Cross(new Vector3(1.0f,0.0f,0.0f),vec);
	}
	
	protected Vector3 PointRotateAround(Vector3 p,Vector3 paxis,Vector3 Axis,float a)//空间内点围绕过任意点的任意轴旋转，旋转a弧度
	{
		Vector3 p3=p-paxis;
		p3=p3*Mathf.Cos(a)+Vector3.Cross(Axis,p3)*Mathf.Sin(a)+Axis*Vector3.Dot(Axis,p3)*(1-Mathf.Cos(a));
		p3+=paxis;
		return p3;
	}
}
