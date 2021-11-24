using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum PlusColor
{
    Blue,
    Green,
    Red,
    Yellow
}

public class Plus : MonoBehaviour
{
    [OnValueChanged("CurrentColor")] public PlusColor plusColor;

    [ReorderableList] [SerializeField] private List<Material> materials;

    [SerializeField] private MeshRenderer meshRenderer;

    public void CurrentColor()
    {
        switch (plusColor)
        {
            case PlusColor.Blue:
                meshRenderer.sharedMaterial = materials[0];
                break;
            case PlusColor.Green:
                meshRenderer.sharedMaterial = materials[1];
                break;
            case PlusColor.Red:
                meshRenderer.sharedMaterial = materials[2];
                break;
            case PlusColor.Yellow:
                meshRenderer.sharedMaterial = materials[3];
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}