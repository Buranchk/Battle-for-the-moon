using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    public GameResult Resuts;

    public void RefreshUI()
    {
        Resuts.LoadUI();
    }

}
