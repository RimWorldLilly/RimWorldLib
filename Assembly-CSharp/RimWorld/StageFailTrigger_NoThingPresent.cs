using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001674 RID: 5748
	public class StageFailTrigger_NoThingPresent : StageFailTrigger
	{
		// Token: 0x06009036 RID: 36918 RVA: 0x0034F524 File Offset: 0x0034D724
		public override bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus)
		{
			if (this.onlyIfTargetIsOfDef != null)
			{
				Thing thing = ritual.selectedTarget.Thing;
				if (((thing != null) ? thing.def : null) != this.onlyIfTargetIsOfDef)
				{
					return false;
				}
			}
			Thing thing2 = ritual.selectedTarget.Thing;
			if (thing2 == null || thing2.Destroyed)
			{
				return true;
			}
			using (List<Thing>.Enumerator enumerator = thing2.Map.listerBuldingOfDefInProximity.GetForCell(thing2.Position, (float)this.maxDistance, this.thingDef, null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetRoom(RegionType.Set_All) == thing2.GetRoom(RegionType.Set_All))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06009037 RID: 36919 RVA: 0x0034F5E8 File Offset: 0x0034D7E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.onlyIfTargetIsOfDef, "onlyIfTargetIsOfDef");
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.maxDistance, "maxDistance", 0, false);
		}

		// Token: 0x04005800 RID: 22528
		public ThingDef onlyIfTargetIsOfDef;

		// Token: 0x04005801 RID: 22529
		public int maxDistance = 12;

		// Token: 0x04005802 RID: 22530
		public ThingDef thingDef;
	}
}
