using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class Menufy : MonoBehaviour
{
    public string SongBaseDir = "Assets//simfile";
    private string[] songs;

    public string song;

    // Use this for initialization
    void Start()
    {
        songs = Directory.GetFiles(SongBaseDir, "*.sm", SearchOption.AllDirectories);

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) selection++;
        if (Input.GetKeyDown(KeyCode.UpArrow)) selection--;

        selection = U.pmod(selection, songs.Length);
        if (Input.GetKeyDown(KeyCode.Return)) SelectSong();
    }
    private int selection = 0;
    // Update is called once per frame
    void OnGUI()
    {
        selection = GUILayout.SelectionGrid(selection, songs.Select(Path.GetFileNameWithoutExtension).ToArray(), 1);
        if (GUILayout.Button("Go"))
            SelectSong();

    }

    void SelectSong()
    {
        song = songs[selection];
        print(song+", I choose you!");
        //Application.LoadLevel("game");
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("game");
        //var main = Object.FindObjectOfType<main>();
        //print(main);
        
        //var main2 = GameObject.Find("main");
        //print(main2);
    }
}
