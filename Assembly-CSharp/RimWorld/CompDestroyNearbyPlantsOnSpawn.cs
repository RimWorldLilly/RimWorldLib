using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200185F RID: 6239
	public class CompDestroyNearbyPlantsOnSpawn : ThingComp
	{
		// Token: 0x17001AAB RID: 6827
		// (get) Token: 0x06009C1F RID: 39967 RVA: 0x003837F1 File Offset: 0x003819F1
		public CompProperties_DestroyNearbyPlantsOnSpawn Props
		{
			get
			{
				return (CompProperties_DestroyNearbyPlantsOnSpawn)this.props;
			}
		}

		// Token: 0x06009C20 RID: 39968 RVA: 0x00383800 File Offset: 0x00381A00
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				return;
			}
			foreach (IntVec3 c in GenRadial.RadialCellsAround(this.parent.Position, (float)this.Props.radius, true))
			{
				if (c.InBounds(this.parent.Map))
				{
					List<Thing> thingList = c.GetThingList(this.parent.Map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						Thing thing = thingList[i];
						if (thing is Plant && !thing.def.preventSkyfallersLandingOn)
						{
							thing.Kill(null, null);
						}
					}
				}
			}
		}
	}
}
