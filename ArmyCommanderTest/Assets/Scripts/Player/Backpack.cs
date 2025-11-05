using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [Header("Anchors")]
    [SerializeField] private Transform _bottomAnchor;
    [SerializeField] private Transform _topAnchor;

    [Header("Stacking settings")]
    [SerializeField] private float _stackOffsetY = 0.2f;
    [SerializeField] private float _topAnchorGap = 0.1f;

    [Header("Movement")]
    [SerializeField] private bool _useSmoothMove = true;
    [SerializeField] private float _moveDurationMs = 300;

    private List<GameObject> _goldStack = new List<GameObject>();
    private List<GameObject> _silverStack = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        Coin coin = other.GetComponent<Coin>();

        if (coin)
        {
            AddCoinAsync(coin).Forget();
        }
    }

    private async UniTask AddCoinAsync(Coin coin)
    {
        if (coin == null) return;

        if (coin.TryGetComponent(out Rigidbody rb)) rb.isKinematic = true;

        if (coin.TryGetComponent(out Collider col)) col.enabled = false;

        if (coin.Type == TypeOfCoin.Silver)
        {
            Vector3 localTarget = new Vector3(0f, _silverStack.Count * _stackOffsetY, 0f);

            coin.transform.SetParent(_bottomAnchor, worldPositionStays: false);

            if (_useSmoothMove)
                await MoveToLocalAsync(coin.transform, localTarget, _moveDurationMs);
            else
                coin.transform.localPosition = localTarget;

            _silverStack.Add(coin.gameObject);

            UpdateTopAnchorPosition(_silverStack.Count);
        }
        else
        {
            UpdateTopAnchorPosition(_goldStack.Count);

            Vector3 localTarget = new Vector3(0, _goldStack.Count * _stackOffsetY, 0);
            coin.transform.SetParent(_topAnchor, worldPositionStays: false);

            if (_useSmoothMove)
                await MoveToLocalAsync(coin.transform, localTarget, _moveDurationMs);
            else
                coin.transform.localPosition = localTarget;

            _goldStack.Add(coin.gameObject);
        }
    }

    private void UpdateTopAnchorPosition(int Count)
    {
        float silverHeight = Mathf.Max(0, (Count) * _stackOffsetY);
        Vector3 desiredLocal = new Vector3(0f, silverHeight + _topAnchorGap, 0f);
        _topAnchor.localPosition = desiredLocal;
    }


    private async UniTask MoveToLocalAsync(Transform coin, Vector3 localTarget, float durationMs)
    {
        Vector3 start = Vector3.zero;
        float elapsed = 0f;
        float duration = Mathf.Max(1f, durationMs) / 1000f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float p = Mathf.Clamp01(elapsed / duration);
            coin.localPosition = Vector3.Lerp(start, localTarget, p);
            await UniTask.Yield();
        }

        coin.localPosition = localTarget;
    }

    public void RemoveLast(TypeOfCoin type)
    {
        if (type == TypeOfCoin.Gold && _goldStack.Count > 0)
        {
            GameObject Silver = _goldStack[_goldStack.Count - 1];
            _goldStack.RemoveAt(_goldStack.Count - 1);
            Destroy(Silver);
        }

        else if (type == TypeOfCoin.Silver && _silverStack.Count > 0)
        {
            GameObject Silver = _silverStack[_silverStack.Count - 1];
            _silverStack.RemoveAt(_silverStack.Count - 1);
            Destroy(Silver);
            UpdateTopAnchorPosition(_silverStack.Count);
        }
    }

    public int GetCountOfCoin(TypeOfCoin Type)
    {
        if(Type == TypeOfCoin.Gold)
        {
            return _goldStack.Count;
        }

        else
        { 
            return _silverStack.Count; 
        }
    }

}