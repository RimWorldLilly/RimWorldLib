using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AC RID: 4780
	public class GenStep_MutatorCriticalStructures : GenStep
	{
		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x06007673 RID: 30323 RVA: 0x0029B27C File Offset: 0x0029947C
		public override int SeedPart
		{
			get
			{
				return 651234136;
			}
		}

		// Token: 0x06007674 RID: 30324 RVA: 0x0029B284 File Offset: 0x00299484
		public override void Generate(Map map, GenStepParams parms)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				if (worker != null)
				{
					worker.GenerateCriticalStructures(map);
				}
			}
		}
	}
}
