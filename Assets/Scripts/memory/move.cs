using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class move : MonoBehaviour
{
    public float maxtime;
    public float minswipedist;
    float starttime;
    float endtime;
    Vector3 startpos;
    Vector3 endpos;
    float swpedict;
    float swipetime;
    Vector3 unite;
    bool test = false;
    MapGenerator1 mapGenerator1;
    Vector3 tilePosition;
    int x, y;

    void Start()
    {

        mapGenerator1 = FindObjectOfType<MapGenerator1>();
        tilePosition = transform.position;

    }

    void Update()
    {
        if (test == true)
        {
            x = (int)transform.position.x;
            y = (int)transform.position.y;
        
               


        }
        if (Input.touchCount > 0)
        {
            Touch toutch = Input.GetTouch(0);

            if (toutch.phase == TouchPhase.Began)
            {
                starttime = Time.time;
                startpos = toutch.position;


            }
            else if (toutch.phase ==TouchPhase.Ended)
            {
                endpos= toutch.position;
                endtime= Time.time;
                swpedict = (endpos - startpos).magnitude;
                starttime = endtime - starttime;
                if (swipetime < maxtime && swpedict > minswipedist)
                {
                    swipe();
                }
            }
        }

    }
    void swipe()
    {
        Vector2 dictens = endpos - startpos;
        if (Mathf.Abs(dictens.x) > Mathf.Abs(dictens.y))
        {
            if (dictens.x > 0)
            {
                unite.x = 1;
                unite.y = 0;
                unite.z = 0;
                transform.Translate(unite);

            }
            if (dictens.x < 0)
            {
                unite.y = 0;
                unite.x = -1;
                unite.z = 0;
                transform.Translate(unite);
            }

        }
        else if (Mathf.Abs(dictens.x) < Mathf.Abs(dictens.y))
        {
            if (dictens.y > 0)
            {
                unite.y = 1;
                unite.z = 0;
                unite.x = 0;
                transform.Translate(unite);
            }
            if (dictens.y < 0)
            {
                unite.y = -1;
                unite.z = 0;
                unite.x = 0;
                transform.Translate(unite);
            }
        }
    }
}

      