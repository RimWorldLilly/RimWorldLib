using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200202D RID: 8237
	public class LayoutThing
	{
		// Token: 0x040084CD RID: 33997
		public ThingDef thing;

		// Token: 0x040084CE RID: 33998
		public IntVec3 offset;

		// Token: 0x040084CF RID: 33999
		public Rot4 rotation;

		// Token: 0x040084D0 RID: 34000
		public ThingDef filthDef;

		// Token: 0x040084D1 RID: 34001
		public int filthExpandBy;

		// Token: 0x040084D2 RID: 34002
		public float filthChance = 0.5f;
	}
}
