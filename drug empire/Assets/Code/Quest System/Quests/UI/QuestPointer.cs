using System;
using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;
using UnityEngine.UI;


namespace QuestingSystem.UI
{
    public class QuestPointer : MonoBehaviour
    {
        [SerializeField] private RectTransform _pointerRect;
        [SerializeField] private Image _pointerImage;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private Sprite _arrowSprite;
        [SerializeField] private Sprite _exclamationSprite;
        [SerializeField] private Vector3 _houseOffset;
        
        private Player _player;
        private HouseEnter _house;
        private Vector3 _targetPosition;
        
        

        private void Start()
        {
            if(QuestsCollection.Instance.CurrentReceiver == null) _pointerRect.gameObject.SetActive(false);
            _player = FindObjectOfType<Player>();
            _house = FindObjectOfType<HouseEnter>();
        }

        private void Update()
        {
            if (!_player.InHouse)
            {
                if (QuestsCollection.Instance.CurrentReceiver != null)
                {
                    if (!_pointerRect.gameObject.activeSelf) _pointerRect.gameObject.SetActive(true);

                    _targetPosition = QuestsCollection.Instance.CurrentReceiver.transform.position;
                    UpdatePointer();

                }
                else
                {
                    if (!_pointerRect.gameObject.activeSelf) _pointerRect.gameObject.SetActive(true);

                    _targetPosition = _house.transform.position + _houseOffset;
                    UpdatePointer();
                }
            }
            else _pointerRect.gameObject.SetActive(false);

        }

        private void UpdatePointer()
        {
            float borderSize = 150f;
            
            Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(_targetPosition);
            bool isOffScreen = targetPositionScreenPoint.x <= borderSize ||
                               targetPositionScreenPoint.x >= Screen.width - borderSize
                               || targetPositionScreenPoint.y <= borderSize ||
                               targetPositionScreenPoint.y >= Screen.height - borderSize;

                    if (isOffScreen)
                    {
                        RotatePointer();
                        _pointerRect.localScale = new Vector3(1f, 1f, 1f);
                        _pointerImage.sprite = _arrowSprite;

                        Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
                        if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
                        if (cappedTargetScreenPosition.x >= Screen.width - borderSize)
                            cappedTargetScreenPosition.x = Screen.width - borderSize;
                        if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
                        if (cappedTargetScreenPosition.y >= Screen.height - borderSize)
                            cappedTargetScreenPosition.y = Screen.height - borderSize;

                        Vector3 pointWorldPosition = _uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
                        _pointerRect.position = pointWorldPosition;
                        _pointerRect.localPosition =
                            new Vector3(_pointerRect.localPosition.x, _pointerRect.localPosition.y, 0f);
                    }
                    else
                    {
                        _pointerRect.localScale = new Vector3(0.35f, 1f, 1f);
                        _pointerImage.sprite = _exclamationSprite;

                        Vector3 pointWorldPosition = _uiCamera.ScreenToWorldPoint(targetPositionScreenPoint);
                        _pointerRect.position = pointWorldPosition;
                        _pointerRect.localPosition =
                            new Vector3(_pointerRect.localPosition.x, _pointerRect.localPosition.y, 0f);

                        _pointerRect.transform.localEulerAngles = Vector3.zero;
                    }
        }
        
        private void RotatePointer()
        {
            Vector3 fromPosition = Camera.main.transform.position;
            Vector3 toPosition = _targetPosition;

            fromPosition.z = 0f;

            Vector3 direction = (toPosition - fromPosition).normalized;

            float angle = GetAngleFromVector(direction);
            _pointerRect.transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
        
        private float GetAngleFromVector(Vector3 direction) 
        {
            direction = direction.normalized;
            float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }
        
    }
}