using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField, Min(0)] private float _maxDistance;

    void Update()
    {
        if (Physics.Raycast(_lineRenderer.transform.position, _lineRenderer.transform.forward, out RaycastHit hit))
        {
			_lineRenderer.SetPosition(1, Vector3.forward * hit.distance);
		}
		else
        {
			_lineRenderer.SetPosition(1, new Vector3(0, 0, _maxDistance));
		}
    }
}
