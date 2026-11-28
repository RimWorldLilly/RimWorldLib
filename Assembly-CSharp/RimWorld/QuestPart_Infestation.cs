using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111D RID: 4381
	public class QuestPart_Infestation : QuestPart
	{
		// Token: 0x170012CB RID: 4811
		// (get) Token: 0x06006D8E RID: 28046 RVA: 0x002661E1 File Offset: 0x002643E1
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_Infestation.<get_QuestLookTargets>d__11 <get_QuestLookTargets>d__ = new QuestPart_Infestation.<get_QuestLookTargets>d__11(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x170012CC RID: 4812
		// (get) Token: 0x06006D8F RID: 28047 RVA: 0x002661F1 File Offset: 0x002643F1
		public override string QuestSelectTargetsLabel
		{
			get
			{
				return "SelectHiveTargets".Translate();
			}
		}

		// Token: 0x170012CD RID: 4813
		// (get) Token: 0x06006D90 RID: 28048 RVA: 0x00266202 File Offset: 0x00264402
		public override IEnumerable<GlobalTargetInfo> QuestSelectTargets
		{
			get
			{
				QuestPart_Infestation.<get_QuestSelectTargets>d__15 <get_QuestSelectTargets>d__ = new QuestPart_Infestation.<get_QuestSelectTargets>d__15(-2);
				<get_QuestSelectTargets>d__.<>4__this = this;
				return <get_QuestSelectTargets>d__;
			}
		}

		// Token: 0x06006D91 RID: 28049 RVA: 0x00266214 File Offset: 0x00264414
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.loc = IntVec3.Invalid;
				if (this.mapParent == null || !this.mapParent.HasMap || !this.quest.IsParentSuitableForQuest(this.mapParent))
				{
					MapParent mapParent;
					if ((mapParent = this.quest.TryFindNewSuitableMapParentForRetarget()) == null)
					{
						Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
						mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
					}
					this.mapParent = mapParent;
				}
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					Thing thing = InfestationUtility.SpawnTunnels(this.hivesCount, this.mapParent.Map, true, true, this.tag, this.overrideLoc, null);
					if (thing != null)
					{
						this.loc = (this.overrideLoc ?? thing.Position);
						if (this.sendStandardLetter)
						{
							TaggedString label = this.customLetterLabel.NullOrEmpty() ? IncidentDefOf.Infestation.letterLabel : this.customLetterLabel.Formatted(IncidentDefOf.Infestation.letterLabel.Named("BASELABEL"));
							TaggedString text = this.customLetterText.NullOrEmpty() ? IncidentDefOf.Infestation.letterText : this.customLetterText.Formatted(IncidentDefOf.Infestation.letterText.Named("BASETEXT"));
							Find.LetterStack.ReceiveLetter(label, text, this.customLetterDef ?? IncidentDefOf.Infestation.letterDef, new TargetInfo(this.loc, this.mapParent.Map, false), null, this.quest, null, null, 0, true);
						}
					}
				}
			}
		}

		// Token: 0x06006D92 RID: 28050 RVA: 0x002663DC File Offset: 0x002645DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<int>(ref this.hivesCount, "hivesCount", 0, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<string>(ref this.customLetterLabel, "customLetterLabel", null, false);
			Scribe_Values.Look<string>(ref this.customLetterText, "customLetterText", null, false);
			Scribe_Defs.Look<LetterDef>(ref this.customLetterDef, "customLetterDef");
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_Values.Look<IntVec3>(ref this.loc, "loc", default(IntVec3), false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Values.Look<IntVec3?>(ref this.overrideLoc, "overrideLoc", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && !this.quest.Historical && (this.mapParent == null || !this.mapParent.HasMap))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(82, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Quest ");
				defaultInterpolatedStringHandler.AppendFormatted<Quest>(this.quest);
				defaultInterpolatedStringHandler.AppendLiteral(" loaded with quest part ");
				defaultInterpolatedStringHandler.AppendFormatted<QuestPart_Infestation>(this);
				defaultInterpolatedStringHandler.AppendLiteral(" with invalid map and no fallback map. Ending quest.");
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				this.quest.End(QuestEndOutcome.Unknown, false, true);
			}
		}

		// Token: 0x06006D93 RID: 28051 RVA: 0x00266538 File Offset: 0x00264738
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
				this.hivesCount = 5;
			}
		}

		// Token: 0x040045CA RID: 17866
		public string inSignal;

		// Token: 0x040045CB RID: 17867
		public int hivesCount;

		// Token: 0x040045CC RID: 17868
		public MapParent mapParent;

		// Token: 0x040045CD RID: 17869
		public string tag;

		// Token: 0x040045CE RID: 17870
		public IntVec3? overrideLoc;

		// Token: 0x040045CF RID: 17871
		public string customLetterText;

		// Token: 0x040045D0 RID: 17872
		public string customLetterLabel;

		// Token: 0x040045D1 RID: 17873
		public LetterDef customLetterDef;

		// Token: 0x040045D2 RID: 17874
		public bool sendStandardLetter = true;

		// Token: 0x040045D3 RID: 17875
		private IntVec3 loc = IntVec3.Invalid;
	}
}
