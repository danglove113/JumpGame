using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehavior<GameManager> {

    public float jumpDuration;  //跳起到落下的时间间隔

    int direction;

    public Vector3 offset;

    [Tooltip("玩家离开后等待的最大时间")]
    public int maxWaitTime = 10;

    [HideInInspector]
    public Player player;
    [HideInInspector]
    public UIController uiController;
    [HideInInspector]
    public SceneController sceneController;
    [HideInInspector]
    public SoundController soundController;

    public enum GameState
    {
        WaitingStart,  //等待游戏开始时的状态
        WaitingPlayer,  //等待玩家加入时
        Gaming,        //游戏进行时的状态
        EndGame        //游戏结束的状态
    }
    [HideInInspector]
    public GameState gameState;

    public override void Awake()
    {
        base.Awake();

        player = GameObject.Find("Player").GetComponent<Player>();
    }

	void Start ()
    {
        gameState = GameState.WaitingStart;

        uiController = UIController.Instance;
        sceneController = SceneController.Instance;
        soundController = SoundController.Instance;

        player.jumpDuration = jumpDuration;
                
    }
	

	void Update ()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    player.movementState = Player.MovementState.Squat;
        //    player.startSquatTime = Time.time;            
        //}

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    player.movementState = Player.MovementState.Jump;
        //    player.jumpPower = (Time.time - player.startSquatTime) * 20;

        //    Vector3 jumpDistance = offset * player.jumpPower;

        //    player.JumpToTarget(player.jumpPower, player.gameObject.transform.position + jumpDistance + Vector3.up * 1.5f);
        //}

        if (gameState == GameState.WaitingStart)
        {
            uiController.ShowInfo("请拍手开始游戏");
        }


        //if (gameState == GameState.Gaming && player.movementState == Player.MovementState.Squat)
        //{
        //    player.userLostTime = 0;
        //    uiController.ShowInfo("");
        //    uiController.StorePower((Time.time - player.startSquatTime) * 0.5f);   //蓄力条增长
        //    //sceneController.CompressCube(Time.time - player.startJumpTime);   //方块压缩
        //    //蓄力声音依次播放
        //    for (int i = 0; i <= soundController.sounds.Length; i++)
        //    {
        //        if (Mathf.Abs(Time.time - player.startSquatTime - i * 0.15f) < 0.02f)
        //        {
        //            soundController.StopPlay();
        //            soundController.PlaySound(i);
        //        }
        //    }
        //}
        //else
        //{
        //    uiController.StorePower(0);
        //    soundController.StopPlay();
        //}

        if (gameState == GameState.Gaming)
        {
            if (player.userLostTime > 0)
            {
                uiController.ShowInfo("游戏继续");
                player.userLostTime = 0;
            }

                if (player.movementState == Player.MovementState.Squat)
            {

                uiController.ShowInfo("");
                uiController.StorePower((Time.time - player.startSquatTime) * 0.5f);   //蓄力条增长
                //sceneController.CompressCube(Time.time - player.startJumpTime);   //方块压缩
                //蓄力声音依次播放
                for (int i = 0; i <= soundController.sounds.Length; i++)
                {
                    if (Mathf.Abs(Time.time - player.startSquatTime - i * 0.15f) < 0.02f)
                    {
                        soundController.StopPlay();
                        soundController.PlaySound(i);
                    }
                }
            }
            else
            {
                uiController.StorePower(0);
                soundController.StopPlay();
            }

        }

        if (gameState == GameState.WaitingPlayer)
        {
            WaitPlayer((int)(maxWaitTime - (Time.time - player.userLostTime)));
        }

	}

    public void StartGame()
    {
        gameState = GameState.Gaming;

        offset = Vector3.forward;

        //sceneController.CreateCube(player.gameObject.transform.position + offset * Random.Range(6f, 9f), 0);

        player.Reset();
        uiController.ShowInfo("");
        uiController.ShowScore(player.score);
        sceneController.Reset();
        sceneController.CreateCube(player.gameObject.transform.position + offset * Random.Range(6f, 9f), 0);
    }

    /// <summary>
    /// 玩家跳跃结束时的回调函数
    /// </summary>
    public void UpdateCube()
    {
        sceneController.currentCube = sceneController.newCube;

        float distance = Vector3.Distance(player.gameObject.transform.position, sceneController.currentCube.transform.position + Vector3.up * 0.5f);  //玩家落点距离方块中心的距离

        //落点在方块之外时
        if (distance > sceneController.currentCube.GetComponent<Cube>().width / 2)
        {
            EndGame();
        }
        //落点在方块之内时
        else
        {
            if (distance < 0.5f)
                player.score += 2;
            else
                player.score += 1;
            uiController.ShowScore(player.score);

            direction = Random.Range(0, 2);   //随机生成下一个方块的方向，0代表向前，1代表向右

            if (direction == 0)
                offset = Vector3.forward;
            else
                offset = Vector3.right;

            Vector3 newCubePos = player.gameObject.transform.position + offset * Random.Range(6f, 9f);   //下一个方块的距离在随机区间内取值

            sceneController.CreateCube(newCubePos, 0);

            player.transform.LookAt(player.transform.position + offset);

            StartCoroutine(sceneController.DeleteCube(0.5f));

        }

    }


    public void WaitPlayer(float remainingTime)
    {
        //gameState = GameState.WaitingPlayer;
        uiController.ShowInfo(string.Format("等待玩家，{0}秒后结束游戏", remainingTime));

        if (remainingTime <= 0)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        gameState = GameState.EndGame;
        player.rigidbody.isKinematic = false;
        player.rigidbody.useGravity = true;
        player.Fall();
        uiController.ShowInfo(string.Format("玩家得分：{0}，\n拍手重新开始", player.score));

        //Debug.Log("超出范围");
    }

}
