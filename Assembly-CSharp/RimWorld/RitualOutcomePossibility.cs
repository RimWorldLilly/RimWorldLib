using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020016B7 RID: 5815
	public class RitualOutcomePossibility : ILordJobOutcomePossibility
	{
		// Token: 0x1700190A RID: 6410
		// (get) Token: 0x06009196 RID: 37270 RVA: 0x00354177 File Offset: 0x00352377
		public TaggedString Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x1700190B RID: 6411
		// (get) Token: 0x06009197 RID: 37271 RVA: 0x00354184 File Offset: 0x00352384
		public TaggedString ToolTip
		{
			get
			{
				return this.potentialExtraOutcomeDesc;
			}
		}

		// Token: 0x1700190C RID: 6412
		// (get) Token: 0x06009198 RID: 37272 RVA: 0x00354191 File Offset: 0x00352391
		public bool Positive
		{
			get
			{
				return this.positivityIndex >= 0;
			}
		}

		// Token: 0x06009199 RID: 37273 RVA: 0x003541A0 File Offset: 0x003523A0
		public bool BestPositiveOutcome(LordJob_Ritual ritual)
		{
			using (List<RitualOutcomePossibility>.Enumerator enumerator = ritual.Ritual.outcomeEffect.def.outcomeChances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.positivityIndex > this.positivityIndex)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600919A RID: 37274 RVA: 0x00354210 File Offset: 0x00352410
		public float Weight(FloatRange qualityRange)
		{
			if (!this.Positive)
			{
				return this.chance;
			}
			return this.chance * qualityRange.min;
		}

		// Token: 0x04005864 RID: 22628
		[MustTranslate]
		public string label;

		// Token: 0x04005865 RID: 22629
		[MustTranslate]
		public string description;

		// Token: 0x04005866 RID: 22630
		[MustTranslate]
		public string potentialExtraOutcomeDesc;

		// Token: 0x04005867 RID: 22631
		public float chance;

		// Token: 0x04005868 RID: 22632
		public ThoughtDef memory;

		// Token: 0x04005869 RID: 22633
		public int positivityIndex;

		// Token: 0x0400586A RID: 22634
		[NoTranslate]
		public List<string> roleIdsNotGainingMemory;

		// Token: 0x0400586B RID: 22635
		public float ideoCertaintyOffset;
	}
}
