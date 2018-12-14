using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cube : MonoBehaviour {

    public float width;
    //public float length;


	// Use this for initialization
	void Start () {

        width = transform.localScale.x;
        //length = transform.localScale.z;

        OnInstantiate();

        GetComponent<Renderer>().material.color = new Color(Random.Range(0, 0.8f), Random.Range(0, 0.8f), Random.Range(0, 0.8f));
	}
	
    void OnInstantiate()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(width, 1, width) * 1.2f, 0.2f));
        sequence.Append(transform.DOScale(new Vector3(width, 1, width) , 0.2f));
        sequence.Play();
    }

    public void OnPlayerSquat(float power)
    {
        float height = 1 / (power * 0.1f + 1);

        transform.localScale = new Vector3(3, height, 3);
    }
}
