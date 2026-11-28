using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012D3 RID: 4819
	public class GenStep_AncientStockpile : GenStep
	{
		// Token: 0x17001450 RID: 5200
		// (get) Token: 0x06007784 RID: 30596 RVA: 0x002A2C64 File Offset: 0x002A0E64
		public override int SeedPart
		{
			get
			{
				return 928734;
			}
		}

		// Token: 0x06007785 RID: 30597 RVA: 0x002A2C6C File Offset: 0x002A0E6C
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			CellRect cellRect = map.Center.RectAbout(new IntVec2(GenStep_AncientStockpile.SizeRange.RandomInRange, GenStep_AncientStockpile.SizeRange.RandomInRange));
			StructureGenParams parms2 = new StructureGenParams
			{
				size = cellRect.Size
			};
			LayoutDef layout = parms.layout;
			LayoutWorker layoutWorker = ((layout != null) ? layout.Worker : null) ?? LayoutDefOf.AncientStockpile.Worker;
			LayoutStructureSketch layoutStructureSketch = layoutWorker.GenerateStructureSketch(parms2);
			map.layoutStructureSketches.Add(layoutStructureSketch);
			float? threatPoints = null;
			if (parms.sitePart != null)
			{
				threatPoints = new float?(parms.sitePart.parms.points);
			}
			layoutWorker.Spawn(layoutStructureSketch, map, cellRect.Min, threatPoints, null, true, false, null);
		}

		// Token: 0x04004B00 RID: 19200
		private static readonly IntRange SizeRange = new IntRange(40, 50);
	}
}
