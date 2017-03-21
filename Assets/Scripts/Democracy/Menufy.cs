using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class Menufy : MonoBehaviour
{
    public string SongBaseDir = "Assets//simfile";
    private string[] songs;

    public string songPath;

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
        songPath = songs[selection];
        print(songPath+", I choose you!");
        StartCoroutine(LoadAudio());
    }

    public AudioClip songClip;

    IEnumerator LoadAudio()
    {
        

        var dir = Path.GetDirectoryName(songPath);
        var musicPath = Path.GetFullPath(Directory.GetFiles(dir, "*.ogg")[0]);
        musicPath = new System.Uri(musicPath).AbsoluteUri;
        //TODO fuck the police. may crash.
        print(musicPath);
        print("go go power rangers");
        using (var www = UnityWebRequest.GetAudioClip(musicPath, AudioType.OGGVORBIS))
        {
            yield return www.Send();
            if (www.isError)
            {
                Debug.Log(www.error);
            }
            else
            {
                songClip = DownloadHandlerAudioClip.GetContent(www);

                DontDestroyOnLoad(gameObject);
                SceneManager.LoadScene("game");

                //AudioSource.PlayClipAtPoint(songClip, Vector3.zero);
                // raise da roof!!!
            }
        }
    }
}
