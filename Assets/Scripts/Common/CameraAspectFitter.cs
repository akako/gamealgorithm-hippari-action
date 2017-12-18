using UnityEngine;

namespace HippariAction.Common
{
    /// <summary>
    /// カメラを常に指定の画角で映すようにします
    /// </summary>
    public class CameraAspectFitter : MonoBehaviour
    {
        [SerializeField] Vector2 size = new Vector2(540f, 960f);
        [SerializeField] float pixelPerUnit = 1f;

        Camera cameraCache;

        void Awake()
        {
            cameraCache = GetComponent<Camera>();
            Fit();
        }

        void Fit()
        {
            var isWideAspectThanScreen = size.x / size.y > Screen.width / Screen.height;
            var scale = isWideAspectThanScreen ? (size.x / Screen.width) : (size.y / Screen.height);
            var fillRatioX = isWideAspectThanScreen ? 1f : (size.x / Screen.width / scale);
            var fillRatioY = isWideAspectThanScreen ? (size.y / Screen.height / scale) : 1f;
            cameraCache.orthographic = true;
            cameraCache.orthographicSize = size.y / 2f / pixelPerUnit;
            cameraCache.rect = new Rect((1f - fillRatioX) / 2f, (1f - fillRatioY) / 2f, fillRatioX, fillRatioY);
        }

#if UNITY_EDITOR
        void Update()
        {
            Fit();
        }
#endif
    }
}