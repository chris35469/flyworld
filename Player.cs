using Unity.Cinemachine.Samples;
using UnityEngine;
public class Player : LivingObject
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float holdVal;
    [SerializeField] private GameObject jetPackParticles;

    private float tiltOffset = 0f;
    private Vector2 inputVector;
    private Vector3 inputDir;
    private Vector3 moveDir;
    private void Start()
    {
        Rigidbody rigid = gameObject.GetComponent<Rigidbody>();
        SetRigidBody(rigid);
        gameInput.OnJump += GameInput_OnJump;
    }

    private void GameInput_OnJump(object sender, System.EventArgs e)
    {
        Jump();
    }

    private void Update()
    {
        inputVector = gameInput.GetMovementVectorNormlized();
        inputDir = new Vector3(inputVector.x, 0f, inputVector.y);
        moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir * speed * Time.deltaTime;
        bool _isMoving = moveDir != Vector3.zero;
        UpdateMoveState(_isMoving);
        UpdateFlyState(!IsGrounded());
        transform.rotation = Quaternion.Euler(tiltOffset, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        if (inputDir != Vector3.back)
        {
            // Input direction is not directly backwards

            Vector3 slerpVal = Vector3.Slerp(transform.forward, moveDir, rotationSpeed * Time.deltaTime);
            transform.forward = slerpVal;
        }

        if (Input.GetKey(KeyCode.LeftShift) || gameInput.GetBurstHold())
        {
            // Shift is being held down
            Jump();
            jetPackParticles.SetActive(true);
        }
        else
        {
            jetPackParticles.SetActive(false);
        }
    }
}
