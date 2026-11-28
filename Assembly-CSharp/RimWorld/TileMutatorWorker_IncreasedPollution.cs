using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200132D RID: 4909
	public class TileMutatorWorker_IncreasedPollution : TileMutatorWorker
	{
		// Token: 0x060079AF RID: 31151 RVA: 0x002B50BC File Offset: 0x002B32BC
		public TileMutatorWorker_IncreasedPollution(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x060079B0 RID: 31152 RVA: 0x002B50C8 File Offset: 0x002B32C8
		public override void OnAddedToTile(PlanetTile tile)
		{
			Tile tile2 = tile.Tile;
			tile2.pollution = Mathf.Clamp01(tile2.pollution + TileMutatorWorker_IncreasedPollution.PollutionOffsetRange.RandomInRange);
		}

		// Token: 0x04004CC2 RID: 19650
		private static readonly FloatRange PollutionOffsetRange = new FloatRange(0.1f, 0.2f);
	}
}
