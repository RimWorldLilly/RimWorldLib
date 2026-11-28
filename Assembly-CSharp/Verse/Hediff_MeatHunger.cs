using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020004B8 RID: 1208
	public class Hediff_MeatHunger : Hediff
	{
		// Token: 0x0600218A RID: 8586 RVA: 0x000C73CB File Offset: 0x000C55CB
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Meat hunger"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x000C73F4 File Offset: 0x000C55F4
		public override void TickInterval(int delta)
		{
			int num = Hediff_MeatHunger.StageIndexToBetrayalMTBHours[this.CurStageIndex];
			if (num > 0 && Rand.MTBEventOccurs((float)num, 2500f, 1f) && this.pawn.IsColonySubhuman)
			{
				this.pawn.SetFaction(Faction.OfEntities, null);
				Find.LetterStack.ReceiveLetter("GhoulBetrayalLabel".Translate(), "GhoulBetrayalText".Translate(this.pawn.Named("PAWN")), LetterDefOf.ThreatBig, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x04001977 RID: 6519
		private static readonly Dictionary<int, int> StageIndexToBetrayalMTBHours = new Dictionary<int, int>
		{
			{
				0,
				0
			},
			{
				1,
				0
			},
			{
				2,
				8
			},
			{
				3,
				2
			}
		};
	}
}
