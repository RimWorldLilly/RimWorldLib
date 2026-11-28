using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AA RID: 4778
	public class GenStep_MutatorPostElevationFertility : GenStep
	{
		// Token: 0x17001416 RID: 5142
		// (get) Token: 0x0600766D RID: 30317 RVA: 0x0029B1A3 File Offset: 0x002993A3
		public override int SeedPart
		{
			get
			{
				return 1239847543;
			}
		}

		// Token: 0x0600766E RID: 30318 RVA: 0x0029B1AC File Offset: 0x002993AC
		public override void Generate(Map map, GenStepParams parms)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				if (worker != null)
				{
					worker.GeneratePostElevationFertility(map);
				}
			}
		}
	}
}
