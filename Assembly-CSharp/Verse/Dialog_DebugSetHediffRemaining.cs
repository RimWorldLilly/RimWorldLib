using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020005D9 RID: 1497
	public class Dialog_DebugSetHediffRemaining : Window
	{
		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002F8A RID: 12170 RVA: 0x00130F9C File Offset: 0x0012F19C
		private HediffComp_Disappears Comp
		{
			get
			{
				return this.hediff.TryGetComp<HediffComp_Disappears>();
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06002F8B RID: 12171 RVA: 0x00130FA9 File Offset: 0x0012F1A9
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(300f, 125f);
			}
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x00130FBA File Offset: 0x0012F1BA
		public Dialog_DebugSetHediffRemaining(Hediff hediff, IWindowDrawing customWindowDrawing = null) : base(customWindowDrawing)
		{
			this.hediff = hediff;
			this.layer = WindowLayer.Super;
			this.closeOnClickedOutside = true;
			this.doWindowBackground = false;
			this.drawShadow = false;
			this.preventCameraMotion = false;
			SoundDefOf.FloatMenu_Open.PlayOneShotOnCamera(null);
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x00130FF8 File Offset: 0x0012F1F8
		protected override void SetInitialSizeAndPosition()
		{
			Vector2 vector = UI.MousePositionOnUIInverted + Dialog_DebugSetHediffRemaining.InitialPositionShift;
			if (vector.x + this.InitialSize.x > (float)UI.screenWidth)
			{
				vector.x = (float)UI.screenWidth - this.InitialSize.x;
			}
			if (vector.y + this.InitialSize.y > (float)UI.screenHeight)
			{
				vector.y = (float)UI.screenHeight - this.InitialSize.y;
			}
			this.windowRect = new Rect(vector.x, vector.y, this.InitialSize.x, this.InitialSize.y);
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x001310A8 File Offset: 0x0012F2A8
		public override void DoWindowContents(Rect inRect)
		{
			if (this.hediff == null || this.Comp == null || !this.hediff.pawn.health.hediffSet.hediffs.Contains(this.hediff))
			{
				this.Close(true);
			}
			Widgets.DrawWindowBackground(inRect);
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(new Rect(0f, 0f, inRect.width, 40f), this.hediff.def.defName + " time remaining");
			Text.Anchor = TextAnchor.UpperLeft;
			float num = 190f;
			float num2 = inRect.width / 2f - num / 2f;
			if (Widgets.ButtonText(new Rect(num2, 50f, 40f, 30f), "1d", true, true, true, null))
			{
				this.SetTicks(60000);
			}
			float num3 = num2 + 50f;
			if (Widgets.ButtonText(new Rect(num3, 50f, 40f, 30f), "12h", true, true, true, null))
			{
				this.SetTicks(30000);
			}
			float num4 = num3 + 50f;
			if (Widgets.ButtonText(new Rect(num4, 50f, 40f, 30f), "6h", true, true, true, null))
			{
				this.SetTicks(15000);
			}
			if (Widgets.ButtonText(new Rect(num4 + 50f, 50f, 40f, 30f), "1h", true, true, true, null))
			{
				this.SetTicks(2500);
			}
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x00131251 File Offset: 0x0012F451
		private void SetTicks(int ticks)
		{
			this.Comp.ticksToDisappear = ticks;
			SoundDefOf.Click.PlayOneShotOnCamera(null);
			this.Close(true);
		}

		// Token: 0x04001E3D RID: 7741
		private Hediff hediff;

		// Token: 0x04001E3E RID: 7742
		private const float HeaderLabelHeight = 40f;

		// Token: 0x04001E3F RID: 7743
		private const float ButtonHeight = 30f;

		// Token: 0x04001E40 RID: 7744
		private const float ButtonWidth = 40f;

		// Token: 0x04001E41 RID: 7745
		private const float Padding = 10f;

		// Token: 0x04001E42 RID: 7746
		private static readonly Vector2 InitialPositionShift = new Vector2(4f, 0f);
	}
}
