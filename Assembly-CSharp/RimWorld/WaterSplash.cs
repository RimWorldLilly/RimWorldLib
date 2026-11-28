using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B49 RID: 6985
	public class WaterSplash : Projectile
	{
		// Token: 0x0600B358 RID: 45912 RVA: 0x003F3544 File Offset: 0x003F1744
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			base.Impact(hitThing, blockedByShield);
			List<Thing> list = new List<Thing>();
			foreach (Thing thing in base.Map.thingGrid.ThingsAt(base.Position))
			{
				if (thing.def == ThingDefOf.Fire)
				{
					list.Add(thing);
				}
			}
			foreach (Thing thing2 in list)
			{
				thing2.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
