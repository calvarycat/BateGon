namespace UnityEngine.UI.Extensions
{
    [RequireComponent(typeof(Text), typeof(RectTransform))]
    [AddComponentMenu("UI/Effects/Extensions/Curved Text")]
    public class CurvedText : BaseMeshEffect
    {
        public AnimationCurve curveForText = AnimationCurve.Linear(0, 0, 1, 10);
        public float curveMultiplier = 1;
        private RectTransform _rectTrans;
        private float _timeOffset;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (curveForText[0].time != 0)
            {
                var tmpRect = curveForText[0];
                tmpRect.time = 0;
                curveForText.MoveKey(0, tmpRect);
            }

            if (_rectTrans == null)
                _rectTrans = GetComponent<RectTransform>();

            if (curveForText[curveForText.length - 1].time != _rectTrans.rect.width)
                OnRectTransformDimensionsChange();
        }
#endif
        protected override void Awake()
        {
            base.Awake();
            _rectTrans = GetComponent<RectTransform>();
            OnRectTransformDimensionsChange();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _rectTrans = GetComponent<RectTransform>();
            OnRectTransformDimensionsChange();
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            int count = vh.currentVertCount;

            if (!IsActive() || count == 0)
            {
                return;
            }

            for (int index = 0; index < vh.currentVertCount; index++)
            {
                UIVertex uiVertex = new UIVertex();
                vh.PopulateUIVertex(ref uiVertex, index);

                float time = _rectTrans.rect.width * _rectTrans.pivot.x + uiVertex.position.x;
                time += _timeOffset;
                uiVertex.position.y += curveForText.Evaluate(time) * curveMultiplier;

                vh.SetUIVertex(uiVertex, index);
            }
        }

        protected void Update()
        {
            _timeOffset += Time.deltaTime * _rectTrans.rect.width;
            if (_timeOffset >= _rectTrans.rect.width)
                _timeOffset -= _rectTrans.rect.width;

            graphic.SetVerticesDirty();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            if (_rectTrans == null)
                _rectTrans = GetComponent<RectTransform>();

            var tmpRect = curveForText[curveForText.length - 1];
            tmpRect.time = _rectTrans.rect.width;
            curveForText.MoveKey(curveForText.length - 1, tmpRect);
        }
    }
}