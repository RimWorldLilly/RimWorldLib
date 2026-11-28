using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F9E RID: 3998
	public abstract class MainButtonWorker
	{
		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x060065E7 RID: 26087 RVA: 0x00243E76 File Offset: 0x00242076
		public virtual float ButtonBarPercent
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x060065E8 RID: 26088 RVA: 0x00243E80 File Offset: 0x00242080
		public virtual bool Disabled
		{
			get
			{
				return (Find.CurrentMap == null && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World)) || (Find.WorldRoutePlanner.Active && Find.WorldRoutePlanner.FormingCaravan && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World)) || (Find.TilePicker.Active && !Find.TilePicker.AllowEscape && (!this.def.validWithoutMap || this.def == MainButtonDefOf.World));
			}
		}

		// Token: 0x17001173 RID: 4467
		// (get) Token: 0x060065E9 RID: 26089 RVA: 0x00243F19 File Offset: 0x00242119
		public virtual bool Visible
		{
			get
			{
				return (!ModsConfig.IdeologyActive || this.def.validWithClassicIdeo || !Find.IdeoManager.classicMode) && this.def.buttonVisible;
			}
		}

		// Token: 0x060065EA RID: 26090
		public abstract void Activate();

		// Token: 0x060065EB RID: 26091 RVA: 0x00243F48 File Offset: 0x00242148
		public virtual void InterfaceTryActivate()
		{
			if (TutorSystem.TutorialMode && this.def.canBeTutorDenied && Find.MainTabsRoot.OpenTab != this.def && !TutorSystem.AllowAction("MainTab-" + this.def.defName + "-Open"))
			{
				return;
			}
			if (this.def.closesWorldView && Find.TilePicker.Active && !Find.TilePicker.AllowEscape)
			{
				Messages.Message("MessagePlayerMustSelectTile".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			this.Activate();
		}

		// Token: 0x060065EC RID: 26092 RVA: 0x00243FE8 File Offset: 0x002421E8
		public virtual void DoButton(Rect rect)
		{
			Text.Font = GameFont.Small;
			string text = this.def.LabelCap;
			float num = this.def.LabelCapWidth;
			if (num > rect.width - 2f)
			{
				text = this.def.ShortenedLabelCap;
				num = this.def.ShortenedLabelCapWidth;
			}
			if (this.Disabled)
			{
				Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
				if (Event.current.type == EventType.mouseDown && Mouse.IsOver(rect))
				{
					Event.current.Use();
					return;
				}
			}
			else
			{
				bool flag = num > 0.85f * rect.width - 1f;
				Rect rect2 = rect;
				string label = (this.def.Icon == null) ? text : "";
				float textLeftMargin = flag ? 2f : -1f;
				if (Widgets.ButtonTextSubtle(rect2, label, this.ButtonBarPercent, textLeftMargin, SoundDefOf.Mouseover_Category, default(Vector2), null, false))
				{
					this.InterfaceTryActivate();
				}
				if (this.def.Icon != null)
				{
					Vector2 vector = rect.center;
					float num2 = 16f;
					if (Mouse.IsOver(rect))
					{
						vector += new Vector2(2f, -2f);
					}
					GUI.DrawTexture(new Rect(vector.x - num2, vector.y - num2, 32f, 32f), this.def.Icon);
				}
				if (Find.MainTabsRoot.OpenTab != this.def && !Find.WindowStack.NonImmediateDialogWindowOpen)
				{
					UIHighlighter.HighlightOpportunity(rect, this.def.cachedHighlightTagClosed);
				}
				if (Mouse.IsOver(rect) && !this.def.description.NullOrEmpty())
				{
					TooltipHandler.TipRegion(rect, this.def.LabelCap.Colorize(ColorLibrary.Yellow) + "\n\n" + this.def.description);
				}
			}
		}

		// Token: 0x04003D5E RID: 15710
		public MainButtonDef def;

		// Token: 0x04003D5F RID: 15711
		private const float CompactModeMargin = 2f;

		// Token: 0x04003D60 RID: 15712
		private const float IconSize = 32f;
	}
}
