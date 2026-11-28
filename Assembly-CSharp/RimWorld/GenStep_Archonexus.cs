using System;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012EA RID: 4842
	public class GenStep_Archonexus : GenStep_Scatterer
	{
		// Token: 0x17001485 RID: 5253
		// (get) Token: 0x0600782A RID: 30762 RVA: 0x002A7CE3 File Offset: 0x002A5EE3
		public override int SeedPart
		{
			get
			{
				return 473957495;
			}
		}

		// Token: 0x0600782B RID: 30763 RVA: 0x002A7CEA File Offset: 0x002A5EEA
		public override void Generate(Map map, GenStepParams parms)
		{
			this.count = 1;
			this.nearMapCenter = true;
			base.Generate(map, parms);
		}

		// Token: 0x0600782C RID: 30764 RVA: 0x002A7D04 File Offset: 0x002A5F04
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (!c.Standable(map))
			{
				return false;
			}
			if (c.Roofed(map))
			{
				return false;
			}
			ThingDef archonexusCore = ThingDefOf.ArchonexusCore;
			IntVec3 c2 = ThingUtility.InteractionCellWhenAt(archonexusCore, c, archonexusCore.defaultPlacingRot, map);
			return map.reachability.CanReachMapEdge(c2, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false));
		}

		// Token: 0x0600782D RID: 30765 RVA: 0x002A7D68 File Offset: 0x002A5F68
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			SitePartParams parms2 = parms.sitePart.parms;
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.threatPoints = new float?(parms2.threatPoints);
			resolveParams.rect = CellRect.CenteredOn(c, 60, 60);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("archonexus", resolveParams, null);
			BaseGen.Generate();
		}

		// Token: 0x04004B59 RID: 19289
		private const int Size = 60;
	}
}
