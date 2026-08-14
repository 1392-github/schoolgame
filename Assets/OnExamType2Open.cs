using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnExamType2Open : MonoBehaviour
{
    [SerializeField] GameObject examType1;
    [SerializeField] GameObject examType2;
    void Start()
    {
        if (ExamManager.openExamType == 2)
        {
            examType1.SetActive(false);
            examType2.SetActive(true);
        }
    }
}
