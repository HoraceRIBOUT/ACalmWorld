using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonHighlighted : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private void OnMouseEnter()
    {
       
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontStyle &= ~FontStyles.Underline;
    }
}
