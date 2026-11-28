using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200133F RID: 4927
	public class TileMutatorWorker_ObsidianDeposits : TileMutatorWorker
	{
		// Token: 0x06007A04 RID: 31236 RVA: 0x002B6D9C File Offset: 0x002B4F9C
		public TileMutatorWorker_ObsidianDeposits(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007A05 RID: 31237 RVA: 0x002B6DA8 File Offset: 0x002B4FA8
		public override void GeneratePostTerrain(Map map)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			float num = GenStep_RocksFromGrid.GetResourceBlotchesPer10KCellsForMap(map) * 0.1f;
			new GenStep_ScatterLumpsMineable
			{
				maxValue = float.MaxValue,
				countPer10kCellsRange = new FloatRange(num, num),
				forcedDefToScatter = ThingDefOf.MineableObsidian
			}.Generate(map, default(GenStepParams));
		}

		// Token: 0x04004D00 RID: 19712
		private const float BlotchFactor = 0.1f;
	}
}
