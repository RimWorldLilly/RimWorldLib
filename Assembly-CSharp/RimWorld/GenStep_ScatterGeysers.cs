using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C4 RID: 4804
	public class GenStep_ScatterGeysers : GenStep_ScatterThings
	{
		// Token: 0x06007737 RID: 30519 RVA: 0x002A0DBC File Offset: 0x0029EFBC
		protected override int CalculateFinalCount(Map map)
		{
			float num = map.Biome.geyserCountFactor;
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				num *= tileMutatorDef.geyserCountFactor;
			}
			return Mathf.RoundToInt((float)base.CalculateFinalCount(map) * num);
		}
	}
}
