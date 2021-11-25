using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public enum CubeColor
{
    Blue,
    Green,
    Red,
    Yellow
}
public class Cube : MonoBehaviour
{
    [OnValueChanged("CurrentColor")] public CubeColor cubeColor;

    [ReorderableList] [SerializeField] private List<Material> materials;

    [SerializeField] private MeshRenderer meshRenderer;

    public void CurrentColor()
    {
        switch (cubeColor)
        {
            case CubeColor.Blue:
                meshRenderer.sharedMaterial = materials[0];
                break;
            case CubeColor.Green:
                meshRenderer.sharedMaterial = materials[1];
                break;
            case CubeColor.Red:
                meshRenderer.sharedMaterial = materials[2];
                break;
            case CubeColor.Yellow:
                meshRenderer.sharedMaterial = materials[3];
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}