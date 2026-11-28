using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FCF RID: 8143
	public class StatWorker_SurgerySuccessChanceFactor : StatWorker
	{
		// Token: 0x0600CEAE RID: 52910 RVA: 0x004C1784 File Offset: 0x004BF984
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			Def def = req.Def;
			if (!(def is ThingDef))
			{
				return false;
			}
			ThingDef thingDef = def as ThingDef;
			return typeof(Building_Bed).IsAssignableFrom(thingDef.thingClass);
		}
	}
}
