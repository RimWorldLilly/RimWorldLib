using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200108B RID: 4235
	public class QuestPart_ShuttleDelay : QuestPart_Delay
	{
		// Token: 0x1700122A RID: 4650
		// (get) Token: 0x06006A16 RID: 27158 RVA: 0x00255969 File Offset: 0x00253B69
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_ShuttleDelay.<get_QuestLookTargets>d__3 <get_QuestLookTargets>d__ = new QuestPart_ShuttleDelay.<get_QuestLookTargets>d__3(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06006A17 RID: 27159 RVA: 0x00255979 File Offset: 0x00253B79
		public override AlertReport AlertReport
		{
			get
			{
				if (!this.alert || base.State != QuestPartState.Enabled)
				{
					return false;
				}
				return AlertReport.CulpritsAre(this.lodgers);
			}
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06006A18 RID: 27160 RVA: 0x0025599E File Offset: 0x00253B9E
		public override bool AlertCritical
		{
			get
			{
				return base.TicksLeft < 60000;
			}
		}

		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x06006A19 RID: 27161 RVA: 0x002559AD File Offset: 0x00253BAD
		public override string AlertLabel
		{
			get
			{
				return "QuestPartShuttleArriveDelay".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose(true, true));
			}
		}

		// Token: 0x1700122E RID: 4654
		// (get) Token: 0x06006A1A RID: 27162 RVA: 0x002559D0 File Offset: 0x00253BD0
		public override string AlertExplanation
		{
			get
			{
				if (this.quest.hidden)
				{
					return "QuestPartShuttleArriveDelayDescHidden".Translate(base.TicksLeft.ToStringTicksToPeriodVerbose(true, true).Colorize(ColoredText.DateTimeColor));
				}
				return "QuestPartShuttleArriveDelayDesc".Translate(this.quest.name, base.TicksLeft.ToStringTicksToPeriodVerbose(true, true).Colorize(ColoredText.DateTimeColor), (from p in this.lodgers
				select p.LabelShort).ToLineList("- ", false));
			}
		}

		// Token: 0x06006A1B RID: 27163 RVA: 0x00255A8C File Offset: 0x00253C8C
		public override string ExtraInspectString(ISelectable target)
		{
			Pawn pawn = target as Pawn;
			if (pawn != null && this.lodgers.Contains(pawn))
			{
				return "ShuttleDelayInspectString".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			return null;
		}

		// Token: 0x06006A1C RID: 27164 RVA: 0x00255AD8 File Offset: 0x00253CD8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.lodgers, "lodgers", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.alert, "alert", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lodgers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06006A1D RID: 27165 RVA: 0x00255B46 File Offset: 0x00253D46
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.lodgers.AddRange(Find.RandomPlayerHomeMap.mapPawns.FreeColonists);
			}
		}

		// Token: 0x06006A1E RID: 27166 RVA: 0x00255B6F File Offset: 0x00253D6F
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.lodgers.Replace(replace, with);
		}

		// Token: 0x04004371 RID: 17265
		public List<Pawn> lodgers = new List<Pawn>();

		// Token: 0x04004372 RID: 17266
		public bool alert;
	}
}
