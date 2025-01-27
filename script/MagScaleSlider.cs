
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class MagScaleSlider : UdonSharpBehaviour
{
    [SerializeField] private Slider beltSizeSlider;
    [SerializeField] private Transform MagagineBelt;
    void Start()
    {
        UpdateBeltSize();
    }

    public void UpdateBeltSize()
    {
        float scale = beltSizeSlider.value;
        MagagineBelt.localScale = Vector3.one * scale;
    }
}
