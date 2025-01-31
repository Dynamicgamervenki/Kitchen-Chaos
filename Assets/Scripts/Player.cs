using UnityEngine;

public class Player : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 10f;
    
    private bool _isWalking;
    
    private void Update()
    {
        Vector2 inputVector = Vector2.zero;
        
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
          inputVector.y -= 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
          inputVector.x -= 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
           inputVector.x += 1;
        }
        
        inputVector = inputVector.normalized;
        
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
        transform.forward = Vector3.Slerp(transform.forward,moveDir,Time.deltaTime * rotateSpeed);
        
        _isWalking = moveDir != Vector3.zero;
        
    }

    public bool IsWalking()
    {
        return _isWalking;
    }
}
