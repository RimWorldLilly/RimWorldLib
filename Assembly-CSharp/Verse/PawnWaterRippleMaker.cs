using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CD RID: 205
	public class PawnWaterRippleMaker
	{
		// Token: 0x060007BA RID: 1978 RVA: 0x0002CA2F File Offset: 0x0002AC2F
		public PawnWaterRippleMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0002CA40 File Offset: 0x0002AC40
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (this.pawn.Flying)
			{
				return;
			}
			if ((this.pawn.Swimming && GenTicks.IsTickInterval(this.pawn.GetHashCode(), 20)) || (this.pawn.pather.Moving && GenTicks.IsTickInterval(this.pawn.GetHashCode(), 25) && this.pawn.Map.terrainGrid.TerrainAt(this.pawn.Position).IsWater))
			{
				this.SpawnRippleFleck(this.pawn);
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0002CAD8 File Offset: 0x0002ACD8
		private void SpawnRippleFleck(Pawn pawn)
		{
			float size = Mathf.Clamp(pawn.BodySize, 0.5f, 2f) * 0.6f;
			FleckMaker.WaterRipple(pawn.DrawPos.WithY(AltitudeLayer.MoteLow.AltitudeFor()), pawn.Map, size);
		}

		// Token: 0x0400036F RID: 879
		private Pawn pawn;

		// Token: 0x04000370 RID: 880
		private const int RippleSpawnIntervalTicks = 25;

		// Token: 0x04000371 RID: 881
		private const int RippleSpawnIntervalTicksSwimming = 20;

		// Token: 0x04000372 RID: 882
		private const float RippleStartingSize = 0.6f;

		// Token: 0x04000373 RID: 883
		private const float MinPawnSizeFactor = 0.5f;

		// Token: 0x04000374 RID: 884
		private const float MaxPawnSizeFactor = 2f;
	}
}
