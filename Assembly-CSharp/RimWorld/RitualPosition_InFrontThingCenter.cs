using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F5 RID: 5877
	public class RitualPosition_InFrontThingCenter : RitualPosition_VerticalThingCenter
	{
		// Token: 0x060092E5 RID: 37605 RVA: 0x0035AED0 File Offset: 0x003590D0
		protected override CellRect GetRect(CellRect thingRect)
		{
			IntVec3 intVec = IntVec3.South + this.offset;
			return new CellRect(thingRect.minX + intVec.x, thingRect.minZ + intVec.z, thingRect.Width, 1);
		}
	}
}
