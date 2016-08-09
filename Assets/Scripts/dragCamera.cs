using UnityEngine;
using System.Collections;

public class dragCamera : MonoBehaviour
{
    public Vector3 mousePosition;
    public Vector3 beginDragPosition;
    public bool isDragging;
    public float targetSize=4;
    public bool mouseMode=true;
    public float startZoomDistance;
    public float oldSize;
    public float minimumZoom = 1;
    public float maximumZoom = 5;

    public float[] bounds; //Left, Right, Top, Bottom (-3.5, 8, 4.4, -2)


    void Update()
    {
        if (mouseMode)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                beginDragPosition = mousePosition;

            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                Vector3 tempVector = beginDragPosition - mousePosition;
                transform.Translate(new Vector3(tempVector.x, tempVector.y, 0));
            }

            targetSize += Input.GetAxis("Mouse ScrollWheel") * 3;
            targetSize = Mathf.Clamp(targetSize, minimumZoom, maximumZoom);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, 0.2f);
        } else
        {
            if (Input.touchCount >= 1)
            {
                Vector3 firstTouchPosition = new Vector3(Camera.main.ScreenToWorldPoint(Input.touches[0].position).x, Camera.main.ScreenToWorldPoint(Input.touches[0].position).y,-10);
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    isDragging = true;
                    beginDragPosition = firstTouchPosition;
                } else if (Input.touches[0].phase == TouchPhase.Ended)
                {
                    isDragging = false;
                }
                if (Input.touchCount>=2)
                {
                    float distance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    //Debug.Log(distance);
                    if (distance>50)
                    {
                        if (Input.touches[1].phase==TouchPhase.Began)
                        {
                            startZoomDistance = distance;
                            oldSize = Camera.main.orthographicSize;
                        }
                        targetSize = oldSize * (startZoomDistance / distance);
                        targetSize = Mathf.Clamp(targetSize, minimumZoom, maximumZoom);
                        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetSize, 0.2f);
                    }

                }
                if (isDragging)
                {
                    Vector3 tempVector = beginDragPosition - firstTouchPosition;
                    transform.Translate(new Vector3(tempVector.x, tempVector.y, 0));
                }
            }
        }


        if (transform.position.x < bounds[0])
        {
            transform.position = new Vector3(bounds[0], transform.position.y, transform.position.z);
        }
        else
        if (transform.position.x > bounds[1])
        {
            transform.position = new Vector3(bounds[1], transform.position.y, transform.position.z);
        }

        if (transform.position.y > bounds[2])
        {
            transform.position = new Vector3(transform.position.x, bounds[2], transform.position.z);
        }
        else
        if (transform.position.y < bounds[3])
        {
            transform.position = new Vector3(transform.position.x, bounds[3], transform.position.z);
        }
    }

}