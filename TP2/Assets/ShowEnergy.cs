using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShowEnergy : MonoBehaviour
{
    [SerializeField]
    private Slider m_slider;

    [SerializeField] private EnergyBarSO _energyBarSo;

    private void OnEnable()
    {
        m_slider.minValue = _energyBarSo.MinEnergy;
        m_slider.maxValue = _energyBarSo.MaxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        m_slider.value = _energyBarSo != null ? _energyBarSo.CurrentEnergy : 0;
    }
    
}
