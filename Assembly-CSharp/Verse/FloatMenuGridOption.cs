using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005F2 RID: 1522
	public class FloatMenuGridOption
	{
		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06002FD6 RID: 12246 RVA: 0x001325E3 File Offset: 0x001307E3
		public bool Disabled
		{
			get
			{
				return this.action == null;
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06002FD7 RID: 12247 RVA: 0x001325EE File Offset: 0x001307EE
		public MenuOptionPriority Priority
		{
			get
			{
				if (this.Disabled)
				{
					return MenuOptionPriority.DisabledOption;
				}
				return MenuOptionPriority.Default;
			}
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x001325FC File Offset: 0x001307FC
		public FloatMenuGridOption(Texture2D texture, Action action, Color? color = null, TipSignal? tooltip = null)
		{
			this.texture = texture;
			this.action = action;
			this.color = (color ?? Color.white);
			this.tooltip = tooltip;
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x00132670 File Offset: 0x00130870
		public bool OnGUI(Rect rect)
		{
			bool flag = !this.Disabled && Mouse.IsOver(rect);
			if (!this.Disabled)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (this.tooltip != null)
			{
				TooltipHandler.TipRegion(rect, this.tooltip.Value);
			}
			Color color = GUI.color;
			if (this.Disabled)
			{
				GUI.color = FloatMenuOption.ColorBGDisabled * color;
			}
			else if (flag)
			{
				GUI.color = FloatMenuOption.ColorBGActiveMouseover * color;
			}
			else
			{
				GUI.color = FloatMenuOption.ColorBGActive * color;
			}
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = ((!this.Disabled) ? FloatMenuOption.ColorTextActive : FloatMenuOption.ColorTextDisabled) * color;
			Widgets.DrawAtlas(rect, TexUI.FloatMenuOptionBG);
			GUI.color = new Color(this.color.r, this.color.g, this.color.b, this.color.a * color.a);
			Rect rect2 = rect.ContractedBy(2f);
			if (!flag)
			{
				rect2 = rect2.ContractedBy(2f);
			}
			Material mat = this.Disabled ? TexUI.GrayscaleGUI : null;
			Widgets.DrawTextureFitted(rect2, this.texture, 1f, new Vector2(1f, 1f), this.iconTexCoords, 0f, mat, 1f);
			GUI.color = color;
			Action<Rect> action = this.postDrawAction;
			if (action != null)
			{
				action(rect2);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.Chosen();
				return true;
			}
			return false;
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x001327F9 File Offset: 0x001309F9
		public void Chosen()
		{
			if (this.Disabled)
			{
				SoundDefOf.ClickReject.PlayOneShotOnCamera(null);
				return;
			}
			Action action = this.action;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x04001E70 RID: 7792
		public Texture2D texture;

		// Token: 0x04001E71 RID: 7793
		public Color color = Color.white;

		// Token: 0x04001E72 RID: 7794
		public Action action;

		// Token: 0x04001E73 RID: 7795
		public TipSignal? tooltip;

		// Token: 0x04001E74 RID: 7796
		public Rect iconTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04001E75 RID: 7797
		public Action<Rect> postDrawAction;
	}
}
