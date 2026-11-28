using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020001DD RID: 477
	public abstract class WorldGenStep
	{
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000D3B RID: 3387
		public abstract int SeedPart { get; }

		// Token: 0x06000D3C RID: 3388
		public abstract void GenerateFresh(string seed, PlanetLayer layer);

		// Token: 0x06000D3D RID: 3389 RVA: 0x00044493 File Offset: 0x00042693
		public virtual void GenerateWithoutWorldData(string seed, PlanetLayer layer)
		{
			this.GenerateFresh(seed, layer);
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x0004449D File Offset: 0x0004269D
		public virtual void GenerateFromScribe(string seed, PlanetLayer layer)
		{
		}

		// Token: 0x04000D7C RID: 3452
		public WorldGenStepDef def;
	}
}
