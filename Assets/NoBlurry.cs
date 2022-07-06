using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoBlurry : MonoBehaviour
{
    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
    }
}
