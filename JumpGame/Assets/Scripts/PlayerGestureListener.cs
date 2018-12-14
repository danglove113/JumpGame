using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGestureListener : MonoBehaviour , KinectGestures.GestureListenerInterface{

    public long userID = -1;

    Vector3 initBodyPos;    //身体的初始位置

    int spineShoulderIndex;   //肩部中心关节的索引
    int handLeftIndex;
    int handRightIndex;


    Vector3 spineShoulderPos;  //肩部中心关节的位置，用来监测是否下蹲
    Vector3 handLeftPos;
    Vector3 handRightPos;
    Vector3 bodyPos;

    GameManager gameManager;


    void Start () {

        print("用户数量为" + KinectManager.Instance.GetUsersCount());

        gameManager = GameManager.Instance;

        if (KinectManager.Instance && !KinectManager.Instance.gestureListeners.Contains(this))
        {
            KinectManager.Instance.gestureListeners.Add(this);
        }
        if (KinectManager.Instance)
        {
            if (KinectManager.Instance.GetUsersCount() > 0)
            {
                //userID = GameManager.instance.userID;//获取用户Id
                initBodyPos = KinectManager.Instance.GetUserPosition(userID);//获取初始位置
            }

            //从KinectManager读取关节位置进行存储
            spineShoulderIndex = KinectManager.Instance.GetJointIndex(KinectInterop.JointType.SpineShoulder);
            handLeftIndex = KinectManager.Instance.GetJointIndex(KinectInterop.JointType.HandLeft);
            handRightIndex = KinectManager.Instance.GetJointIndex(KinectInterop.JointType.HandRight);

        }
    }
	
	
	void Update () {


        if (userID < 0 && gameManager.gameState == GameManager.GameState.WaitingPlayer)
        {
            //gameManager.WaitPlayer((int)(gameManager.maxWaitTime - (Time.time - userLostTime)));
            return;
        }

        GetJoints();

        if (gameManager.gameState == GameManager.GameState.Gaming)
            CheckSquat();
        else if (gameManager.gameState == GameManager.GameState.WaitingStart || gameManager.gameState == GameManager.GameState.EndGame)
            CheckStart();
	}

    public void UserDetected(long userId, int userIndex)
    {
        if (userID == -1)   //没有用户则添加用户
        {
            userID = userId;
            if (gameManager.gameState == GameManager.GameState.WaitingPlayer)
                gameManager.gameState = GameManager.GameState.Gaming;
            if (gameManager.player.movementState == Player.MovementState.Miss)
                gameManager.player.movementState = Player.MovementState.Idle;

            initBodyPos = KinectManager.Instance.GetUserPosition(userID);
            print(string.Format("检测到用户加入，用户ID：{0}", userID));
            print("用户数量为" + KinectManager.Instance.GetUsersCount());
        }
        else
        {
            print("检测到路人");
            print("用户数量为" + KinectManager.Instance.GetUsersCount());
        }
    }

    //用户丢失，若有其他用户由此用户接管，否则丢失
    public void UserLost(long userId, int userIndex)
    {
        if (userID == userId)
        {
            gameManager.player.userLostTime = Time.time;  //记录用户丢失的时刻

            userID = -1;
            gameManager.gameState = GameManager.GameState.WaitingPlayer;
            gameManager.player.movementState = Player.MovementState.Miss;
            print("用户数量为" + KinectManager.Instance.GetUsersCount());
        }
        else
        {
            print("有路人离开");
            print("用户数量为" + KinectManager.Instance.GetUsersCount());
        }

            //if (userId == userID)//只有在丢失的是当前控制的用户时才进行处理
            //{
            //    if (KinectManager.Instance.GetUsersCount() >= 1)
            //    {
            //        userID = KinectManager.Instance.GetUserIdByIndex(0);
            //        print("剩余用户ID：" + userID);
            //        //if (userID == userId)
            //        //{
            //        //    userID = KinectManager.Instance.GetUserIdByIndex(1);
            //        //}
            //        initBodyPos = KinectManager.Instance.GetUserPosition(userID);
            //        gameManager.gameState = GameManager.GameState.Gaming;
            //    }
            //    else
            //    {
            //        userID = -1;
            //        gameManager.gameState = GameManager.GameState.WaitingPlayer;
            //        gameManager.player.movementState = Player.MovementState.Idle;                
            //    }
            //}
        }


    //每帧更新关节坐标
    void GetJoints()
    {
        //if (userID <= 0)
        //{
        //    return;
        //}
        handLeftPos = KinectManager.Instance.GetJointPosition(userID, handLeftIndex);
        handRightPos = KinectManager.Instance.GetJointPosition(userID, handRightIndex);
        bodyPos = KinectManager.Instance.GetUserPosition(userID);
    }


    void CheckSquat()
    {

        //静止状态切换至下蹲状态
        if (gameManager.player.movementState == Player.MovementState.Idle && initBodyPos.y - bodyPos.y > 0.1f)
        {
            gameManager.player.movementState = Player.MovementState.Squat;
            gameManager.player.startSquatTime = Time.time;
            print("squat");
        }

        //下蹲状态切换至起跳状态
        if (gameManager.player.movementState == Player.MovementState.Squat && bodyPos.y - initBodyPos.y > 0f)
        {
            gameManager.player.movementState = Player.MovementState.Jump;
            gameManager.player.startJumpTime = Time.time;

            gameManager.player.jumpPower = (gameManager.player.startJumpTime - gameManager.player.startSquatTime) * 10;   //跳跃的力量和下蹲时间成正比
            Vector3 jumpDistance = gameManager.offset * gameManager.player.jumpPower;
            gameManager.player.JumpToTarget(gameManager.player.jumpPower, gameManager.player.gameObject.transform.position + jumpDistance + Vector3.up * 0.5f);

            print("Jump");
        }

        //起跳状态切换至静止状态
        if (gameManager.player.movementState == Player.MovementState.Jump && bodyPos.y - initBodyPos.y < 0.1f)
        {
            gameManager.player.movementState = Player.MovementState.Idle;
            print("Idle");
        }
    }


    void CheckStart()
    {
        //print(handLeftPos);
        //print(handRightPos);
        if (userID >=0 && Vector3.Distance(handLeftPos, handRightPos) < 0.1f)
        {
            gameManager.StartGame();
        }

    }


    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint)
    {
        return true;
    }

    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture, KinectInterop.JointType joint, Vector3 screenPos)
    {
        return true;
    }

    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
    }

}
