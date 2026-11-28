using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F4A RID: 8010
	public class SpecialThingFilterWorker_CorpsesStranger : SpecialThingFilterWorker
	{
		// Token: 0x0600CC66 RID: 52326 RVA: 0x004B596C File Offset: 0x004B3B6C
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && (!ModsConfig.AnomalyActive || !(t is UnnaturalCorpse)) && corpse.InnerPawn.Faction != Faction.OfPlayer;
		}
	}
}
