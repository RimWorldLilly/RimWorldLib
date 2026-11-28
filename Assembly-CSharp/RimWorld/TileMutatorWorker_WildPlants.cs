using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200134F RID: 4943
	public class TileMutatorWorker_WildPlants : TileMutatorWorker
	{
		// Token: 0x06007A5D RID: 31325 RVA: 0x002B9DB8 File Offset: 0x002B7FB8
		public TileMutatorWorker_WildPlants(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A5E RID: 31326 RVA: 0x002B9DC1 File Offset: 0x002B7FC1
		public override string GetLabel(PlanetTile tile)
		{
			return "WildPlants".Translate(this.GetPlantKind(tile).label);
		}

		// Token: 0x06007A5F RID: 31327 RVA: 0x002B9DE3 File Offset: 0x002B7FE3
		public override string GetDescription(PlanetTile tile)
		{
			return this.def.description.Formatted(this.GetPlantKind(tile).label);
		}

		// Token: 0x06007A60 RID: 31328 RVA: 0x002B9E0B File Offset: 0x002B800B
		public override IEnumerable<BiomePlantRecord> AdditionalWildPlants(PlanetTile tile)
		{
			TileMutatorWorker_WildPlants.<AdditionalWildPlants>d__4 <AdditionalWildPlants>d__ = new TileMutatorWorker_WildPlants.<AdditionalWildPlants>d__4(-2);
			<AdditionalWildPlants>d__.<>4__this = this;
			<AdditionalWildPlants>d__.<>3__tile = tile;
			return <AdditionalWildPlants>d__;
		}

		// Token: 0x06007A61 RID: 31329 RVA: 0x002B9E22 File Offset: 0x002B8022
		private ThingDef GetPlantKind(PlanetTile tile)
		{
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			ThingDef result = this.def.plantKinds.RandomElement<ThingDef>();
			Rand.PopState();
			return result;
		}

		// Token: 0x04004D54 RID: 19796
		private const float PlantCommonality = 0.1f;
	}
}
