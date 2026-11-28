using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200053C RID: 1340
	public class CompColorable : ThingComp
	{
		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06002766 RID: 10086 RVA: 0x000F6458 File Offset: 0x000F4658
		// (set) Token: 0x06002767 RID: 10087 RVA: 0x000F6460 File Offset: 0x000F4660
		public Color? DesiredColor
		{
			get
			{
				return this.desiredColor;
			}
			set
			{
				this.desiredColor = value;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06002768 RID: 10088 RVA: 0x000F6469 File Offset: 0x000F4669
		public Color Color
		{
			get
			{
				if (!this.active)
				{
					return this.parent.def.graphicData.color;
				}
				return this.color;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06002769 RID: 10089 RVA: 0x000F648F File Offset: 0x000F468F
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x0600276A RID: 10090 RVA: 0x000F6498 File Offset: 0x000F4698
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.parent.def.colorGenerator != null && (this.parent.Stuff == null || this.parent.Stuff.stuffProps.allowColorGenerators))
			{
				this.SetColor(this.parent.def.colorGenerator.NewRandomizedColor());
			}
		}

		// Token: 0x0600276B RID: 10091 RVA: 0x000F6500 File Offset: 0x000F4700
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Color?>(ref this.desiredColor, "desiredColor", null, false);
			if (Scribe.mode == LoadSaveMode.Saving && !this.active)
			{
				return;
			}
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.active, "colorActive", false, false);
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000F656A File Offset: 0x000F476A
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			if (this.active)
			{
				piece.SetColor(this.color, true);
			}
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000F6588 File Offset: 0x000F4788
		public void Recolor()
		{
			if (this.desiredColor == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried recoloring apparel ");
				defaultInterpolatedStringHandler.AppendFormatted<ThingWithComps>(this.parent);
				defaultInterpolatedStringHandler.AppendLiteral(" which does not have a desired color set!");
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			this.SetColor(this.DesiredColor.Value);
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000F65F2 File Offset: 0x000F47F2
		public void Disable()
		{
			this.active = false;
			this.color = Color.white;
			this.desiredColor = null;
			this.parent.Notify_ColorChanged();
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000F661D File Offset: 0x000F481D
		public void SetColor(Color value)
		{
			if (value == this.color)
			{
				return;
			}
			this.active = true;
			this.color = value;
			this.desiredColor = null;
			this.parent.Notify_ColorChanged();
		}

		// Token: 0x04001C45 RID: 7237
		private Color? desiredColor;

		// Token: 0x04001C46 RID: 7238
		private Color color = Color.white;

		// Token: 0x04001C47 RID: 7239
		private bool active;
	}
}
