using UnityEngine;

public class TurretShooting : MonoBehaviour
{
    [SerializeField] private Transform _muzzle;
    [SerializeField] private LineRenderer _laser;
    [SerializeField] private AudioSource _audioSource;

    [Space(5)]
    [SerializeField, Min(1)] private float _fireRate;
    [SerializeField, Min(1)] private int _shotCount;
    [SerializeField, Min(0)] private float _damage;
    [SerializeField, Min(0)] private float _cooldown;
    [SerializeField, Range(0, 15)] private float _spreadAngle;

    [Space(5)]
    [SerializeField] private ParticleSystem _muzzleFlashPrefab;
    [SerializeField] private GameObject _hole;

    private float _pause;
    private int _leftShots;

    private void Start()
    {
		_leftShots = _shotCount;
	}

    private void Update()
    {
        if (_pause > 0)
        {
            _pause -= Time.deltaTime;
            if (_pause < 0 && !(_leftShots < _shotCount))
                _laser.enabled = true;
            else
                return;
        }

        if (Physics.Raycast(_muzzle.position, _muzzle.forward, out RaycastHit hit) && hit.transform == Player.Instance.transform || _leftShots < _shotCount)
        {
            _laser.enabled = false;
            if (--_leftShots <= 0)
            {
                _pause = _cooldown;
                _leftShots = _shotCount;
            }
            else
            {
                _pause = 1f / _fireRate;
            }

            Vector3 directionWithSpread = GunBase.GetDirectionWithSpread(_spreadAngle, _muzzle.forward);
            if (Physics.Raycast(_muzzle.position, directionWithSpread, out RaycastHit h))
                if (h.transform.TryGetComponent(out IHittable hittable))
                    hittable.Hit(_damage);
                else
                    Instantiate(_hole, h.point, Quaternion.LookRotation(h.normal, Vector3.up), h.transform);

            if (_muzzleFlashPrefab)
				Instantiate(_muzzleFlashPrefab, _muzzle.position, _muzzle.rotation);

            _audioSource?.Play();
        }
	}
}
