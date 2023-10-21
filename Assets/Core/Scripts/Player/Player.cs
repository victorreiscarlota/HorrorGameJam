using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInteraction playerInteraction { get; private set; }
    public PlayerMovement playerMovement { get; private set; }
    public PlayerMadness playerMadness { get; private set; }
    public PlayerCameraControl playerCameraControl { get; private set; }
    public HeadBob headBob { get; private set; }
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Animator handAnim;

    private void Start()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
        playerMadness = GetComponent<PlayerMadness>();
        playerCameraControl = GetComponent<PlayerCameraControl>();
        headBob = GetComponent<HeadBob>();
    }


    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Running)
        {
            handAnim.speed = 0;
            return;
        }

        handAnim.speed = 1;
        playerCameraControl.Tick();
        playerMovement.Tick();
        headBob.Tick();

        handAnim.SetFloat("MoveAmount", playerMovement.currentSpeed);
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentGameState != GameState.Running) return;
        playerInteraction.FixedTick();
    }
}