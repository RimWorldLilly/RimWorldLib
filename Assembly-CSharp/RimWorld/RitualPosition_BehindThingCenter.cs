using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020016F4 RID: 5876
	public class RitualPosition_BehindThingCenter : RitualPosition_VerticalThingCenter
	{
		// Token: 0x060092E3 RID: 37603 RVA: 0x0035AE80 File Offset: 0x00359080
		protected override CellRect GetRect(CellRect thingRect)
		{
			IntVec3 intVec = IntVec3.North + this.offset;
			return new CellRect(thingRect.minX + intVec.x, thingRect.maxZ + intVec.z, thingRect.Width, 1);
		}
	}
}
