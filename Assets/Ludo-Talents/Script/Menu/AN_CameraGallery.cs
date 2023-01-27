using System;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Storage;
using SA.Android.Camera;
using SA.Android.Gallery;
using System.Collections;
using StansAssets.Foundation.Extensions;
using Firebase.Extensions;
using UnityEngine.Networking;

public class AN_CameraGallery : MonoBehaviour
{
#pragma warning disable 649

    [SerializeField] Image m_sprite;
    [SerializeField] Text _Text;

    [Header("Camera")]
    [SerializeField] Button m_captureAdvanced;


    [Header("Gallery")]
    [SerializeField] Button m_getPictures;

    FirebaseStorage storage;
    StorageReference storageRef;
    // Start is called before the first frame update
    void Start()
    {

        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl(StaticString.storageKey);

        ImageLoader();

        m_captureAdvanced.onClick.AddListener(() =>
        {
            AN_Camera.CaptureImage(PrintCaptureResult);
        });

        m_getPictures.onClick.AddListener(() =>
        {
            var picker = new AN_MediaPicker(AN_MediaType.Image);

            // Defines if multiple images picker is allowed.
            // The default value is < c > false </ c >
            picker.AllowMultiSelect = true;

            // Max thumbnail size that will be transferred to the Unity side.
            // The thumbnail will be resized before it sent.
            // The default value is 512.
            picker.MaxSize = 512;

            // Starts pick media from a gallery flow.
            picker.Show((result) =>
            {
                PrintPickerResult(result);
            });
        });


    }

    void PrintPickerResult(AN_GalleryPickResult result)
    {
        if (result.IsFailed)
        {
            Debug.Log("Picker Filed:  " + result.Error.Message);
            return;
        }

        Debug.Log("Picked media count: " + result.Media.Count);
        foreach (var anMedia in result.Media)
        {
            Debug.Log("an_media.Type: " + anMedia.Type);
            Debug.Log("an_media.Path: " + anMedia.Path);
        }

        result.Media[0].GetThumbnailAsync(ApplyImageToGUI);
    }

    void PrintCaptureResult(AN_CameraCaptureResult result)
    {
        if (result.IsFailed)
        {
            Debug.Log("Filed:  " + result.Error.Message);
            return;
        }

        Debug.Log("result.Media.Type: " + result.Media.Type);
        Debug.Log("result.Media.Path: " + result.Media.Path);

        result.Media.GetThumbnailAsync(ApplyImageToGUI);
    }

    void ApplyImageToGUI(Texture2D image)
    {       
        m_sprite.sprite = image.CreateSprite();
        GameManager.Instance.avatarMy = m_sprite.sprite;
        GameManager.Instance.initMenuScript.ChangeImage();

        byte[] bytes = image.EncodeToPNG();
        
        var newMetadata = new MetadataChange();
        newMetadata.ContentType = "image/png";

        

        StorageReference profile_ref = storageRef.Child($"/profileImages/{PlayerPrefs.GetString("username")}/myprofile.png");

        profile_ref.PutBytesAsync(bytes, newMetadata).ContinueWithOnMainThread((task) =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
                _Text.text = task.Exception.ToString();
            }
            else
            {
                Debug.Log("File Uploaded Successfully!");
                ImageLoader();
                _Text.text = "File Uploaded Successfully!";
              
            }
        });
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
            _Text.text = "Load Image" + request.error;
        }
        else
        {
            _Text.text = "File loaded Successfully!";
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
