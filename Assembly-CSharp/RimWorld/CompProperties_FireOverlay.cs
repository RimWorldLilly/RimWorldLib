using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ED8 RID: 3800
	public class CompProperties_FireOverlay : CompProperties
	{
		// Token: 0x0600639B RID: 25499 RVA: 0x0023BE30 File Offset: 0x0023A030
		public CompProperties_FireOverlay()
		{
			this.compClass = typeof(CompFireOverlay);
		}

		// Token: 0x0600639C RID: 25500 RVA: 0x0023BE6C File Offset: 0x0023A06C
		public Vector3 DrawOffsetForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				Vector3? vector = this.offsetNorth;
				if (vector == null)
				{
					return this.offset;
				}
				return vector.GetValueOrDefault();
			}
			case 1:
			{
				Vector3? vector = this.offsetEast;
				if (vector == null)
				{
					return this.offset;
				}
				return vector.GetValueOrDefault();
			}
			case 2:
			{
				Vector3? vector = this.offsetSouth;
				if (vector == null)
				{
					return this.offset;
				}
				return vector.GetValueOrDefault();
			}
			case 3:
			{
				Vector3? vector = this.offsetWest;
				if (vector == null)
				{
					return this.offset;
				}
				return vector.GetValueOrDefault();
			}
			default:
				return this.offset;
			}
		}

		// Token: 0x0600639D RID: 25501 RVA: 0x0023BF1C File Offset: 0x0023A11C
		public override void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
			Graphic graphic = GhostUtility.GhostGraphicFor(CompFireOverlay.FireGraphic, thingDef, ghostCol, null);
			Vector3 loc = center.ToVector3ShiftedWithAltitude(drawAltitude) + thingDef.graphicData.DrawOffsetForRot(rot) + this.DrawOffsetForRot(rot);
			graphic.DrawFromDef(loc, rot, thingDef, 0f);
		}

		// Token: 0x040037EB RID: 14315
		public float fireSize = 1f;

		// Token: 0x040037EC RID: 14316
		public float finalFireSize = 1f;

		// Token: 0x040037ED RID: 14317
		public float fireGrowthDurationTicks = -1f;

		// Token: 0x040037EE RID: 14318
		public Vector3 offset;

		// Token: 0x040037EF RID: 14319
		public Vector3? offsetNorth;

		// Token: 0x040037F0 RID: 14320
		public Vector3? offsetSouth;

		// Token: 0x040037F1 RID: 14321
		public Vector3? offsetWest;

		// Token: 0x040037F2 RID: 14322
		public Vector3? offsetEast;
	}
}
