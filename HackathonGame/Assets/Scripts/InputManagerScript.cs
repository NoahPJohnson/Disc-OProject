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
    [SerializeField] string horizontalAxisName = "Horizontal1";
    [SerializeField] string verticalAxisName = "Vertical1";
    [SerializeField] string hTurnAxisName = "RightStick X1";
    [SerializeField] string vTurnAxisName = "RightStick Y1";
    [SerializeField] string button1Name = "Fire1Player1";
    [SerializeField] string pauseButtonName = "Pause1";
    [SerializeField] string[] hAxisArray;
    [SerializeField] string[] vAxisArray;
    [SerializeField] string[] buttonArray;


    [SerializeField] Transform optionsParent;
    [SerializeField] CharacterController playerController;
    [SerializeField] Transform playerRotationTransform;
    [SerializeField] PlayerMovementInterface movementInterface;
    [SerializeField] PlayerRotationInterface rotationInterface;
    [SerializeField] PlayerCatchReleaseInterface catchInterface;
    [SerializeField] PlayerPauseInterface pauseInterface;

    [SerializeField] Text controllerNameLabel;
    string defaultControllerLabelText;
    [SerializeField] GameObject selectorObject;
    [SerializeField] GameObject[] buttonSelectorArray;
    [SerializeField] GameObject controllerGroup;
    [SerializeField] GameObject keyboardGroup;

	// Use this for initialization
	void Start ()
    {
        
        if (Input.GetJoystickNames().Length == 0)
        {
            inputDevice = 0;
            selectorObject.GetComponent<SelectIndexScript>().SetIndex(0);
            SetInterfaceType(new PlayerMovementType1(), new PlayerRotationType2(), new PlayerCatchReleaseType1(), new PlayerPauseType1());
        }
        else if (Input.GetJoystickNames().Length >= 1 && controllerNumber == 1)
        {
            if (Input.GetJoystickNames()[0] == "Wireless Controller")
            {
                hAxisArray[1] = "DPadXPS4P1";
                hAxisArray[2] = "RightStickXForPS4P1";
                vAxisArray[1] = "DPadYPS4P1";
                vAxisArray[2] = "RightStickYForPS4P1";
                buttonArray[5] = "PausePS4P1";
            }
            else
            {
                hAxisArray[1] = "HorizontalDPad1";
                hAxisArray[2] = "RightStick X1";
                vAxisArray[1] = "VerticalDPad1";
                vAxisArray[2] = "RightStick Y1";
                buttonArray[5] = "Pause1";
            }
            inputDevice = 1;
            Debug.Log(Input.GetJoystickNames()[0] + " | ");
            selectorObject.GetComponent<SelectIndexScript>().SetIndex(1);
            SetInterfaceType(new PlayerMovementType1(), new PlayerRotationType1(), new PlayerCatchReleaseType1(), new PlayerPauseType1());
        }
        else if (Input.GetJoystickNames().Length == 2 && controllerNumber == 2)
        {
            if (Input.GetJoystickNames()[1] == "Wireless Controller")
            {
                hAxisArray[1] = "DPadXPS4P2";
                hAxisArray[2] = "RightStickXForPS4P2";
                vAxisArray[1] = "DPadYPS4P2";
                vAxisArray[2] = "RightStickYForPS4P2";
                buttonArray[5] = "PausePS4P2";
            }
            else
            {
                hAxisArray[1] = "HorizontalDPad2";
                hAxisArray[2] = "RightStick X2";
                vAxisArray[1] = "VerticalDPad2";
                vAxisArray[2] = "RightStick Y2";
                buttonArray[5] = "Pause2";
            }
            inputDevice = 2;
            selectorObject.GetComponent<SelectIndexScript>().SetIndex(2);
            SetInterfaceType(new PlayerMovementType1(), new PlayerRotationType1(), new PlayerCatchReleaseType1(), new PlayerPauseType1());
        }
        else if (Input.GetJoystickNames().Length == 1 && controllerNumber == 2)
        {
            inputDevice = 0;
            selectorObject.GetComponent<SelectIndexScript>().SetIndex(0);
            SetInterfaceType(new PlayerMovementType1(), new PlayerRotationType2(), new PlayerCatchReleaseType1(), new PlayerPauseType1());
        }
        SetDefaultControls();
        ChangeInputDisplay();
        defaultControllerLabelText = controllerNameLabel.text;
        SetControllerLabelText();
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
                rotationInterface.Rotate(Input.GetAxis(hTurnAxisName), Input.GetAxis(vTurnAxisName));
                catchInterface.Catch(Input.GetKeyDown(inputsDictionary["ThrowAndCatch"]));
                catchInterface.Throw(Input.GetKeyUp(inputsDictionary["ThrowAndCatch"]));
                pauseInterface.Pause(Input.GetKeyDown(inputsDictionary["Pause"]));
            }
            else
            {
                movementInterface.Move(Input.GetAxis(horizontalAxisName), Input.GetAxis(verticalAxisName));
                rotationInterface.Rotate(Input.GetAxis(hTurnAxisName), Input.GetAxis(vTurnAxisName));
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

    public void ChangeButton(int index)
    {
        if (index == 0)
        {
            horizontalAxisName = hAxisArray[buttonSelectorArray[index].GetComponent<SelectIndexScript>().GetIndex()];
            verticalAxisName = vAxisArray[buttonSelectorArray[index].GetComponent<SelectIndexScript>().GetIndex()];
        }
        else if (index == 1)
        {
            hTurnAxisName = hAxisArray[buttonSelectorArray[index].GetComponent<SelectIndexScript>().GetIndex()];
            vTurnAxisName = vAxisArray[buttonSelectorArray[index].GetComponent<SelectIndexScript>().GetIndex()];
        }
        else if (index == 2)
        {
            button1Name = buttonArray[buttonSelectorArray[index].GetComponent<SelectIndexScript>().GetIndex()];
        }
        else if (index == 3)
        {
            pauseButtonName = buttonArray[buttonSelectorArray[index].GetComponent<SelectIndexScript>().GetIndex()];
        }
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
        hTurnAxisName = hTurmAxis;
        vTurnAxisName = vTurnAxis;
        button1Name = button1;
        pauseButtonName = pauseButton;
    }

    public void SetInputManagerActive(bool setting)
    {
        inputManagerActive = setting;
    }

    public void SetDefaultControls()
    {
        if (controllerGroup.transform.parent.gameObject.activeSelf == true || keyboardGroup.transform.parent.gameObject.activeSelf == true)
        {
            if (inputDevice == 0)
            {
                if (inputsDictionary.ContainsKey("MoveUp") == false)
                {
                    inputsDictionary.Add("MoveUp", KeyCode.W);
                    inputsDictionary.Add("MoveLeft", KeyCode.A);
                    inputsDictionary.Add("MoveDown", KeyCode.S);
                    inputsDictionary.Add("MoveRight", KeyCode.D);
                    inputsDictionary.Add("ThrowAndCatch", KeyCode.Mouse0);
                    inputsDictionary.Add("Pause", KeyCode.Escape);
                }
                else
                {
                    inputsDictionary["MoveUp"] = KeyCode.W;
                    inputsDictionary["MoveLeft"] = KeyCode.A;
                    inputsDictionary["MoveDown"] = KeyCode.S;
                    inputsDictionary["MoveRight"] = KeyCode.D;
                    inputsDictionary["ThrowAndCatch"] = KeyCode.Mouse0;
                    inputsDictionary["Pause"] = KeyCode.Escape;
                }
                for (int i = 0; i < optionsParent.childCount; i++)
                {
                    optionsParent.GetChild(i).GetChild(0).GetComponent<Text>().text = inputsDictionary[optionsParent.GetChild(i).name].ToString();
                }
            }
            else
            {
                buttonSelectorArray[0].GetComponent<SelectIndexScript>().SetIndex(0);
                horizontalAxisName = hAxisArray[buttonSelectorArray[0].GetComponent<SelectIndexScript>().GetIndex()];
                verticalAxisName = vAxisArray[buttonSelectorArray[0].GetComponent<SelectIndexScript>().GetIndex()];
                buttonSelectorArray[1].GetComponent<SelectIndexScript>().SetIndex(2);
                hTurnAxisName = hAxisArray[buttonSelectorArray[1].GetComponent<SelectIndexScript>().GetIndex()];
                vTurnAxisName = vAxisArray[buttonSelectorArray[1].GetComponent<SelectIndexScript>().GetIndex()];
                buttonSelectorArray[2].GetComponent<SelectIndexScript>().SetIndex(6);
                button1Name = buttonArray[buttonSelectorArray[2].GetComponent<SelectIndexScript>().GetIndex()];
                buttonSelectorArray[3].GetComponent<SelectIndexScript>().SetIndex(5);
                pauseButtonName = buttonArray[buttonSelectorArray[3].GetComponent<SelectIndexScript>().GetIndex()];
                if (inputDevice == 2)
                {
                    SwitchControllers(true);
                }
                else
                {
                    SwitchControllers(false);
                }
            }
        }
    }

    void SwitchControllers(bool controller1)
    {
        if (controller1 == true)
        {
            controller1 = false;
            if (Input.GetJoystickNames()[1] == "Wireless Controller")
            {
                hAxisArray[1] = "DPadXPS4P1";
                hAxisArray[2] = "RightStickXForPS4P1";
                vAxisArray[1] = "DPadYPS4P1";
                vAxisArray[2] = "RightStickYForPS4P1";
                buttonArray[5] = "PausePS4P1";
            }
            else
            {
                hAxisArray[1] = "HorizontalDPad1";
                hAxisArray[2] = "RightStick X1";
                vAxisArray[1] = "VerticalDPad1";
                vAxisArray[2] = "RightStick Y1";
                buttonArray[5] = "Pause1";
            }
            for (int i = 0; i < buttonArray.Length; i ++)
            {
                buttonArray[i] = buttonArray[i].Remove(buttonArray[i].Length-1);
                buttonArray[i] = buttonArray[i].Insert(buttonArray[i].Length, "2");
            }
            for (int i = 0; i < hAxisArray.Length; i++)
            {
                hAxisArray[i] = hAxisArray[i].Remove(hAxisArray[i].Length-1);
                hAxisArray[i] = hAxisArray[i].Insert(hAxisArray[i].Length, "2");
                vAxisArray[i] = vAxisArray[i].Remove(vAxisArray[i].Length-1);
                vAxisArray[i] = vAxisArray[i].Insert(vAxisArray[i].Length, "2");
            }
            for (int i = 0; i < 4; i++)
            {
                ChangeButton(i);
            }
        }
        else
        {
            controller1 = true;
            if (Input.GetJoystickNames()[0] == "Wireless Controller")
            {
                hAxisArray[1] = "DPadXPS4P1";
                hAxisArray[2] = "RightStickXForPS4P1";
                vAxisArray[1] = "DPadYPS4P1";
                vAxisArray[2] = "RightStickYForPS4P1";
                buttonArray[5] = "PausePS4P1";
            }
            else
            {
                hAxisArray[1] = "HorizontalDPad1";
                hAxisArray[2] = "RightStick X1";
                vAxisArray[1] = "VerticalDPad1";
                vAxisArray[2] = "RightStick Y1";
                buttonArray[5] = "Pause1";
            }
            for (int i = 0; i < buttonArray.Length; i++)
            {
                buttonArray[i] = buttonArray[i].Remove(buttonArray[i].Length-1);
                buttonArray[i] = buttonArray[i].Insert(buttonArray[i].Length, "1");
            }
            for (int i = 0; i < hAxisArray.Length; i++)
            {
                hAxisArray[i] = hAxisArray[i].Remove(hAxisArray[i].Length-1);
                hAxisArray[i] = hAxisArray[i].Insert(hAxisArray[i].Length, "1");
                vAxisArray[i] = vAxisArray[i].Remove(vAxisArray[i].Length-1);
                vAxisArray[i] = vAxisArray[i].Insert(vAxisArray[i].Length, "1");
            }
            for (int i = 0; i < 4; i++)
            {
                ChangeButton(i);
            }
        }
    }

    public void ChangeInputDisplay()
    {
        if (inputDevice == 0 && controllerGroup.activeSelf == true)
        {
            keyboardGroup.SetActive(true);
            controllerGroup.SetActive(false);
        }
        else if (inputDevice != 0 && controllerGroup.activeSelf == false)
        {
            keyboardGroup.SetActive(false);
            controllerGroup.SetActive(true);
        }
    }

    public void ChangeInputDevice()
    {
        inputDevice = selectorObject.GetComponent<SelectIndexScript>().GetIndex();
        SetDefaultControls();
        ChangeInputDisplay();
        SetControllerLabelText();
        if (inputDevice == 0)
        {
            rotationInterface = new PlayerRotationType2();
        }
        else
        {
            rotationInterface = new PlayerRotationType1();
        }
        rotationInterface.IdentifyPlayer(playerRotationTransform);
    }

    public void SetControllerLabelText()
    {
        if (inputDevice > 0)
        {
            controllerNameLabel.text = defaultControllerLabelText + Input.GetJoystickNames()[inputDevice-1];
        }
        else
        {
            controllerNameLabel.text = defaultControllerLabelText + "Keyboard";
        }
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
        hTurnAxisName = hTurnAxis;
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
