using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F49 RID: 8009
	public class SpecialThingFilterWorker_CorpsesSlave : SpecialThingFilterWorker
	{
		// Token: 0x0600CC64 RID: 52324 RVA: 0x004B5910 File Offset: 0x004B3B10
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && corpse.InnerPawn.Faction == Faction.OfPlayer && corpse.InnerPawn.IsSlave;
		}
	}
}
