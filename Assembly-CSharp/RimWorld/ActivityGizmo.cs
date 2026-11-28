using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020017ED RID: 6125
	[StaticConstructorOnStartup]
	public class ActivityGizmo : Gizmo_Slider
	{
		// Token: 0x170019F1 RID: 6641
		// (get) Token: 0x060098D5 RID: 39125 RVA: 0x00375C48 File Offset: 0x00373E48
		private CompActivity Comp
		{
			get
			{
				return this.thing.GetComp<CompActivity>();
			}
		}

		// Token: 0x170019F2 RID: 6642
		// (get) Token: 0x060098D6 RID: 39126 RVA: 0x00375C55 File Offset: 0x00373E55
		protected override string Title
		{
			get
			{
				return "ActivityGizmo".Translate();
			}
		}

		// Token: 0x170019F3 RID: 6643
		// (get) Token: 0x060098D7 RID: 39127 RVA: 0x00375C66 File Offset: 0x00373E66
		protected override float ValuePercent
		{
			get
			{
				return this.Comp.ActivityLevel;
			}
		}

		// Token: 0x170019F4 RID: 6644
		// (get) Token: 0x060098D8 RID: 39128 RVA: 0x00375C73 File Offset: 0x00373E73
		protected override bool IsDraggable
		{
			get
			{
				return this.Comp.CanBeSuppressed;
			}
		}

		// Token: 0x170019F5 RID: 6645
		// (get) Token: 0x060098D9 RID: 39129 RVA: 0x00375C80 File Offset: 0x00373E80
		protected override FloatRange DragRange
		{
			get
			{
				return new FloatRange(0.1f, 0.9f);
			}
		}

		// Token: 0x170019F6 RID: 6646
		// (get) Token: 0x060098DA RID: 39130 RVA: 0x00375C91 File Offset: 0x00373E91
		protected override string HighlightTag
		{
			get
			{
				return "ActivityGizmo";
			}
		}

		// Token: 0x170019F7 RID: 6647
		// (get) Token: 0x060098DB RID: 39131 RVA: 0x00375C98 File Offset: 0x00373E98
		// (set) Token: 0x060098DC RID: 39132 RVA: 0x00375C9F File Offset: 0x00373E9F
		protected override bool DraggingBar
		{
			get
			{
				return ActivityGizmo.draggingBar;
			}
			set
			{
				ActivityGizmo.draggingBar = value;
			}
		}

		// Token: 0x170019F8 RID: 6648
		// (get) Token: 0x060098DD RID: 39133 RVA: 0x00375CA7 File Offset: 0x00373EA7
		// (set) Token: 0x060098DE RID: 39134 RVA: 0x00375CB4 File Offset: 0x00373EB4
		protected override float Target
		{
			get
			{
				return this.Comp.suppressIfAbove;
			}
			set
			{
				this.Comp.suppressIfAbove = value;
			}
		}

		// Token: 0x060098DF RID: 39135 RVA: 0x00375CC2 File Offset: 0x00373EC2
		public ActivityGizmo(ThingWithComps thing)
		{
			this.thing = thing;
		}

		// Token: 0x060098E0 RID: 39136 RVA: 0x00375CD4 File Offset: 0x00373ED4
		protected override string GetTooltip()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ActivitySuppressionTooltipTitle".Translate().CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor));
			stringBuilder.AppendLine();
			stringBuilder.Append("ActivitySuppressionTooltipDesc".Translate(this.thing.LabelNoParenthesis, this.Comp.suppressIfAbove.ToStringPercent("0").Colorize(ColoredText.TipSectionTitleColor).Named("LEVEL")).Resolve());
			this.Comp.Props.Worker.GetSummary(this.thing, stringBuilder);
			if (!this.Comp.suppressionEnabled)
			{
				stringBuilder.Append("\n\n" + "ActivitySuppressionTooltipDisabled".Translate(this.thing.LabelNoParenthesis).CapitalizeFirst().Colorize(ColoredText.FactionColor_Hostile));
			}
			foreach (IActivity activity in this.thing.GetComps<IActivity>())
			{
				if (!activity.ActivityTooltipExtra().NullOrEmpty())
				{
					stringBuilder.Append("\n\n" + activity.ActivityTooltipExtra());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060098E1 RID: 39137 RVA: 0x00375E34 File Offset: 0x00374034
		protected override void DrawHeader(Rect headerRect, ref bool mouseOverElement)
		{
			headerRect.xMax -= 24f;
			if (this.Comp.CanBeSuppressed)
			{
				Rect rect = new Rect(headerRect.xMax, headerRect.y, 24f, 24f);
				GUI.DrawTexture(rect, ActivityGizmo.IconTex);
				GUI.DrawTexture(new Rect(rect.center.x, rect.y, rect.width / 2f, rect.height / 2f), this.Comp.suppressionEnabled ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
				if (Widgets.ButtonInvisible(rect, true))
				{
					this.Comp.suppressionEnabled = !this.Comp.suppressionEnabled;
					if (this.Comp.suppressionEnabled)
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
					TooltipHandler.TipRegion(rect, new Func<string>(this.GetTooltipDesc), 828267373);
					mouseOverElement = true;
				}
			}
			base.DrawHeader(headerRect, ref mouseOverElement);
		}

		// Token: 0x060098E2 RID: 39138 RVA: 0x00375F58 File Offset: 0x00374158
		private string GetTooltipDesc()
		{
			string arg = (this.Comp.suppressionEnabled ? "On" : "Off").Translate().ToString().UncapitalizeFirst();
			string arg2 = this.Comp.suppressIfAbove.ToStringPercent("0").Colorize(ColoredText.TipSectionTitleColor);
			return "ActivitySuppressionToggleTooltipDesc".Translate(this.thing.LabelNoParenthesis, arg2.Named("LEVEL"), arg.Named("ONOFF")).Resolve();
		}

		// Token: 0x04005B57 RID: 23383
		private readonly ThingWithComps thing;

		// Token: 0x04005B58 RID: 23384
		private static bool draggingBar;

		// Token: 0x04005B59 RID: 23385
		private static readonly Texture2D IconTex = ContentFinder<Texture2D>.Get("UI/Icons/SuppressionToggle", true);
	}
}
