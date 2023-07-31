using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float offsetX;
    private Rigidbody2D rb;
    private const string _LeftBorder = "LeftBorder";
    private const float movementMiltiplier = 1.8f;
    private bool touchedBorder;
    private bool touchedLeftBorder;
    private float lastTouchBorderPosX;
    private bool movingLeft;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            // Check if finger is over a UI element
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;

            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = touch.position;


            if (touchedBorder)
            {
                movingLeft = touchPos.x < lastTouchBorderPosX;
                if ((movingLeft && touchedLeftBorder) || (!movingLeft && !touchedLeftBorder)) return;
                else touchedBorder = false;
            }

            if (touch.phase == TouchPhase.Began)
            {
                DragStart(touchPos);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                UpdatePlayerPos(touchPos);
            }
        }
#endif

#if UNITY_EDITOR
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            //It means clicked on panel. So we do not consider this as click on game Object. Hence returning. 
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos = Input.mousePosition;
            DragStart(touchPos);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 touchPos = Input.mousePosition;
            if (touchedBorder)
            {
                movingLeft = touchPos.x < lastTouchBorderPosX;
                if ((movingLeft && touchedLeftBorder) || (!movingLeft && !touchedLeftBorder)) return;
                else touchedBorder = false;
            }
            UpdatePlayerPos(touchPos);
        }
#endif
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Vector3 touchPos = Vector3.zero;
#if UNITY_ANDROID
            if (Input.touchCount > 0) touchPos = Input.GetTouch(0).position;
#endif
#if UNITY_EDITOR
            touchPos = Input.mousePosition;
#endif
            touchedBorder = true;
            touchedLeftBorder = collision.gameObject.CompareTag(_LeftBorder);
            lastTouchBorderPosX = touchPos.x;
            DragStart(touchPos);
        }
    }

    private void DragStart(Vector3 touchPos)
    {
        Vector2 startPos = Camera.main.ScreenToWorldPoint(touchPos);
        offsetX = transform.position.x - startPos.x * movementMiltiplier;
    }

    private void UpdatePlayerPos(Vector3 touchPos)
    {
        Vector2 movetPos = Camera.main.ScreenToWorldPoint(touchPos);
        rb.MovePosition(new Vector2(movetPos.x * movementMiltiplier + offsetX, rb.position.y));
    }

}
