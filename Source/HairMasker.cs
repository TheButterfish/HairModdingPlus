using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace ButterfishHairModdingPlus
{
    [StaticConstructorOnStartup]
    class HairMasker
    {
        public static Dictionary<string, Texture2D> defaultMasksCache = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> customMasksCache = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> gradientMasksCache = new Dictionary<string, Texture2D>();
        public static Dictionary<string, Texture2D> combinedMasksCache = new Dictionary<string, Texture2D>();

        public static Texture2D GetDefaultMask(int width, int height)
        {
            string dimensions = width + "x" + height;

            if (!defaultMasksCache.TryGetValue(dimensions, out Texture2D redMask))
            {
                redMask = new Texture2D(width, height);
                Color[] fillColorArray = redMask.GetPixels();

                for (var i = 0; i < fillColorArray.Length; ++i)
                {
                    fillColorArray[i] = Color.red;
                }

                redMask.SetPixels(fillColorArray);
                redMask.Apply();

                defaultMasksCache.Add(dimensions, redMask);
            }

            return redMask;
        }

        public static Texture2D GetMask(Dictionary<string, Texture2D> cache, Texture2D maskTexture, int width, int height)
        {
            string maskID = maskTexture.name + ":" + width + "x" + height;

            if (!cache.TryGetValue(maskID, out Texture2D mask))
            {
                mask = Utilities_Texture2D.GetReadableTexture2D(maskTexture, width, height);

                cache.Add(maskID, mask);
            }

            return mask;
        }

        public static Texture2D GetMask(Dictionary<string, Texture2D> cache, Texture2D maskTexture)
        {
            return GetMask(cache, maskTexture, maskTexture.width, maskTexture.height);
        }

        public static Texture2D CombineWithGradientMask(Texture2D redBlackMask, Texture2D gradientMask)
        {
            if (redBlackMask == null)
            {
                return gradientMask;
            }
            if (gradientMask == null)
            {
                return redBlackMask;
            }

            string maskComboID = redBlackMask.name + ":" + gradientMask.name;

            if (!combinedMasksCache.TryGetValue(maskComboID, out Texture2D combinedMask))
            {
                redBlackMask = GetMask(customMasksCache, redBlackMask);
                gradientMask = GetMask(gradientMasksCache, gradientMask, redBlackMask.width, redBlackMask.height);

                Color[] redBlackMaskArray = redBlackMask.GetPixels();
                Color[] gradientMaskArray = gradientMask.GetPixels();

                for (var i = 0; i < gradientMaskArray.Length; ++i)
                {
                    if (redBlackMaskArray[i] != Color.red)
                    {
                        if (redBlackMaskArray[i] == Color.black)
                        {
                            gradientMaskArray[i] = Color.black;
                        }
                        else
                        {
                            float limitCap = redBlackMaskArray[i].r;
                            float[] rgb = { gradientMaskArray[i].r, gradientMaskArray[i].g, gradientMaskArray[i].b };
                            float highestVal = rgb.Max();

                            if (highestVal > limitCap)
                            {
                                float rPerStep = gradientMaskArray[i].r / highestVal;
                                float gPerStep = gradientMaskArray[i].g / highestVal;
                                float bPerStep = gradientMaskArray[i].b / highestVal;

                                float difference = highestVal - limitCap;

                                gradientMaskArray[i].r -= (difference * rPerStep);
                                gradientMaskArray[i].g -= (difference * gPerStep);
                                gradientMaskArray[i].b -= (difference * bPerStep);
                            }
                        }
                    }
                }

                combinedMask = new Texture2D(redBlackMask.width, redBlackMask.height, TextureFormat.RGBA32, mipChain: true);
                combinedMask.SetPixels(gradientMaskArray);
                combinedMask.filterMode = FilterMode.Trilinear;
                combinedMask.anisoLevel = 2;
                combinedMask.Apply(updateMipmaps: true);

                combinedMasksCache.Add(maskComboID, combinedMask);
            }
            
            return combinedMask;
        }
    }
}
