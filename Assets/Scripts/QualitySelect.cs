using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QualitySelect : MonoBehaviour
{

    public TMP_Dropdown qualitySelectList;
    
    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(qualitySelectList.value);
    }
}
