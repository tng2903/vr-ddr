using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// Step_Generator is attached to GameManager.
// It has a bunch of publics that contain prefabs and sprites.

public class Step_Generator : MonoBehaviour {

    //prefabs
    public GameObject leftArrow;
    public GameObject downArrow;
    public GameObject upArrow;
    public GameObject rightArrow;

    //sprites
    public GameObject leftArrowBack;
    public GameObject downArrowBack;
    public GameObject upArrowBack;
    public GameObject rightArrowBack;

    private bool isInit = false;
    private int currentMeasure = 0;

    public const float arrowSpeed = 2f;
    private const float distance = 3.0f; // height moving arrows spawn from, how high above 0,0,0
    private const float changeInterval = 12f; //time between changes in seconds

    private Song_Parser.Metadata songData;
    private float measureDuration = 0.0f;
    private float timeAtStartOfMeasure = 0.0f;
    private GameObject heart;
    // private AudioSource heartAudio;
    private AudioSource heartAudio;
    private Song_Parser.difficulties difficulty;
    private Song_Parser.NoteData noteData;

    private Animator leftAnim;
    private Animator downAnim;
    private Animator upAnim;
    private Animator rightAnim;

    private float numChanges = 0;

    private GameObject[] cylinders = new GameObject[4];
    public int openCylinderNum = 0;
    private int[] cylinderOrder = {0, 1, 2, 3, 2, 1};
    private int orderPos = 0;
	// Use this for initialization
	void Start () {
        Debug.Log("Step_Generator init. arrowSpeed="+arrowSpeed);
        heart = GameObject.FindGameObjectWithTag("Player");
        heartAudio = heart.GetComponent<AudioSource>(); // gets audio source on our player sphere

        // get animator components for arrow targets (the outlines)
        leftAnim = leftArrowBack.GetComponent<Animator>();
        downAnim = downArrowBack.GetComponent<Animator>();
        rightAnim = rightArrowBack.GetComponent<Animator>();
        upAnim = upArrowBack.GetComponent<Animator>();
        
        // spawn the 3 other targets
        spawnRotatedOutlines(leftArrowBack);
        spawnRotatedOutlines(rightArrowBack);
        spawnRotatedOutlines(downArrowBack);
        spawnRotatedOutlines(upArrowBack);

        GameObject nCylinder = GameObject.FindGameObjectWithTag("nCylinder");
        GameObject sCylinder = GameObject.FindGameObjectWithTag("sCylinder");
        GameObject eCylinder = GameObject.FindGameObjectWithTag("eCylinder");
        GameObject wCylinder = GameObject.FindGameObjectWithTag("wCylinder");

        cylinders[0] = nCylinder;
        cylinders[1] = eCylinder;
        cylinders[2] = sCylinder;
        cylinders[3] = wCylinder;

        openCylinder(0);
    }
	
    public void InitSteps(Song_Parser.Metadata newSongData)//, Song_Parser.difficulties newDifficulty)
    {
        songData = newSongData;
        isInit = true;
        //Debug.Log("bpm:"+songData.bpm);
        //Debug.Log("Offset:"+songData.offset);
        measureDuration = (60.0f / songData.bpm) * 4.0f; //= length of a measure?
        noteData = songData.challenge;
        //Debug.Log("# measures:"+noteData.bars.Count);
    }

	// Update is called once per frame
	void Update () {
        /*if (Input.GetKeyDown(KeyCode.Joystick1Button2)) {
            Debug.Log("RIGHT PRESS");
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button5)) {
            Debug.Log("UP PRESS");
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button3)) {
            Debug.Log("DOWN PRESS");
        }
        if (Input.GetKeyDown(KeyCode.Joystick1Button4)) {
            Debug.Log("LEFT PRESS");
        }*/

        if (isInit && currentMeasure < noteData.bars.Count) // if isInit and song isn't over
        {
            //Debug.Log(currentMeasure+",\t"+noteData.bars.Count);
            // deceptive var name
            float timeForArrowsToReachTarget = distance / arrowSpeed;
            if (Mathf.Floor(heartAudio.time/changeInterval) > numChanges) {
                //initiate a change!
                Debug.Log("Randomly changing open cylinder!");
                //openRandomCylinder();
                openNextCylinder();
                numChanges = Mathf.Floor(heartAudio.time/changeInterval);
            }
            //Debug.Log("time-tatrt-sdo "+(heartAudio.time - timeForArrowsToReachTarget - songData.offset));
            //Debug.Log("time "+(heartAudio.time));
            //Debug.Log("startofmeasure - md "+(timeAtStartOfMeasure - measureDuration));
            //Debug.Log("startofmeasure "+(timeAtStartOfMeasure));
            //Debug.Log("md "+(measureDuration));
            if (heartAudio.time - timeForArrowsToReachTarget - songData.offset >= (timeAtStartOfMeasure - measureDuration)) // if currentTimeInSong - bullshitValue >= 
            // if ( heartAudio.time >= timeAtStartOfMeasure + timeForArrowsToReachTarget)
            {
                StartCoroutine(spawnNotesForBar(noteData.bars[currentMeasure++]));
                timeAtStartOfMeasure += measureDuration;
            }
        }
	}

    IEnumerator spawnNotesForBar(List<Song_Parser.Notes> bar)
    {
        //Debug.Log(bar.Count);
        //Debug.Log((barTime / bar.Count) - Time.deltaTime);
        for (int i = 0; i < bar.Count; i++)
        {
            if (bar[i].left)
            {
                spawnRotated(leftArrow, leftArrowBack);
                //Debug.Log("L ");
            }
            if (bar[i].down)
            {
                spawnRotated(downArrow, downArrowBack);
                //Debug.Log("D ");
            }
            if (bar[i].up)
            {
                spawnRotated(upArrow, upArrowBack);
                //Debug.Log("U ");
            }
            if (bar[i].right)
            {
                spawnRotated(rightArrow, rightArrowBack);
               //Debug.Log("R ");
            }
            yield return new WaitForSeconds((measureDuration / bar.Count) - Time.deltaTime);
        }
    }

    void openRandomCylinder() {
        int rnd = (int)(Random.value*4);
        if(rnd==4) rnd=3;
        //openCylinder((openCylinderNum+1)%4);
        openCylinder(rnd);
    }
    void openNextCylinder()
    {
        orderPos = (orderPos+1)%cylinderOrder.Length;
        openCylinder(cylinderOrder[orderPos]);
    }
    void openCylinder(int idx) {
        for(int i=0;i<4;i++) {
            if(i!=idx) {
                cylinders[i].SetActive(true);
            }
            else {
                cylinders[i].SetActive(false);
            }
        }
        openCylinderNum = idx;
    }


    void spawnRotatedOutlines(GameObject back) {
        Vector3 spawnPos = new Vector3(back.transform.position.x, back.transform.position.y, back.transform.position.z - 0*0.3f);
        for(int j=1;j<4;j++) {
            GameObject obj = (GameObject)Instantiate(back, spawnPos, Quaternion.identity);
            obj.transform.RotateAround(new Vector3(0,0,0), Vector3.up, 90*j);
        }
    }

    void spawnRotated(GameObject arrow, GameObject back) {
        Vector3 spawnPos = new Vector3(back.transform.position.x, back.transform.position.y + distance, back.transform.position.z - 0*0.3f);
        for(int j=0;j<4;j++) {
            GameObject obj = (GameObject)Instantiate(arrow, spawnPos, Quaternion.identity);
            obj.transform.RotateAround(new Vector3(0,0,0), Vector3.up, 90*j);
            obj.GetComponent<Arrow_Movement>().arrowBack = back;
        }
    }

}
