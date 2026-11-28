using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FEB RID: 4075
	public class PawnCapacityOffset
	{
		// Token: 0x0600670D RID: 26381 RVA: 0x00248892 File Offset: 0x00246A92
		public float GetOffset(float capacityEfficiency)
		{
			return (Mathf.Min(capacityEfficiency, this.max) - 1f) * this.scale;
		}

		// Token: 0x040040A2 RID: 16546
		public PawnCapacityDef capacity;

		// Token: 0x040040A3 RID: 16547
		public float scale = 1f;

		// Token: 0x040040A4 RID: 16548
		public float max = 9999f;
	}
}
