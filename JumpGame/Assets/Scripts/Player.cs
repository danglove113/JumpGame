using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour {

    public float initHight;

    [HideInInspector]
    public int playerIndex;
    [HideInInspector]
    public float jumpDuration;
    [HideInInspector]
    public int score;
    [HideInInspector]
    public float startSquatTime;  //开始下蹲的时间
    [HideInInspector]
    public float startJumpTime;  //开始起跳的时间，通过与下蹲的时间差计算起跳力量
    [HideInInspector]
    public float jumpPower;
    [HideInInspector]
    public Rigidbody rigidbody;
    [HideInInspector]
    public float userLostTime = 0;  //玩家丢失的时刻

    GameManager gameManager;

    Animation animation;
    //TweenCallback jumpEndCallBack = ChangeMovementState(MovementState.Idle);

    void Awake()
    {
        
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        movementState = MovementState.Miss;
        //jumpEndCallBack = ChangeMovementState(MovementState.Idle);
        animation = GetComponent<Animation>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public enum MovementState
    {
        Idle,  //角色静止时的状态
        Squat,  //角色下蹲蓄力时的状态
        Jump,  //角色跳起时的状态
        Miss   //丢失玩家时的状态
    }
    [HideInInspector]
    public MovementState movementState;

    public void JumpToTarget(float jumpPower, Vector3 targetPos)
    {
        //GetComponent<Rigidbody>().useGravity = false;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(transform.DOJump(targetPos, jumpPower, 0, 1f));
        sequence.OnComplete(()=>OnJumpEnd());  //跳跃结束的回调
        sequence.Play();

        if(animation)
            animation.Play("Jump_NoDagger");
    }

    void OnJumpEnd()
    {
        movementState = MovementState.Idle;

        //GetComponent<Rigidbody>().useGravity = true;

        gameManager.UpdateCube();
    }

    public void Fall()
    {
        if (animation)
            animation.Play("Death");
    }

    public void Reset()
    {
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        movementState = MovementState.Idle;
        transform.position = new Vector3(0, initHight, 0);
        transform.rotation = Quaternion.identity;
        score = 0;
        if (animation)
            animation.Play();
    }
}
