using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RereCodeCopy : MonoBehaviour
{
    public void clickBTT()
    {
        CopyText(PlayerPrefs.GetString("refer_code"));
    }
    private void CopyText(string textToCopy)
    {
        TextEditor editor = new TextEditor
        {
            text = textToCopy
        };
        editor.SelectAll();
        editor.Copy();
    }
}
