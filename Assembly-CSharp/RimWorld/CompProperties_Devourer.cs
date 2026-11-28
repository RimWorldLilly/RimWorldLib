using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001860 RID: 6240
	public class CompProperties_Devourer : CompProperties
	{
		// Token: 0x06009C22 RID: 39970 RVA: 0x003838D4 File Offset: 0x00381AD4
		public CompProperties_Devourer()
		{
			this.compClass = typeof(CompDevourer);
		}

		// Token: 0x04005CC8 RID: 23752
		[MustTranslate]
		public string messageDigested;

		// Token: 0x04005CC9 RID: 23753
		[MustTranslate]
		public string messageEmerged;

		// Token: 0x04005CCA RID: 23754
		[MustTranslate]
		public string messageEmergedCorpse;

		// Token: 0x04005CCB RID: 23755
		[MustTranslate]
		public string messageDigestionCompleted;

		// Token: 0x04005CCC RID: 23756
		[MustTranslate]
		public string digestingInspector;

		// Token: 0x04005CCD RID: 23757
		public int completeDigestionDamage = 125;

		// Token: 0x04005CCE RID: 23758
		public SimpleCurve bodySizeDigestTimeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 10f),
				true
			},
			{
				new CurvePoint(1f, 60f),
				true
			},
			{
				new CurvePoint(3.5f, 90f),
				true
			}
		};

		// Token: 0x04005CCF RID: 23759
		public SimpleCurve timeDamageCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 5f),
				true
			},
			{
				new CurvePoint(60f, 35f),
				true
			}
		};
	}
}
