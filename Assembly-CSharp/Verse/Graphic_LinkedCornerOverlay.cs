using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200056F RID: 1391
	public class Graphic_LinkedCornerOverlay : Graphic_Linked
	{
		// Token: 0x06002942 RID: 10562 RVA: 0x000FD64C File Offset: 0x000FB84C
		public Graphic_LinkedCornerOverlay(Graphic subGraphic) : base(subGraphic)
		{
			this.subGraphic = subGraphic;
			this.data = subGraphic.data;
			this.overlayGraphic = (GraphicDatabase.Get<Graphic_Random>(this.data.cornerOverlayPath, ShaderDatabase.Transparent, this.drawSize, this.color) as Graphic_Random);
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x000FD6A0 File Offset: 0x000FB8A0
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			base.Print(layer, thing, extraRotation);
			IntVec3 position = thing.Position;
			if (this.ShouldLinkWith(position + IntVec3.East, thing) && this.ShouldLinkWith(position + IntVec3.North, thing) && this.ShouldLinkWith(position + IntVec3.NorthEast, thing))
			{
				Rand.PushState(thing.thingIDNumber * 9);
				Material mat = this.overlayGraphic.MatSingleFor(thing);
				float rot = Rand.Range(0f, 360f);
				bool @bool = Rand.Bool;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(mat, TextureAtlasGroup.Building, @bool, false, out mat, out uvs, out color);
				Printer_Plane.PrintPlane(layer, thing.TrueCenter() + new Vector3(0.5f, 0.1f, 0.5f), Vector3.one, mat, rot, false, uvs, null, 0.01f, 0f);
				Rand.PopState();
			}
		}

		// Token: 0x04001CCF RID: 7375
		public Graphic_Random overlayGraphic;
	}
}
