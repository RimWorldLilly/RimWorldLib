using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200056D RID: 1389
	public class Graphic_LinkedAsymmetric : Graphic_Linked
	{
		// Token: 0x06002937 RID: 10551 RVA: 0x000FD074 File Offset: 0x000FB274
		public Graphic_LinkedAsymmetric(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002938 RID: 10552 RVA: 0x000FD07D File Offset: 0x000FB27D
		public override LinkDrawerType LinkerType
		{
			get
			{
				return LinkDrawerType.Asymmetric;
			}
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x000FD080 File Offset: 0x000FB280
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return new Graphic_LinkedAsymmetric(this.subGraphic.GetColoredVersion(newShader, newColor, newColorTwo))
			{
				data = this.data
			};
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x000FD0A4 File Offset: 0x000FB2A4
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Graphic_LinkedAsymmetric.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.thing = thing;
			CS$<>8__locals1.layer = layer;
			CS$<>8__locals1.extraRotation = extraRotation;
			base.Print(CS$<>8__locals1.layer, CS$<>8__locals1.thing, CS$<>8__locals1.extraRotation);
			if (CS$<>8__locals1.thing.def.graphicData.asymmetricLink == null || !CS$<>8__locals1.thing.def.graphicData.asymmetricLink.linkToDoors)
			{
				return;
			}
			CS$<>8__locals1.cell = CS$<>8__locals1.thing.Position;
			CS$<>8__locals1.map = CS$<>8__locals1.thing.Map;
			if (CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderEast != null)
			{
				Graphic_LinkedAsymmetric.<Print>g__DrawBorder|4_0(IntVec3.East, CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderEast, ref CS$<>8__locals1);
			}
			if (CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderWest != null)
			{
				Graphic_LinkedAsymmetric.<Print>g__DrawBorder|4_0(IntVec3.West, CS$<>8__locals1.thing.def.graphicData.asymmetricLink.drawDoorBorderWest, ref CS$<>8__locals1);
			}
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x000FD1C0 File Offset: 0x000FB3C0
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			if (base.ShouldLinkWith(c, parent))
			{
				return true;
			}
			if (!c.InBounds(parent.Map))
			{
				return (parent.def.graphicData.linkFlags & LinkFlags.MapEdge) > LinkFlags.None;
			}
			if (parent.def.graphicData.asymmetricLink != null)
			{
				if ((parent.Map.linkGrid.LinkFlagsAt(c) & parent.def.graphicData.asymmetricLink.linkFlags) != LinkFlags.None)
				{
					return true;
				}
				if (parent.def.graphicData.asymmetricLink.linkToDoors && c.GetDoor(parent.Map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x000FD264 File Offset: 0x000FB464
		[CompilerGenerated]
		internal static void <Print>g__DrawBorder|4_0(IntVec3 dir, AsymmetricLinkData.BorderData border, ref Graphic_LinkedAsymmetric.<>c__DisplayClass4_0 A_2)
		{
			IntVec3 c = A_2.cell + dir;
			if (c.InBounds(A_2.map) && c.GetDoor(A_2.map) != null)
			{
				Vector3 center = A_2.thing.DrawPos + border.offset + Altitudes.AltIncVect;
				Printer_Plane.PrintPlane(A_2.layer, center, border.size, border.Mat, A_2.extraRotation, false, null, null, 0.01f, 0f);
			}
		}
	}
}
