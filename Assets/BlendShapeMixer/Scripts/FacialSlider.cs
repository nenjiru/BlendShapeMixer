using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacialSlider : MonoBehaviour
{
    #region VARIABLE
    [SerializeField]
    BlendShapeMixer _shapeMixer = null;

    [SerializeField]
    Slider[] _slider = null;

    BlendShapeMixer.PresetShape[] _presets = null;
    #endregion

    #region UNITY_EVENT
    void Start()
    {
        _presets = _shapeMixer.presets;
        for (int i = 0; i < _presets.Length; i++)
        {
            _slider[i].GetComponentInChildren<Text>().text = _presets[i].name;
        }
    }

    void Update()
    {
        for (int i = 0; i < _presets.Length; i++)
        {
            _slider[i].value = _presets[i].weight;
        }
    }
    #endregion

    #region PRIVATE_METHODS
    #endregion
}