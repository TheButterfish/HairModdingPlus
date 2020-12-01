using UnityEngine;

public class Utilities_Texture2D
{
    //static int counter = 0;

    public static Texture2D GetReadableTexture2D(Texture2D source, int newWidth, int newHeight, Material mat = null) //rescales texture to newWidth and newHeight
    {
        source.filterMode = FilterMode.Trilinear;

        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Trilinear;

        RenderTexture.active = rt;
        if (mat != null)
        {
            Graphics.Blit(source, rt, mat);
        }
        else
        {
            Graphics.Blit(source, rt);
        }

        Texture2D nTex = new Texture2D(newWidth, newHeight, TextureFormat.RGBA32, mipChain: true);
        nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        nTex.name = source.name;
        nTex.filterMode = FilterMode.Trilinear;
        nTex.anisoLevel = 2;
        nTex.Apply(updateMipmaps: true);

        GL.Clear(true, true, Color.clear);
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return nTex;
    }

    public static Texture2D GetReadableTexture2D(Texture2D source, Material mat = null)
    {
        return GetReadableTexture2D(source, source.width, source.height, mat);
    }
}