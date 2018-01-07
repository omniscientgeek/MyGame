using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class EditManager : MonoBehaviour
{
	WebCamTexture webCamTexture;
    GameObject btnCrop;

	// Use this for initialization
	void Start ()
	{
        webCamTexture = new WebCamTexture();
		var cube = GameObject.Find ("Cube") as GameObject;
		//cube.
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = webCamTexture;
		renderer.transform.Translate (Vector3.forward /1000000);
		webCamTexture.Play();

        foreach (var current in gameObject.GetComponents<UnityEngine.Cubemap>())
        {
            var foo = current;
        }
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		var foo = Screen.orientation;
		if (Screen.orientation == ScreenOrientation.Landscape) {
			int foo2 = 0;
		}
		int horizontal = 0;     //Used to store the horizontal move direction.
		int vertical = 0;       //Used to store the vertical move direction.


		//Check if Input has registered more than zero touches
		if (Input.touchCount > 0) {
			//Store the first touch detected.
			Touch myTouch = Input.touches [0];

            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                {
                    // Construct a ray from the current touch coordinates
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                    RaycastHit resultInfo;
                    // Create a particle if hit
                    if (Physics.Raycast(ray, out resultInfo))
                    {
						ProcessTouch (resultInfo);
                    }
                }
            }
        }
	}

	void ProcessTouch(RaycastHit hitInfo)
	{
		string btnName = hitInfo.collider.name;

		switch (btnName) {
		case "btnCamera":
			SaveImage ();
			break;
		case "btnExit":
			SceneManager.LoadScene ("Main");
			break;
		case "btnCrop":
			break;
		case "btnPaint":
			break;
		case "btnSave":
			break;
		}
	}

	void SaveImage()
	{
		webCamTexture.Pause ();
		Texture2D destTexture = new Texture2D(webCamTexture.width, webCamTexture.height, TextureFormat.ARGB32, false);

		Color[] textureData = webCamTexture.GetPixels();

		destTexture.SetPixels(textureData);
		destTexture.Apply();
		byte[] pngData = destTexture.EncodeToPNG();
		string absolutePath = Application.dataPath + "/";
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

