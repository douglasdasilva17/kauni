using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnderwaterDepth : MonoBehaviour
{
    [Header("Post Processing Volume")]
    [SerializeField] private Volume postProcessingVolume;

    [Header("Post Processing Profiles")]
    [SerializeField] private VolumeProfile surfacePostProcessing;
    [SerializeField] private VolumeProfile underwaterPostProcessing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            EnableEffects(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            EnableEffects(false);
        }
    }

    private void EnableEffects(bool active)
    {
        if (active)
        {            
            postProcessingVolume.profile = underwaterPostProcessing;
        }
        else
        {            
            postProcessingVolume.profile = null;
        }
    }
}
