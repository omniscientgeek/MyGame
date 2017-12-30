using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class EditManager : MonoBehaviour
{
	WebCamTexture webCamTexture;

	// Use this for initialization
	void Start ()
	{		webCamTexture = new WebCamTexture();
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = webCamTexture;
		webCamTexture.Play();
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		int horizontal = 0;     //Used to store the horizontal move direction.
		int vertical = 0;       //Used to store the vertical move direction.
		var cube = GameObject.Find ("Cube") as GameObject;


		//Check if Input has registered more than zero touches
		if (Input.touchCount > 0) {
			//Store the first touch detected.
			Touch myTouch = Input.touches [0];

			if (myTouch.phase == TouchPhase.Ended) {
				if (myTouch.tapCount > 2) {
					SceneManager.LoadScene ("Main");
				} else {
					SaveImage ();
				}
			}
		}
	
	}
	void SaveImage()
	{
		Texture2D destTexture = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false);

		Color[] textureData = webCamTexture.GetPixels();

		destTexture.SetPixels(textureData);
		destTexture.Apply();
		byte[] pngData = destTexture.EncodeToPNG();
		string absolutePath = "";
		//if(File.Exists(Application.persistentDataPath+"/capturedPic2.png"))
		//if(File.Exists("WebcamSnaps" + "photo.png"))
		if(File.Exists(absolutePath + "surveillanceCapture01.png"))
		{
			//File.Delete(Application.persistentDataPath+"/capturedPic2.png");
			//File.Delete("WebcamSnaps" + "photo.png");
			File.Delete(absolutePath + "surveillanceCapture01.png");
		}
		//File.WriteAllBytes(Application.persistentDataPath+"/capturedPic2.png",pngData);
		//File.WriteAllBytes("WebcamSnaps" + "photo.png",pngData);
		File.WriteAllBytes(absolutePath + "surveillanceCapture01.png", pngData);
		//Debug.Log("pic saved to"+Application.persistentDataPath);

		//Debug.Log("WebcamSnaps");
		Debug.Log("File Saved to Desktop/Surveillance/CamCapture/");

	}
}

