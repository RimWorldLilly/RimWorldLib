using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016FF RID: 5887
	public class RitualPosition_BesideTree : RitualPosition_ThingDef
	{
		// Token: 0x17001938 RID: 6456
		// (get) Token: 0x0600930C RID: 37644 RVA: 0x0035B7B6 File Offset: 0x003599B6
		protected override ThingDef ThingDef
		{
			get
			{
				return ThingDefOf.Plant_TreeGauranlen;
			}
		}

		// Token: 0x0600930D RID: 37645 RVA: 0x0035B7BD File Offset: 0x003599BD
		public override IntVec3 PositionForThing(Thing t)
		{
			return t.Position - new IntVec3(1, 0, 0);
		}

		// Token: 0x0600930E RID: 37646 RVA: 0x0035B7D2 File Offset: 0x003599D2
		protected override Rot4 FacingDir(Thing t)
		{
			return Rot4.East;
		}
	}
}
