using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class main : MonoBehaviour {

    [DllImport("user32.dll")]
    private static extern void FolderBrowserDialog();

    private AudioSource audioSource;

    private string currentSongPath;

    private bool songLoaded = false;
    public Song_Parser.Metadata meta;

    //not currently used
    public const float offsetSeconds = 0.8f;
    public const int sampleRate = 44100;
    // todo:
    // make it play audio
    // make it play steps
    // make it get kb input

	// Use this for initialization
	void Start () {
        // System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
		// System.Windows.Forms.DialogResult result = fbd.ShowDialog();
        // Debug.Log(fbd.SelectedPath);

		// load game directory
        // Game_Data.songDirectory = "C:\\Users\\Jacqueline Liang\\Desktop\\simfile";
        // Game_Data.validSongDir = true;
	        // Debug.Log(Game_Data.songDirectory);

        // set up audiosource
        audioSource = GetComponent<AudioSource>();

        // Song_Parser.Metadata meta = Parse(); // the simfile
        Song_Parser parser = new Song_Parser();
        // Song_Parser.Metadata meta = parser.Parse("Dir: D:\\ddrvr\\ddrvr\\Assets\\simfile | Amount: 0");
        //Song_Parser.Metadata 
        // meta = parser.Parse("D:\\ddrvr\\ddrvr\\Assets\\simfile\\Eros and Apollo\\eros.sm");
        meta = parser.Parse("Assets\\simfile\\cheatcodes\\cheatcodes.sm");
       	// meta = parser.Parse("D:\\ddrvr\\ddrvr\\Assets\\simfile\\Vinyl\\Vinyl.sm");
        //audioSource.PlayDelayed(meta.offset); // can pas a delay an an argument
        GameObject manager = GameObject.FindGameObjectWithTag("GameManager");
        manager.GetComponent<Step_Generator>().InitSteps(meta);//, Game_Data.difficulty);
        /*int i;
        for (i=0;i<meta.challenge.bars.Count;i++)
        {
        	Debug.Log(""+meta.challenge.bars[i][0].up);
        }*/
        audioSource.Play();
        //audioSource.PlayDelayed(300000);
        StartCoroutine(stopDelay(60f));

        //Song_Parser.NoteData notes = parser.ParseNotes("D:\\ddrvr\\ddrvr\\Assets\\simfile\\Vinyl\\Lone Digger.sm");
        // StartCoroutine(LoadTrack(meta.musicPath, meta));

        // start the game

	}
	
	IEnumerator stopDelay(float t)
	{
		yield return new WaitForSeconds(t);
	    audioSource.Stop();
	}
	// safe to delete, not called
    Song_Parser.Metadata Parse() // calls Song_Parser.cs
    {
        Debug.Log("Parsing");

        // get all simfiles in a FileInfo[] array
        DirectoryInfo info = new DirectoryInfo(Game_Data.songDirectory);
        FileInfo[] smFiles = info.GetFiles("*.sm", SearchOption.AllDirectories);
        Debug.Log("Parsing Dir: " + Game_Data.songDirectory + " | Amount: " + smFiles.Length);

            Song_Parser parser = new Song_Parser();
            Song_Parser.Metadata songData = parser.Parse(Game_Data.songDirectory);
            return songData;
    } // end parse fxn

	
	// Update is called once per frame
	void Update () {
		
	}
}
