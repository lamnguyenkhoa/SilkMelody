using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public GameObject keyboardText;
    public GameObject xboxText;
    public GameObject ps4Text;

    [SerializeField] private string[] connectedDevices;
    public bool Xbox_One_Controller = false;
    public bool PS4_Controller = false;

    private void Start()
    {
        xboxText.SetActive(false);
        ps4Text.SetActive(false);
        keyboardText.SetActive(true);
    }

    private void Update()
    {
        connectedDevices = Input.GetJoystickNames();

        // Some gamepad/joystick connected. But we only support PS4 and Xbox
        if (connectedDevices.Length > 0)
        {
            for (int i = 0; i < connectedDevices.Length; i++)
            {
                if (connectedDevices[i].Length == 19)
                {
                    PS4_Controller = true;
                    Xbox_One_Controller = false;
                }
                else if (connectedDevices[i].Length == 33)
                {
                    PS4_Controller = false;
                    Xbox_One_Controller = true;
                }
                else
                {
                    PS4_Controller = false;
                    Xbox_One_Controller = false;
                }
            }
        }
        else
        {
            // Nothing connected. Use keyboard

            PS4_Controller = false;
            Xbox_One_Controller = false;
        }

        if (Xbox_One_Controller)
        {
            xboxText.SetActive(true);
            ps4Text.SetActive(false);
            keyboardText.SetActive(false);
        }
        else if (PS4_Controller)
        {
            xboxText.SetActive(false);
            ps4Text.SetActive(true);
            keyboardText.SetActive(false);
        }
        else
        {
            xboxText.SetActive(false);
            ps4Text.SetActive(false);
            keyboardText.SetActive(true);
        }
    }
}
