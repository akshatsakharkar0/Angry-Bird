using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconHandler : MonoBehaviour
{
    [SerializeField] private Image[] icons;
    [SerializeField] private UnityEngine.Color usedColour;

    public void UseShot(int shotNumber){
        for(int i=0 ; i < icons.Length ; i++){
            if (shotNumber == i + 1){
                icons[i].color = usedColour;
                return;
            }
        }
    }
}
