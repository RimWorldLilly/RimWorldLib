using System;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020004C3 RID: 1219
	public class Hediff_PsychicTrance : HediffWithComps
	{
		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x060021F7 RID: 8695 RVA: 0x000C9BEC File Offset: 0x000C7DEC
		public override bool ShouldRemove
		{
			get
			{
				if (base.ShouldRemove)
				{
					return true;
				}
				if (this.pawn.mindState.duty != null && this.pawn.mindState.duty.def == DutyDefOf.PerformHateChant)
				{
					return false;
				}
				Lord lord = this.pawn.GetLord();
				return lord == null || !(lord.LordJob is LordJob_PsychicRitual);
			}
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x000C9C52 File Offset: 0x000C7E52
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Psychic trance"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}
	}
}
