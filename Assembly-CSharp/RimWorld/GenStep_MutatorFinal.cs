using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AE RID: 4782
	public class GenStep_MutatorFinal : GenStep
	{
		// Token: 0x1700141A RID: 5146
		// (get) Token: 0x06007679 RID: 30329 RVA: 0x0029B354 File Offset: 0x00299554
		public override int SeedPart
		{
			get
			{
				return 756452345;
			}
		}

		// Token: 0x0600767A RID: 30330 RVA: 0x0029B35C File Offset: 0x0029955C
		public override void Generate(Map map, GenStepParams parms)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				if (worker != null)
				{
					worker.GeneratePostFog(map);
				}
			}
		}
	}
}
