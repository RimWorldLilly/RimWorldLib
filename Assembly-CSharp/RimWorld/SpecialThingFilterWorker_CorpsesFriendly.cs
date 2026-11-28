using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F4C RID: 8012
	public class SpecialThingFilterWorker_CorpsesFriendly : SpecialThingFilterWorker
	{
		// Token: 0x0600CC6A RID: 52330 RVA: 0x004B59E0 File Offset: 0x004B3BE0
		public override bool Matches(Thing t)
		{
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.Faction == Faction.OfPlayer;
		}
	}
}
