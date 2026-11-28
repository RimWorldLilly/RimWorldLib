using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000473 RID: 1139
	public class HediffCompProperties_ReplaceHediff : HediffCompProperties
	{
		// Token: 0x06001FF8 RID: 8184 RVA: 0x000C1100 File Offset: 0x000BF300
		public HediffCompProperties_ReplaceHediff()
		{
			this.compClass = typeof(HediffComp_ReplaceHediff);
		}

		// Token: 0x040018DD RID: 6365
		public float severity = 1f;

		// Token: 0x040018DE RID: 6366
		public bool manuallyTriggered;

		// Token: 0x040018DF RID: 6367
		[MustTranslate]
		public string message;

		// Token: 0x040018E0 RID: 6368
		public MessageTypeDef messageDef;

		// Token: 0x040018E1 RID: 6369
		[MustTranslate]
		public string letterLabel;

		// Token: 0x040018E2 RID: 6370
		[MustTranslate]
		public string letterDesc;

		// Token: 0x040018E3 RID: 6371
		public LetterDef letterDef;

		// Token: 0x040018E4 RID: 6372
		public List<HediffCompProperties_ReplaceHediff.TriggeredHediff> hediffs = new List<HediffCompProperties_ReplaceHediff.TriggeredHediff>();

		// Token: 0x020026E5 RID: 9957
		public class TriggeredHediff
		{
			// Token: 0x0600F5B0 RID: 62896 RVA: 0x00597900 File Offset: 0x00595B00
			public void ApplyTo(Pawn pawn, List<Hediff> outAddedHediffs = null)
			{
				List<Hediff> list = new List<Hediff>();
				HediffGiverUtility.TryApply(pawn, this.hediff, this.partsToAffect, false, this.countRange.RandomInRange, list, false);
				if (this.severityRange != FloatRange.Zero)
				{
					foreach (Hediff hediff in list)
					{
						hediff.Severity = this.severityRange.RandomInRange;
					}
				}
				if (outAddedHediffs != null)
				{
					outAddedHediffs.AddRange(list);
				}
			}

			// Token: 0x04009F70 RID: 40816
			public HediffDef hediff;

			// Token: 0x04009F71 RID: 40817
			public IntRange countRange = new IntRange(1, 1);

			// Token: 0x04009F72 RID: 40818
			public List<BodyPartDef> partsToAffect;

			// Token: 0x04009F73 RID: 40819
			public FloatRange severityRange = FloatRange.Zero;
		}
	}
}
