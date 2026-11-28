using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FCD RID: 8141
	public class StatWorker_RoomReadingBonus : StatWorker
	{
		// Token: 0x0600CEAA RID: 52906 RVA: 0x004C1628 File Offset: 0x004BF828
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && typeof(Building_Bookcase).IsAssignableFrom(thingDef.thingClass);
		}
	}
}
