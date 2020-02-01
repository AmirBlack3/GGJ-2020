using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Mesh[] itemMeshs;
    public Material greenMat;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            EditorSceneManager.LoadScene(0);
    }
}
