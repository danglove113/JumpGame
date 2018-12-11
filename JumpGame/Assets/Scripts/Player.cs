using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour {

    [HideInInspector]
    public int playerIndex;
    [HideInInspector]
    public float jumpDuration;
    [HideInInspector]
    public int score;
    [HideInInspector]
    public float startSquatTime;  //开始下蹲的时间
    [HideInInspector]
    public float jumpPower;

    GameManager gameManager;
    //TweenCallback jumpEndCallBack = ChangeMovementState(MovementState.Idle);

    void Awake()
    {
        
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        movementState = MovementState.Idle;
        //jumpEndCallBack = ChangeMovementState(MovementState.Idle);
    }

    public enum MovementState
    {
        Idle,  //角色静止时的状态
        Squat,  //角色下蹲蓄力时的状态
        Jump  //角色跳起时的状态
    }
    [HideInInspector]
    public MovementState movementState;

    public void JumpToTarget(float jumpPower, Vector3 targetPos)
    {

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOJump(targetPos, jumpPower, 0, 1f));
        sequence.OnComplete(()=>OnJumpEnd());  //跳跃结束的回调
        sequence.Play();

    }

    void OnJumpEnd()
    {
        movementState = MovementState.Idle;

        gameManager.UpdateCube();
    }

}
