using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001308 RID: 4872
	public abstract class TileMutatorWorker
	{
		// Token: 0x06007904 RID: 30980 RVA: 0x002B026D File Offset: 0x002AE46D
		public TileMutatorWorker(TileMutatorDef def)
		{
			this.def = def;
		}

		// Token: 0x06007905 RID: 30981 RVA: 0x002B027C File Offset: 0x002AE47C
		public virtual bool IsValidTile(PlanetTile tile, PlanetLayer layer)
		{
			return true;
		}

		// Token: 0x06007906 RID: 30982 RVA: 0x002B027F File Offset: 0x002AE47F
		public virtual void OnAddedToTile(PlanetTile tile)
		{
		}

		// Token: 0x06007907 RID: 30983 RVA: 0x002B0281 File Offset: 0x002AE481
		public virtual void Init(Map map)
		{
		}

		// Token: 0x06007908 RID: 30984 RVA: 0x002B0283 File Offset: 0x002AE483
		public virtual void Tick(Map map)
		{
		}

		// Token: 0x06007909 RID: 30985 RVA: 0x002B0285 File Offset: 0x002AE485
		public virtual string GetLabel(PlanetTile tile)
		{
			return this.def.label;
		}

		// Token: 0x0600790A RID: 30986 RVA: 0x002B0292 File Offset: 0x002AE492
		public virtual string GetDescription(PlanetTile tile)
		{
			return this.def.description;
		}

		// Token: 0x0600790B RID: 30987 RVA: 0x002B029F File Offset: 0x002AE49F
		public virtual void GeneratePostElevationFertility(Map map)
		{
		}

		// Token: 0x0600790C RID: 30988 RVA: 0x002B02A1 File Offset: 0x002AE4A1
		public virtual void GeneratePostTerrain(Map map)
		{
		}

		// Token: 0x0600790D RID: 30989 RVA: 0x002B02A3 File Offset: 0x002AE4A3
		public virtual void GenerateCriticalStructures(Map map)
		{
		}

		// Token: 0x0600790E RID: 30990 RVA: 0x002B02A5 File Offset: 0x002AE4A5
		public virtual void GenerateNonCriticalStructures(Map map)
		{
		}

		// Token: 0x0600790F RID: 30991 RVA: 0x002B02A7 File Offset: 0x002AE4A7
		public virtual void GeneratePostFog(Map map)
		{
		}

		// Token: 0x06007910 RID: 30992 RVA: 0x002B02A9 File Offset: 0x002AE4A9
		public virtual void MutateWeatherCommonalityFor(WeatherDef weather, PlanetTile tile, ref float commonality)
		{
		}

		// Token: 0x06007911 RID: 30993 RVA: 0x002B02AB File Offset: 0x002AE4AB
		public virtual float AnimalCommonalityFactorFor(PawnKindDef animal, PlanetTile tile)
		{
			return 1f;
		}

		// Token: 0x06007912 RID: 30994 RVA: 0x002B02B2 File Offset: 0x002AE4B2
		public virtual float PlantCommonalityFactorFor(ThingDef plant, PlanetTile tile)
		{
			return 1f;
		}

		// Token: 0x06007913 RID: 30995 RVA: 0x002B02B9 File Offset: 0x002AE4B9
		public virtual IEnumerable<BiomePlantRecord> AdditionalWildPlants(PlanetTile tile)
		{
			return this.def.additionalWildPlants;
		}

		// Token: 0x04004BF6 RID: 19446
		public TileMutatorDef def;
	}
}
