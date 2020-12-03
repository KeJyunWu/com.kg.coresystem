using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CommonFunction : MonoBehaviour
{
    public static Vector4 QuaternionToVector4(Quaternion _rotation)
    {
        return new Vector4(_rotation.x, _rotation.y, _rotation.z, _rotation.w);
    }

    public static void Swap<T>(T[] buffer)
    {
        T tmp = buffer[0];
        buffer[0] = buffer[1];
        buffer[1] = tmp;
    }

    public static RenderTexture CreateRT(int _width, int _height, int _depth, int _volume, RenderTextureFormat _format)
    {
        RenderTexture _rt = new RenderTexture(_width, _height, _depth, _format);
        _rt.dimension = UnityEngine.Rendering.TextureDimension.Tex3D;
        _rt.volumeDepth = _volume;
        _rt.enableRandomWrite = true;
        _rt.Create();
        return _rt;
    }

    public static RenderTexture CreateRT(int _width, int _height, RenderTextureFormat _format)
    {
        RenderTexture _rt = new RenderTexture(_width, _height, 0, _format);
        _rt.enableRandomWrite = true;
        _rt.Create();
        return _rt;
    }

#if UNITY_EDITOR
    public static void SaveRenderTextureToPNG(RenderTexture _RT, string _path, string _name)
    {
        RenderTexture.active = _RT;
        Texture2D _tex = new Texture2D(_RT.width, _RT.height, TextureFormat.RGBAFloat, false);
        _tex.ReadPixels(new Rect(0, 0, _RT.width, _RT.height), 0, 0);
        RenderTexture.active = null;

        byte[] _bytes = _tex.EncodeToPNG();
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }
        File.WriteAllBytes(_path + _name + ".png", _bytes);
        //AssetDatabase.LoadAssetAtPath();
    }

    public static void SaveAsset(Object _asset, string _path, string _name)
    {
        if (_asset == null)
            return;

        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }

        AssetDatabase.CreateAsset(_asset, _path + _name);
    }
#endif
}


