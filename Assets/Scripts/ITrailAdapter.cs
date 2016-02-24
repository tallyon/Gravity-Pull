using UnityEngine;

namespace Assets.Scripts
{
    interface ITrailAdapter
    {
        Transform TargetTrailPoint { get; }
        int CurrentTrailPointIndex { get; }
    }
}
