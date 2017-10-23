using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManagerScript : MonoBehaviour
{
    
    Dictionary<string, KeyCode> inputsDictionary = new Dictionary<string, KeyCode>();
    GameObject buttonActivated = null;
    //[SerializeField] int playerCount = 2;
    [SerializeField] int controllerNumber = 1;
    [SerializeField] bool inputManagerActive;
    [SerializeField] int inputDevice;
    [SerializeField] string horizontalAxisName;
    [SerializeField] string verticalAxisName;
    [SerializeField] string hTurmAxisName;
    [SerializeField] string vTurnAxisName;
    [SerializeField] string button1Name;
    [SerializeField] string pauseButtonName;
    [SerializeField] string button2Name;

    [SerializeField] Transform optionsParent;
    [SerializeField] CharacterController playerController;
    [SerializeField] Transform playerRotationTransform;
    [SerializeField] PlayerMovementInterface movementInterface;
    [SerializeField] PlayerRotationInterface rotationInterface;
    [SerializeField] PlayerCatchReleaseInterface catchInterface;
    [SerializeField] PlayerPauseInterface pauseInterface;

	// Use this for initialization
	void Start ()
    {
        if (Input.GetJoystickNames().Length == 0)
        {
            inputDevice = 0;
            SetDefaultControlsKeyboard();
            SetInterfaceType(new PlayerMovementType1(), new PlayerRotationType2(), new PlayerCatchReleaseType1(), new PlayerPauseType1());
        }
        else if (Input.GetJoystickNames().Length >= 1 && controllerNumber == 1)
        {
            inputDevice = 1;
            SetDefaultControlsController1();
            SetInterfaceType(new PlayerMovementType1(), new PlayerRotationType1(), new PlayerCatchReleaseType1(), new PlayerPauseType1());
        }
        else if (Input.GetJoystickNames().Length == 2 && controllerNumber == 2)
        {
            inputDevice = 2;
            SetDefaultControlsController2();
            SetInterfaceType(new PlayerMovementType1(), new PlayerRotationType1(), new PlayerCatchReleaseType1(), new PlayerPauseType1());
        }
        else if (Input.GetJoystickNames().Length == 1 && controllerNumber == 2)
        {
            inputDevice = 0;
            SetDefaultControlsKeyboard();
            SetInterfaceType(new PlayerMovementType1(), new PlayerRotationType2(), new PlayerCatchReleaseType1(), new PlayerPauseType1());
        }
        //movementInterface = moveType1;
        movementInterface.IdentifyPlayer(playerController);
        rotationInterface.IdentifyPlayer(playerRotationTransform);
        catchInterface.IdentifyCatchBox(playerRotationTransform);
        pauseInterface.IdentifyGameStateManager(GameObject.FindGameObjectWithTag("GameManager").transform);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (inputManagerActive == true)
        {
            if (inputDevice == 0)
            {
                movementInterface.Move(Input.GetKey(inputsDictionary["MoveUp"]), Input.GetKey(inputsDictionary["MoveLeft"]), Input.GetKey(inputsDictionary["MoveDown"]), Input.GetKey(inputsDictionary["MoveRight"])/*Input.GetAxis(horizontalAxisName), Input.GetAxis(verticalAxisName)*/);
                rotationInterface.Rotate(Input.GetAxis(hTurmAxisName), Input.GetAxis(vTurnAxisName));
                catchInterface.Catch(Input.GetKeyDown(inputsDictionary["ThrowAndCatch"]));
                catchInterface.Throw(Input.GetKeyUp(inputsDictionary["ThrowAndCatch"]));
                pauseInterface.Pause(Input.GetKeyDown(inputsDictionary["Pause"]));
            }
            else
            {
                movementInterface.Move(Input.GetAxis(horizontalAxisName), Input.GetAxis(verticalAxisName));
                rotationInterface.Rotate(Input.GetAxis(hTurmAxisName), Input.GetAxis(vTurnAxisName));
                catchInterface.Catch(Input.GetButtonDown(button1Name));
                catchInterface.Throw(Input.GetButtonUp(button1Name));
                pauseInterface.Pause(Input.GetButtonDown(pauseButtonName));
            }
        }
    }

    void OnGUI()
    {
        if (buttonActivated != null)
        {
            //Debug.Log("It is time.");
            Event inputEvent = Event.current;
            //Debug.Log("I pushed: " + Event.current);
            if (inputEvent.isKey)
            {
                inputsDictionary[buttonActivated.name] = inputEvent.keyCode;
                Debug.Log("INPUT: " + inputEvent.keyCode);
                buttonActivated.transform.GetChild(0).GetComponent<Text>().text = inputEvent.keyCode.ToString();
                buttonActivated = null;
            }
        }
    }

    public void ChangeInput(GameObject pressed)
    {
        buttonActivated = pressed;
        //Debug.Log("Button: " + buttonActivated);
    }

    void SetInterfaceType(PlayerMovementInterface moveType, PlayerRotationInterface rotationType, PlayerCatchReleaseInterface catchType, PlayerPauseInterface pauseType)
    {
        movementInterface = moveType;
        rotationInterface = rotationType;
        catchInterface = catchType;
        pauseInterface = pauseType;
    }

    public void SetInputSource(string horizontalAxis, string verticalAxis, string hTurmAxis, string vTurnAxis, string button1, string pauseButton)
    {
        horizontalAxisName = horizontalAxis;
        verticalAxisName = verticalAxis;
        hTurmAxisName = hTurmAxis;
        vTurnAxisName = vTurnAxis;
        button1Name = button1;
        pauseButtonName = pauseButton;
    }

    public void SetInputManagerActive(bool setting)
    {
        inputManagerActive = setting;
    }

    public void SetDefaultControlsKeyboard()
    {
        inputsDictionary.Add("MoveUp", KeyCode.W);
        inputsDictionary.Add("MoveLeft", KeyCode.A);
        inputsDictionary.Add("MoveDown", KeyCode.S);
        inputsDictionary.Add("MoveRight", KeyCode.D);
        inputsDictionary.Add("ThrowAndCatch", KeyCode.Mouse0);
        inputsDictionary.Add("Pause", KeyCode.Escape);
        for (int i = 1; i < optionsParent.childCount; i++)
        {
            optionsParent.GetChild(i).GetChild(0).GetComponent<Text>().text = inputsDictionary[optionsParent.GetChild(i).name].ToString();
        }
    }

    public void SetDefaultControlsController1()
    {

    }

    public void SetDefaultControlsController2()
    {

    }

    public void SetInputDevice()
    {
        //No two input managers can have same device
        //Keyboard, controller 1, controller 2

    }

    public int GetInputDevice()
    {
        return inputDevice;
    }



    void SetHorizontalAxis(string horizontalAxis)
    {
        horizontalAxisName = horizontalAxis;
    }

    void SetVerticalAxis(string verticalAxis)
    {
        verticalAxisName = verticalAxis;
    }

    void SetHTurnAxis(string hTurnAxis)
    {
        hTurmAxisName = hTurnAxis;
    }

    void SetVTurnAxis(string vTurnAxis)
    {
        vTurnAxisName = vTurnAxis;
    }

    void SetButton1(string button1)
    {
        button1Name = button1;
    }

    void SetPauseButton(string pauseButton)
    {
        pauseButtonName = pauseButton;
    }
}
