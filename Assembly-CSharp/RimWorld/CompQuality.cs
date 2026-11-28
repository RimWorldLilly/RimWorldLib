using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F2 RID: 6642
	public class CompQuality : ThingComp
	{
		// Token: 0x17001D4A RID: 7498
		// (get) Token: 0x0600A73A RID: 42810 RVA: 0x003B91E7 File Offset: 0x003B73E7
		public QualityCategory Quality
		{
			get
			{
				return this.qualityInt;
			}
		}

		// Token: 0x0600A73B RID: 42811 RVA: 0x003B91F0 File Offset: 0x003B73F0
		public void SetQuality(QualityCategory q, ArtGenerationContext? source)
		{
			this.qualityInt = q;
			CompArt compArt = this.parent.TryGetComp<CompArt>();
			if (compArt != null && source != null)
			{
				compArt.InitializeArt(source.Value);
			}
			this.parent.PostQualitySet();
			CompFacilityQualityBased compFacilityQualityBased = this.parent.TryGetComp<CompFacilityQualityBased>();
			if (compFacilityQualityBased == null)
			{
				return;
			}
			compFacilityQualityBased.PostQualitySet();
		}

		// Token: 0x0600A73C RID: 42812 RVA: 0x003B9249 File Offset: 0x003B7449
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<QualityCategory>(ref this.qualityInt, "quality", QualityCategory.Awful, false);
		}

		// Token: 0x0600A73D RID: 42813 RVA: 0x003B9263 File Offset: 0x003B7463
		public override void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
		{
			this.SetQuality(QualityUtility.GenerateQualityTraderItem(), new ArtGenerationContext?(ArtGenerationContext.Outsider));
		}

		// Token: 0x0600A73E RID: 42814 RVA: 0x003B9278 File Offset: 0x003B7478
		public override bool AllowStackWith(Thing other)
		{
			QualityCategory qualityCategory;
			return other.TryGetQuality(out qualityCategory) && this.qualityInt == qualityCategory;
		}

		// Token: 0x0600A73F RID: 42815 RVA: 0x003B929A File Offset: 0x003B749A
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			piece.TryGetComp<CompQuality>().qualityInt = this.qualityInt;
		}

		// Token: 0x0600A740 RID: 42816 RVA: 0x003B92B4 File Offset: 0x003B74B4
		public override string CompInspectStringExtra()
		{
			return "QualityIs".Translate(this.Quality.GetLabel().CapitalizeFirst());
		}

		// Token: 0x0400627D RID: 25213
		private QualityCategory qualityInt = QualityCategory.Normal;
	}
}
