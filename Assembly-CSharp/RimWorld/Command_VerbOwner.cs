using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001812 RID: 6162
	public class Command_VerbOwner : Command_VerbTarget
	{
		// Token: 0x17001A34 RID: 6708
		// (get) Token: 0x060099F9 RID: 39417 RVA: 0x0037A388 File Offset: 0x00378588
		public override string Label
		{
			get
			{
				if (this.comp.parent.def.mergeVerbGizmos || Find.Selector.SelectedPawns.Count < 2)
				{
					return base.Label;
				}
				return base.Label + " (" + this.verb.caster.LabelShortCap + ")";
			}
		}

		// Token: 0x060099FA RID: 39418 RVA: 0x0037A3EA File Offset: 0x003785EA
		public Command_VerbOwner(CompApparelVerbOwner comp)
		{
			this.comp = comp;
		}

		// Token: 0x17001A35 RID: 6709
		// (get) Token: 0x060099FB RID: 39419 RVA: 0x0037A3F9 File Offset: 0x003785F9
		public override string TopRightLabel
		{
			get
			{
				return this.comp.GizmoExtraLabel;
			}
		}

		// Token: 0x17001A36 RID: 6710
		// (get) Token: 0x060099FC RID: 39420 RVA: 0x0037A408 File Offset: 0x00378608
		public override Color IconDrawColor
		{
			get
			{
				Color? color = this.overrideColor;
				if (color == null)
				{
					return base.IconDrawColor;
				}
				return color.GetValueOrDefault();
			}
		}

		// Token: 0x060099FD RID: 39421 RVA: 0x0037A433 File Offset: 0x00378633
		public override void GizmoUpdateOnMouseover()
		{
			this.verb.DrawHighlight(LocalTargetInfo.Invalid);
		}

		// Token: 0x060099FE RID: 39422 RVA: 0x0037A448 File Offset: 0x00378648
		public override bool GroupsWith(Gizmo other)
		{
			if (!base.GroupsWith(other))
			{
				return false;
			}
			Command_VerbOwner command_VerbOwner = other as Command_VerbOwner;
			return command_VerbOwner != null && this.comp.parent.def == command_VerbOwner.comp.parent.def && this.comp.parent.def.mergeVerbGizmos;
		}

		// Token: 0x04005BBB RID: 23483
		private readonly CompApparelVerbOwner comp;

		// Token: 0x04005BBC RID: 23484
		public Color? overrideColor;
	}
}
