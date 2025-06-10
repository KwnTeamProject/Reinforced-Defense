using UnityEngine;
using UnityEngine.UI;

public class TowerSpawner : MonoBehaviour
{
    private float needProductfloat = 5;

    public GameObject needToProduct;
    public Text needToProductText;

    public void TowerSpawn()
    {
        if (MainSystem.mainSystemInstance.byProductCount < needProductfloat)
        {
            needToProduct.SetActive(true);
            needToProductText.text = "부산물이 부족합니다.\n" + "(부족한 부산물 수 : " + Mathf.Abs(needProductfloat - MainSystem.mainSystemInstance.byProductCount).ToString() + ")";

            return;
        }

        TowerManager.Instance.TowerSpawn();
        MainSystem.mainSystemInstance.MinusProduct(needProductfloat);
    }

    public void TowerDeSpawn()
    {
        TowerManager.Instance.TowerDeSpawn();
        MainSystem.mainSystemInstance.PlusProduct(needProductfloat / 5);
    }
}
