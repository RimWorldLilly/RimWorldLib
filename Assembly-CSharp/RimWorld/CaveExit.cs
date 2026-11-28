using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B21 RID: 6945
	public class CaveExit : PocketMapExit
	{
		// Token: 0x17002013 RID: 8211
		// (get) Token: 0x0600B1F5 RID: 45557 RVA: 0x003EADD7 File Offset: 0x003E8FD7
		private Graphic RopeGraphic
		{
			get
			{
				if (this.cachedRopeGraphic == null)
				{
					this.cachedRopeGraphic = GraphicDatabase.Get<Graphic_Single_AgeSecs>("Things/Building/Misc/CaveExit/CaveExit_Rope", ShaderDatabase.CaveExitRope, this.def.graphicData.drawSize, Color.white);
				}
				return this.cachedRopeGraphic;
			}
		}

		// Token: 0x0600B1F6 RID: 45558 RVA: 0x003EAE11 File Offset: 0x003E9011
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.RopeGraphic.Draw(this.DrawPos + CaveExit.RopeDrawOffset, Rot4.North, this, 0f);
		}

		// Token: 0x040066B0 RID: 26288
		private static readonly Vector3 RopeDrawOffset = new Vector3(0f, 1f, 1f);

		// Token: 0x040066B1 RID: 26289
		[Unsaved(false)]
		private Graphic cachedRopeGraphic;
	}
}
