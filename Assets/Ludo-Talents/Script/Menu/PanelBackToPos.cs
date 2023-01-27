using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PanelBackToPos : MonoBehaviour
{
    public void clickBTT()
    {
        CopyText("VikAS");
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
