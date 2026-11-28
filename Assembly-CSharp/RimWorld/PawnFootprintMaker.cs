using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001496 RID: 5270
	public class PawnFootprintMaker
	{
		// Token: 0x0600812A RID: 33066 RVA: 0x002E1AEC File Offset: 0x002DFCEC
		public PawnFootprintMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600812B RID: 33067 RVA: 0x002E1AFC File Offset: 0x002DFCFC
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (!this.pawn.RaceProps.makesFootprints || (this.pawn.IsMutant && !this.pawn.mutant.Def.makesFootprints))
			{
				TerrainDef terrain = this.pawn.Position.GetTerrain(this.pawn.Map);
				if (terrain == null || !terrain.takeSplashes)
				{
					return;
				}
			}
			if ((this.pawn.Drawer.DrawPos - this.lastFootprintPlacePos).MagnitudeHorizontalSquared() > 0.39942405f)
			{
				this.TryPlaceFootprint();
			}
		}

		// Token: 0x0600812C RID: 33068 RVA: 0x002E1B94 File Offset: 0x002DFD94
		private void TryPlaceFootprint()
		{
			Vector3 drawPos = this.pawn.Drawer.DrawPos;
			Vector3 normalized = (drawPos - this.lastFootprintPlacePos).normalized;
			float rot = normalized.AngleFlat();
			float angle = (float)(this.lastFootprintRight ? 90 : -90);
			Vector3 b = normalized.RotatedBy(angle) * 0.17f * Mathf.Sqrt(this.pawn.BodySize);
			Vector3 vector = drawPos + PawnFootprintMaker.FootprintOffset + b;
			IntVec3 c = vector.ToIntVec3();
			if (c.InBounds(this.pawn.Map))
			{
				TerrainDef terrain = c.GetTerrain(this.pawn.Map);
				if (terrain != null)
				{
					if (terrain.takeSplashes)
					{
						FleckMaker.WaterSplash(vector, this.pawn.Map, Mathf.Sqrt(this.pawn.BodySize) * 2f, 1.5f);
					}
					if (this.pawn.RaceProps.makesFootprints && terrain.takeFootprints && this.pawn.Map.snowGrid.GetDepth(this.pawn.Position) >= 0.4f)
					{
						FleckMaker.PlaceFootprint(vector, this.pawn.Map, rot);
					}
				}
			}
			this.lastFootprintPlacePos = drawPos;
			this.lastFootprintRight = !this.lastFootprintRight;
		}

		// Token: 0x04005057 RID: 20567
		private Pawn pawn;

		// Token: 0x04005058 RID: 20568
		private Vector3 lastFootprintPlacePos;

		// Token: 0x04005059 RID: 20569
		private bool lastFootprintRight;

		// Token: 0x0400505A RID: 20570
		private const float FootprintIntervalDist = 0.632f;

		// Token: 0x0400505B RID: 20571
		private static readonly Vector3 FootprintOffset = new Vector3(0f, 0f, -0.3f);

		// Token: 0x0400505C RID: 20572
		private const float LeftRightOffsetDist = 0.17f;

		// Token: 0x0400505D RID: 20573
		private const float FootprintSplashSize = 2f;
	}
}
