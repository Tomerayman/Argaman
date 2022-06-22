using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraRotation : MonoBehaviour
{
    public static CameraRotation Instance { get; private set; }
    public PivotPoint pivot;
    public float mouseRotationSpeed = 3;
    public float mouseScrollSpeed = 3;
    public float damping = 5;
    private Vector2 _prevPos;
    private Vector3 _force;
    private Stack<PivotPoint> _backStack;
    private float _originalDistance;
    
    public bool IsMidTransition { get; private set; }


    private void Awake()
    {
        Instance = this;
        IsMidTransition = false;
        _prevPos = Vector2.zero;
        _force = Vector3.zero;
        _backStack = new Stack<PivotPoint>();
    }

    private void Start()
    {
        transform.parent = pivot.transform;
        transform.position = pivot.spawnPoint.transform.position;
        transform.rotation = pivot.spawnPoint.transform.rotation;
        transform.forward = pivot.transform.position - transform.position;
        transform.eulerAngles += pivot.offset;
        _originalDistance = (transform.position - pivot.transform.position).magnitude;
        pivot.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            OnMouseDrag();
        }
        else
        {
            _prevPos = Vector2.zero;
        }
        ApplyRotation();
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            OnMouseScroll(Input.mouseScrollDelta);
        }
    }

    public void OnMouseDrag()
    {
        if (IsMidTransition) return;
        if (_prevPos == Vector2.zero)
        {
            _prevPos = Input.mousePosition;
            return;
        }
        Vector2 currPos = Input.mousePosition;
        Vector2 v = (currPos - _prevPos) * mouseRotationSpeed * Time.deltaTime;
        _prevPos = currPos;
        _force += new Vector3(v.y, v.x, 0);
    }

    public void ApplyRotation()
    {
        if (IsMidTransition) return;
        if (_force.magnitude < 0.01f)
        {
            _force = Vector3.zero;
            return;
        }
        transform.forward = pivot.transform.position - transform.position;
        transform.eulerAngles += pivot.offset;
        pivot.transform.eulerAngles += _force;
        _force *= damping;
        
        // rotation clamping:
        Vector3 rotation = pivot.transform.eulerAngles;
        float nX = rotation.x > 180 ? rotation.x - 360 : rotation.x;
        float nY = rotation.y > 180 ? rotation.y - 360 : rotation.y;
        float clampedX = pivot.xRange != null ?
            Mathf.Clamp(nX, pivot.xRange.x, pivot.xRange.y)
            : nX;
        float clampedY = pivot.yRange != null ?
            Mathf.Clamp(nY, pivot.yRange.x, pivot.yRange.y):
            nY;
        Vector3 result = new Vector3(clampedX, clampedY, rotation.z);
        pivot.transform.eulerAngles = result;
    }

    public void OnMouseScroll(Vector2 s)
    {
        if (IsMidTransition) return;
        float scroll = -s.normalized.y;
        Vector3 pivotPos = pivot.transform.position;
        Vector3 direction = transform.position - pivotPos;
        float maxDistance = _originalDistance;
        float minDistance = _originalDistance * 0.6f;
        Vector3 add = mouseScrollSpeed * scroll * direction.normalized;
        transform.position += add;
        float distance = Vector3.Distance(transform.position, pivotPos);
        //Debug.Log($"max: {maxDistance}, min: {minDistance}, current: {distance}");
        if (distance > maxDistance) 
            transform.position = pivotPos + direction.normalized * maxDistance;
        else if (distance < minDistance)
            transform.position = pivotPos + direction.normalized * minDistance;
        //_distanceRatio = (distance - minDistance) / (maxDistance - minDistance);
    }

    public void OnClickPivotPoint(PivotPoint p, bool isBack)
    {
        if (IsMidTransition || ReferenceEquals(p, pivot)) return;
        IsMidTransition = true;
        _prevPos = Vector2.zero;
        _force = Vector3.zero;
        Vector3 newPos = p.spawnPoint.transform.position;
        Quaternion newRot = p.spawnPoint.transform.rotation;
        transform.DOMove(newPos, 2);
        transform.DORotateQuaternion(newRot, 2).OnComplete(() =>
        {
            _originalDistance = Vector3.Distance(transform.position, pivot.transform.position);
            IsMidTransition = false;
        });
        transform.parent = p.transform;        
        pivot.Reset();
        p.SetActive(true);
        if (!isBack) _backStack.Push(pivot);
        pivot = p;
    }

    public void OnPressBack()
    {
        if (IsMidTransition || _backStack.Count == 0) return;
        OnClickPivotPoint(_backStack.Pop(), true);
    }
}
