using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Monobehavior对象类的单例泛型基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>{

    static T instance;

	public static T Instance
	{
		get
        {
            return instance;
        }
	}

	
	public virtual void Awake()
	{
        if (instance == null)
            instance = this as T;
        else
            Destroy(gameObject);
	}



}
