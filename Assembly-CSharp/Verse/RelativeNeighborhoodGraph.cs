using System;
using System.Collections.Generic;
using DelaunatorSharp;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200020A RID: 522
	public class RelativeNeighborhoodGraph
	{
		// Token: 0x06000F2B RID: 3883 RVA: 0x000527EC File Offset: 0x000509EC
		public RelativeNeighborhoodGraph(Delaunator delaunator)
		{
			foreach (Vector2 key in delaunator.Points)
			{
				this.connections[key] = new List<Vector2>();
			}
			foreach (Vector2 vector in delaunator.Points)
			{
				foreach (Vector2 vector2 in delaunator.Points)
				{
					Edge edge;
					if (!this.connections[vector].Contains(vector2) && RelativeNeighborhoodGraph.TryGetEdge(delaunator, vector, vector2, out edge))
					{
						float weight = RelativeNeighborhoodGraph.GetWeight(edge);
						bool flag = false;
						foreach (Vector2 vector3 in delaunator.Points)
						{
							Edge edge2;
							Edge edge3;
							if (RelativeNeighborhoodGraph.TryGetEdge(delaunator, vector, vector3, out edge2) && RelativeNeighborhoodGraph.TryGetEdge(delaunator, vector3, vector2, out edge3))
							{
								float weight2 = RelativeNeighborhoodGraph.GetWeight(edge2);
								float weight3 = RelativeNeighborhoodGraph.GetWeight(edge3);
								if (Mathf.Max(weight2, weight3) < weight)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							this.connections[vector].Add(vector2);
							this.connections[vector2].Add(vector);
						}
					}
				}
			}
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x00052948 File Offset: 0x00050B48
		private static float GetWeight(Edge edge)
		{
			return (edge.Q - edge.P).magnitude;
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00052970 File Offset: 0x00050B70
		private static bool TryGetEdge(Delaunator delaunator, Vector2 a, Vector2 b, out Edge edge)
		{
			foreach (IEdge edge2 in delaunator.GetEdges())
			{
				if ((edge2.P == a || edge2.Q == a) && (edge2.P == b || edge2.Q == b))
				{
					edge = (Edge)edge2;
					return true;
				}
			}
			edge = default(Edge);
			return false;
		}

		// Token: 0x04000EA3 RID: 3747
		public readonly Dictionary<Vector2, List<Vector2>> connections = new Dictionary<Vector2, List<Vector2>>();
	}
}
