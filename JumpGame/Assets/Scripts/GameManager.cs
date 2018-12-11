using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehavior<GameManager> {

    public float jumpDuration;  //跳起到落下的时间间隔

    int direction;

    Vector3 offset;

    [HideInInspector]
    public Player player;

    UIController uiController;
    SceneController sceneController;

    public enum GameState
    {
        WaitingStart,  //等待游戏开始时的状态
        Gaming,        //游戏进行时的状态
        EndGame        //游戏结束的状态
    }
    [HideInInspector]
    public GameState gameState;

    public override void Awake()
    {
        base.Awake();
        uiController = UIController.Instance;
        sceneController = SceneController.Instance;

        player = GameObject.Find("Player").GetComponent<Player>();
    }

	void Start ()
    {
        //state = GameState.WaitingStart;

        player.jumpDuration = jumpDuration;

        offset = Vector3.forward;

        sceneController.CreateCube(player.gameObject.transform.position + offset * Random.Range(6f, 9f), 0);
    }
	

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.movementState = Player.MovementState.Squat;
            player.startSquatTime = Time.time;            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            player.movementState = Player.MovementState.Jump;
            player.jumpPower = (Time.time - player.startSquatTime) * 10;

            Vector3 jumpDistance = offset * player.jumpPower;
                        
            player.JumpToTarget(player.jumpPower, player.gameObject.transform.position + jumpDistance + Vector3.up * 1.5f);
        }
	}

    public void StartGame()
    {

    }

    /// <summary>
    /// 玩家跳跃结束时的回调函数
    /// </summary>
    public void UpdateCube()
    {
        sceneController.currentCube = sceneController.newCube;

        if (Vector3.Distance(player.gameObject.transform.position, sceneController.currentCube.transform.position + Vector3.up * 1.5f) > 1.5f)
        {
            gameState = GameState.EndGame;
            Debug.Log("超出范围");
        }
        else
        {
            direction = Random.Range(0, 2);

            if (direction == 0)
                offset = Vector3.forward;
            else
                offset = Vector3.right;

            Vector3 newCubePos = player.gameObject.transform.position + offset * Random.Range(6f, 9f);

            sceneController.CreateCube(newCubePos, 0);

            StartCoroutine(sceneController.DeleteCube(0.5f));

        }

    }

}
