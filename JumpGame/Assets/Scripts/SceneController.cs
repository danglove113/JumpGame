using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : SingletonBehavior<SceneController> {

    public GameObject[] cubes;

    [HideInInspector]
    public GameObject newCube;  //新生成的方块的引用
    [HideInInspector]
    public GameObject currentCube;  //玩家所在方块的引用
    [HideInInspector]
    public GameObject lastCube;  //玩家已离开的方块的引用

	// Use this for initialization
	void Start () {

        lastCube = currentCube = Instantiate(cubes[0], Vector3.zero, Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
}
