using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TexturedNumber : MonoBehaviour
{
    //  public GridLayoutGroup Group;
    public GameObject Group;
    public GameObject NumberPrefab;
    public int numberScale = 1;
    public GameObject[] prefabsScore;

    private string _value;

  
    public string Value
    {
        get { return _value; }
        set
        {
            _value = value;
            RemoveAllTRanForm(Group.transform);
            for (int i = 0; i < _value.Length; i++)
            {
                int number = int.Parse(_value[i].ToString());
                GameObject numberObject = Instantiate(prefabsScore[number]);
                numberObject.transform.SetParent(Group.transform);
      
                numberObject.transform.localScale = new Vector3(numberScale, numberScale, numberScale);
             

            }
         
        }
    }
    public void RemoveAllTRanForm(Transform group)
    {
        foreach (Transform child in group.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}