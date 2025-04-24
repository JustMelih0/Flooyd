using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossArea : MonoBehaviour
{
    [SerializeField]private List<GameObject> Doors = new();
    [SerializeField]private List<Boss_Health> BossHealth = new();
    [SerializeField]private Transform barsArea;
    private int bossCount = 0;
    private int deathCount = 0;

    void Start()
    {
        barsArea.gameObject.SetActive(false);
        foreach (Boss_Health item in BossHealth)
        {
            bossCount++;
            item.mobDeadAction += BossDead;
            item.healthBarBorder.transform.SetParent(barsArea);
        }
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }  
    }
    void OnDisable()
    {
        foreach (Boss_Health item in BossHealth)
        {
            item.mobDeadAction -= BossDead;
        }
    }
    private void BossDead()
    {
        deathCount++;
        if (deathCount == bossCount)
        {
            AreaClear();
        }
    }
    private void AreaClear()
    {
        foreach (GameObject item in Doors)
        {
            item.SetActive(false);
        }
        BossAreaCompleted();
    }
    private void BossAreaCompleted()
    {
        gameObject.SetActive(false);
        barsArea.gameObject.SetActive(false);
    }
    private void AreaOpened()
    {
        foreach (GameObject item in Doors)
        {
            item.SetActive(true);
        }
        barsArea.gameObject.SetActive(true);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AreaOpened();
        }
    }

}
