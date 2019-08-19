using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacialController : MonoBehaviour
{
    #region VARIABLE
    [SerializeField]
    BlendShapeMixer _shapeMixer = null;

    [SerializeField]
    float _blinkDuration = 0.16f;

    [SerializeField]
    float _blinkIntervalMin = 4f;

    [SerializeField]
    float _blinkIntervalMax = 8f;

    BlendShapeMixer.PresetShape[] _presets = null;
    float[] _weights = null;
    float _blinkTime = 0f;
    #endregion

    #region UNITY_EVENT
    void Start()
    {
        _presets = _shapeMixer.presets;
        _weights = new float[_presets.Length];
        _blinkTime = Random.Range(_blinkIntervalMin, _blinkIntervalMax);
    }

    void Update()
    {
        _updateBlink();

        for (int i = 1; i < _weights.Length; i++)
        {
            _weights[i] = 0f;
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            _weights[0] = 0f;
            _weights[1] = 1f;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            _weights[0] = 0f;
            _weights[2] = 1f;
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            _weights[0] = 0f;
            _weights[3] = 1f;
        }

        for (int i = 0; i < _presets.Length; i++)
        {
            _presets[i].weight = Mathf.Lerp(_presets[i].weight, _weights[i], 0.3f);
        }
    }
    #endregion

    #region PRIVATE_METHODS
    void _updateBlink()
    {
        for (int i = 1; i < _presets.Length; i++)
        {
            if (_presets[i].weight > 0.01f)
            {
                return;
            }
        }

        _blinkTime -= Time.deltaTime;
        if (_blinkTime <= 0f)
        {
            _weights[0] = 1f;
            _blinkTime = Random.Range(_blinkIntervalMin, _blinkIntervalMax);
            StartCoroutine(_resetBlink(_blinkDuration));
        }
    }

    IEnumerator _resetBlink(float time)
    {
        yield return new WaitForSeconds(time);
        _weights[0] = 0f;
    }
    #endregion
}