using System;

namespace Verse
{
	// Token: 0x02000541 RID: 1345
	public class CompHeatPusher : ThingComp
	{
		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x0600278A RID: 10122 RVA: 0x000F69CC File Offset: 0x000F4BCC
		public CompProperties_HeatPusher Props
		{
			get
			{
				return (CompProperties_HeatPusher)this.props;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x0600278B RID: 10123 RVA: 0x000F69DC File Offset: 0x000F4BDC
		public virtual bool ShouldPushHeatNow
		{
			get
			{
				if (!this.parent.SpawnedOrAnyParentSpawned)
				{
					return false;
				}
				CompProperties_HeatPusher props = this.Props;
				float ambientTemperature = this.parent.AmbientTemperature;
				return this.enabled && ambientTemperature < props.heatPushMaxTemperature && ambientTemperature > props.heatPushMinTemperature;
			}
		}

		// Token: 0x0600278C RID: 10124 RVA: 0x000F6A28 File Offset: 0x000F4C28
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(60) && this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond);
			}
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x000F6A79 File Offset: 0x000F4C79
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.ShouldPushHeatNow)
			{
				GenTemperature.PushHeat(this.parent.PositionHeld, this.parent.MapHeld, this.Props.heatPerSecond * 4.1666665f);
			}
		}

		// Token: 0x0600278E RID: 10126 RVA: 0x000F6AB6 File Offset: 0x000F4CB6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.enabled, "enabled", true, false);
		}

		// Token: 0x04001C4D RID: 7245
		private const int HeatPushInterval = 60;

		// Token: 0x04001C4E RID: 7246
		public bool enabled = true;
	}
}
