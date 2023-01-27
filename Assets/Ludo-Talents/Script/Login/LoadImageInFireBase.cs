using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Storage;
using System.Collections;
using Firebase.Extensions;
using UnityEngine.Networking;
using StansAssets.Foundation.Extensions;

public class LoadImageInFireBase : MonoBehaviour
{
	Image m_sprite;

	FirebaseStorage storage;
	StorageReference storageRef;
	// Start is called before the first frame update

	void Start()
    {
		storage = FirebaseStorage.DefaultInstance;
		storageRef = storage.GetReferenceFromUrl(StaticString.storageKey);
		ImageLoader();
	}

    public void ImageLoader()
    {
        StorageReference profile_ref = storageRef.Child($"/profileImages/{PlayerPrefs.GetString("username")}/myprofile.png");

        profile_ref.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                StartCoroutine(LoadImage(Convert.ToString(task.Result))); //Fetch file from the link
            }
            else
            {
                Debug.Log(task.Exception);
            }
        });
    }

    IEnumerator LoadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl); //Create a request

        yield return request.SendWebRequest(); //Wait for the request to complete

        if (request.result != UnityWebRequest.Result.Success) //(request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
          
        }
        else
        {
           
            ApplyImageToGUI1(((DownloadHandlerTexture)request.downloadHandler).texture);

        }
    
    }
    void ApplyImageToGUI1(Texture2D image)
	{

		//m_sprite is a UnityEngine.UI.Image
		m_sprite.sprite = image.CreateSprite();
		GameManager.Instance.avatarMy = m_sprite.sprite;
		GameManager.Instance.initMenuScript.ChangeImage();

	}
}
