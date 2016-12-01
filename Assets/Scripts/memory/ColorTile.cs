using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ColorTile : MonoBehaviour
{
    bool isDragging;
    Vector3 touchPosition;

    //public float maxtime;
    //public float minswipedist;
    //float starttime;
    //float endtime;
    //Vector3 startpos;
    //Vector3 endpos;
    //float swpedict;
    //float swipetime;
    //Vector3 unite;
    //bool test = false;
    MapGenerator1 mapGenerator1;
    Vector2 tileCoordinatePosition;
    int x, y, finger_x, finger_y;

    void Awake()
    {
        //defaultColor = GetComponent<MeshRenderer>().material.color;
        //tilePosition = transform.position;
        mapGenerator1 = FindObjectOfType<MapGenerator1>();

        x = (int)transform.position.x;
        y = (int)transform.position.y;

        tileCoordinatePosition = new Vector2((int)transform.position.x, (int)transform.position.y);
        BindToTile();
    }

    public void ChangeColor(Color color)
    {
        GetComponent<MeshRenderer>().material.color = color;
    }
    private void BindToTile()
    {
        mapGenerator1.tileArray[x, y].ColorTile = this;
    }

    //void Update()
    //{
    //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //    {
    //        touchPosition = Input.GetTouch(0).position;
    //        Vector3 realPos = Input.GetTouch(0).position;
    //        realPos.z = 10f;
    //        realPos = Camera.main.ScreenToWorldPoint(realPos);

    //        isDragging = true;

    //        //if (!isDragging)
    //        //{
    //        //    //mapGenerator.tileArray[x, y].InitialMoveDecrease -= 1;
    //        //    //circleCopy.transform.localScale -= new Vector3(0.2f, 0.2f, 0.0f);
    //        //    //transform.localScale = circleCopy.transform.localScale;

    //        //    //transform.localScale -= new Vector3(0.2f, 0.2f, 0.0f);
    //        //    //amountOfMoves--;

    //        //}
    //    }
    //    //Release
    //    else if (isDragging && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
    //    {
    //        GetComponent<MeshRenderer>().material.color = defaultColor;
    //        isDragging = false;
    //    }
    //    //Drag
    //    else if (isDragging)
    //    {
    //        touchPosition = Input.GetTouch(0).position;
    //        Vector3 realPos = Input.GetTouch(0).position;
    //        realPos.z = 10f;
    //        realPos = Camera.main.ScreenToWorldPoint(realPos);

    //        Vector2 fingerPos = new Vector2((int)realPos.x, (int)realPos.y);

    //        if (fingerPos == tileCoordinatePosition)
    //        {
    //            GetComponent<MeshRenderer>().material.color = Color.green;
    //        }
    //    }
    //    //if (test == true)
    //    //{
    //    //    x = (int)transform.position.x;
    //    //    y = (int)transform.position.y;
    //    //}
    //    //if (Input.touchCount > 0)
    //    //{
    //    //    Touch toutch = Input.GetTouch(0);
    //    //    if (toutch.phase == TouchPhase.Began)
    //    //    {
    //    //        starttime = Time.time;
    //    //        startpos = toutch.position;
    //    //    }
    //    //    else if (toutch.phase == TouchPhase.Ended)
    //    //    {
    //    //        endpos= toutch.position;
    //    //        endtime= Time.time;
    //    //        swpedict = (endpos - startpos).magnitude;
    //    //        starttime = endtime - starttime;
    //    //        if (swipetime < maxtime && swpedict > minswipedist)
    //    //        {
    //    //            swipe();
    //    //        }
    //    //    }
    //    //}
    //}
    //void swipe()
    //{
    //    Vector2 dictens = endpos - startpos;
    //    if (Mathf.Abs(dictens.x) > Mathf.Abs(dictens.y))
    //    {
    //        if (dictens.x > 0)
    //        {
    //            unite.x = 1;
    //            unite.y = 0;
    //            unite.z = 0;
    //            transform.Translate(unite);

    //        }
    //        if (dictens.x < 0)
    //        {
    //            unite.y = 0;
    //            unite.x = -1;
    //            unite.z = 0;
    //            transform.Translate(unite);
    //        }

    //    }
    //    else if (Mathf.Abs(dictens.x) < Mathf.Abs(dictens.y))
    //    {
    //        if (dictens.y > 0)
    //        {
    //            unite.y = 1;
    //            unite.z = 0;
    //            unite.x = 0;
    //            transform.Translate(unite);
    //        }
    //        if (dictens.y < 0)
    //        {
    //            unite.y = -1;
    //            unite.z = 0;
    //            unite.x = 0;
    //            transform.Translate(unite);
    //        }
    //    }
    //}
}

      