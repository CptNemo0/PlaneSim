using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    #region Variables
    [SerializeField] public int nextSceneNumber;
    #endregion

    public void OnClick()
    {
        SceneManager.LoadScene(nextSceneNumber);
    }
}
