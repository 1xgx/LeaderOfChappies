using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TriInspector;
using System.Threading;
using System;

public class ShopOfBuildingTmp : MonoBehaviour
{
    [HideInPlayMode]
    [SerializeField]
    private PositionSpawner _positionsSpawner;
    [SerializeField]
    private int _waitingToOperate;
    [SerializeField]
    private int _delayBeforeGrub;
    [SerializeField]
    private int _costGold;
    [SerializeField]
    private List<GameObject> _armyReserved;

    private CancellationTokenSource _cts;
    private Backpack _backpack;
    private int _saveCoins;
    private int _currentCost;
    private TypeOfCoin _typeOfCoin = TypeOfCoin.Gold;
    


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Backpack>(out var backpack))
        {
            _backpack = backpack;
            _cts = new CancellationTokenSource();
            DelayBeforeGrub(_delayBeforeGrub, _cts.Token).Forget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<Backpack>(out var backpack))
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            _backpack = null;
        }
    }

    async UniTask DelayBeforeGrub(int Delay, CancellationToken token)
    {
        try 
        {
            await UniTask.Delay(Delay);
            WaitingToCompletePayment(_waitingToOperate, token).Forget();
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Player exit, operation was cancled");
        }
    }

    async UniTask WaitingToCompletePayment(int Delay, CancellationToken token)
    {
        try
        {
            _currentCost = _costGold;
            int CountOfCoinsinBackpack = _backpack.GetCountOfCoin(_typeOfCoin);
            for(int i = 0;  i < CountOfCoinsinBackpack; i++)
            {
                await UniTask.Delay(Delay);
                _backpack.RemoveLast(_typeOfCoin);
                _saveCoins++;

                if(_saveCoins == _costGold)
                {
                    Debug.Log("Build");
                    _currentCost -= _costGold;
                    _saveCoins -= _costGold;
                    ShowBuild();
                }
            }

            if (_currentCost == 0)
            {
                _currentCost = _costGold;
            }
        }
        catch(OperationCanceledException)
        {
            Debug.Log("Player exit, operation was cancled");
        }
    }

    private void ShowBuild()
    {
        for(int i = 0; i < _armyReserved.Count; i++)
        {
            if (_armyReserved[i].activeInHierarchy == false)
            {
                _armyReserved[i].SetActive(true);
                return;
            }
        }
    }


}
