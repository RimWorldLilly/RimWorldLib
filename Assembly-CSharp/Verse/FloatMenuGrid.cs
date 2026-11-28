using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005F1 RID: 1521
	public class FloatMenuGrid : Window
	{
		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06002FCC RID: 12236 RVA: 0x0013229C File Offset: 0x0013049C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(this.TotalWidth, this.TotalHeight);
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06002FCD RID: 12237 RVA: 0x001322AF File Offset: 0x001304AF
		public float TotalWidth
		{
			get
			{
				return (float)this.calculatedSquareSize * (FloatMenuGrid.OptionSize.x - 1f);
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06002FCE RID: 12238 RVA: 0x001322C9 File Offset: 0x001304C9
		public float TotalHeight
		{
			get
			{
				return (float)this.calculatedSquareSize * (FloatMenuGrid.OptionSize.y - 1f);
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06002FCF RID: 12239 RVA: 0x001322E3 File Offset: 0x001304E3
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x001322EC File Offset: 0x001304EC
		public FloatMenuGrid(List<FloatMenuGridOption> options) : base(null)
		{
			this.options = (from op in options
			orderby op.Priority descending
			select op).ToList<FloatMenuGridOption>();
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			this.calculatedSquareSize = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(Mathf.Round(Mathf.Sqrt((float)options.Count)), 2f)));
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x00132394 File Offset: 0x00130594
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (mousePositionOnUIInverted.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				mousePositionOnUIInverted.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (mousePositionOnUIInverted.y > (float)UI.screenHeight)
			{
				mousePositionOnUIInverted.y = (float)UI.screenHeight;
			}
			this.windowRect = new Rect(mousePositionOnUIInverted.x, mousePositionOnUIInverted.y - this.InitialSize.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x00132430 File Offset: 0x00130630
		public override void DoWindowContents(Rect rect)
		{
			this.UpdateBaseColor();
			GUI.color = this.baseColor;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this.options.Count; i++)
			{
				FloatMenuGridOption floatMenuGridOption = this.options[i];
				float num3 = (float)num * FloatMenuGrid.OptionSize.x;
				float num4 = (float)num2 * FloatMenuGrid.OptionSize.y;
				if (num3 > 0f)
				{
					num3 -= (float)num;
				}
				if (num4 > 0f)
				{
					num4 -= (float)num2;
				}
				if (floatMenuGridOption.OnGUI(new Rect(num3, num4, FloatMenuGrid.OptionSize.x, FloatMenuGrid.OptionSize.y)))
				{
					Find.WindowStack.TryRemove(this, true);
					break;
				}
				num++;
				if (num >= this.calculatedSquareSize)
				{
					num = 0;
					num2++;
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x00132504 File Offset: 0x00130704
		private void UpdateBaseColor()
		{
			this.baseColor = Color.white;
			Rect r = new Rect(0f, 0f, this.TotalWidth, this.TotalHeight).ExpandedBy(5f);
			if (!r.Contains(Event.current.mousePosition))
			{
				float num = GenUI.DistFromRect(r, Event.current.mousePosition);
				this.baseColor = new Color(1f, 1f, 1f, 1f - num / 95f);
				if (num > 95f)
				{
					this.Close(false);
					SoundDefOf.FloatMenu_Cancel.PlayOneShotOnCamera(null);
					Find.WindowStack.TryRemove(this, true);
					return;
				}
			}
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x001325B5 File Offset: 0x001307B5
		public override void PostClose()
		{
			base.PostClose();
			Action action = this.onCloseCallback;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04001E6B RID: 7787
		private List<FloatMenuGridOption> options;

		// Token: 0x04001E6C RID: 7788
		private int calculatedSquareSize;

		// Token: 0x04001E6D RID: 7789
		private Color baseColor = Color.white;

		// Token: 0x04001E6E RID: 7790
		public Action onCloseCallback;

		// Token: 0x04001E6F RID: 7791
		private static readonly Vector2 OptionSize = new Vector2(34f, 34f);
	}
}
