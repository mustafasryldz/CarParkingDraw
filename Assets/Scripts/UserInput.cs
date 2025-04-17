using UnityEngine;
using UnityEngine.Events;
public class UserInput : MonoBehaviour
{
    public UnityAction OnMouseDown;
    public UnityAction OnMouseMove;
    public UnityAction OnMouseUp;

    private bool isMouseDown;

    private void Update()
    {
        /*if (Input.GetMouseButton(0))
        {
            isMouseDown = true;
            OnMouseDown?.Invoke();
        }

        if (isMouseDown)
        {
            OnMouseMove?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            OnMouseUp?.Invoke();
        }*/
#if UNITY_EDITOR || UNITY_STANDALONE
        // Bilgisayarda fare kullanýmý
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            OnMouseDown?.Invoke();
        }

        if (isMouseDown)
        {
            OnMouseMove?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            OnMouseUp?.Invoke();
        }
#elif UNITY_ANDROID || UNITY_IOS                           
        // Mobil cihazlarda dokunma kullanýmý
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isMouseDown = true;
                    OnMouseDown?.Invoke();
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (isMouseDown)
                        OnMouseMove?.Invoke();
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isMouseDown = false;
                    OnMouseUp?.Invoke();
                    break;
            }
        }
#endif
    }
}
