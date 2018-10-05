using UnityEngine;
using System.Collections;

public class UI2dTkSlider : MonoBehaviour {
    [SerializeField] tk2dSlicedSprite m_TargetSprite;
    [SerializeField] float m_SliderValue = 1;
    Vector2 m_FullSize;
    void Awake()
    {
        this.m_FullSize = m_TargetSprite.dimensions;
    }
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public float SliderValue
    {
       
        set
        {
            m_SliderValue = value > 1 ? 1 : value < 0.001f ? 0 : value;
            Vector2 size = m_FullSize;
            float length = m_FullSize.x * m_SliderValue;
            Vector3 borderLeft = (m_TargetSprite.CurrentSprite.positions[1] - m_TargetSprite.CurrentSprite.positions[0]) * m_TargetSprite.borderLeft;
            Vector3 borderRight = (m_TargetSprite.CurrentSprite.positions[1] - m_TargetSprite.CurrentSprite.positions[0]) * m_TargetSprite.borderRight;
            Vector3 border = borderLeft + borderRight;
            
			size.x = length <= border.x && length != 0 ? border.x : length;
            if(length == 0)
			{
				m_TargetSprite.renderer.enabled = false;
			}
			else
			{
				m_TargetSprite.renderer.enabled = true;
				m_TargetSprite.dimensions = size;
			}
			
			
        }
    }
    public Vector2 FullSize { get { return m_FullSize; } set { m_FullSize = value; } }
}
