using UnityEngine;
using System.Collections;

public class ButtonBehaviour : MonoBehaviour
{
    private bool m_IsTouchDown;
    private bool m_IsTouchUp;
    private bool m_IsPressed;
    private bool m_IsHolding;

    [SerializeField]
    private int m_CoolDown = 30;

    private int m_CoolDownTimeTick;
    private Vector2 m_TouchDownPosition;

    private Vector2 m_HoldingScreenPosition;
    public Vector2 HoldingScreenPosition
    {
        get
        {
            return m_HoldingScreenPosition;
        }
    }

    private Touch m_FingerTouch;
    public Touch FingerTouch
    {
        get
        {
            return m_FingerTouch;
        }
    }

    public bool IsTouchDown
    {
        get
        {
            return m_IsTouchDown;
        }
    }

    public bool IsTouchUp
    {
        get
        {
            return m_IsTouchUp;
        }
    }

    public bool IsPressed
    {
        get
        {
            return m_IsPressed;
        }
    }

    public bool IsHolding
    {
        get
        {
            return m_IsHolding;
        }
    }

    // Use this for initialization
    public virtual void Start()
    {


    }

    // Update is called once per frame
    public virtual void Update()
    {
        m_IsPressed = false;
        m_IsTouchDown = false;
        m_IsTouchUp = false;

        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_CoolDownTimeTick <= 0)
                {
                    m_CoolDownTimeTick = m_CoolDown;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    m_HoldingScreenPosition = Input.mousePosition;
                    m_TouchDownPosition = m_HoldingScreenPosition;
                    RaycastHit raycastHit;
                    Physics.Raycast(ray.origin, ray.direction, out raycastHit, Camera.main.farClipPlane, Physics.kDefaultRaycastLayers);
                    if (raycastHit.collider != null && raycastHit.collider == this.collider)
                    {
                        this.ExecuteTouchDown();
                        m_IsTouchDown = true;
                        m_IsHolding = true;
                    }
                }
            }
            if (m_IsHolding)
            {
                m_HoldingScreenPosition = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (m_IsHolding)
                {
                    m_IsTouchUp = true;
					this.ExecuteTouchUp();
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Vector2 distance = (Vector2)Input.mousePosition - m_TouchDownPosition;
                    m_HoldingScreenPosition = Input.mousePosition;
                    RaycastHit raycastHit;
                    Physics.Raycast(ray.origin, ray.direction, out raycastHit, Camera.main.farClipPlane, Physics.kDefaultRaycastLayers);
                    if (raycastHit.collider != null && raycastHit.collider == this.collider || distance.magnitude < 80)
                    {
                        m_IsPressed = true;
                        this.ExecutePressed();
                    }
                    m_IsHolding = false;
                }
            }
        }
        else
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.touches[i];

                if (touch.phase == TouchPhase.Canceled)
                {
                    m_IsHolding = false;
                    m_IsTouchUp = true;
                    break;
                }

                if (touch.phase == TouchPhase.Began && !m_IsHolding)
                {
                    if (m_CoolDownTimeTick <= 0)
                    {
                        m_CoolDownTimeTick = m_CoolDown;
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        m_HoldingScreenPosition = touch.position;
                        m_TouchDownPosition = m_HoldingScreenPosition;
                        RaycastHit raycastHit;
                        Physics.Raycast(ray.origin, ray.direction, out raycastHit, Camera.main.farClipPlane, Physics.kDefaultRaycastLayers);
                        if (raycastHit.collider != null && raycastHit.collider == this.collider)
                        {
                            m_FingerTouch = touch;
                            m_IsTouchDown = true;
                            m_IsHolding = true;
                            this.ExecuteTouchDown();
                        }
                    }
                }
                if (m_IsHolding && m_FingerTouch.fingerId == touch.fingerId)
                {
                    m_FingerTouch = touch;
                    m_HoldingScreenPosition = m_FingerTouch.position;
                }
                if (touch.phase == TouchPhase.Ended && m_FingerTouch.fingerId == touch.fingerId)
                {
                    if (m_IsHolding)
                    {
                        m_IsTouchUp = true;
						this.ExecuteTouchUp();
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        /////
                        Vector2 distance = touch.position - m_TouchDownPosition;
                        m_HoldingScreenPosition = touch.position;
                        RaycastHit raycastHit;
                        Physics.Raycast(ray.origin, ray.direction, out raycastHit, Camera.main.farClipPlane, Physics.kDefaultRaycastLayers);
                        if (raycastHit.collider != null && raycastHit.collider == this.collider || distance.magnitude < 80)
                        {
                            m_IsPressed = true;
                            this.ExecutePressed();
                        }
                        m_IsHolding = false;
                    }
                }
            }
        }
        m_CoolDownTimeTick--;
    }
	
	public virtual void ExecutePressed()
	{
	}
	
	public virtual void ExecuteTouchDown()
	{
	}
	
	public virtual void ExecuteTouchUp()
	{
	}
}
