using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001344 RID: 4932
	public class RiverNode
	{
		// Token: 0x04004D0C RID: 19724
		public List<RiverNode> childNodes = new List<RiverNode>();

		// Token: 0x04004D0D RID: 19725
		public Vector3 start;

		// Token: 0x04004D0E RID: 19726
		public Vector3 end;

		// Token: 0x04004D0F RID: 19727
		public float width;

		// Token: 0x04004D10 RID: 19728
		public int seed = Rand.Int;
	}
}
