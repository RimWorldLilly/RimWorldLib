using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E80 RID: 3712
	public class ThoughtWorker_MyChildEnslaved : ThoughtWorker
	{
		// Token: 0x0600623B RID: 25147 RVA: 0x0023532E File Offset: 0x0023352E
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, ThoughtWorker_MyChildEnslaved.ChildrenEnslavedCount(p));
		}

		// Token: 0x0600623C RID: 25148 RVA: 0x00235348 File Offset: 0x00233548
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = Mathf.RoundToInt(this.MoodMultiplier(p));
			if (num <= 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x0600623D RID: 25149 RVA: 0x00235388 File Offset: 0x00233588
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (p.Suspended)
			{
				return ThoughtState.Inactive;
			}
			if (ThoughtWorker_MyChildEnslaved.ChildrenEnslavedCount(p) <= 0)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x0600623E RID: 25150 RVA: 0x002353B9 File Offset: 0x002335B9
		private static int ChildrenEnslavedCount(Pawn parent)
		{
			return parent.relations.Children.Count((Pawn child) => child.DevelopmentalStage.Juvenile() && !child.Dead && child.IsSlave);
		}
	}
}
