using System;
using System.Linq;
using GradientHair;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace ButterfishHairModdingPlus
{
    class Graphic_Multi_BHair : Graphic_Multi
    {
        private Material[] matsBack = new Material[4];

        public Material BackMatAt(Rot4 rot)
        {
            if (rot.AsInt >= 0 && rot.AsInt <= 3)
            {
                return matsBack[rot.AsInt];
            }
            else
            {
                return BaseContent.BadMat;
            }
        }

        public override void Init(GraphicRequest req)
        {
            bool hasGradient = false;
            Texture2D gradientMask = null;
            Color gradientColor = Color.white;
            if (HarmonyPatches_BHair.loadedGradientHair)
            {
                string[] reqPaths = req.path.Split(':');

                path = reqPaths[0];

                if (reqPaths.Length > 5)
                {
                    if (reqPaths[1] != "0")
                    {
                        hasGradient = true;
                        gradientColor = new Color(float.Parse(reqPaths[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat),
                                                  float.Parse(reqPaths[3], System.Globalization.CultureInfo.InvariantCulture.NumberFormat),
                                                  float.Parse(reqPaths[4], System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
                        gradientMask = ContentFinder<Texture2D>.Get(reqPaths[5], reportFailure: false);
                    }
                }
            }
            else
            {
                path = req.path;
            }

            //-------------------------REPLICATED VANILLA CODE-------------------------
            data = req.graphicData;
            color = req.color;
            colorTwo = req.colorTwo;
            drawSize = req.drawSize;

            //look for front layer textures
            Texture2D[] frontLayerArray = new Texture2D[4];
            frontLayerArray[0] = ContentFinder<Texture2D>.Get(path + "_north", reportFailure: false);
            frontLayerArray[1] = ContentFinder<Texture2D>.Get(path + "_east", reportFailure: false);
            frontLayerArray[2] = ContentFinder<Texture2D>.Get(path + "_south", reportFailure: false);
            frontLayerArray[3] = ContentFinder<Texture2D>.Get(path + "_west", reportFailure: false);
            if (frontLayerArray[0] == null)
            {
                if (frontLayerArray[2] != null)
                {
                    frontLayerArray[0] = frontLayerArray[2];
                    Traverse.Create(this).Field("drawRotatedExtraAngleOffset").SetValue(180f);
                }
                else if (frontLayerArray[1] != null)
                {
                    frontLayerArray[0] = frontLayerArray[1];
                    Traverse.Create(this).Field("drawRotatedExtraAngleOffset").SetValue(-90f);
                }
                else if (frontLayerArray[3] != null)
                {
                    frontLayerArray[0] = frontLayerArray[3];
                    Traverse.Create(this).Field("drawRotatedExtraAngleOffset").SetValue(90f);
                }
                else
                {
                    frontLayerArray[0] = ContentFinder<Texture2D>.Get(path, reportFailure: false);
                }
            }
            if (frontLayerArray[0] == null)
            {
                Log.Error("Failed to find any textures at " + path + " while constructing " + this.ToStringSafe());
                return;
            }
            if (frontLayerArray[2] == null)
            {
                frontLayerArray[2] = frontLayerArray[0];
            }
            if (frontLayerArray[1] == null)
            {
                if (frontLayerArray[3] != null)
                {
                    frontLayerArray[1] = frontLayerArray[3];
                    Traverse.Create(this).Field("eastFlipped").SetValue(DataAllowsFlip);
                }
                else
                {
                    frontLayerArray[1] = frontLayerArray[0];
                }
            }
            if (frontLayerArray[3] == null)
            {
                if (frontLayerArray[1] != null)
                {
                    frontLayerArray[3] = frontLayerArray[1];
                    Traverse.Create(this).Field("westFlipped").SetValue(DataAllowsFlip);
                }
                else
                {
                    frontLayerArray[3] = frontLayerArray[0];
                }
            }

            //-------------------------REPLICATED VANILLA CODE-------------------------



            //look for front layer masks
            Texture2D[] frontLayerMaskArray = new Texture2D[4];
            frontLayerMaskArray[0] = ContentFinder<Texture2D>.Get(path + "_northm", reportFailure: false);
            frontLayerMaskArray[1] = ContentFinder<Texture2D>.Get(path + "_eastm", reportFailure: false);
            frontLayerMaskArray[2] = ContentFinder<Texture2D>.Get(path + "_southm", reportFailure: false);
            frontLayerMaskArray[3] = ContentFinder<Texture2D>.Get(path + "_westm", reportFailure: false);

            Material[] tempMats;
            if (hasGradient)
            {
                tempMats = ProcessGradientMats(req, frontLayerArray, frontLayerMaskArray, gradientMask, gradientColor);
            }
            else
            {
                tempMats = GetMatsFrom(req, frontLayerArray, frontLayerMaskArray);
            }

            //get Materials from textures and set "mats" variable
            Traverse.Create(this).Field("mats").SetValue(tempMats);



            //look for back layer textures
            Texture2D[] backLayerArray = new Texture2D[4];
            backLayerArray[0] = ContentFinder<Texture2D>.Get(path + "_north_back", reportFailure: false);
            backLayerArray[1] = ContentFinder<Texture2D>.Get(path + "_east_back", reportFailure: false);
            backLayerArray[2] = ContentFinder<Texture2D>.Get(path + "_south_back", reportFailure: false);
            backLayerArray[3] = ContentFinder<Texture2D>.Get(path + "_west_back", reportFailure: false);

            //look for back layer masks
            Texture2D[] backLayerMaskArray = new Texture2D[backLayerArray.Length];
            backLayerMaskArray[0] = ContentFinder<Texture2D>.Get(path + "_north_backm", reportFailure: false);
            backLayerMaskArray[1] = ContentFinder<Texture2D>.Get(path + "_east_backm", reportFailure: false);
            backLayerMaskArray[2] = ContentFinder<Texture2D>.Get(path + "_south_backm", reportFailure: false);
            backLayerMaskArray[3] = ContentFinder<Texture2D>.Get(path + "_west_backm", reportFailure: false);

            if (hasGradient)
            {
                matsBack = ProcessGradientMats(req, backLayerArray, backLayerMaskArray, gradientMask, gradientColor);
            }
            else
            {
                //get Materials from textures and set "matsBack" variable
                matsBack = GetMatsFrom(req, backLayerArray, backLayerMaskArray);
            }
        }

        public static Material[] GetMatsFrom(GraphicRequest req, Texture2D[] inputTextureArray, Texture2D[] inputMaskArray)
        {
            Material[] matArray = new Material[inputTextureArray.Length];

            for (int i = 0; i < inputTextureArray.Length; i++)
            {
                if (inputTextureArray[i] != null)
                {
                    Texture2D mask;
                    if (inputMaskArray[i] != null)
                    {
                        mask = inputMaskArray[i];
                    }
                    else
                    {
                        mask = HairMasker.GetDefaultMask(inputTextureArray[i].width, inputTextureArray[i].height);
                    }

                    MaterialRequest tempMatReq = default(MaterialRequest);
                    tempMatReq.mainTex = inputTextureArray[i];
                    tempMatReq.shader = req.shader;
                    tempMatReq.color = req.color;
                    tempMatReq.colorTwo = req.colorTwo;
                    tempMatReq.maskTex = mask;
                    tempMatReq.shaderParameters = req.shaderParameters;

                    matArray[i] = MaterialPool.MatFrom(tempMatReq);
                }
            }

            return matArray;
        }

        public static Material[] ProcessGradientMats(GraphicRequest req, Texture2D[] inputTextures, Texture2D[] inputTextureMasks, Texture2D gradientMask, Color gradientColor)
        {
            //bake textures with only HairColor2 applied via original texture masks
            Color temp = req.color;
            req.color = Color.white;
            Material[] firstPassMats = GetMatsFrom(req, inputTextures, inputTextureMasks);
            for (int i = 0; i < inputTextures.Length; i++)
            {
                inputTextures[i] = BakeTexture(inputTextures[i], firstPassMats[i]);
            }

            //prepare combined gradient masks
            for (int i = 0; i < inputTextures.Length; i++)
            {
                inputTextureMasks[i] = HairMasker.CombineWithGradientMask(inputTextureMasks[i], gradientMask);
            }

            //use combined masks on baked textures with original colors
            req.color = temp;
            req.colorTwo = gradientColor;
            return GetMatsFrom(req, inputTextures, inputTextureMasks);
        }

        public static Texture2D BakeTexture(Texture2D sourceTexture, Material firstPassMat)
        {
            if (sourceTexture != null)
            {
                sourceTexture = Utilities_Texture2D.GetReadableTexture2D(sourceTexture, firstPassMat);
            }

            return sourceTexture;
        }
    }
}
