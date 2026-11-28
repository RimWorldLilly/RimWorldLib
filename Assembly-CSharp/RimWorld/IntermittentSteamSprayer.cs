using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B26 RID: 6950
	public class IntermittentSteamSprayer
	{
		// Token: 0x1700201B RID: 8219
		// (get) Token: 0x0600B21C RID: 45596 RVA: 0x003EB9B6 File Offset: 0x003E9BB6
		// (set) Token: 0x0600B21D RID: 45597 RVA: 0x003EB9BE File Offset: 0x003E9BBE
		public int MinTicksBetweenSprays { get; set; } = 500;

		// Token: 0x1700201C RID: 8220
		// (get) Token: 0x0600B21E RID: 45598 RVA: 0x003EB9C7 File Offset: 0x003E9BC7
		// (set) Token: 0x0600B21F RID: 45599 RVA: 0x003EB9CF File Offset: 0x003E9BCF
		public int MaxTicksBetweenSprays { get; set; } = 2000;

		// Token: 0x1700201D RID: 8221
		// (get) Token: 0x0600B220 RID: 45600 RVA: 0x003EB9D8 File Offset: 0x003E9BD8
		// (set) Token: 0x0600B221 RID: 45601 RVA: 0x003EB9E0 File Offset: 0x003E9BE0
		public int MinSprayDuration { get; set; } = 200;

		// Token: 0x1700201E RID: 8222
		// (get) Token: 0x0600B222 RID: 45602 RVA: 0x003EB9E9 File Offset: 0x003E9BE9
		// (set) Token: 0x0600B223 RID: 45603 RVA: 0x003EB9F1 File Offset: 0x003E9BF1
		public int MaxSprayDuration { get; set; } = 500;

		// Token: 0x1700201F RID: 8223
		// (get) Token: 0x0600B224 RID: 45604 RVA: 0x003EB9FA File Offset: 0x003E9BFA
		// (set) Token: 0x0600B225 RID: 45605 RVA: 0x003EBA02 File Offset: 0x003E9C02
		public int PushHeatInterval { get; set; } = 20;

		// Token: 0x17002020 RID: 8224
		// (get) Token: 0x0600B226 RID: 45606 RVA: 0x003EBA0B File Offset: 0x003E9C0B
		// (set) Token: 0x0600B227 RID: 45607 RVA: 0x003EBA13 File Offset: 0x003E9C13
		public float SprayThickness { get; set; } = 0.6f;

		// Token: 0x0600B228 RID: 45608 RVA: 0x003EBA1C File Offset: 0x003E9C1C
		public IntermittentSteamSprayer(Thing parent)
		{
			this.parent = parent;
			this.ticksUntilSpray = this.MinTicksBetweenSprays;
		}

		// Token: 0x0600B229 RID: 45609 RVA: 0x003EBA84 File Offset: 0x003E9C84
		public void SteamSprayerTick()
		{
			if (this.sprayTicksLeft > 0)
			{
				this.sprayTicksLeft--;
				if (Rand.Value < this.SprayThickness)
				{
					FleckMaker.ThrowAirPuffUp(this.parent.TrueCenter(), this.parent.Map);
				}
				if (this.parent.IsHashIntervalTick(this.PushHeatInterval))
				{
					GenTemperature.PushHeat(this.parent, 40f);
				}
				if (this.sprayTicksLeft <= 0)
				{
					if (this.endSprayCallback != null)
					{
						this.endSprayCallback();
					}
					this.ticksUntilSpray = Rand.RangeInclusive(this.MinTicksBetweenSprays, this.MaxTicksBetweenSprays);
					return;
				}
			}
			else
			{
				this.ticksUntilSpray--;
				if (this.ticksUntilSpray <= 0)
				{
					if (this.startSprayCallback != null)
					{
						this.startSprayCallback();
					}
					this.sprayTicksLeft = Rand.RangeInclusive(this.MinSprayDuration, this.MaxSprayDuration);
				}
			}
		}

		// Token: 0x040066BC RID: 26300
		private readonly Thing parent;

		// Token: 0x040066BD RID: 26301
		private int ticksUntilSpray;

		// Token: 0x040066BE RID: 26302
		private int sprayTicksLeft;

		// Token: 0x040066BF RID: 26303
		public Action startSprayCallback;

		// Token: 0x040066C0 RID: 26304
		public Action endSprayCallback;
	}
}
