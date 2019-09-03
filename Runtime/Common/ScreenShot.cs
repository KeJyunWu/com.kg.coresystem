using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour {

	public static Texture2D m_Texture;

	public static Texture2D GameViewScreenshot(int resWidth,int resHeight,Camera myCamera){

		int resWidthN = resWidth;
		int resHeightN = resHeight;
		RenderTexture rt = RenderTexture.GetTemporary(resWidthN, resHeightN, 24);
		myCamera.targetTexture = rt;

		TextureFormat tFormat;
		tFormat = TextureFormat.RGB24;


		m_Texture = new Texture2D(resWidthN, resHeightN, tFormat,false);
		myCamera.Render();
		RenderTexture.active = rt;
		m_Texture.ReadPixels(new Rect(0, 0, resWidthN, resHeightN), 0, 0);
		myCamera.targetTexture = null;
		RenderTexture.active = null; 
		m_Texture.Apply();

        RenderTexture.ReleaseTemporary(rt);
        return m_Texture;
	}
}
