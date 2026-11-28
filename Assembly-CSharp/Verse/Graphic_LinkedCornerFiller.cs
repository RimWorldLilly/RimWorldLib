using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200056E RID: 1390
	public class Graphic_LinkedCornerFiller : Graphic_Linked
	{
		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x0600293D RID: 10557 RVA: 0x000FD2E6 File Offset: 0x000FB4E6
		public override LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.CornerFiller;
			}
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x000FD2E9 File Offset: 0x000FB4E9
		public Graphic_LinkedCornerFiller(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x000FD2F2 File Offset: 0x000FB4F2
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_LinkedCornerFiller(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x000FD314 File Offset: 0x000FB514
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			base.Print(layer, thing, extraRotation);
			IntVec3 position = thing.Position;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = thing.Position + GenAdj.DiagonalDirectionsAround[i];
				if (this.ShouldLinkWith(intVec, thing) && (i != 0 || (this.ShouldLinkWith(position + IntVec3.West, thing) && this.ShouldLinkWith(position + IntVec3.South, thing))) && (i != 1 || (this.ShouldLinkWith(position + IntVec3.West, thing) && this.ShouldLinkWith(position + IntVec3.North, thing))) && (i != 2 || (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.North, thing))) && (i != 3 || (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.South, thing))))
				{
					Vector3 center = thing.DrawPos + GenAdj.DiagonalDirectionsAround[i].ToVector3().normalized * Graphic_LinkedCornerFiller.CoverOffsetDist + Altitudes.AltIncVect + new Vector3(0f, 0f, 0.09f);
					Vector2 size = new Vector2(0.5f, 0.5f);
					if (!intVec.InBounds(thing.Map))
					{
						if (intVec.x == -1)
						{
							center.x -= 1f;
							size.x *= 5f;
						}
						if (intVec.z == -1)
						{
							center.z -= 1f;
							size.y *= 5f;
						}
						if (intVec.x == thing.Map.Size.x)
						{
							center.x += 1f;
							size.x *= 5f;
						}
						if (intVec.z == thing.Map.Size.z)
						{
							center.z += 1f;
							size.y *= 5f;
						}
					}
					Printer_Plane.PrintPlane(layer, center, size, this.LinkedDrawMatFrom(thing, thing.Position), extraRotation, false, Graphic_LinkedCornerFiller.CornerFillUVs, null, 0.01f, 0f);
				}
			}
		}

		// Token: 0x04001CC9 RID: 7369
		private const float ShiftUp = 0.09f;

		// Token: 0x04001CCA RID: 7370
		private const float CoverSize = 0.5f;

		// Token: 0x04001CCB RID: 7371
		private static readonly float CoverSizeCornerCorner = new Vector2(0.5f, 0.5f).magnitude;

		// Token: 0x04001CCC RID: 7372
		private static readonly float DistCenterCorner = new Vector2(0.5f, 0.5f).magnitude;

		// Token: 0x04001CCD RID: 7373
		private static readonly float CoverOffsetDist = Graphic_LinkedCornerFiller.DistCenterCorner - Graphic_LinkedCornerFiller.CoverSizeCornerCorner * 0.5f;

		// Token: 0x04001CCE RID: 7374
		private static readonly Vector2[] CornerFillUVs = new Vector2[]
		{
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f),
			new Vector2(0.5f, 0.6f)
		};
	}
}
