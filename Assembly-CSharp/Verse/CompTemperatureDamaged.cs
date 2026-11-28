using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000544 RID: 1348
	public class CompTemperatureDamaged : ThingComp
	{
		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600279A RID: 10138 RVA: 0x000F6D55 File Offset: 0x000F4F55
		public CompProperties_TemperatureDamaged Props
		{
			get
			{
				return (CompProperties_TemperatureDamaged)this.props;
			}
		}

		// Token: 0x0600279B RID: 10139 RVA: 0x000F6D62 File Offset: 0x000F4F62
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.CheckTakeDamage();
			}
		}

		// Token: 0x0600279C RID: 10140 RVA: 0x000F6D7C File Offset: 0x000F4F7C
		public override void CompTickRare()
		{
			this.CheckTakeDamage();
		}

		// Token: 0x0600279D RID: 10141 RVA: 0x000F6D84 File Offset: 0x000F4F84
		private void CheckTakeDamage()
		{
			if (!this.Props.safeTemperatureRange.Includes(this.parent.AmbientTemperature))
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Deterioration, (float)this.Props.damagePerTickRare, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}
	}
}
