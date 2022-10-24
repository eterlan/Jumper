using DG.Tweening;
using UnityEngine;

public class AutoRotateHandCard : MonoBehaviour
{
    private Vector2 m_prevMousePos;
    [SerializeField]
    private Vector2 m_mouseDeltaMovement;
    [SerializeField]
    private Vector2 m_remapDeltaMovement;
    
    public Vector2    maxRotation = new(20, 20);
    public float      remapValue  = 10;
    public float      duration    = 0.5f;
    public RotateMode mode;
    public Camera     camera;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 鼠标速度有关
        var mouseSpeed = Input.mouseScrollDelta;
        var mousePos   = (Vector2)Input.mousePosition;
        if (m_prevMousePos != Vector2.zero)
        {
            var wPos = camera.ScreenToWorldPoint(Input.mousePosition);
            // TODO
            wPos.z = 0; 
            
            m_mouseDeltaMovement = mousePos - m_prevMousePos;
            m_remapDeltaMovement = new Vector2(Mathf.Clamp(m_mouseDeltaMovement.x, -remapValue, remapValue), Mathf.Clamp(m_mouseDeltaMovement.y, -remapValue, remapValue));
            
            var targetRotation = new Vector3(m_remapDeltaMovement.y / remapValue * maxRotation.y, -m_remapDeltaMovement.x / remapValue * maxRotation.x, 0);
            // transform.DOKill();

            DOTween.Sequence().Kill();
            DOTween.Sequence().Insert(0, transform.DOMove(wPos, duration)).Insert(0, transform.DORotate(targetRotation, duration, mode));
        }
        m_prevMousePos = Input.mousePosition;
    }
}