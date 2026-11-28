using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200024B RID: 587
	internal sealed class DebugCell
	{
		// Token: 0x06001146 RID: 4422 RVA: 0x000631D8 File Offset: 0x000613D8
		public void Draw()
		{
			if (this.customMat != null)
			{
				CellRenderer.RenderCell(this.c, this.customMat);
				return;
			}
			CellRenderer.RenderCell(this.c, this.colorPct);
		}

		// Token: 0x06001147 RID: 4423 RVA: 0x0006320C File Offset: 0x0006140C
		public void OnGUI()
		{
			if (this.displayString != null)
			{
				Vector2 vector = this.c.ToUIPosition();
				Rect rect = new Rect(vector.x - 20f, vector.y - 20f, 40f, 40f);
				if (new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight).Overlaps(rect))
				{
					Widgets.Label(rect, this.displayString);
				}
			}
		}

		// Token: 0x04000FC8 RID: 4040
		public IntVec3 c;

		// Token: 0x04000FC9 RID: 4041
		public string displayString;

		// Token: 0x04000FCA RID: 4042
		public float colorPct;

		// Token: 0x04000FCB RID: 4043
		public int ticksLeft;

		// Token: 0x04000FCC RID: 4044
		public Material customMat;
	}
}
