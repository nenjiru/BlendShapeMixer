using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlendShapeMixer : MonoBehaviour
{
    #region VARIABLE
    [SerializeField]
    GameObject _meshRoot = null;

    public bool isActive = true;
    public PresetShape[] presets = null;

    SkinnedMeshRenderer[] _meshs = null;
    #endregion

    #region CLASS_DEFINITION
    [System.Serializable]
    public class PresetShape
    {
        public string name = "";
        [Range(0f, 1f)] public float weight = 0f;
        [HideInInspector] public MeshHandler[] handlers = new MeshHandler[0];
    }

    [System.Serializable]
    public class MeshHandler
    {
        public SkinnedMeshRenderer skinnedMeshRenderer;
        public ShapeTarget[] targets = new ShapeTarget[0];
        [HideInInspector] public int bufferIndex = 0;
    }

    [System.Serializable]
    public class ShapeTarget
    {
        public int index = 0;
        public float weight = 0f;
    }

    class BulendBuffer
    {
        public SkinnedMeshRenderer skinnedMeshRenderer;
        public float[] weight = new float[0];
    }

    List<BulendBuffer> _buffer = new List<BulendBuffer>();
    #endregion

    #region UNITY_EVENT
    void Start()
    {
        Clear();
    }

    void Update()
    {
        if (isActive)
        {
            _updateShape();
        }
    }

    void OnValidate()
    {
        if (_meshRoot == null)
        {
            _meshs = null;
            Debug.LogWarning("Plese attach root object");
            return;
        }

        _meshs = _meshRoot.GetComponentsInChildren<SkinnedMeshRenderer>();
        _updateBuffer();
        _updateShape();
    }
    #endregion

    #region PUBLIC_METHODS
    public void Apply(int index)
    {
        _updateBuffer(presets[index].handlers, 1f);
        _applyShape();
    }

    public void Capture(int index)
    {
        if (_meshs == null)
        {
            Debug.LogWarning("Plese attach mesh root");
            return;
        }
        presets[index].handlers = _captureShape(index);
        _updateBuffer();
    }

    public void Clear()
    {
        for (int i = 0; i < presets.Length; i++)
        {
            presets[i].weight = 0f;
        }
        _updateShape();
    }
    #endregion

    #region PRIVATE_METHODS
    public void _updateBuffer()
    {
        _buffer.Clear();
        for (int i = 0; i < presets.Length; i++)
        {
            presets[i].handlers = _allocate(presets[i].handlers);
        }
    }

    MeshHandler[] _allocate(MeshHandler[] handlers)
    {
        handlers = handlers.Where(v => v.skinnedMeshRenderer != null).ToArray();

        for (int i = 0; i < handlers.Length; i++)
        {
            SkinnedMeshRenderer mesh = handlers[i].skinnedMeshRenderer;
            int meshIndex = -1;

            for (int j = 0; j < _buffer.Count; j++)
            {
                if (mesh.name == _buffer[j].skinnedMeshRenderer.name)
                {
                    meshIndex = j;
                    break;
                }
            }

            if (meshIndex == -1)
            {
                meshIndex = _buffer.Count;
                BulendBuffer buffer = new BulendBuffer();
                buffer.skinnedMeshRenderer = mesh;
                buffer.weight = new float[mesh.sharedMesh.blendShapeCount];
                _buffer.Add(buffer);
            }

            handlers[i].bufferIndex = meshIndex;
        }

        return handlers;
    }

    void _updateShape()
    {
        _resetBuffer();
        for (int i = 0; i < presets.Length; i++)
        {
            _updateBuffer(presets[i].handlers, presets[i].weight);
        }
        _applyShape();
    }

    void _resetBuffer()
    {
        for (int i = 0; i < _buffer.Count; i++)
        {
            float[] weights = _buffer[i].weight;
            for (int j = 0; j < weights.Length; j++)
            {
                weights[j] = -1f;
            }
        }

        for (int i = 0; i < presets.Length; i++)
        {
            _updateBuffer(presets[i].handlers, 0);
        }
    }

    void _updateBuffer(MeshHandler[] handlers, float weight)
    {
        for (int i = 0; i < handlers.Length; i++)
        {
            ShapeTarget[] targets = handlers[i].targets;
            int k = handlers[i].bufferIndex;
            for (int j = 0; j < targets.Length; j++)
            {
                ShapeTarget target = targets[j];
                float v = _buffer[k].weight[target.index] + (target.weight * weight);
                _buffer[k].weight[target.index] = Mathf.Clamp(v, 0f, 100f);
            }
        }
    }

    void _applyShape()
    {
        for (int i = 0; i < _buffer.Count; i++)
        {
            SkinnedMeshRenderer skin = _buffer[i].skinnedMeshRenderer;
            float[] weight = _buffer[i].weight;
            for (int j = 0; j < weight.Length; j++)
            {
                if (weight[j] >= 0f)
                {
                    skin.SetBlendShapeWeight(j, weight[j]);
                }
            }
        }
    }

    MeshHandler[] _captureShape(int index)
    {
        List<MeshHandler> items = new List<MeshHandler>();

        for (int i = 0; i < _meshs.Length; i++)
        {
            Mesh mesh = _meshs[i].sharedMesh;
            MeshHandler handler = new MeshHandler();
            handler.skinnedMeshRenderer = _meshs[i];
            List<ShapeTarget> targets = new List<ShapeTarget>();

            for (int j = 0; j < mesh.blendShapeCount; j++)
            {
                if (_meshs[i].GetBlendShapeWeight(j) > 0)
                {
                    var target = new ShapeTarget();
                    target.index = j;
                    target.weight = _meshs[i].GetBlendShapeWeight(j);
                    targets.Add(target);
                }
            }

            if (targets.Count == 0)
            {
                handler = null;
                targets = null;
            }
            else
            {
                handler.targets = targets.ToArray();
                items.Add(handler);
            }
        }

        return items.ToArray();
    }
    #endregion
}