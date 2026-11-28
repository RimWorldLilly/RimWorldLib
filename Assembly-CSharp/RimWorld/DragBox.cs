using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D5B RID: 7515
	public class DragBox
	{
		// Token: 0x170023F6 RID: 9206
		// (get) Token: 0x0600C292 RID: 49810 RVA: 0x0046EDEC File Offset: 0x0046CFEC
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x170023F7 RID: 9207
		// (get) Token: 0x0600C293 RID: 49811 RVA: 0x0046EE08 File Offset: 0x0046D008
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MouseMapPosition().x);
			}
		}

		// Token: 0x170023F8 RID: 9208
		// (get) Token: 0x0600C294 RID: 49812 RVA: 0x0046EE24 File Offset: 0x0046D024
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x170023F9 RID: 9209
		// (get) Token: 0x0600C295 RID: 49813 RVA: 0x0046EE40 File Offset: 0x0046D040
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.z, UI.MouseMapPosition().z);
			}
		}

		// Token: 0x170023FA RID: 9210
		// (get) Token: 0x0600C296 RID: 49814 RVA: 0x0046EE5C File Offset: 0x0046D05C
		public Rect ScreenRect
		{
			get
			{
				Vector2 vector = this.start.MapToUIPosition();
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				if (mousePositionOnUIInverted.x < vector.x)
				{
					float x = mousePositionOnUIInverted.x;
					mousePositionOnUIInverted.x = vector.x;
					vector.x = x;
				}
				if (mousePositionOnUIInverted.y < vector.y)
				{
					float y = mousePositionOnUIInverted.y;
					mousePositionOnUIInverted.y = vector.y;
					vector.y = y;
				}
				return new Rect
				{
					xMin = vector.x,
					xMax = mousePositionOnUIInverted.x,
					yMin = vector.y,
					yMax = mousePositionOnUIInverted.y
				};
			}
		}

		// Token: 0x170023FB RID: 9211
		// (get) Token: 0x0600C297 RID: 49815 RVA: 0x0046EF10 File Offset: 0x0046D110
		public bool IsValid
		{
			get
			{
				return (this.start - UI.MouseMapPosition()).magnitude > 0.5f;
			}
		}

		// Token: 0x170023FC RID: 9212
		// (get) Token: 0x0600C298 RID: 49816 RVA: 0x0046EF3C File Offset: 0x0046D13C
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x0600C299 RID: 49817 RVA: 0x0046EF4E File Offset: 0x0046D14E
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2, null);
			}
		}

		// Token: 0x0600C29A RID: 49818 RVA: 0x0046EF68 File Offset: 0x0046D168
		public bool Contains(Thing t)
		{
			if (t is Pawn)
			{
				return this.Contains((t as Pawn).Drawer.DrawPos);
			}
			foreach (IntVec3 intVec in t.OccupiedRect())
			{
				if (this.Contains(intVec.ToVector3Shifted()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C29B RID: 49819 RVA: 0x0046EFEC File Offset: 0x0046D1EC
		public bool Contains(Vector3 v)
		{
			return v.x + 0.5f > this.LeftX && v.x - 0.5f < this.RightX && v.z + 0.5f > this.BotZ && v.z - 0.5f < this.TopZ;
		}

		// Token: 0x04006F46 RID: 28486
		public bool active;

		// Token: 0x04006F47 RID: 28487
		public Vector3 start;

		// Token: 0x04006F48 RID: 28488
		private const float DragBoxMinDiagonal = 0.5f;
	}
}
