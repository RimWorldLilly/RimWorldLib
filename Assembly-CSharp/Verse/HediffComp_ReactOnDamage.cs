using System;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200046C RID: 1132
	public class HediffComp_ReactOnDamage : HediffComp
	{
		// Token: 0x170005F8 RID: 1528
		// (get) Token: 0x06001FE5 RID: 8165 RVA: 0x000C0D94 File Offset: 0x000BEF94
		public HediffCompProperties_ReactOnDamage Props
		{
			get
			{
				return (HediffCompProperties_ReactOnDamage)this.props;
			}
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000C0DA1 File Offset: 0x000BEFA1
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.damageDefIncoming == dinfo.Def)
			{
				this.React();
			}
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x000C0DC0 File Offset: 0x000BEFC0
		private void React()
		{
			if (this.Props.createHediff != null)
			{
				BodyPartRecord part = this.parent.Part;
				if (this.Props.createHediffOn != null)
				{
					part = this.parent.pawn.RaceProps.body.AllParts.FirstOrFallback((BodyPartRecord p) => p.def == this.Props.createHediffOn, null);
				}
				this.parent.pawn.health.AddHediff(this.Props.createHediff, part, null, null);
			}
			if (this.Props.vomit)
			{
				this.parent.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
			}
		}
	}
}
