using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangeEventArgs> OnSelectedCounterChange;

    public class OnSelectedCounterChangeEventArgs : EventArgs
    {
        public ClearCounter _selectedCounter;
    }
    
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 10f;
    
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    
    private bool _isWalking;
    private Vector3 _lastInteractDir;
    
   [SerializeField] private ClearCounter _selectedCounter;


   private void Awake()
   {
       Instance = this;
   }
   
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking()
    {
        return _isWalking;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnOnInteractAction;
    }

    private void GameInputOnOnInteractAction(object sender, EventArgs e)
    {
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact();
        }
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir =  new Vector3(inputVector.x, 0, inputVector.y);
    
        float moveDistance = moveSpeed * Time.deltaTime;
        const float playerRadius = .7f;
        const float playerHeight = 2f;
            
        bool bCanMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDir,moveDistance);

        if (!bCanMove)
        {
            //Cannot Move Towards moveDir
            
            //Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            bCanMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirX,moveDistance);

            if (bCanMove)
            {
                //can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                //cannot move only on the X
                
                //Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0,0,moveDir.z).normalized;
                bCanMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight,playerRadius,moveDirZ,moveDistance);

                if (bCanMove)
                {
                    moveDirZ = moveDirZ;
                }
                else
                {
                    //cannot move in any direction
                }
                
            }
        }
        
        if (bCanMove)
        {
            transform.position += moveDir * (moveSpeed * Time.deltaTime);
        }
        transform.forward = Vector3.Slerp(transform.forward,moveDir,Time.deltaTime * rotateSpeed);
        
        _isWalking = moveDir != Vector3.zero;

    }

    private void HandleInteractions()
    {
        float interactDistance = 2.0f;
        
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir =  new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            _lastInteractDir = moveDir;
        }

        if (Physics.Raycast(transform.position, _lastInteractDir, out RaycastHit raycastHit, interactDistance,countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // Has ClearCounter
                if (clearCounter != _selectedCounter)
                {   
                    SetSlectedCounter(clearCounter);
                }
            }
            else
            {
                SetSlectedCounter(null);
            }
        }
        else
        {
            SetSlectedCounter(null);
        }
    }

    private void SetSlectedCounter(ClearCounter selectedCounter)
    {
        _selectedCounter = selectedCounter;
        OnSelectedCounterChange?.Invoke(this,new OnSelectedCounterChangeEventArgs() { _selectedCounter = selectedCounter });
    }
}
