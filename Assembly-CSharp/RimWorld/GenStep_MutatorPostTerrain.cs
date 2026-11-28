using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AB RID: 4779
	public class GenStep_MutatorPostTerrain : GenStep
	{
		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x06007670 RID: 30320 RVA: 0x0029B210 File Offset: 0x00299410
		public override int SeedPart
		{
			get
			{
				return 562343345;
			}
		}

		// Token: 0x06007671 RID: 30321 RVA: 0x0029B218 File Offset: 0x00299418
		public override void Generate(Map map, GenStepParams parms)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				if (worker != null)
				{
					worker.GeneratePostTerrain(map);
				}
			}
		}
	}
}
