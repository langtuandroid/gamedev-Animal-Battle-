using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Serialization;

public class PauseCameraAB : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam1M;
    [SerializeField] private CinemachineVirtualCamera vcam2M;


    [SerializeField] private bool xX = true;
    private float _lastTime;
    [SerializeField] private float tT = 0;
    [SerializeField] private float xXX;

    private void ChangeCameraA()
    {
        xX = !xX;

        if (xX)
        {
            vcam1M.m_Lens.FieldOfView = Random.Range(30, 50);
            vcam1M.m_Priority = 25;
            FindTarget(vcam1M);
            vcam2M.m_Priority = 10;
        }
        else
        {
            vcam2M.m_Lens.FieldOfView = Random.Range(30, 50);
            vcam2M.m_Priority = 25;
            FindTarget(vcam2M);
            vcam1M.m_Priority = 10;
        }
    }


    private void OnEnable()
    {
        tT = 0;
        _lastTime = Time.realtimeSinceStartup;
        vcam1M.gameObject.SetActive(true);
        vcam2M.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        vcam1M.m_Priority = 10;
        vcam1M.m_Priority = 10;
    }

    private void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - _lastTime;
        tT += deltaTime;

        if (tT > xXX)
        {
            xXX = Random.Range(3.5f, 5.5f);
            tT = 0;
            ChangeCameraA();
        }

        _lastTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    private void FindTarget(CinemachineVirtualCamera vcam)
    {
        GameObject[] t = GameObject.FindGameObjectsWithTag("Knight");
        GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
        if (t.Length > 0 && e.Length > 0)
        {
            int r = Random.Range(0, 2);

            if (r == 0)
            {
                int r2 = Random.Range(0, t.Length);
                vcam.m_Follow = t[r2].transform;
                vcam.m_LookAt = t[r2].transform;
            }
            else
            {
                int r2 = Random.Range(0, e.Length);
                vcam.m_Follow = e[r2].transform;
                vcam.m_LookAt = e[r2].transform;
            }
        }
        else if (t.Length > 0)
        {
            int r2 = Random.Range(0, t.Length);
            vcam.m_Follow = t[r2].transform;
            vcam.m_LookAt = t[r2].transform;
        }
        else
        {
            int r2 = Random.Range(0, e.Length);

            vcam.m_Follow = e[r2].transform;
            vcam.m_LookAt = e[r2].transform;
        }
    }
}