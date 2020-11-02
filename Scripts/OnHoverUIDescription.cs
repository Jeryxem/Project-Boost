using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnHoverUIDescription : MonoBehaviour
{
    [SerializeField] Text textToShow;

    [TextArea(3, 10)]
    [SerializeField] string description;

    public void ShowDescription()
    {
        textToShow.text = description;
    }

    
}
