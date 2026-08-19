using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalLightHandler : MonoBehaviour
{
    [SerializeField] private BakeData _day;
    [SerializeField] private BakeData _night;

    //private void OnEnable()
    //{
    //    EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    //}

    //private void OnDisable()
    //{
    //    EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    //}

    //private void OnPlayModeStateChanged(PlayModeStateChange state)
    //{
    //    if (state == PlayModeStateChange.ExitingPlayMode)
    //    {
    //        SetDay();
    //    }
    //}


    public void SetDay()
    {
        ApplyBake(_day);
    }

    public void SetNight()
    {
        ApplyBake(_night);
    }

    private void ApplyBake(BakeData data)
    {
        // 1. Cambia el APV
        ProbeReferenceVolume.instance.lightingScenario = data.scenarioName;

        // 2. Cambia los Lightmaps
        var lightmaps = new LightmapData[data.colorMaps.Length];

        for (int i = 0; i < lightmaps.Length; i++)
        {
            lightmaps[i] = new LightmapData
            {
                lightmapColor = data.colorMaps[i],
                lightmapDir = data.directionMaps[i]
            };
        }

        LightmapSettings.lightmaps = lightmaps;

        // 3. Actualiza el entorno
        DynamicGI.UpdateEnvironment();
    }

   
}

[System.Serializable]
public class BakeData
{
    public string scenarioName;

    public Texture2D[] colorMaps;
    public Texture2D[] directionMaps;
}