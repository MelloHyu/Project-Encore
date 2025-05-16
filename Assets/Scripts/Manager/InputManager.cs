using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; } //Singleton to be used in other scipts
    public InputSystem_Actions PlayerActions { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //keep this object even on reload
            PlayerActions = new InputSystem_Actions(); //Create a new input action instance
            PlayerActions.Enable();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
