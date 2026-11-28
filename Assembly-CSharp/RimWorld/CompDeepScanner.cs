using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F9 RID: 6649
	public class CompDeepScanner : CompScanner
	{
		// Token: 0x17001D4F RID: 7503
		// (get) Token: 0x0600A770 RID: 42864 RVA: 0x003BA3FF File Offset: 0x003B85FF
		public new CompProperties_ScannerMineralsDeep Props
		{
			get
			{
				return this.props as CompProperties_ScannerMineralsDeep;
			}
		}

		// Token: 0x17001D50 RID: 7504
		// (get) Token: 0x0600A771 RID: 42865 RVA: 0x003BA40C File Offset: 0x003B860C
		public override AcceptanceReport CanUseNow
		{
			get
			{
				if (!this.parent.Map.Biome.hasBedrock)
				{
					return "CannotUseScannerNoBedrock".Translate();
				}
				return base.CanUseNow;
			}
		}

		// Token: 0x0600A772 RID: 42866 RVA: 0x003BA43C File Offset: 0x003B863C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (respawningAfterLoad)
			{
				return;
			}
			if (!this.parent.Map.Biome.hasBedrock)
			{
				Messages.Message("MessageGroundPenetratingScannerNoBedrock".Translate(this.parent.Named("THING")), this.parent, MessageTypeDefOf.NegativeEvent, false);
			}
		}

		// Token: 0x0600A773 RID: 42867 RVA: 0x003BA4A0 File Offset: 0x003B86A0
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.ShouldShowDeepResourceOverlay())
			{
				this.parent.Map.deepResourceGrid.MarkForDraw();
			}
		}

		// Token: 0x0600A774 RID: 42868 RVA: 0x003BA4BF File Offset: 0x003B86BF
		public bool ShouldShowDeepResourceOverlay()
		{
			return this.powerComp != null && this.powerComp.PowerOn;
		}

		// Token: 0x0600A775 RID: 42869 RVA: 0x003BA4D8 File Offset: 0x003B86D8
		protected override void DoFind(Pawn worker)
		{
			Map map = this.parent.Map;
			IntVec3 intVec;
			if (!CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.CanScatterAt(x, map), map, out intVec))
			{
				Log.Error("Could not find a center cell for deep scanning lump generation!");
			}
			ThingDef thingDef = this.ChooseLumpThingDef();
			int numCells = Mathf.CeilToInt((float)thingDef.deepLumpSizeRange.RandomInRange);
			foreach (IntVec3 intVec2 in GridShapeMaker.IrregularLump(intVec, map, numCells, null))
			{
				if (this.CanScatterAt(intVec2, map) && !intVec2.InNoBuildEdgeArea(map))
				{
					map.deepResourceGrid.SetAt(intVec2, thingDef, thingDef.deepCountPerCell);
				}
			}
			string key;
			if ("LetterDeepScannerFoundLump".CanTranslate())
			{
				key = "LetterDeepScannerFoundLump";
			}
			else if ("DeepScannerFoundLump".CanTranslate())
			{
				key = "DeepScannerFoundLump";
			}
			else
			{
				key = "LetterDeepScannerFoundLump";
			}
			Find.LetterStack.ReceiveLetter("LetterLabelDeepScannerFoundLump".Translate() + ": " + thingDef.LabelCap, key.Translate(thingDef.label, worker.Named("FINDER")), LetterDefOf.PositiveEvent, new LookTargets(intVec, map), null, null, null, null, 0, true);
		}

		// Token: 0x0600A776 RID: 42870 RVA: 0x003BA654 File Offset: 0x003B8854
		private bool CanScatterAt(IntVec3 pos, Map map)
		{
			int index = CellIndicesUtility.CellToIndex(pos, map.Size.x);
			TerrainDef terrainDef = map.terrainGrid.BaseTerrainAt(pos);
			return (terrainDef == null || !terrainDef.IsWater || terrainDef.passability != Traversability.Impassable) && pos.GetAffordances(map).Contains(ThingDefOf.DeepDrill.terrainAffordanceNeeded) && !map.deepResourceGrid.GetCellBool(index);
		}

		// Token: 0x0600A777 RID: 42871 RVA: 0x003BA6BD File Offset: 0x003B88BD
		protected ThingDef ChooseLumpThingDef()
		{
			return DefDatabase<ThingDef>.AllDefs.RandomElementByWeight((ThingDef def) => def.deepCommonality);
		}
	}
}
