using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012DE RID: 4830
	public class GenStep_AncientRuins : GenStep_BaseRuins
	{
		// Token: 0x17001458 RID: 5208
		// (get) Token: 0x060077CD RID: 30669 RVA: 0x002A67E1 File Offset: 0x002A49E1
		public override int SeedPart
		{
			get
			{
				return 964521;
			}
		}

		// Token: 0x17001459 RID: 5209
		// (get) Token: 0x060077CE RID: 30670 RVA: 0x002A67E8 File Offset: 0x002A49E8
		protected override LayoutDef LayoutDef
		{
			get
			{
				return this.layoutDef;
			}
		}

		// Token: 0x1700145A RID: 5210
		// (get) Token: 0x060077CF RID: 30671 RVA: 0x002A67F0 File Offset: 0x002A49F0
		protected override int RegionSize
		{
			get
			{
				return 45;
			}
		}

		// Token: 0x1700145B RID: 5211
		// (get) Token: 0x060077D0 RID: 30672 RVA: 0x002A67F4 File Offset: 0x002A49F4
		protected override FloatRange DefaultMapFillPercentRange
		{
			get
			{
				return new FloatRange(0.15f, 0.3f);
			}
		}

		// Token: 0x1700145C RID: 5212
		// (get) Token: 0x060077D1 RID: 30673 RVA: 0x002A6805 File Offset: 0x002A4A05
		protected override FloatRange MergeRange
		{
			get
			{
				return new FloatRange(0.1f, 0.35f);
			}
		}

		// Token: 0x1700145D RID: 5213
		// (get) Token: 0x060077D2 RID: 30674 RVA: 0x002A6816 File Offset: 0x002A4A16
		protected override int MoveRangeLimit
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700145E RID: 5214
		// (get) Token: 0x060077D3 RID: 30675 RVA: 0x002A6819 File Offset: 0x002A4A19
		protected override int ContractLimit
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x1700145F RID: 5215
		// (get) Token: 0x060077D4 RID: 30676 RVA: 0x002A681C File Offset: 0x002A4A1C
		protected override int MinRegionSize
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x17001460 RID: 5216
		// (get) Token: 0x060077D5 RID: 30677 RVA: 0x002A6820 File Offset: 0x002A4A20
		protected override Faction Faction
		{
			get
			{
				return Faction.OfAncientsHostile;
			}
		}

		// Token: 0x060077D6 RID: 30678 RVA: 0x002A6828 File Offset: 0x002A4A28
		public override void GenerateRuins(Map map, GenStepParams parms, FloatRange mapFillPercentRange)
		{
			if (!ModLister.CheckOdyssey("Ancient Ruins"))
			{
				return;
			}
			base.GenerateRuins(map, parms, mapFillPercentRange);
			MapGenUtility.SpawnExteriorLumps(map, ThingDefOf.RubblePile, GenStep_AncientRuins.RubblePilesPer10K, GenStep_AncientRuins.RubblePileCountRange, GenStep_AncientRuins.RubblePileDistanceRange, 6);
			MapGenUtility.SpawnScatter(map, ThingDefOf.Filth_BlastMark, GenStep_AncientRuins.BlastMarksPer10K);
		}

		// Token: 0x04004B3C RID: 19260
		private LayoutDef layoutDef;

		// Token: 0x04004B3D RID: 19261
		private static readonly FloatRange BlastMarksPer10K = new FloatRange(2f, 6f);

		// Token: 0x04004B3E RID: 19262
		private static readonly FloatRange RubblePilesPer10K = new FloatRange(4f, 10f);

		// Token: 0x04004B3F RID: 19263
		private static readonly IntRange RubblePileCountRange = new IntRange(3, 8);

		// Token: 0x04004B40 RID: 19264
		private static readonly IntRange RubblePileDistanceRange = new IntRange(3, 8);
	}
}
