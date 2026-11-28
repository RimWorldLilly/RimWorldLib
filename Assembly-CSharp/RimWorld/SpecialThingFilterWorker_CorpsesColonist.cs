using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F48 RID: 8008
	public class SpecialThingFilterWorker_CorpsesColonist : SpecialThingFilterWorker
	{
		// Token: 0x0600CC62 RID: 52322 RVA: 0x004B589C File Offset: 0x004B3A9C
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.def.race.Humanlike && (!ModsConfig.IdeologyActive || !corpse.InnerPawn.IsSlave) && (!ModsConfig.AnomalyActive || !(t is UnnaturalCorpse)) && corpse.InnerPawn.Faction == Faction.OfPlayer;
		}
	}
}
