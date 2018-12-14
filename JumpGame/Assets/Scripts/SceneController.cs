using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : SingletonBehavior<SceneController> {

    public GameObject[] cubes;
    public GameObject plane;

    [HideInInspector]
    public GameObject newCube;  //新生成的方块的引用
    [HideInInspector]
    public GameObject currentCube;  //玩家所在方块的引用
    [HideInInspector]
    public GameObject lastCube;  //玩家已离开的方块的引用

	// Use this for initialization
	void Start () {

        plane.GetComponent<Renderer>().material.color = new Color(101f/255f, 147f/255f, 74f/255f);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 在目标位置生成新的方块
    /// </summary>
    /// <param name="pos">目标位置</param>
    /// <param name="cubeIndex">方块序号</param>
    public void CreateCube(Vector3 pos, int cubeIndex)
    {
        //if (newCube)
        //    currentCube = newCube;
        //lastCube = currentCube;

        newCube = Instantiate(cubes[cubeIndex], pos, Quaternion.identity);
    }

    public IEnumerator DeleteCube(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        if(lastCube)
            Destroy(lastCube);

        lastCube = currentCube;
    }

    /// <summary>
    /// 玩家下蹲时压缩方块
    /// </summary>
    public void CompressCube(float power)
    {
        currentCube.GetComponent<Cube>().OnPlayerSquat(power);
    }

    public void Reset()
    {
        if (newCube)
            Destroy(newCube);
        if (currentCube)
            Destroy(currentCube);
        if (lastCube)
            Destroy(lastCube);

        lastCube = currentCube = Instantiate(cubes[0], Vector3.zero, Quaternion.identity);
    }
}
