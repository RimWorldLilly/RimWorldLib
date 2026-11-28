using System;

namespace Verse
{
	// Token: 0x02000464 RID: 1124
	public class HediffComp_Pollution : HediffComp
	{
		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06001FCC RID: 8140 RVA: 0x000C082A File Offset: 0x000BEA2A
		public HediffCompProperties_Pollution Props
		{
			get
			{
				return (HediffCompProperties_Pollution)this.props;
			}
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x000C0838 File Offset: 0x000BEA38
		public override void CompPostTick(ref float severityAdjustment)
		{
			Pawn pawn = this.parent.pawn;
			if (!pawn.IsHashIntervalTick(this.Props.interval))
			{
				return;
			}
			if (pawn.Spawned && pawn.Position.IsPolluted(pawn.Map))
			{
				severityAdjustment += this.Props.pollutedSeverity;
				return;
			}
			severityAdjustment += this.Props.unpollutedSeverity;
		}
	}
}
