using System;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x020012E8 RID: 4840
	public class GenStep_AncientReactor : GenStep_LargeRuins
	{
		// Token: 0x1700147A RID: 5242
		// (get) Token: 0x06007816 RID: 30742 RVA: 0x002A7A0E File Offset: 0x002A5C0E
		public override int SeedPart
		{
			get
			{
				return 79884699;
			}
		}

		// Token: 0x1700147B RID: 5243
		// (get) Token: 0x06007817 RID: 30743 RVA: 0x002A7A15 File Offset: 0x002A5C15
		protected override int RegionSize
		{
			get
			{
				return 45;
			}
		}

		// Token: 0x1700147C RID: 5244
		// (get) Token: 0x06007818 RID: 30744 RVA: 0x002A7A19 File Offset: 0x002A5C19
		protected override FloatRange DefaultMapFillPercentRange
		{
			get
			{
				return new FloatRange(0.6f, 0.75f);
			}
		}

		// Token: 0x1700147D RID: 5245
		// (get) Token: 0x06007819 RID: 30745 RVA: 0x002A7A2A File Offset: 0x002A5C2A
		protected override FloatRange MergeRange
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x1700147E RID: 5246
		// (get) Token: 0x0600781A RID: 30746 RVA: 0x002A7A3B File Offset: 0x002A5C3B
		protected override int MoveRangeLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x1700147F RID: 5247
		// (get) Token: 0x0600781B RID: 30747 RVA: 0x002A7A3E File Offset: 0x002A5C3E
		protected override int ContractLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17001480 RID: 5248
		// (get) Token: 0x0600781C RID: 30748 RVA: 0x002A7A41 File Offset: 0x002A5C41
		protected override int MinRegionSize
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17001481 RID: 5249
		// (get) Token: 0x0600781D RID: 30749 RVA: 0x002A7A45 File Offset: 0x002A5C45
		protected override IntRange RuinsMinMaxRange
		{
			get
			{
				return new IntRange(2, 6);
			}
		}

		// Token: 0x17001482 RID: 5250
		// (get) Token: 0x0600781E RID: 30750 RVA: 0x002A7A4E File Offset: 0x002A5C4E
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientRuinsReactor_Standard;
			}
		}

		// Token: 0x17001483 RID: 5251
		// (get) Token: 0x0600781F RID: 30751 RVA: 0x002A7A55 File Offset: 0x002A5C55
		protected override Faction Faction
		{
			get
			{
				return Faction.OfAncientsHostile;
			}
		}

		// Token: 0x06007820 RID: 30752 RVA: 0x002A7A5C File Offset: 0x002A5C5C
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Ancient Reactor"))
			{
				return;
			}
			this.placedReactorLayout = false;
			base.Generate(map, parms);
		}

		// Token: 0x06007821 RID: 30753 RVA: 0x002A7A7A File Offset: 0x002A5C7A
		protected override LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			if (!this.placedReactorLayout)
			{
				this.placedReactorLayout = true;
				layoutDef = LayoutDefOf.AncientRuinsReactor_Reactor;
				MapGenerator.SetVar<CellRect>("SpawnRect", rect.ExpandedBy(1));
			}
			return base.GenerateAndSpawn(rect, map, parms, layoutDef);
		}

		// Token: 0x06007822 RID: 30754 RVA: 0x002A7AAF File Offset: 0x002A5CAF
		public override void PostMapInitialized(Map map, GenStepParams parms)
		{
			BaseGenUtility.ScatterSentryDronesInMap(GenStep_AncientReactor.SentryCountFromPointsCurve, map, Faction.OfAncientsHostile, parms);
		}

		// Token: 0x04004B53 RID: 19283
		private bool placedReactorLayout;

		// Token: 0x04004B54 RID: 19284
		private static readonly SimpleCurve SentryCountFromPointsCurve = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(0f, 0f),
			new CurvePoint(100f, 2f),
			new CurvePoint(1000f, 10f),
			new CurvePoint(5000f, 20f)
		});
	}
}
