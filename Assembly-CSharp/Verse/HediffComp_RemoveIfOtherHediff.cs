using System;

namespace Verse
{
	// Token: 0x02000472 RID: 1138
	public class HediffComp_RemoveIfOtherHediff : HediffComp_MessageBase
	{
		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001FF4 RID: 8180 RVA: 0x000C0FC7 File Offset: 0x000BF1C7
		protected HediffCompProperties_RemoveIfOtherHediff Props
		{
			get
			{
				return (HediffCompProperties_RemoveIfOtherHediff)this.props;
			}
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x000C0FD4 File Offset: 0x000BF1D4
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.ShouldRemove(delta))
			{
				this.Message();
				this.parent.pawn.health.RemoveHediff(this.parent);
			}
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000C1000 File Offset: 0x000BF200
		private bool ShouldRemove(int delta)
		{
			if (base.CompShouldRemove)
			{
				return true;
			}
			foreach (HediffDef def in this.Props.hediffs)
			{
				Hediff firstHediffOfDef = base.Pawn.health.hediffSet.GetFirstHediffOfDef(def, false);
				if (firstHediffOfDef != null && (this.Props.stages == null || this.Props.stages.Value.Includes(firstHediffOfDef.CurStageIndex)) && (this.Props.mtbHours <= 0 || (base.Pawn.IsHashIntervalTick(1000, delta) && Rand.MTBEventOccurs((float)this.Props.mtbHours, 2500f, 1000f))))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040018DC RID: 6364
		private const int MtbRemovalCheckInterval = 1000;
	}
}
