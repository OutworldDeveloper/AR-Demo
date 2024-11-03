using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class QR : MonoBehaviour
{

    [SerializeField] private Renderer _renderer;

    public void Setup(Texture2D texture)
    {
        _renderer.material.mainTexture = texture;
    }

}
