using UnityEngine;
using System.Collections;

public class Arrow_Movement : MonoBehaviour {

    public GameObject arrowBack;

    private GameObject gameManager;
    private Step_Generator stepGen;
    // private Score_Handler scoreHandler;
    private bool scoreApplied = false;

    public direction dir;
    public enum direction {  left, down, up, right };

    private const float strumOffset = 6*0.075f;
    private const float despawnTime = 1.5f;

	// Use this for initialization
	void Start () {

        pad = FindObjectOfType<SerialPad>();

        switch (GetComponent<SpriteRenderer>().sprite.name)
        {
            case "arrowsheet_down":
                dir = direction.down;
                break;
            case "arrowsheet_left":
                dir = direction.left;
                break;
            case "arrowsheet_right":
                dir = direction.right;
                break;
            case "arrowsheet_up":
                dir = direction.up;
                break;
        }
	
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    // Update is called once per frame
                    //directin facing   0(north)        1 (East)            2(South)        3(West)
    private direction[] dirLUTup =   {  direction.up,   direction.left,     direction.down, direction.right};
    private direction[] dirLUTright ={  direction.right,direction.up,       direction.left, direction.down};
    private direction[] dirLUTdown = {  direction.down,   direction.right,     direction.up, direction.left};
    private direction[] dirLUTleft = {  direction.left,   direction.down,     direction.right, direction.up};

    public SerialPad pad;

    void Update() {
        float arrowSpeed = Step_Generator.arrowSpeed;
        Vector3 tempPos = transform.position;
        tempPos.y -= arrowSpeed * Time.deltaTime;
        transform.position = tempPos;

        //down takes out right
        //up left
        //left down
        //162
        //3 5
        // 4


        int openCyl = gameManager.GetComponent<Step_Generator>().openCylinderNum;
            //Debug.Log(dirLUTup[openCyl]);
        /*if (Input.GetKeyDown(KeyCode.JoystickButton5))// && dir == dirLUTup[openCyl])
        {
                    //    Debug.Log("UP PRESS!   ");
                        Debug.Log("UP PRESS!   "+dirLUTup[openCyl] + "," + openCyl+","+dir);
                        // Debug.Log(dirLUTup[openCyl]);
        }*/
        if (pad.justPressedDown[SerialPad.UpPort]  && dir == dirLUTup[openCyl])
        {
            CheckLocation();
        }

        if (pad.justPressedDown[SerialPad.LeftPort] && dir == dirLUTleft[openCyl])
        {
            CheckLocation();
        }
        if (pad.justPressedDown[SerialPad.DownPort] && dir == dirLUTdown[openCyl])
        {
            CheckLocation();
        }
        if (pad.justPressedDown[SerialPad.RightPort] && dir == dirLUTright[openCyl])
        {
            CheckLocation();
        }
        /*if (Input.GetKeyDown(KeyCode.JoystickButton4) && dir == direction.left)
        {
            Debug.Log("LEFT PRESS!");
            CheckLocation();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton3) && dir == direction.down)
        {
                        Debug.Log("DOWN PRESS!");

            CheckLocation();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton5) && dir == direction.up)
        {
                        Debug.Log("UP PRESS!");
            CheckLocation();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton2) && dir == direction.right)
        {
                        Debug.Log("RIGHT PRESS!");
            CheckLocation();
        }*/

        //Missed
        if (transform.position.y < arrowBack.transform.position.y - strumOffset)
        {
            GetComponent<Renderer>().material.SetColor("_Color", new Color(0.5f, 0.0f, 0.0f));
            StartCoroutine(DespawnArrow());
            if (!scoreApplied)
            {
                scoreApplied = true;
                // scoreHandler.SendMessage("LoseScore");
            }
        }
    }

    void CheckLocation()
    {
        if (Mathf.Abs(transform.position.y - arrowBack.transform.position.y) <= strumOffset)
        {
            //arrowBack.GetComponent<Animator>().SetBool("isLit", true);
            //scoreHandler.SendMessage("AddScore");
            Destroy(this.gameObject);
            //Debug.Log("AddScore");
        }
    }

    IEnumerator DespawnArrow()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(this.gameObject);
    }
}
