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
            needToProductText.text = "�λ깰�� �����մϴ�.\n" + "(������ �λ깰 �� : " + Mathf.Abs(needProductfloat - MainSystem.mainSystemInstance.byProductCount).ToString() + ")";

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
