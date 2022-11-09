using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;


public class DayNightTesting : MonoBehaviour
{
    public ReflectionProbe reflectionProbe;
    public RenderTexture target;
    public RenderTexture equi;

    private void Start()
    {
        reflectionProbe.RenderProbe(target);
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
            reflectionProbe.RenderProbe(target);
    }

    [ContextMenu("Export to EXR")]
    void Export()
    {
        // https://docs.unity3d.com/560/Documentation/ScriptReference/Texture2D.EncodeToEXR.html
        //int width = target.width;
        //int height = target.height;

        int width = equi.width;
        int height = equi.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBAFloat, false);

        target.ConvertToEquirect(equi);

        // Read screen contents into the texture
        Graphics.SetRenderTarget(equi);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into the EXR
        byte[] bytes = tex.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat | Texture2D.EXRFlags.CompressZIP);
        File.WriteAllBytes(Application.dataPath + "/HDRI/SavedRenderTexture.exr", bytes);
    }

    [ContextMenu("Just save EQUI")]
    void ExportJustEqui()
    {
        int width = equi.width;
        int height = equi.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBAFloat, false);

        // Read screen contents into the texture
        Graphics.SetRenderTarget(equi);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into the EXR
        byte[] bytes = tex.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat | Texture2D.EXRFlags.CompressZIP);
        File.WriteAllBytes(Application.dataPath + "/HDRI/SavedRenderTexture.exr", bytes);
    }
}
