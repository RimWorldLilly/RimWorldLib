using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F4D RID: 8013
	public class SpecialThingFilterWorker_CorpsesEnemy : SpecialThingFilterWorker
	{
		// Token: 0x0600CC6C RID: 52332 RVA: 0x004B5A14 File Offset: 0x004B3C14
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.Faction != null && corpse.InnerPawn.Faction.HostileTo(Faction.OfPlayer);
		}
	}
}
