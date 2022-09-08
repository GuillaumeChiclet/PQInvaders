using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterStock : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private int stockMax = 10;

    [SerializeField] private RectTransform lifeMask;
    [SerializeField] private Vector2 lifeMinMax = new Vector2(0, 140);

    private int stock = 0;

    private void Awake()
    {
        stock = stockMax;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyGathering>(out EnemyGathering enemy))
        {
            enemy.StartGathering(this);
        }
    }

    public void SuccessGather()
    {
        if (stock <= 0)
            return;

        stock -= 1;
        stock = Mathf.Clamp(stock, 0, stockMax);

        lifeMask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(lifeMinMax.x, lifeMinMax.y, ((float)stock / (float)stockMax)));

        if (stock == 0)
        {
            gameManager.GameOver();
        }
    }

    
}
