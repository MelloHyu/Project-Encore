using UnityEngine;

public class TimerBlock2D : MonoBehaviour
{
    BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.GameState == 1)
        {
            if (boxCollider != null)
            {
                if (PerspectiveStateManager.instance.getPerspectiveState())
                {
                    boxCollider.enabled = false;
                }
                else
                {
                    boxCollider.enabled = true;
                }
            }
        }
    }
}
