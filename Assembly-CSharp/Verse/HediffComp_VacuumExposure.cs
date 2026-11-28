using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200048B RID: 1163
	public class HediffComp_VacuumExposure : HediffComp
	{
		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06002054 RID: 8276 RVA: 0x000C25D1 File Offset: 0x000C07D1
		public HediffCompProperties_VacuumExposure Props
		{
			get
			{
				return (HediffCompProperties_VacuumExposure)this.props;
			}
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x000C25E0 File Offset: 0x000C07E0
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (base.Pawn.IsHashIntervalTick(60, delta))
			{
				float num = this.Props.severityPerSecondUnexposed;
				if (base.Pawn.Spawned && base.Pawn.Position.GetVacuum(base.Pawn.Map) >= 0.5f && base.Pawn.GetStatValue(StatDefOf.VacuumResistance, true, -1) < 1f)
				{
					num = 0f;
				}
				severityAdjustment += num;
			}
		}

		// Token: 0x0400190F RID: 6415
		private const int SeverityUpdateInterval = 60;
	}
}
