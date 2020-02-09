using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    
    [SerializeField]
    private GameObject tooltip;
    [SerializeField]
    private Text m_Text;

    public string tooltipText;

    private float offset = 20.0f;

    public OffsetVisible m_OffsetVisible;
    public enum OffsetVisible
    {

        Left, 
        Up, 
        Right,
        Down

    }


    private Vector2 pos;
    private Transform m_Transform;



    private void Awake()
    {

        m_Transform = gameObject.transform;

        //offset = gameObject.GetComponent<RectTransform>().rect.width / 2;

        tooltip.SetActive(false);
        
    }    
        
    void Update()
    {

    }

    void OnEnable()
    {

        //OnOffVisibleToolTip();

    }

    private void OnOffVisibleToolTip()
    {

        switch (m_OffsetVisible)
        {

            case OffsetVisible.Left:
                pos = new Vector2(m_Transform.position.x - offset, m_Transform.position.y);
                break;

            case OffsetVisible.Up:
                pos = new Vector2(m_Transform.position.x, m_Transform.position.y - offset);
                break;

            case OffsetVisible.Right:
                pos = new Vector2(m_Transform.position.x + offset, m_Transform.position.y);
                break;

            case OffsetVisible.Down:
                pos = new Vector2(m_Transform.position.x, m_Transform.position.y + offset);
                break;

        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        tooltip.SetActive(true);
        //tooltip.transform.position = new Vector3(pos.x, pos.y, 0.0f);
        m_Text.text = tooltipText;

        LeanTween.scale(tooltip, Vector3.one, 0.2f)
            .setFrom(Vector3.zero)
            .setEase(LeanTweenType.easeInCirc);

    }

    public void OnPointerExit(PointerEventData eventData)
    {

        tooltip.SetActive(false);

    }

}