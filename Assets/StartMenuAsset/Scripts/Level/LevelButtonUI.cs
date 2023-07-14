using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButtonUI : MonoBehaviour
{
    [SerializeField] private LevelSO _levelSO;
    void Start()
    {
        this.gameObject.SetActive(_levelSO.unlock);
    }

}
