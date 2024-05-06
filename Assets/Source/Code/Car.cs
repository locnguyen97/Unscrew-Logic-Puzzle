using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{

    public void SetData(Sprite cl , string slot)
    {
        GetComponent<SpriteRenderer>().sprite = cl;
    }
    private void OnMouseDown()
    {
        if (GameManager.Instance.canDrag)
        {
            CheckAndMove();
        }
    }

    void CheckAndMove()
    {
        GameManager.Instance.canDrag = false;
        var target = GameManager.Instance.GetCurLevel().gameObjectsSlot[GameManager.Instance.curSlot];
        Move(target.transform);
        GameManager.Instance.curSlot++;
    }
    
    public void Move(Transform target)
    {
        StartCoroutine(MoveToTarget(target));
    }

    IEnumerator MoveToTarget(Transform target)
    {
        var dis = Vector3.Distance(target.position , transform.position);
        var dir = target.position - transform.position;
        while (dis > 0.1f)
        {
            yield return new WaitForEndOfFrame();
            transform.position = transform.position + dir * 0.013f;
            dis = Vector3.Distance(target.position , transform.position);
        }

        transform.position = target.position;
        transform.rotation = Quaternion.Euler(0,0,0);
        CheckOnMoveDone();
        target.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        target.GetComponent<SpriteRenderer>().enabled = true;
        //GameManager.Instance.GetCurLevel().RemoveObject(gameObject);
    }

    void CheckOnMoveDone()
    {
        GameManager.Instance.GetCurLevel().RemoveObject(gameObject);
        Destroy(gameObject);
        GameManager.Instance.EnableDrag();
        var particleVFXs = GameManager.Instance.particleVFXs;
        GameObject explosion = Instantiate(particleVFXs[UnityEngine.Random.Range(0,particleVFXs.Count)], transform.position, transform.rotation);
        Destroy(explosion, .75f);
    }
}
