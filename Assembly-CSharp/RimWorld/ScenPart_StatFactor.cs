using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020017BB RID: 6075
	public class ScenPart_StatFactor : ScenPart
	{
		// Token: 0x06009787 RID: 38791 RVA: 0x0036F84E File Offset: 0x0036DA4E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<StatDef>(ref this.stat, "stat");
			Scribe_Values.Look<float>(ref this.factor, "factor", 0f, false);
		}

		// Token: 0x06009788 RID: 38792 RVA: 0x0036F87C File Offset: 0x0036DA7C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			if (Widgets.ButtonText(scenPartRect.TopHalf(), this.stat.LabelCap, true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (StatDef statDef in DefDatabase<StatDef>.AllDefs)
				{
					if (!statDef.forInformationOnly && statDef.CanShowWithLoadedMods())
					{
						StatDef localSd = statDef;
						list.Add(new FloatMenuOption(localSd.LabelForFullStatListCap, delegate()
						{
							this.stat = localSd;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			Rect rect = scenPartRect.BottomHalf();
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, "multiplier".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldPercent(rect3, ref this.factor, ref this.factorBuf, 0f, 100f);
		}

		// Token: 0x06009789 RID: 38793 RVA: 0x0036F9C8 File Offset: 0x0036DBC8
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StatFactor".Translate(this.stat.label, this.factor.ToStringPercent());
		}

		// Token: 0x0600978A RID: 38794 RVA: 0x0036F9FC File Offset: 0x0036DBFC
		public override void Randomize()
		{
			this.stat = (from d in DefDatabase<StatDef>.AllDefs
			where d.scenarioRandomizable
			select d).RandomElement<StatDef>();
			this.factor = GenMath.RoundedHundredth(Rand.Range(0.1f, 3f));
		}

		// Token: 0x0600978B RID: 38795 RVA: 0x0036FA58 File Offset: 0x0036DC58
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_StatFactor scenPart_StatFactor = other as ScenPart_StatFactor;
			if (scenPart_StatFactor != null && scenPart_StatFactor.stat == this.stat)
			{
				this.factor *= scenPart_StatFactor.factor;
				return true;
			}
			return false;
		}

		// Token: 0x0600978C RID: 38796 RVA: 0x0036FA93 File Offset: 0x0036DC93
		public float GetStatFactor(StatDef stat)
		{
			if (stat == this.stat)
			{
				return this.factor;
			}
			return 1f;
		}

		// Token: 0x0600978D RID: 38797 RVA: 0x0036FAAA File Offset: 0x0036DCAA
		public override bool HasNullDefs()
		{
			return base.HasNullDefs() || this.stat == null;
		}

		// Token: 0x0600978E RID: 38798 RVA: 0x0036FABF File Offset: 0x0036DCBF
		public override int GetHashCode()
		{
			return base.GetHashCode() ^ ((this.stat != null) ? this.stat.GetHashCode() : 0) ^ this.factor.GetHashCode();
		}

		// Token: 0x04005ABD RID: 23229
		private StatDef stat;

		// Token: 0x04005ABE RID: 23230
		private float factor;

		// Token: 0x04005ABF RID: 23231
		private string factorBuf;
	}
}
