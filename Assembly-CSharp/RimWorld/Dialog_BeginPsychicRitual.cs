using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001C96 RID: 7318
	public class Dialog_BeginPsychicRitual : Dialog_BeginLordJob
	{
		// Token: 0x170021F2 RID: 8690
		// (get) Token: 0x0600BAEA RID: 47850 RVA: 0x004204B5 File Offset: 0x0041E6B5
		public override TaggedString HeaderLabel
		{
			get
			{
				return this.psychicRitualDef.LabelCap;
			}
		}

		// Token: 0x170021F3 RID: 8691
		// (get) Token: 0x0600BAEB RID: 47851 RVA: 0x004204C2 File Offset: 0x0041E6C2
		public override TaggedString DescriptionLabel
		{
			get
			{
				return this.psychicRitualDef.description.CapitalizeFirst();
			}
		}

		// Token: 0x170021F4 RID: 8692
		// (get) Token: 0x0600BAEC RID: 47852 RVA: 0x004204DC File Offset: 0x0041E6DC
		public override TaggedString ExpectedQualityLabel
		{
			get
			{
				return "PsychicRitualExpectedQualityLabel".Translate().CapitalizeFirst();
			}
		}

		// Token: 0x170021F5 RID: 8693
		// (get) Token: 0x0600BAED RID: 47853 RVA: 0x004204FC File Offset: 0x0041E6FC
		public override TaggedString QualityFactorsLabel
		{
			get
			{
				return "PsychicRitualQualityFactorLabel".Translate().CapitalizeFirst();
			}
		}

		// Token: 0x170021F6 RID: 8694
		// (get) Token: 0x0600BAEE RID: 47854 RVA: 0x0042051B File Offset: 0x0041E71B
		public override Texture2D Icon
		{
			get
			{
				return this.psychicRitualDef.uiIcon;
			}
		}

		// Token: 0x0600BAEF RID: 47855 RVA: 0x00420528 File Offset: 0x0041E728
		public Dialog_BeginPsychicRitual(PsychicRitualDef def, PsychicRitualCandidatePool candidatePool, PsychicRitualRoleAssignments assignments, Map map) : base(new PawnPsychicRitualRoleSelectionWidget(def, candidatePool, assignments)
		{
			showIdeoIcon = false
		})
		{
			this.psychicRitualDef = def;
			this.assignments = assignments;
			this.map = map;
		}

		// Token: 0x0600BAF0 RID: 47856 RVA: 0x00420555 File Offset: 0x0041E755
		protected override IEnumerable<string> BlockingIssues()
		{
			return this.assignments.BlockingIssues().Concat(this.psychicRitualDef.BlockingIssues(this.assignments, this.map));
		}

		// Token: 0x0600BAF1 RID: 47857 RVA: 0x0042057E File Offset: 0x0041E77E
		protected override void Start()
		{
			this.psychicRitualDef.MakeNewLord(this.assignments);
			base.Start();
			Find.PsychicRitualManager.RegisterCooldown(this.psychicRitualDef);
		}

		// Token: 0x0600BAF2 RID: 47858 RVA: 0x004205A8 File Offset: 0x0041E7A8
		protected override List<QualityFactor> PopulateQualityFactors(out FloatRange qualityRange)
		{
			FloatRange floatRange;
			List<QualityFactor> list = base.PopulateQualityFactors(out floatRange);
			float num;
			this.psychicRitualDef.CalculateMaxPower(this.assignments, list, out num);
			if (num > 0f)
			{
				qualityRange = new FloatRange(num, num);
			}
			else
			{
				qualityRange = new FloatRange(0f, 0f);
			}
			foreach (QualityFactor qualityFactor in list)
			{
				qualityFactor.qualityChange = ((qualityFactor.quality >= 0f) ? "+ " : "- ") + Mathf.Abs(qualityFactor.quality).ToStringPercent();
			}
			return list;
		}

		// Token: 0x0600BAF3 RID: 47859 RVA: 0x00420674 File Offset: 0x0041E874
		private static List<Pawn> SleepingPawns(PsychicRitualRoleAssignments assignments)
		{
			Dialog_BeginPsychicRitual.tmpSleepingPawns.Clear();
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in assignments.RoleAssignments)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<Pawn> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef psychicRitualRoleDef2 = psychicRitualRoleDef;
				List<Pawn> list2 = list;
				if (!psychicRitualRoleDef2.ConditionAllowed(PsychicRitualRoleDef.Condition.Sleeping))
				{
					foreach (Pawn pawn in list2)
					{
						if (!pawn.Awake() && pawn.health.capacities.CanBeAwake)
						{
							Dialog_BeginPsychicRitual.tmpSleepingPawns.Add(pawn);
						}
					}
				}
			}
			return Dialog_BeginPsychicRitual.tmpSleepingPawns;
		}

		// Token: 0x0600BAF4 RID: 47860 RVA: 0x00420740 File Offset: 0x0041E940
		private static List<Pawn> DraftedPawns(PsychicRitualRoleAssignments assignments)
		{
			Dialog_BeginPsychicRitual.tmpDraftedPawns.Clear();
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in assignments.RoleAssignments)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<Pawn> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef psychicRitualRoleDef2 = psychicRitualRoleDef;
				List<Pawn> list2 = list;
				if (!psychicRitualRoleDef2.ConditionAllowed(PsychicRitualRoleDef.Condition.Drafted))
				{
					foreach (Pawn pawn in list2)
					{
						if (pawn.Drafted)
						{
							Dialog_BeginPsychicRitual.tmpDraftedPawns.Add(pawn);
						}
					}
				}
			}
			return Dialog_BeginPsychicRitual.tmpDraftedPawns;
		}

		// Token: 0x0600BAF5 RID: 47861 RVA: 0x004207FC File Offset: 0x0041E9FC
		public override void DrawExtraOutcomeDescriptions(Rect viewRect, FloatRange qualityRange, string qualityNumber, ref float curY, ref float totalInfoHeight)
		{
			curY += Text.LineHeight;
			totalInfoHeight += Text.LineHeight;
			TaggedString label = this.psychicRitualDef.OutcomeDescription(qualityRange, qualityNumber, this.assignments);
			float num = Text.CalcHeight(label.Resolve(), viewRect.width);
			Widgets.Label(new Rect(viewRect.x, curY, viewRect.width, num), label);
			curY += num;
			totalInfoHeight += num;
			foreach (TaggedString taggedString in this.psychicRitualDef.OutcomeWarnings(this.assignments))
			{
				float num2 = Math.Max(Text.CalcHeight(taggedString, viewRect.width) + 3f, 28f);
				Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num2), taggedString);
				curY += num2;
				totalInfoHeight += num2;
			}
			List<Pawn> list = Dialog_BeginPsychicRitual.SleepingPawns(this.assignments);
			if (list.Count > 0)
			{
				string value = (from pawn in list
				select pawn.LabelShortCap).ToCommaList(true, false);
				TaggedString taggedString2 = ((list.Count > 1) ? "PsychicRitualWakingPawnsWarning" : "PsychicRitualWakingPawnWarning").Translate(value);
				float num3 = Math.Max(Text.CalcHeight(taggedString2, viewRect.width) + 3f, 28f);
				Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num3), taggedString2);
				curY += num3;
				totalInfoHeight += num3;
			}
			List<Pawn> list2 = Dialog_BeginPsychicRitual.DraftedPawns(this.assignments);
			if (list2.Count > 0)
			{
				string value2 = (from pawn in list2
				select pawn.LabelShortCap).ToCommaList(true, false);
				TaggedString taggedString3 = ((list2.Count > 1) ? "PsychicRitualUndraftPawnsWarning" : "PsychicRitualUndraftPawnWarning").Translate(value2);
				float num4 = Math.Max(Text.CalcHeight(taggedString3, viewRect.width) + 3f, 28f);
				Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num4), taggedString3);
				curY += num4;
				totalInfoHeight += num4;
			}
		}

		// Token: 0x0600BAF6 RID: 47862 RVA: 0x00420AA8 File Offset: 0x0041ECA8
		public override TaggedString ExpectedDurationLabel(FloatRange qualityRange)
		{
			return this.psychicRitualDef.TimeAndOfferingLabel();
		}

		// Token: 0x04006A3D RID: 27197
		private PsychicRitualDef psychicRitualDef;

		// Token: 0x04006A3E RID: 27198
		private PsychicRitualRoleAssignments assignments;

		// Token: 0x04006A3F RID: 27199
		private Map map;

		// Token: 0x04006A40 RID: 27200
		private static List<Pawn> tmpSleepingPawns = new List<Pawn>(16);

		// Token: 0x04006A41 RID: 27201
		private static List<Pawn> tmpDraftedPawns = new List<Pawn>(16);
	}
}
