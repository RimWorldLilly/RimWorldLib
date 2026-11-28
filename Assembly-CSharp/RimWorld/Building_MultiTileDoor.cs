using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AC0 RID: 6848
	public class Building_MultiTileDoor : Building_SupportedDoor
	{
		// Token: 0x17001F46 RID: 8006
		// (get) Token: 0x0600AE33 RID: 44595 RVA: 0x003D643E File Offset: 0x003D463E
		protected override bool CanDrawMovers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600AE34 RID: 44596 RVA: 0x003D6441 File Offset: 0x003D4641
		protected override void Tick()
		{
			base.Tick();
			this.tmpStuckOpen = base.StuckOpen;
		}

		// Token: 0x0600AE35 RID: 44597 RVA: 0x003D6458 File Offset: 0x003D4658
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DoorPreDraw();
			if (!this.tmpStuckOpen)
			{
				float offsetDist = 0.25f + 0.35000002f * this.OpenPct;
				base.DrawMovers(drawLoc, offsetDist, this.Graphic, AltitudeLayer.DoorMoveable.AltitudeFor(), Building_MultiTileDoor.MoverDrawScale, this.Graphic.ShadowGraphic);
				if (this.def.building.upperMoverGraphic != null)
				{
					float offsetDist2 = 0.25f + 0.35000002f * Mathf.Clamp01(this.OpenPct * 2.5f);
					base.DrawMovers(drawLoc, offsetDist2, this.def.building.upperMoverGraphic.Graphic, Building_MultiTileDoor.UpperMoverAltitude, Building_MultiTileDoor.MoverDrawScale, null);
				}
			}
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x040064DA RID: 25818
		private bool tmpStuckOpen;

		// Token: 0x040064DB RID: 25819
		private const float UpperMoverOffsetStart = 0.25f;

		// Token: 0x040064DC RID: 25820
		private const float UpperMoverOffsetEnd = 0.6f;

		// Token: 0x040064DD RID: 25821
		private static readonly float UpperMoverAltitude = AltitudeLayer.DoorMoveable.AltitudeFor() + 0.018292684f;

		// Token: 0x040064DE RID: 25822
		private static readonly Vector3 MoverDrawScale = new Vector3(0.5f, 1f, 1f);
	}
}
