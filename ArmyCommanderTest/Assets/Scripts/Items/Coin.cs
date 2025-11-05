using UnityEngine;

public class Coin : MonoBehaviour, ICollectable, IPlaySound
{
    [SerializeField] 
    private TypeOfCoin _type;
    public TypeOfCoin Type => _type;

    public void Collect()
    {
        //something
    }
    
    public void PlaySound()
    {
        //play
    }
}
