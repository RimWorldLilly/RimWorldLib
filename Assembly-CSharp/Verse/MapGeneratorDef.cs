using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000185 RID: 389
	public class MapGeneratorDef : Def
	{
		// Token: 0x04000971 RID: 2417
		public bool isUnderground;

		// Token: 0x04000972 RID: 2418
		public bool forceCaves;

		// Token: 0x04000973 RID: 2419
		public List<GenStepDef> genSteps;

		// Token: 0x04000974 RID: 2420
		public PocketMapProperties pocketMapProperties;

		// Token: 0x04000975 RID: 2421
		public List<Type> customMapComponents = new List<Type>();

		// Token: 0x04000976 RID: 2422
		public bool ignoreAreaRevealedLetter;

		// Token: 0x04000977 RID: 2423
		public RoofDef roofDef;

		// Token: 0x04000978 RID: 2424
		public bool disableShadows;

		// Token: 0x04000979 RID: 2425
		public bool disableCallAid;

		// Token: 0x0400097A RID: 2426
		public bool disableMapClippers;

		// Token: 0x0400097B RID: 2427
		public bool renderWorld;

		// Token: 0x0400097C RID: 2428
		public bool defeatRequiresCantReachUnfogged = true;

		// Token: 0x0400097D RID: 2429
		public TerrainDef defaultUnderGridTerrain;

		// Token: 0x0400097E RID: 2430
		public bool validScenarioMap;

		// Token: 0x0400097F RID: 2431
		[NoTranslate]
		public string mapClipperTexturePath;

		// Token: 0x04000980 RID: 2432
		public ShaderTypeDef mapClipperShader;
	}
}
