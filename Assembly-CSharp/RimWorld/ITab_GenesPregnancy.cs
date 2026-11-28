using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6D RID: 7533
	public class ITab_GenesPregnancy : ITab_Genes
	{
		// Token: 0x1700242F RID: 9263
		// (get) Token: 0x0600C33A RID: 49978 RVA: 0x00473A09 File Offset: 0x00471C09
		public override bool IsVisible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C33B RID: 49979 RVA: 0x00473A0C File Offset: 0x00471C0C
		private GeneSet UnbornBabyHediffGeneset()
		{
			Pawn selPawnForGenes = base.SelPawnForGenes;
			if (selPawnForGenes != null)
			{
				foreach (Hediff hediff in selPawnForGenes.health.hediffSet.hediffs)
				{
					HediffWithParents hediffWithParents = hediff as HediffWithParents;
					if (hediffWithParents != null)
					{
						return hediffWithParents.geneSet;
					}
				}
			}
			return null;
		}

		// Token: 0x0600C33C RID: 49980 RVA: 0x00473A80 File Offset: 0x00471C80
		protected override void FillTab()
		{
			GeneUIUtility.DrawGenesInfo(new Rect(0f, 20f, this.size.x, this.size.y - 20f), null, 550f, ref this.size, ref this.scrollPosition, this.UnbornBabyHediffGeneset());
		}
	}
}
