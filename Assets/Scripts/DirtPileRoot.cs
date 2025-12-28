using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DirtPileRoot : PausableObject, IPointerClickHandler
{
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySweep();
        Destroy(gameObject);

    }
}
