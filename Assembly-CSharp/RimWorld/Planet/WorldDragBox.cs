using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023F7 RID: 9207
	public class WorldDragBox
	{
		// Token: 0x1700289E RID: 10398
		// (get) Token: 0x0600E624 RID: 58916 RVA: 0x005596EA File Offset: 0x005578EA
		public float LeftX
		{
			get
			{
				return Math.Min(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x1700289F RID: 10399
		// (get) Token: 0x0600E625 RID: 58917 RVA: 0x00559706 File Offset: 0x00557906
		public float RightX
		{
			get
			{
				return Math.Max(this.start.x, UI.MousePositionOnUIInverted.x);
			}
		}

		// Token: 0x170028A0 RID: 10400
		// (get) Token: 0x0600E626 RID: 58918 RVA: 0x00559722 File Offset: 0x00557922
		public float BotZ
		{
			get
			{
				return Math.Min(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x170028A1 RID: 10401
		// (get) Token: 0x0600E627 RID: 58919 RVA: 0x0055973E File Offset: 0x0055793E
		public float TopZ
		{
			get
			{
				return Math.Max(this.start.y, UI.MousePositionOnUIInverted.y);
			}
		}

		// Token: 0x170028A2 RID: 10402
		// (get) Token: 0x0600E628 RID: 58920 RVA: 0x0055975A File Offset: 0x0055795A
		public Rect ScreenRect
		{
			get
			{
				return new Rect(this.LeftX, this.BotZ, this.RightX - this.LeftX, this.TopZ - this.BotZ);
			}
		}

		// Token: 0x170028A3 RID: 10403
		// (get) Token: 0x0600E629 RID: 58921 RVA: 0x00559788 File Offset: 0x00557988
		public float Diagonal
		{
			get
			{
				return (this.start - new Vector2(UI.MousePositionOnUIInverted.x, UI.MousePositionOnUIInverted.y)).magnitude;
			}
		}

		// Token: 0x170028A4 RID: 10404
		// (get) Token: 0x0600E62A RID: 58922 RVA: 0x005597C1 File Offset: 0x005579C1
		public bool IsValid
		{
			get
			{
				return this.Diagonal > 7f;
			}
		}

		// Token: 0x170028A5 RID: 10405
		// (get) Token: 0x0600E62B RID: 58923 RVA: 0x005597D0 File Offset: 0x005579D0
		public bool IsValidAndActive
		{
			get
			{
				return this.active && this.IsValid;
			}
		}

		// Token: 0x0600E62C RID: 58924 RVA: 0x005597E2 File Offset: 0x005579E2
		public void DragBoxOnGUI()
		{
			if (this.IsValidAndActive)
			{
				Widgets.DrawBox(this.ScreenRect, 2, null);
			}
		}

		// Token: 0x0600E62D RID: 58925 RVA: 0x005597F9 File Offset: 0x005579F9
		public bool Contains(WorldObject o)
		{
			return this.Contains(o.ScreenPos());
		}

		// Token: 0x0600E62E RID: 58926 RVA: 0x00559808 File Offset: 0x00557A08
		public bool Contains(Vector2 screenPoint)
		{
			return screenPoint.x + 0.5f > this.LeftX && screenPoint.x - 0.5f < this.RightX && screenPoint.y + 0.5f > this.BotZ && screenPoint.y - 0.5f < this.TopZ;
		}

		// Token: 0x0400924C RID: 37452
		public bool active;

		// Token: 0x0400924D RID: 37453
		public Vector2 start;

		// Token: 0x0400924E RID: 37454
		private const float DragBoxMinDiagonal = 7f;
	}
}
