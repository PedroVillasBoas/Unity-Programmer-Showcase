using UnityEngine;
using TriInspector;
using System.Linq;

namespace GoodVillageGames.Core.Util.GameFeel
{
    /// <summary>
    /// A seamless, infinitely repeating parallax effect using child sprites.
    /// The parent container moves smoothly, while the children snap to create the loop.
    /// </summary>
    public class ParallaxLayer : MonoBehaviour
    {
        [Title("Target")]
        [SerializeField] private Transform _cameraTransform;

        [Title("Parallax Effect")]
        [SerializeField] private Vector2 _parallaxFactor;

        private Vector3 _lastCameraPosition;
        private float _spriteWidth;
        private Transform[] _childSprites;

        private void Start()
        {
            if (_cameraTransform == null)
            {
                Debug.LogError("ParallaxLayer: Camera Transform is not assigned!", this);
                enabled = false;
                return;
            }

            _lastCameraPosition = _cameraTransform.position;

            _childSprites = GetComponentsInChildren<SpriteRenderer>().Select(s => s.transform).ToArray();
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                _spriteWidth = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit * spriteRenderer.transform.lossyScale.x;
            }
            else
            {
                Debug.LogError("ParallaxLayer: No SpriteRenderer found in children. Cannot calculate width.", this);
                enabled = false;
            }
        }

        private void LateUpdate()
        {
            // Calculating how much the camera has moved since the last frame
            Vector3 cameraDelta = _cameraTransform.position - _lastCameraPosition;
            transform.position += new Vector3(cameraDelta.x * _parallaxFactor.x, cameraDelta.y * _parallaxFactor.y, 0);
            _lastCameraPosition = _cameraTransform.position;

            // Checking each child to see if it has moved too far off-screen
            foreach (var child in _childSprites)
            {
                if (_cameraTransform.position.x - child.position.x > _spriteWidth * 1.5f)
                {
                    child.position += new Vector3(_childSprites.Length * _spriteWidth, 0, 0);
                }
                else if (child.position.x - _cameraTransform.position.x > _spriteWidth * 1.5f)
                {
                    child.position -= new Vector3(_childSprites.Length * _spriteWidth, 0, 0);
                }
            }
        }
    }
}