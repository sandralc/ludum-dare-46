using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowDoorPointer : MonoBehaviour
{
    private Vector2 targetPosition;
    private RectTransform pointerRectTransform;

    [SerializeField] private Camera uiCamera;

    public Transform targetObject;

    private void Awake()
    {
        targetPosition = targetObject.position;
        pointerRectTransform = transform.Find("Pointer").GetComponent<RectTransform>();
    }

    private void Update()
    {

        Vector2 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= 0 || targetPositionScreenPoint.x >= Screen.width || targetPositionScreenPoint.y <= 0 || targetPositionScreenPoint.y >= Screen.height;

        if (isOffScreen)
        {
            Vector2 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= 0)
                cappedTargetScreenPosition.x = 0f;
            if (cappedTargetScreenPosition.x >= Screen.width)
                cappedTargetScreenPosition.x = Screen.width;
            if (cappedTargetScreenPosition.y <= 0)
                cappedTargetScreenPosition.y = 0f;
            if (cappedTargetScreenPosition.y >= Screen.height)
                cappedTargetScreenPosition.y = Screen.height;

            Vector2 pointerWorldPosition = Camera.main.ScreenToWorldPoint(cappedTargetScreenPosition);
            pointerRectTransform.position = pointerWorldPosition;
            pointerRectTransform.localPosition = new Vector2(pointerRectTransform.localPosition.x, pointerRectTransform.localPosition.y);

        }
        else
        {
            //Not show
        }

    }
}
