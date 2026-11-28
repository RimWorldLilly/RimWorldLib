using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DA3 RID: 7587
	public class MainTabWindow_Wildlife : MainTabWindow_PawnTable
	{
		// Token: 0x170024A5 RID: 9381
		// (get) Token: 0x0600C541 RID: 50497 RVA: 0x00487E0E File Offset: 0x0048600E
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Wildlife;
			}
		}

		// Token: 0x170024A6 RID: 9382
		// (get) Token: 0x0600C542 RID: 50498 RVA: 0x00487E15 File Offset: 0x00486015
		protected override IEnumerable<Pawn> Pawns
		{
			get
			{
				return from p in Find.CurrentMap.mapPawns.AllPawns
				where p.Spawned && (p.Faction == null || p.Faction == Faction.OfInsects) && p.AnimalOrWildMan() && !p.Position.Fogged(p.Map) && !p.IsPrisonerInPrisonCell()
				select p;
			}
		}
	}
}
