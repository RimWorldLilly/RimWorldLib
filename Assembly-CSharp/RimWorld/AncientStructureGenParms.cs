using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200130F RID: 4879
	public class AncientStructureGenParms
	{
		// Token: 0x04004BFD RID: 19453
		public LayoutDef structureLayoutDef;

		// Token: 0x04004BFE RID: 19454
		public IntRange structureSizeRange = IntRange.Zero;

		// Token: 0x04004BFF RID: 19455
		public List<ScatteredPrefabs> scatteredPrefabs = new List<ScatteredPrefabs>();

		// Token: 0x04004C00 RID: 19456
		public ThingDef perimeterWallDef;

		// Token: 0x04004C01 RID: 19457
		public int perimeterExpandBy;

		// Token: 0x04004C02 RID: 19458
		public float perimeterWallChance = 1f;

		// Token: 0x04004C03 RID: 19459
		public TerrainDef perimeterTerrainDef;

		// Token: 0x04004C04 RID: 19460
		public List<LayoutScatterTerrainParms> perimeterScatterTerrain = new List<LayoutScatterTerrainParms>();

		// Token: 0x04004C05 RID: 19461
		public List<AncientStructureGenParms.ScatteredThing> perimeterScatteredThings = new List<AncientStructureGenParms.ScatteredThing>();

		// Token: 0x02003237 RID: 12855
		public class ScatteredThing
		{
			// Token: 0x06012ACE RID: 76494 RVA: 0x0063D18C File Offset: 0x0063B38C
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "thingDef", "chancePer100Cells");
			}

			// Token: 0x0400C9EF RID: 51695
			public ThingDef thingDef;

			// Token: 0x0400C9F0 RID: 51696
			public float chancePer100Cells = 1f;
		}
	}
}
