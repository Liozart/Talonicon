using UnityEngine;

public class Coin : MonoBehaviour
{
    public int amount;

    public Sprite purse;

    public void SetAmount(int m)
    {
        amount = m;
        if (amount > 10)
            GetComponent<SpriteRenderer>().sprite = purse;
    }
}
