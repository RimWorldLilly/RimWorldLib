using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200058F RID: 1423
	public class MoteText : MoteThrown
	{
		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x060029FF RID: 10751 RVA: 0x00100A6D File Offset: 0x000FEC6D
		protected float TimeBeforeStartFadeout
		{
			get
			{
				if (this.overrideTimeBeforeStartFadeout < 0f)
				{
					return base.SolidTime;
				}
				return this.overrideTimeBeforeStartFadeout;
			}
		}

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002A00 RID: 10752 RVA: 0x00100A89 File Offset: 0x000FEC89
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.TimeBeforeStartFadeout + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x00100AAD File Offset: 0x000FECAD
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x00100AB0 File Offset: 0x000FECB0
		public override void DrawGUIOverlay()
		{
			float a = 1f - (base.AgeSecs - this.TimeBeforeStartFadeout) / this.def.mote.fadeOutTime;
			Color color = new Color(this.textColor.r, this.textColor.g, this.textColor.b, a);
			GenMapUI.DrawText(new Vector2(this.exactPosition.x, this.exactPosition.z), this.text, color);
		}

		// Token: 0x04001D17 RID: 7447
		public string text;

		// Token: 0x04001D18 RID: 7448
		public Color textColor = Color.white;

		// Token: 0x04001D19 RID: 7449
		public float overrideTimeBeforeStartFadeout = -1f;
	}
}
