using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D1 RID: 1233
	public class SummaryHealthHandler
	{
		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x06002263 RID: 8803 RVA: 0x000CC46C File Offset: 0x000CA66C
		public float SummaryHealthPercent
		{
			get
			{
				if (this.pawn.Dead)
				{
					return 0f;
				}
				if (this.dirty)
				{
					List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
					float num = 1f;
					for (int i = 0; i < hediffs.Count; i++)
					{
						if (!(hediffs[i] is Hediff_MissingPart))
						{
							float num2 = Mathf.Min(hediffs[i].SummaryHealthPercentImpact, 0.95f);
							num *= 1f - num2;
						}
					}
					List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
					for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
					{
						float num3 = Mathf.Min(missingPartsCommonAncestors[j].SummaryHealthPercentImpact, 0.95f);
						num *= 1f - num3;
					}
					this.cachedSummaryHealthPercent = Mathf.Clamp(num, 0.05f, 1f);
					this.dirty = false;
				}
				return this.cachedSummaryHealthPercent;
			}
		}

		// Token: 0x06002264 RID: 8804 RVA: 0x000CC567 File Offset: 0x000CA767
		public SummaryHealthHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002265 RID: 8805 RVA: 0x000CC588 File Offset: 0x000CA788
		public void Notify_HealthChanged()
		{
			this.dirty = true;
		}

		// Token: 0x040019CA RID: 6602
		private Pawn pawn;

		// Token: 0x040019CB RID: 6603
		private float cachedSummaryHealthPercent = 1f;

		// Token: 0x040019CC RID: 6604
		private bool dirty = true;
	}
}
