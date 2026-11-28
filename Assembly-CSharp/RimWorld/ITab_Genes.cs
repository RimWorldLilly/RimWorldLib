using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D6C RID: 7532
	public class ITab_Genes : ITab
	{
		// Token: 0x1700242B RID: 9259
		// (get) Token: 0x0600C330 RID: 49968 RVA: 0x0047387C File Offset: 0x00471A7C
		public override bool Hidden
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700242C RID: 9260
		// (get) Token: 0x0600C331 RID: 49969 RVA: 0x0047387F File Offset: 0x00471A7F
		public override bool IsVisible
		{
			get
			{
				return ITab_Genes.CanShowGenesTab();
			}
		}

		// Token: 0x1700242D RID: 9261
		// (get) Token: 0x0600C332 RID: 49970 RVA: 0x00473886 File Offset: 0x00471A86
		protected override bool StillValid
		{
			get
			{
				return base.StillValid && ITab_Genes.ThingWithGenes() != null;
			}
		}

		// Token: 0x1700242E RID: 9262
		// (get) Token: 0x0600C333 RID: 49971 RVA: 0x0047389A File Offset: 0x00471A9A
		protected Pawn SelPawnForGenes
		{
			get
			{
				return ITab_Genes.PawnForGenes(base.SelThing);
			}
		}

		// Token: 0x0600C334 RID: 49972 RVA: 0x004738A7 File Offset: 0x00471AA7
		public ITab_Genes()
		{
			this.size = new Vector2(Mathf.Min(736f, (float)UI.screenWidth), 550f);
			this.labelKey = "TabGenes";
		}

		// Token: 0x0600C335 RID: 49973 RVA: 0x004738DA File Offset: 0x00471ADA
		public override void OnOpen()
		{
			if (!ModLister.CheckBiotech("genes viewing"))
			{
				this.CloseTab();
				return;
			}
			base.OnOpen();
		}

		// Token: 0x0600C336 RID: 49974 RVA: 0x004738F8 File Offset: 0x00471AF8
		protected override void FillTab()
		{
			GeneUIUtility.DrawGenesInfo(new Rect(0f, 20f, this.size.x, this.size.y - 20f), ITab_Genes.ThingWithGenes(), 550f, ref this.size, ref this.scrollPosition, null);
		}

		// Token: 0x0600C337 RID: 49975 RVA: 0x0047394C File Offset: 0x00471B4C
		private static Thing ThingWithGenes()
		{
			Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
			Pawn pawn = ITab_Genes.PawnForGenes(singleSelectedThing);
			if (pawn != null)
			{
				return pawn;
			}
			if (singleSelectedThing is GeneSetHolderBase)
			{
				return singleSelectedThing;
			}
			Building_GrowthVat building_GrowthVat = singleSelectedThing as Building_GrowthVat;
			if (building_GrowthVat != null)
			{
				return building_GrowthVat.selectedEmbryo;
			}
			return null;
		}

		// Token: 0x0600C338 RID: 49976 RVA: 0x0047398C File Offset: 0x00471B8C
		private static Pawn PawnForGenes(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.genes != null)
			{
				return pawn;
			}
			Corpse corpse = thing as Corpse;
			if (corpse != null && corpse.InnerPawn.genes != null)
			{
				return corpse.InnerPawn;
			}
			return null;
		}

		// Token: 0x0600C339 RID: 49977 RVA: 0x004739CC File Offset: 0x00471BCC
		public static bool CanShowGenesTab()
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Pawn pawn = ITab_Genes.PawnForGenes(Find.Selector.SingleSelectedThing);
			return (pawn != null && pawn.genes != null) || ITab_Genes.ThingWithGenes() != null;
		}

		// Token: 0x04006F87 RID: 28551
		protected Vector2 scrollPosition;

		// Token: 0x04006F88 RID: 28552
		protected const float TopPadding = 20f;

		// Token: 0x04006F89 RID: 28553
		public const float GeneSize = 90f;

		// Token: 0x04006F8A RID: 28554
		public const float GeneGap = 6f;

		// Token: 0x04006F8B RID: 28555
		public const int MaxGenesHorizontal = 7;

		// Token: 0x04006F8C RID: 28556
		public const float InitialWidth = 736f;

		// Token: 0x04006F8D RID: 28557
		protected const float InitialHeight = 550f;
	}
}
