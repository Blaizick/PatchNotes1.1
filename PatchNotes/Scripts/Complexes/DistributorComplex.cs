// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;

// public class DistributorComplexType : ComplexType
// {
//     public float capacity;

//     public override string GetDesc()
//     {

//         return $"Capacity: {capacity}\n{base.GetDesc()}";
//     }
// }

// public class DistributorComplex : Complex
// {
//     public GameObject lineRendererGo;
//     public Dictionary<LineRenderer, Complex> lineRenderers;

//     public DistributorComplexType DistributorComplexType => (DistributorComplexType)type;

//     public Dictionary<DetailType, float> details = new();

//     public override void Init()
//     {
//         base.Init();
//     }

//     public override void Update()
//     {
//         HashSet<LineRenderer> remove = new();
//         foreach (var (k, v) in lineRenderers)
//         {
//             if (v == null)
//             {
//                 remove.Add(k);
//             }
//             else
//             {
//                 k.positionCount = 2;
//                 k.SetPositions(new[]{transform.position, v.transform.position});
//             }
//         }
//         foreach (var i in remove)
//         {
//             lineRenderers.Remove(i);
//             Destroy(i);
//         }
//     }

//     public override void OnPointerClick()
//     {
//         base.OnPointerClick();
//     }

//     public override void Receive(DetailStack stack)
//     {
//         if (!details.ContainsKey(stack.detail))
//         {
//             details[stack.detail] = 0.0f;
//         }
//         details[stack.detail] += stack.count;
//     }

//     public override float GetReceiveCount(DetailStack stack)
//     {
//         if (!details.ContainsKey(stack.detail))
//         {
//             details[stack.detail] = 0.0f;
//         }

//         return Mathf.Clamp(stack.count, 0.0f, DistributorComplexType.capacity - details[stack.detail]);        
//     }
// }