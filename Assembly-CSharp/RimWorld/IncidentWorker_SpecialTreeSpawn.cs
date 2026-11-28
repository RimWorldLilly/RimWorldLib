using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200119E RID: 4510
	public class IncidentWorker_SpecialTreeSpawn : IncidentWorker
	{
		// Token: 0x1700132F RID: 4911
		// (get) Token: 0x060070D2 RID: 28882 RVA: 0x00277B05 File Offset: 0x00275D05
		protected GenStep_SpecialTrees GenStep
		{
			get
			{
				if (this.genStep == null)
				{
					this.genStep = (GenStep_SpecialTrees)this.def.treeGenStepDef.genStep;
				}
				return this.genStep;
			}
		}

		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x060070D3 RID: 28883 RVA: 0x00277B30 File Offset: 0x00275D30
		protected virtual int MaxTreesPerIncident
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x060070D4 RID: 28884 RVA: 0x00277B33 File Offset: 0x00275D33
		protected virtual bool SendLetter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060070D5 RID: 28885 RVA: 0x00277B38 File Offset: 0x00275D38
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			if (map == null)
			{
				return false;
			}
			if (!parms.bypassStorytellerSettings && map.Biome.isExtremeBiome)
			{
				return false;
			}
			int num = this.GenStep.DesiredTreeCountForMap(map);
			IntVec3 intVec;
			return map.listerThings.ThingsOfDef(this.def.treeDef).Count < num && this.TryFindRootCell(map, out intVec);
		}

		// Token: 0x060070D6 RID: 28886 RVA: 0x00277BB0 File Offset: 0x00275DB0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			int num = Math.Min(this.MaxTreesPerIncident, this.GenStep.DesiredTreeCountForMap(map) - map.listerThings.ThingsOfDef(this.def.treeDef).Count);
			List<Thing> list = new List<Thing>();
			int num2 = 0;
			IntVec3 cell;
			Thing item;
			while (num2 < num && this.TryFindRootCell(map, out cell) && this.GenStep.TrySpawnAt(cell, map, this.def.treeGrowth, out item))
			{
				list.Add(item);
				num2++;
			}
			if (list.NullOrEmpty<Thing>())
			{
				return false;
			}
			if (this.SendLetter)
			{
				base.SendStandardLetter(this.def.letterLabel, (list.Count > 1) ? this.def.letterTextPlural : this.def.letterText, this.def.letterDef, parms, list, Array.Empty<NamedArgument>());
			}
			return true;
		}

		// Token: 0x060070D7 RID: 28887 RVA: 0x00277CA8 File Offset: 0x00275EA8
		protected virtual bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.GenStep.CanSpawnAt(x, map, 40, 0, 0f, 22, 10, 0, -1, 15), map, out cell) || CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => this.GenStep.CanSpawnAt(x, map, 10, 0, 0f, 18, 20, 0, -1, 15), map, out cell);
		}

		// Token: 0x040047BF RID: 18367
		private GenStep_SpecialTrees genStep;
	}
}
