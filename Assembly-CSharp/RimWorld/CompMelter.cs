using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001908 RID: 6408
	public class CompMelter : ThingComp
	{
		// Token: 0x0600A0A0 RID: 41120 RVA: 0x003974CC File Offset: 0x003956CC
		public override void CompTickRare()
		{
			float ambientTemperature = this.parent.AmbientTemperature;
			if (ambientTemperature < 0f)
			{
				return;
			}
			int num = GenMath.RoundRandom(0.15f * (ambientTemperature / 10f));
			if (num > 0)
			{
				this.parent.TakeDamage(new DamageInfo(DamageDefOf.Rotting, (float)num, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
		}

		// Token: 0x04005ED6 RID: 24278
		private const float MeltPerIntervalPer10Degrees = 0.15f;
	}
}
