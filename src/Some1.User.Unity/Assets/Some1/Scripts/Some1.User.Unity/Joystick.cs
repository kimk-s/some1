using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Some1.User.Unity
{
    public enum JoystickHandledType : byte
    {
        Down,
        Drag,
        Up,
        Click,
        Cancel
    }

    public readonly struct JoystickHandledEventArgs
    {
        public readonly JoystickHandledType type;
        public readonly float rotation;
        public readonly float magnitude;
        public readonly bool isMagnitudeInClick;

        public JoystickHandledEventArgs(JoystickHandledType type)
            : this(type, default, default, default)
        {

        }

        public JoystickHandledEventArgs(JoystickHandledType type, float rotation, float magnitude, bool isMagnitudeInClick)
        {
            this.type = type;
            this.rotation = rotation;
            this.magnitude = magnitude;
            this.isMagnitudeInClick = isMagnitudeInClick;
        }
    }

    public class Joystick : Selectable, IDragHandler
    {
        public class JoystickHandledEvent : UnityEvent<JoystickHandledEventArgs> { }

        public CanvasGroup canvasGroup;
        public Image outerBackground;
        public Image innerBackground;
        public Image handle;
        public Image realHandle;

        private const float Rad2Deg = (float)(180 / Math.PI);

        private float outerBackgroundRadius;
        private float innerBackgroundRadius;
        private float outerBackgroundOriginColorAlpha;
        private Vector2 outerBackgroundOriginPosition;

        private bool downed;
        private bool canceled;

#pragma warning disable IDE1006 // Naming Styles
        public JoystickHandledEvent onHandle { get; } = new();

        public bool clickable { get; set; }

        public bool handleFollowable { get; set; }

        public float maxMagnitude { get; set; } = 2;

        public bool vibrationOnCancel { get; set; }

        public bool blocksRaycasts
        {
            get => canvasGroup.blocksRaycasts;
            set
            {
                if (canvasGroup.blocksRaycasts == value) return;

                canvasGroup.blocksRaycasts = value;

                outerBackground.color = new Color(
                    outerBackground.color.r,
                    outerBackground.color.g,
                    outerBackground.color.b,
                    value ? outerBackgroundOriginColorAlpha : 0);

                if (!value)
                {
                    UpInternal(default);
                }
            }
        }
#pragma warning restore IDE1006 // Naming Styles

        private float Rotation => ToRotation(handle.rectTransform.anchoredPosition);

        private float Magnitude
        {
            get
            {
                var magnitude = outerBackgroundRadius == 0 ?
                    0 :
                    1 + (realHandle.rectTransform.anchoredPosition.magnitude - outerBackgroundRadius) / outerBackgroundRadius;

                if (magnitude > maxMagnitude)
                {
                    magnitude = maxMagnitude;
                }

                return magnitude;
            }
        }

        private bool IsMagnitudeInClick => handle.rectTransform.anchoredPosition.magnitude < innerBackgroundRadius;

        protected override void Awake()
        {
            base.Awake();

            outerBackgroundRadius = outerBackground.rectTransform.sizeDelta.x * 0.5f;
            innerBackgroundRadius = innerBackground.rectTransform.sizeDelta.x * 0.5f;
            outerBackgroundOriginColorAlpha = outerBackground.color.a;
            outerBackgroundOriginPosition = outerBackground.rectTransform.anchoredPosition;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            DownInternal(eventData.position);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            UpInternal(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            DragInternal(eventData.position);
        }

        public void Clear()
        {
            outerBackground.rectTransform.anchoredPosition = outerBackgroundOriginPosition;
            handle.rectTransform.anchoredPosition = Vector2.zero;
            realHandle.rectTransform.anchoredPosition = Vector2.zero;

            downed = false;
            canceled = false;
        }

        protected virtual void OnHandle(JoystickHandledEventArgs args)
        {
            onHandle.Invoke(args);
        }

        private void DownInternal(Vector2 position)
        {
            if (!interactable)
            {
                return;
            }

            outerBackground.transform.position = position;

            downed = true;

            OnHandle(new JoystickHandledEventArgs(JoystickHandledType.Down, default, default, true));
        }

        private void UpInternal(Vector2 position)
        {
            if (!downed)
                return;

            handle.transform.position = position;
            realHandle.transform.position = position;

            bool click = clickable &&
                !canceled &&
                IsMagnitudeInClick;

            var upArgs = new JoystickHandledEventArgs(
                JoystickHandledType.Up,
                Rotation,
                Magnitude,
                IsMagnitudeInClick);

            Clear();

            if (click)
            {
                OnHandle(new JoystickHandledEventArgs(JoystickHandledType.Click, default, default, true));
            }
            else
            {
                OnHandle(upArgs);
            }
        }

        private void DragInternal(Vector2 position)
        {
            if (!downed)
                return;

            bool prevIsMagnitudeInClick = IsMagnitudeInClick;

            handle.transform.position = position;
            realHandle.transform.position = position;

            if (!prevIsMagnitudeInClick && IsMagnitudeInClick)
            {
                if (clickable)
                {
                    OnHandle(new JoystickHandledEventArgs(JoystickHandledType.Cancel, default, default, true));

                    if (!canceled)
                    {
                        canceled = true;
                    }

                    if (vibrationOnCancel)
                    {
                        //Vibration.VibratePop();
                    }
                }
            }

            if (handle.rectTransform.anchoredPosition.magnitude > outerBackgroundRadius)
            {
                var max = handle.rectTransform.anchoredPosition.normalized * outerBackgroundRadius;

                var over = handle.rectTransform.anchoredPosition - max;

                handle.rectTransform.anchoredPosition -= over;

                if (handleFollowable)
                {
                    outerBackground.rectTransform.anchoredPosition += over;
                }
            }

            if (!clickable || !IsMagnitudeInClick)
            {
                OnHandle(new JoystickHandledEventArgs(
                    JoystickHandledType.Drag,
                    Rotation,
                    Magnitude,
                    IsMagnitudeInClick));
            }
        }

        private static float ToRotation(Vector2 a)
        {
            return Convert180To360Degrees(Mathf.Atan2(a.y, a.x) * Rad2Deg);
        }

        private static float Convert180To360Degrees(float _180Degrees)
        {
            return (_180Degrees + 360) % 360;
        }
    }
}
