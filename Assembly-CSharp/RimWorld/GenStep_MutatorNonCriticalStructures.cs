using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AD RID: 4781
	public class GenStep_MutatorNonCriticalStructures : GenStep
	{
		// Token: 0x17001419 RID: 5145
		// (get) Token: 0x06007676 RID: 30326 RVA: 0x0029B2E8 File Offset: 0x002994E8
		public override int SeedPart
		{
			get
			{
				return 125123443;
			}
		}

		// Token: 0x06007677 RID: 30327 RVA: 0x0029B2F0 File Offset: 0x002994F0
		public override void Generate(Map map, GenStepParams parms)
		{
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				TileMutatorWorker worker = tileMutatorDef.Worker;
				if (worker != null)
				{
					worker.GenerateNonCriticalStructures(map);
				}
			}
		}
	}
}
