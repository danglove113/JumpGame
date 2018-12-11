using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Scripts : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKey)
        {
            JumpToTarget(2f, target.position);
        }

	}

    public void JumpToTarget(float jumpPower, Vector3 targetPos)
    {
        Sequence sequence = DOTween.Sequence();
        //sequence.Append(transform.DOMove(target.position, 1f));
        //sequence.Join(transform.DOJump(Vector3.up * 10, 0.5f, 1, 1f));
        sequence.Append(transform.DOJump(targetPos + Vector3.up, jumpPower, 1, 1f));
        sequence.Play();

        //Tweener tweener = transform.DOMove(targetCube.transform.position, 1f);
    }


}
