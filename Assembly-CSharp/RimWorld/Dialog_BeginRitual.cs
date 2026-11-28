using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C97 RID: 7319
	[StaticConstructorOnStartup]
	public class Dialog_BeginRitual : Dialog_BeginLordJob
	{
		// Token: 0x170021F7 RID: 8695
		// (get) Token: 0x0600BAF8 RID: 47864 RVA: 0x00420ACF File Offset: 0x0041ECCF
		public override TaggedString HeaderLabel
		{
			get
			{
				return this.ritualLabel;
			}
		}

		// Token: 0x170021F8 RID: 8696
		// (get) Token: 0x0600BAF9 RID: 47865 RVA: 0x00420ADC File Offset: 0x0041ECDC
		public override TaggedString OkButtonLabel
		{
			get
			{
				return this.okButtonText;
			}
		}

		// Token: 0x170021F9 RID: 8697
		// (get) Token: 0x0600BAFA RID: 47866 RVA: 0x00420AE9 File Offset: 0x0041ECE9
		public override Texture2D Icon
		{
			get
			{
				Precept_Ritual precept_Ritual = this.ritual;
				if (precept_Ritual == null)
				{
					return null;
				}
				RitualPatternDef sourcePattern = precept_Ritual.sourcePattern;
				if (sourcePattern == null)
				{
					return null;
				}
				return sourcePattern.Icon;
			}
		}

		// Token: 0x170021FA RID: 8698
		// (get) Token: 0x0600BAFB RID: 47867 RVA: 0x00420B08 File Offset: 0x0041ED08
		public string SleepingWarning
		{
			get
			{
				if (this.sleepingMessage.NullOrEmpty())
				{
					this.sleepingMessage = "RitualBeginSleepingWarning".Translate();
				}
				if (this.assignments.Participants.Any((Pawn p) => !p.Awake()))
				{
					return this.sleepingMessage;
				}
				return null;
			}
		}

		// Token: 0x0600BAFC RID: 47868 RVA: 0x00420B70 File Offset: 0x0041ED70
		public override Color QualitySummaryColor(FloatRange qualityRange)
		{
			if (qualityRange.min >= 0.25f)
			{
				return Color.white;
			}
			return ColorLibrary.RedReadable;
		}

		// Token: 0x170021FB RID: 8699
		// (get) Token: 0x0600BAFD RID: 47869 RVA: 0x00420B8C File Offset: 0x0041ED8C
		public override TaggedString DescriptionLabel
		{
			get
			{
				Precept_Ritual precept_Ritual = this.ritual;
				string text;
				if (precept_Ritual == null)
				{
					text = null;
				}
				else
				{
					RitualBehaviorWorker behavior = precept_Ritual.behavior;
					text = ((behavior != null) ? behavior.descriptionOverride : null);
				}
				string text2;
				if ((text2 = text) == null)
				{
					Precept_Ritual precept_Ritual2 = this.ritual;
					text2 = ((precept_Ritual2 != null) ? precept_Ritual2.Description : null);
				}
				string text3 = text2;
				if (text3 == null)
				{
					return TaggedString.Empty;
				}
				return text3.Formatted(this.organizer.Named("ORGANIZER"));
			}
		}

		// Token: 0x170021FC RID: 8700
		// (get) Token: 0x0600BAFE RID: 47870 RVA: 0x00420BEC File Offset: 0x0041EDEC
		public override TaggedString ExtraExplanationLabel
		{
			get
			{
				Precept_Ritual precept_Ritual = this.ritual;
				string text;
				if (precept_Ritual == null)
				{
					text = null;
				}
				else
				{
					RitualBehaviorWorker behavior = precept_Ritual.behavior;
					text = ((behavior != null) ? behavior.GetExplanation(this.ritual, this.assignments, this.PredictedQuality(null).min) : null);
				}
				string text2 = text;
				if (!this.ritualExplanation.NullOrEmpty() || !text2.NullOrEmpty())
				{
					string text3 = this.ritualExplanation;
					if (!text2.NullOrEmpty())
					{
						if (!text3.NullOrEmpty())
						{
							text3 += "\n\n";
						}
						text3 += text2;
					}
					return text3;
				}
				return "";
			}
		}

		// Token: 0x170021FD RID: 8701
		// (get) Token: 0x0600BAFF RID: 47871 RVA: 0x00420C81 File Offset: 0x0041EE81
		public override TaggedString ExpectedQualityLabel
		{
			get
			{
				Precept_Ritual precept_Ritual = this.ritual;
				bool flag;
				if (precept_Ritual == null)
				{
					flag = (null != null);
				}
				else
				{
					RitualOutcomeEffectWorker outcomeEffect = precept_Ritual.outcomeEffect;
					flag = (((outcomeEffect != null) ? outcomeEffect.ExpectedQualityLabel() : null) != null);
				}
				if (flag)
				{
					return this.ritual.outcomeEffect.ExpectedQualityLabel();
				}
				return base.ExpectedQualityLabel;
			}
		}

		// Token: 0x0600BB00 RID: 47872 RVA: 0x00420CBF File Offset: 0x0041EEBF
		protected override IEnumerable<string> BlockingIssues()
		{
			Dialog_BeginRitual.<BlockingIssues>d__35 <BlockingIssues>d__ = new Dialog_BeginRitual.<BlockingIssues>d__35(-2);
			<BlockingIssues>d__.<>4__this = this;
			return <BlockingIssues>d__;
		}

		// Token: 0x0600BB01 RID: 47873 RVA: 0x00420CD0 File Offset: 0x0041EED0
		public override TaggedString ExpectedDurationLabel(FloatRange qualityRange)
		{
			if (this.ritual == null)
			{
				return TaggedString.Empty;
			}
			string text = this.ritual.behavior.ExpectedDuration(this.ritual, this.assignments, qualityRange.min);
			if (text.NullOrEmpty())
			{
				return null;
			}
			return "{0}: {1}".Formatted("ExpectedLordJobDuration".Translate(), text);
		}

		// Token: 0x0600BB02 RID: 47874 RVA: 0x00420D3C File Offset: 0x0041EF3C
		public override string OutcomeToolTip(ILordJobOutcomePossibility outcomeChance)
		{
			string text = this.outcome.OutcomeMoodBreakdown(outcomeChance as RitualOutcomePossibility);
			string text2 = base.OutcomeToolTip(outcomeChance);
			if (text2 != null && !text2.NullOrEmpty())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += text2;
			}
			return text;
		}

		// Token: 0x0600BB03 RID: 47875 RVA: 0x00420D8B File Offset: 0x0041EF8B
		private Dialog_BeginRitual(RitualRoleAssignments assignments, Precept_Ritual ritual, TargetInfo target, RitualOutcomeEffectDef outcome) : base(new PawnRitualRoleSelectionWidget(assignments, ritual, target, outcome))
		{
			this.assignments = assignments;
			this.ritual = ritual;
			this.target = target;
			this.outcome = outcome;
		}

		// Token: 0x0600BB04 RID: 47876 RVA: 0x00420DBC File Offset: 0x0041EFBC
		public Dialog_BeginRitual(string ritualLabel, Precept_Ritual ritual, TargetInfo target, Map map, Dialog_BeginRitual.ActionCallback action, Pawn organizer, RitualObligation obligation, Dialog_BeginRitual.PawnFilter filter = null, string okButtonText = null, List<Pawn> requiredPawns = null, Dictionary<string, Pawn> forcedForRole = null, RitualOutcomeEffectDef outcome = null, List<string> extraInfoText = null, Pawn selectedPawn = null)
		{
			RitualRoleAssignments ritualRoleAssignments = Dialog_BeginRitual.CreateRitualRoleAssignments(ritual, target, map, filter, requiredPawns, forcedForRole, selectedPawn);
			RitualOutcomeEffectDef ritualOutcomeEffectDef;
			if (ritual == null)
			{
				ritualOutcomeEffectDef = null;
			}
			else
			{
				RitualOutcomeEffectWorker outcomeEffect = ritual.outcomeEffect;
				ritualOutcomeEffectDef = ((outcomeEffect != null) ? outcomeEffect.def : null);
			}
			this..ctor(ritualRoleAssignments, ritual, target, ritualOutcomeEffectDef ?? outcome);
			this.obligation = obligation;
			this.filter = filter;
			this.organizer = organizer;
			this.map = map;
			this.action = action;
			this.ritualExplanation = ((ritual != null) ? ritual.ritualExplanation : null);
			this.ritualLabel = ritualLabel;
			this.okButtonText = (okButtonText ?? base.OkButtonLabel);
			this.extraInfos = extraInfoText;
			Dialog_BeginRitual.cachedRoles.Clear();
			if (((ritual != null) ? ritual.ideo : null) != null)
			{
				Dialog_BeginRitual.cachedRoles.AddRange(from r in ritual.ideo.RolesListForReading
				where !r.def.leaderRole
				select r);
				Precept_Role precept_Role = Faction.OfPlayer.ideos.PrimaryIdeo.RolesListForReading.FirstOrDefault((Precept_Role p) => p.def.leaderRole);
				if (precept_Role != null)
				{
					Dialog_BeginRitual.cachedRoles.Add(precept_Role);
				}
				Dialog_BeginRitual.cachedRoles.SortBy((Precept_Role x) => x.def.displayOrderInImpact);
			}
		}

		// Token: 0x0600BB05 RID: 47877 RVA: 0x00420F24 File Offset: 0x0041F124
		protected override void Start()
		{
			if (!this.confirmText.NullOrEmpty())
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(this.confirmText, delegate()
				{
					if (this.PredictedQuality(null).min < 0.25f && this.outcome.warnOnLowQuality)
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("RitualQualityLowWarning".Translate(this.HeaderLabel, 0.25f.ToStringPercent()), new Action(this.<Start>g__InnerStart|40_0), true, null, WindowLayer.Dialog));
						return;
					}
					this.<Start>g__InnerStart|40_0();
				}, true, null, WindowLayer.Dialog));
				return;
			}
			this.<Start>g__InnerStart|40_0();
		}

		// Token: 0x0600BB06 RID: 47878 RVA: 0x00420F64 File Offset: 0x0041F164
		private FloatRange PredictedQuality(List<QualityFactor> expectedOutcomeEffects = null)
		{
			float num = this.outcome.startingQuality;
			float num2 = 0f;
			foreach (RitualOutcomeComp ritualOutcomeComp in this.outcome.comps)
			{
				RitualOutcomeComp ritualOutcomeComp2 = ritualOutcomeComp;
				Precept_Ritual precept_Ritual = this.ritual;
				TargetInfo ritualTarget = this.target;
				RitualObligation ritualObligation = this.obligation;
				RitualRoleAssignments ritualRoleAssignments = this.assignments;
				Precept_Ritual precept_Ritual2 = this.ritual;
				RitualOutcomeComp_Data data;
				if (precept_Ritual2 == null)
				{
					data = null;
				}
				else
				{
					RitualOutcomeEffectWorker outcomeEffect = precept_Ritual2.outcomeEffect;
					data = ((outcomeEffect != null) ? outcomeEffect.DataForComp(ritualOutcomeComp) : null);
				}
				QualityFactor qualityFactor = ritualOutcomeComp2.GetQualityFactor(precept_Ritual, ritualTarget, ritualObligation, ritualRoleAssignments, data);
				if (qualityFactor != null)
				{
					if (!qualityFactor.label.NullOrEmpty() && expectedOutcomeEffects != null)
					{
						expectedOutcomeEffects.Add(qualityFactor);
					}
					if (qualityFactor.uncertainOutcome)
					{
						num2 += qualityFactor.quality;
					}
					else
					{
						num += qualityFactor.quality;
					}
				}
			}
			if (this.ritual != null && this.ritual.RepeatPenaltyActive)
			{
				num += this.ritual.RepeatQualityPenalty;
			}
			Map map = this.map;
			Precept_Ritual precept_Ritual3 = this.ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (precept_Ritual3 != null) ? precept_Ritual3.def : null);
			if (expectationsOffset != null)
			{
				num += expectationsOffset.Item2;
			}
			num = Mathf.Clamp(num, this.outcome.minQuality, this.outcome.maxQuality);
			num2 += num;
			num2 = Mathf.Clamp(num2, this.outcome.minQuality, this.outcome.maxQuality);
			return new FloatRange(num, num2);
		}

		// Token: 0x0600BB07 RID: 47879 RVA: 0x004210E0 File Offset: 0x0041F2E0
		protected override List<QualityFactor> PopulateQualityFactors(out FloatRange qualityRange)
		{
			List<QualityFactor> list = base.PopulateQualityFactors(out qualityRange);
			if (this.outcome == null)
			{
				return list;
			}
			float startingQuality = this.outcome.startingQuality;
			qualityRange = this.PredictedQuality(list);
			if (startingQuality > 0f)
			{
				list.Add(new QualityFactor
				{
					label = "StartingQuality".Translate(),
					qualityChange = "+" + startingQuality.ToStringPercent("F0"),
					quality = startingQuality,
					noMiddleColumnInfo = true,
					positive = true,
					priority = 5f
				});
			}
			if (this.ritual != null && this.ritual.RepeatPenaltyActive)
			{
				float repeatQualityPenalty = this.ritual.RepeatQualityPenalty;
				list.Add(new QualityFactor
				{
					label = "RitualOutcomePerformedRecently".Translate(),
					qualityChange = repeatQualityPenalty.ToStringPercent(),
					quality = repeatQualityPenalty,
					noMiddleColumnInfo = true,
					positive = false,
					priority = 5f
				});
			}
			Map map = this.map;
			Precept_Ritual precept_Ritual = this.ritual;
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(map, (precept_Ritual != null) ? precept_Ritual.def : null);
			if (expectationsOffset != null)
			{
				list.Add(new QualityFactor
				{
					label = "RitualQualityExpectations".Translate(expectationsOffset.Item1.LabelCap),
					qualityChange = "+" + expectationsOffset.Item2.ToStringPercent(),
					quality = expectationsOffset.Item2,
					noMiddleColumnInfo = true,
					positive = true,
					priority = 5f
				});
			}
			return list;
		}

		// Token: 0x0600BB08 RID: 47880 RVA: 0x0042127C File Offset: 0x0041F47C
		protected override List<ILordJobOutcomePossibility> PopulateOutcomePossibilities()
		{
			List<ILordJobOutcomePossibility> list = base.PopulateOutcomePossibilities();
			RitualOutcomeEffectDef ritualOutcomeEffectDef = this.outcome;
			bool flag;
			if (ritualOutcomeEffectDef == null)
			{
				flag = false;
			}
			else
			{
				List<RitualOutcomePossibility> outcomeChances = ritualOutcomeEffectDef.outcomeChances;
				bool? flag2 = (outcomeChances != null) ? new bool?(outcomeChances.NullOrEmpty<RitualOutcomePossibility>()) : null;
				bool flag3 = false;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				list.AddRange(this.outcome.outcomeChances);
				if (this.ritual != null && this.ritual.outcomeEffect != null)
				{
					RitualOutcomePossibility forcedOutcome = this.ritual.outcomeEffect.GetForcedOutcome(this.ritual, this.target, this.obligation, this.assignments);
					if (forcedOutcome != null)
					{
						list.Clear();
						list.Add(forcedOutcome);
					}
				}
			}
			return list;
		}

		// Token: 0x0600BB09 RID: 47881 RVA: 0x00421338 File Offset: 0x0041F538
		public override void DrawExtraOutcomeDescriptions(Rect viewRect, FloatRange qualityRange, string qualityNumber, ref float curY, ref float totalInfoHeight)
		{
			this.DrawExtraRitualOutcomeDescriptions(viewRect, qualityRange.min, ref curY, ref totalInfoHeight);
			curY += 10f;
			totalInfoHeight += 10f;
			this.DrawExtraInfo(viewRect, ref curY, ref totalInfoHeight);
			this.DrawOutcomeWarnings(viewRect, ref curY, ref totalInfoHeight);
			this.DrawFinalOutcomeInfo(viewRect, ref curY, ref totalInfoHeight);
		}

		// Token: 0x0600BB0A RID: 47882 RVA: 0x00421390 File Offset: 0x0041F590
		public void DrawExtraRitualOutcomeDescriptions(Rect viewRect, float totalQuality, ref float curY, ref float totalInfoHeight)
		{
			if (this.outcome.extraOutcomeDescriptions == null)
			{
				return;
			}
			curY += Text.LineHeight;
			totalInfoHeight += Text.LineHeight;
			int num = 0;
			for (;;)
			{
				int num2 = num;
				List<RitualOutcomeEffectDef.ExtraOutcomeChanceDescription> extraOutcomeDescriptions = this.outcome.extraOutcomeDescriptions;
				int? num3 = (extraOutcomeDescriptions != null) ? new int?(extraOutcomeDescriptions.Count) : null;
				if (!(num2 < num3.GetValueOrDefault() & num3 != null))
				{
					break;
				}
				RitualOutcomeEffectDef.ExtraOutcomeChanceDescription extraOutcomeChanceDescription = this.outcome.extraOutcomeDescriptions[num];
				float value = extraOutcomeChanceDescription.qualityToValue(totalQuality);
				TaggedString taggedString = extraOutcomeChanceDescription.description.Formatted(value);
				Vector2 vector = Text.CalcSize(taggedString);
				if (vector.x > viewRect.width)
				{
					vector = new Vector2(viewRect.width, Text.CalcHeight(taggedString, viewRect.width));
				}
				Widgets.Label(new Rect(viewRect.x, curY, vector.x, Mathf.Max(32f, vector.y)), taggedString);
				curY += vector.y;
				totalInfoHeight += vector.y;
				num++;
			}
		}

		// Token: 0x0600BB0B RID: 47883 RVA: 0x004214B8 File Offset: 0x0041F6B8
		public void DrawExtraInfo(Rect viewRect, ref float curY, ref float totalInfoHeight)
		{
			if (this.extraInfos == null)
			{
				return;
			}
			foreach (string text in this.extraInfos)
			{
				float num = Math.Max(Text.CalcHeight(text, viewRect.width) + 3f, 28f);
				Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num), text);
				curY += num;
				totalInfoHeight += num;
			}
		}

		// Token: 0x0600BB0C RID: 47884 RVA: 0x0042155C File Offset: 0x0041F75C
		public void DrawOutcomeWarnings(Rect viewRect, ref float curY, ref float totalInfoHeight)
		{
			string sleepingWarning = this.SleepingWarning;
			if (sleepingWarning.NullOrEmpty())
			{
				return;
			}
			float num = Math.Max(Text.CalcHeight(sleepingWarning, viewRect.width) + 3f, 28f);
			Widgets.Label(new Rect(viewRect.x, curY + 4f, viewRect.width, num), sleepingWarning);
			curY += num;
		}

		// Token: 0x0600BB0D RID: 47885 RVA: 0x004215C0 File Offset: 0x0041F7C0
		public void DrawFinalOutcomeInfo(Rect viewRect, ref float curY, ref float totalInfoHeight)
		{
			if (this.outcome.outcomeChances.NullOrEmpty<RitualOutcomePossibility>())
			{
				return;
			}
			Precept_Ritual precept_Ritual = this.ritual;
			bool flag;
			if (precept_Ritual == null)
			{
				flag = true;
			}
			else
			{
				Ideo ideo = precept_Ritual.ideo;
				flag = !((ideo != null) ? new bool?(ideo.Fluid) : null).GetValueOrDefault();
			}
			if (flag)
			{
				return;
			}
			SimpleCurve developmentPointsOverOutcomeIndexCurveForRitual = IdeoDevelopmentUtility.GetDevelopmentPointsOverOutcomeIndexCurveForRitual(this.ritual.ideo, this.ritual);
			if (developmentPointsOverOutcomeIndexCurveForRitual == null)
			{
				return;
			}
			curY += 10f;
			totalInfoHeight += 10f;
			TaggedString taggedString = "RitualDevelopmentPointRewards".Translate() + ":\n";
			float num = Text.CalcHeight(taggedString, viewRect.width);
			Widgets.Label(new Rect(viewRect.x, curY, viewRect.width, num), taggedString);
			curY += num;
			for (int i = 0; i < this.outcome.outcomeChances.Count; i++)
			{
				RitualOutcomePossibility ritualOutcomePossibility = this.outcome.outcomeChances[i];
				string label = "  - " + ritualOutcomePossibility.label + ": " + developmentPointsOverOutcomeIndexCurveForRitual.Evaluate((float)i).ToStringWithSign("0.##");
				Widgets.Label(new Rect(viewRect.x, curY, viewRect.width, 32f), label);
				curY += Text.LineHeight;
				totalInfoHeight += Text.LineHeight;
			}
		}

		// Token: 0x0600BB0E RID: 47886 RVA: 0x00421728 File Offset: 0x0041F928
		public override void DoExtraHeaderInfo(ref RectDivider layout, ref RectDivider headerLabelRow)
		{
			if (this.ritual == null)
			{
				return;
			}
			if (ModsConfig.IdeologyActive && this.ritual.ideo != null && !Find.IdeoManager.classicMode && !this.ritual.def.isNonIdeoRitual)
			{
				Ideo ideo = this.ritual.ideo;
				Vector2 vector = Text.CalcSize(ideo.name.CapitalizeFirst());
				IdeoUIUtility.DrawIdeoPlate(headerLabelRow.NewRow(vector.y, VerticalJustification.Top, null).NewCol(vector.x + 30f, HorizontalJustification.Right, null), ideo, null);
			}
			using (new TextBlock(new GameFont?(GameFont.Small), new TextAnchor?(TextAnchor.LowerRight), new bool?(false), new Color?(Color.gray)))
			{
				if (!this.ritual.Label.EqualsIgnoreCase(this.ritual.UIInfoFirstLine))
				{
					Widgets.Label(new Rect(headerLabelRow)
					{
						height = Text.CalcHeight(this.ritual.UIInfoFirstLine, headerLabelRow.Rect.width)
					}, this.ritual.UIInfoFirstLine);
				}
			}
		}

		// Token: 0x0600BB0F RID: 47887 RVA: 0x0042187C File Offset: 0x0041FA7C
		public override void PostOpen()
		{
			base.PostOpen();
			this.assignments.FillPawns(this.filter, this.target);
			if (this.outcome != null && this.ritual != null && this.ritual.outcomeEffect != null)
			{
				foreach (RitualOutcomeComp ritualOutcomeComp in this.outcome.comps)
				{
					ritualOutcomeComp.Notify_AssignmentsChanged(this.assignments, this.ritual.outcomeEffect.DataForComp(ritualOutcomeComp));
				}
			}
		}

		// Token: 0x0600BB10 RID: 47888 RVA: 0x00421924 File Offset: 0x0041FB24
		protected override void DrawQualityFactors(Rect viewRect)
		{
			Precept_Ritual precept_Ritual = this.ritual;
			bool flag;
			if (precept_Ritual == null)
			{
				flag = false;
			}
			else
			{
				RitualOutcomeEffectWorker outcomeEffect = precept_Ritual.outcomeEffect;
				bool? flag2 = (outcomeEffect != null) ? new bool?(outcomeEffect.ShowQuality) : null;
				bool flag3 = false;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				Rect rect = viewRect;
				rect.y += 17f;
				string t = this.ritual.outcomeEffect.def.outcomeChances.MaxBy((RitualOutcomePossibility c) => c.positivityIndex).label.CapitalizeFirst();
				TaggedString taggedString = "RitualOutcomeNoQuality".Translate() + ":\n\n  - " + t + " " + this.ritual.Label;
				Rect rect2 = rect;
				rect2.width += 10f;
				rect2.height = Text.CalcHeight(taggedString, rect.width);
				rect2 = rect2.ExpandedBy(9f);
				GUI.color = new Color(0.25f, 0.25f, 0.25f);
				Widgets.DrawBox(rect2, 2, null);
				GUI.color = Color.white;
				Widgets.Label(rect, taggedString);
				return;
			}
			base.DrawQualityFactors(viewRect);
		}

		// Token: 0x0600BB11 RID: 47889 RVA: 0x00421A84 File Offset: 0x0041FC84
		private void UpdateRoleChangeTargetRole(Pawn p)
		{
			Precept_Role roleToChangeTo = null;
			Ideo ideo = p.Ideo;
			if (((ideo != null) ? ideo.GetRole(p) : null) == null)
			{
				roleToChangeTo = RitualUtility.AllRolesForPawn(p).FirstOrDefault((Precept_Role r) => r.Active && r.RequirementsMet(p));
			}
			this.SetRoleToChangeTo(roleToChangeTo);
		}

		// Token: 0x0600BB12 RID: 47890 RVA: 0x00421AE3 File Offset: 0x0041FCE3
		public void SetRoleToChangeTo(Precept_Role role)
		{
			this.assignments.SetRoleChangeSelection(role);
		}

		// Token: 0x0600BB13 RID: 47891 RVA: 0x00421AF4 File Offset: 0x0041FCF4
		private void DrawRoleSelection(Pawn pawn, Rect rect)
		{
			Dialog_BeginRitual.<>c__DisplayClass54_0 CS$<>8__locals1 = new Dialog_BeginRitual.<>c__DisplayClass54_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pawn = pawn;
			Precept_Role roleChangeSelection = this.assignments.RoleChangeSelection;
			Dialog_BeginRitual.<>c__DisplayClass54_0 CS$<>8__locals2 = CS$<>8__locals1;
			Pawn pawn2 = CS$<>8__locals1.pawn;
			Precept_Role currentRole;
			if (pawn2 == null)
			{
				currentRole = null;
			}
			else
			{
				Ideo ideo = pawn2.Ideo;
				currentRole = ((ideo != null) ? ideo.GetRole(CS$<>8__locals1.pawn) : null);
			}
			CS$<>8__locals2.currentRole = currentRole;
			if (roleChangeSelection == null && CS$<>8__locals1.currentRole == null)
			{
				this.UpdateRoleChangeTargetRole(CS$<>8__locals1.pawn);
				roleChangeSelection = this.assignments.RoleChangeSelection;
			}
			if (roleChangeSelection != null || CS$<>8__locals1.currentRole != null)
			{
				SocialCardUtility.DrawPawnRole(CS$<>8__locals1.pawn, roleChangeSelection, (roleChangeSelection != null) ? roleChangeSelection.LabelCap : "RemoveRole".Translate(CS$<>8__locals1.currentRole.Label).Resolve(), rect, false);
			}
			Rect rect2 = new Rect(rect.x + 220f, rect.y + 2f, 140f, 32f);
			Pawn pawn3 = CS$<>8__locals1.pawn;
			bool flag = ((pawn3 != null) ? pawn3.Ideo : null) != null;
			if (!flag)
			{
				GUI.color = Color.gray;
			}
			if (Dialog_BeginRitual.cachedRoles.Count > 1 && Widgets.ButtonText(rect2, "ChooseNewRole".Translate() + "...", true, true, flag, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				if (CS$<>8__locals1.currentRole != null)
				{
					list.Add(new FloatMenuOption("None".Translate(), delegate()
					{
						CS$<>8__locals1.<>4__this.confirmText = "ChooseRoleConfirmUnassign".Translate(CS$<>8__locals1.currentRole.Named("ROLE"), CS$<>8__locals1.pawn.Named("PAWN")) + "\n\n" + "ChooseRoleConfirmAssignPostfix".Translate();
						CS$<>8__locals1.<>4__this.SetRoleToChangeTo(null);
					}, Widgets.PlaceholderIconTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
				}
				using (List<Precept_Role>.Enumerator enumerator = Dialog_BeginRitual.cachedRoles.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Dialog_BeginRitual.<>c__DisplayClass54_1 CS$<>8__locals3 = new Dialog_BeginRitual.<>c__DisplayClass54_1();
						CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals3.newRole = enumerator.Current;
						if (CS$<>8__locals3.newRole != roleChangeSelection && CS$<>8__locals3.newRole != CS$<>8__locals3.CS$<>8__locals1.currentRole && CS$<>8__locals3.newRole.Active && CS$<>8__locals3.newRole.RequirementsMet(CS$<>8__locals3.CS$<>8__locals1.pawn) && (!CS$<>8__locals3.newRole.def.leaderRole || CS$<>8__locals3.CS$<>8__locals1.pawn.Ideo == Faction.OfPlayer.ideos.PrimaryIdeo))
						{
							string text = CS$<>8__locals3.newRole.LabelForPawn(CS$<>8__locals3.CS$<>8__locals1.pawn) + " (" + CS$<>8__locals3.newRole.def.label + ")";
							TaggedString confirmTextLocal = "ChooseRoleConfirmAssign".Translate(CS$<>8__locals3.newRole.Named("ROLE"), CS$<>8__locals3.CS$<>8__locals1.pawn.Named("PAWN"));
							string extraConfirmText = RitualUtility.RoleChangeConfirmation(CS$<>8__locals3.CS$<>8__locals1.pawn, CS$<>8__locals3.CS$<>8__locals1.currentRole, CS$<>8__locals3.newRole);
							Pawn pawn4 = CS$<>8__locals3.newRole.ChosenPawns().FirstOrDefault<Pawn>();
							if (pawn4 != null && CS$<>8__locals3.newRole is Precept_RoleSingle)
							{
								text = text + ": " + pawn4.LabelShort;
							}
							if (!extraConfirmText.NullOrEmpty())
							{
								list.Add(new FloatMenuOption(text, delegate()
								{
									CS$<>8__locals3.CS$<>8__locals1.<>4__this.confirmText = confirmTextLocal + "\n\n" + extraConfirmText + "\n\n" + "ChooseRoleConfirmAssignPostfix".Translate();
									CS$<>8__locals3.CS$<>8__locals1.<>4__this.SetRoleToChangeTo(CS$<>8__locals3.newRole);
								}, CS$<>8__locals3.newRole.Icon, CS$<>8__locals3.newRole.ideo.Color, MenuOptionPriority.Default, new Action<Rect>(CS$<>8__locals3.<DrawRoleSelection>g__DrawTooltip|1), null, 0f, null, null, true, 0, HorizontalJustification.Left, false)
								{
									orderInPriority = CS$<>8__locals3.newRole.def.displayOrderInImpact
								});
							}
							else
							{
								list.Add(new FloatMenuOption(text, delegate()
								{
									CS$<>8__locals3.newRole.Assign(CS$<>8__locals3.CS$<>8__locals1.pawn, true);
								}, CS$<>8__locals3.newRole.Icon, CS$<>8__locals3.newRole.ideo.Color, MenuOptionPriority.Default, new Action<Rect>(CS$<>8__locals3.<DrawRoleSelection>g__DrawTooltip|1), null, 0f, null, null, true, 0, HorizontalJustification.Left, false)
								{
									orderInPriority = CS$<>8__locals3.newRole.def.displayOrderInImpact
								});
							}
						}
					}
				}
				foreach (Precept_Role precept_Role in Dialog_BeginRitual.cachedRoles)
				{
					if ((precept_Role != roleChangeSelection && !precept_Role.RequirementsMet(CS$<>8__locals1.pawn)) || !precept_Role.Active)
					{
						string text2 = precept_Role.LabelForPawn(CS$<>8__locals1.pawn) + " (" + precept_Role.def.label + ")";
						if (precept_Role.ChosenPawnSingle() != null)
						{
							text2 = text2 + ": " + precept_Role.ChosenPawnSingle().LabelShort;
						}
						else if (!precept_Role.RequirementsMet(CS$<>8__locals1.pawn))
						{
							text2 = text2 + ": " + precept_Role.GetFirstUnmetRequirement(CS$<>8__locals1.pawn).GetLabel(precept_Role).CapitalizeFirst();
						}
						else if (!precept_Role.Active && precept_Role.def.activationBelieverCount > precept_Role.ideo.ColonistBelieverCountCached)
						{
							text2 += ": " + "InactiveRoleRequiresMoreBelievers".Translate(precept_Role.def.activationBelieverCount, precept_Role.ideo.memberName, precept_Role.ideo.ColonistBelieverCountCached).CapitalizeFirst();
						}
						list.Add(new FloatMenuOption(text2, null, precept_Role.Icon, precept_Role.ideo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false)
						{
							orderInPriority = precept_Role.def.displayOrderInImpact
						});
					}
				}
				if (list.Any<FloatMenuOption>())
				{
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			GUI.color = Color.white;
		}

		// Token: 0x0600BB14 RID: 47892 RVA: 0x00422194 File Offset: 0x00420394
		public void DoRoleSelection(ref RectDivider layout)
		{
			RitualRoleIdeoRoleChanger ritualRoleIdeoRoleChanger = this.assignments.AllRolesForReading.OfType<RitualRoleIdeoRoleChanger>().FirstOrDefault<RitualRoleIdeoRoleChanger>();
			if (ritualRoleIdeoRoleChanger != null && Dialog_BeginRitual.cachedRoles.Any<Precept_Role>())
			{
				Pawn pawn = this.assignments.FirstAssignedPawn(ritualRoleIdeoRoleChanger);
				if (pawn != null)
				{
					RectDivider rect = layout.NewRow(40f, VerticalJustification.Top, new float?(10f)).NewCol(320f, HorizontalJustification.Left, null);
					this.DrawRoleSelection(pawn, rect);
				}
			}
		}

		// Token: 0x0600BB15 RID: 47893 RVA: 0x00422214 File Offset: 0x00420414
		public override void DoRightColumn(ref RectDivider layout)
		{
			if (this.target.Thing != null)
			{
				Rect targetRow = layout.NewRow(32f, VerticalJustification.Bottom, new float?(28f));
				this.DrawTargetLocation(targetRow);
			}
			this.DoRoleSelection(ref layout);
			base.DoRightColumn(ref layout);
		}

		// Token: 0x0600BB16 RID: 47894 RVA: 0x00422260 File Offset: 0x00420460
		public void DrawTargetLocation(Rect targetRow)
		{
			TaggedString taggedString = "RitualTakesPlaceAt".Translate() + ": ";
			TaggedString taggedString2 = taggedString + this.target.Thing.LabelShortCap;
			float x = Text.CalcSize(taggedString).x;
			float x2 = Text.CalcSize(this.target.Thing.LabelShortCap).x;
			float num = Text.CalcSize(taggedString2).x + 4f + 32f;
			Rect rect = new Rect(targetRow.xMax - (x2 + 4f + 32f), targetRow.y - 6f, 32f, 32f);
			Rect rect2 = new Rect(targetRow.xMax - num, targetRow.y, x, 32f);
			Rect rect3 = new Rect(targetRow.xMax - (x2 + 4f + 32f) + 32f + 4f, targetRow.y, x2, 32f);
			Widgets.Label(rect2, taggedString);
			Widgets.Label(rect3, this.target.Thing.LabelShortCap);
			Widgets.ThingIcon(rect, this.target.Thing, 1f, null, false, 1f, false);
			if (Mouse.IsOver(rect2) || Mouse.IsOver(rect3) || Mouse.IsOver(rect))
			{
				Find.WindowStack.ImmediateWindow(738453, new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight), WindowLayer.Super, delegate
				{
					GenUI.DrawArrowPointingAtWorldspace(this.target.Cell.ToVector3(), Find.Camera);
				}, false, false, 0f, null, false);
			}
		}

		// Token: 0x0600BB17 RID: 47895 RVA: 0x00422408 File Offset: 0x00420608
		public static RitualRoleAssignments CreateRitualRoleAssignments(Precept_Ritual ritual, TargetInfo target, Map map, Dialog_BeginRitual.PawnFilter filter, List<Pawn> requiredPawns, Dictionary<string, Pawn> forcedForRole, Pawn selectedPawn)
		{
			RitualRoleAssignments ritualRoleAssignments = new RitualRoleAssignments(ritual, target);
			List<Pawn> list = new List<Pawn>(map.mapPawns.FreeColonistsAndPrisonersSpawned);
			List<Pawn> list2 = new List<Pawn>(list.Count);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Pawn pawn = list[i];
				if (filter != null && !filter(pawn, true, true))
				{
					list.RemoveAt(i);
				}
				else
				{
					bool flag2;
					bool flag = ritualRoleAssignments.PawnNotAssignableReason(pawn, null, out flag2) == null || flag2;
					if (!flag && ritual != null)
					{
						if (pawn.DevelopmentalStage != DevelopmentalStage.Baby && pawn.DevelopmentalStage != DevelopmentalStage.Newborn)
						{
							list2.AddUnique(pawn);
						}
						foreach (RitualRole ritualRole in ritual.behavior.def.roles)
						{
							if ((ritualRoleAssignments.PawnNotAssignableReason(pawn, ritualRole, out flag2) == null || flag2) && (filter == null || filter(pawn, !(ritualRole is RitualRoleForced), ritualRole.allowOtherIdeos)) && (ritualRole.maxCount > 1 || forcedForRole == null || !forcedForRole.ContainsKey(ritualRole.id)))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						list.RemoveAt(i);
					}
				}
			}
			if (requiredPawns != null)
			{
				foreach (Pawn obj in requiredPawns)
				{
					list.AddUnique(obj);
				}
			}
			if (forcedForRole != null)
			{
				foreach (KeyValuePair<string, Pawn> keyValuePair in forcedForRole)
				{
					list.AddUnique(keyValuePair.Value);
				}
			}
			if (ritual != null)
			{
				using (List<RitualRole>.Enumerator enumerator = ritual.behavior.def.roles.GetEnumerator())
				{
					Func<Pawn, bool> <>9__0;
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Animal)
						{
							List<Pawn> list3 = list;
							IEnumerable<Pawn> spawnedColonyAnimals = map.mapPawns.SpawnedColonyAnimals;
							Func<Pawn, bool> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((Pawn p) => filter == null || filter(p, true, true)));
							}
							list3.AddRange(spawnedColonyAnimals.Where(predicate));
							break;
						}
					}
				}
			}
			ritualRoleAssignments.Setup(list, list2, forcedForRole, requiredPawns, selectedPawn);
			return ritualRoleAssignments;
		}

		// Token: 0x0600BB1B RID: 47899 RVA: 0x004226DF File Offset: 0x004208DF
		[CompilerGenerated]
		private void <Start>g__InnerStart|40_0()
		{
			Dialog_BeginRitual.ActionCallback actionCallback = this.action;
			if (actionCallback != null && actionCallback(this.assignments))
			{
				this.Close(true);
			}
		}

		// Token: 0x04006A42 RID: 27202
		protected RitualRoleAssignments assignments;

		// Token: 0x04006A43 RID: 27203
		protected Precept_Ritual ritual;

		// Token: 0x04006A44 RID: 27204
		protected TargetInfo target;

		// Token: 0x04006A45 RID: 27205
		protected RitualObligation obligation;

		// Token: 0x04006A46 RID: 27206
		protected RitualOutcomeEffectDef outcome;

		// Token: 0x04006A47 RID: 27207
		protected Dialog_BeginRitual.PawnFilter filter;

		// Token: 0x04006A48 RID: 27208
		protected Pawn organizer;

		// Token: 0x04006A49 RID: 27209
		protected Map map;

		// Token: 0x04006A4A RID: 27210
		protected Dialog_BeginRitual.ActionCallback action;

		// Token: 0x04006A4B RID: 27211
		protected List<string> extraInfos;

		// Token: 0x04006A4C RID: 27212
		private string sleepingMessage;

		// Token: 0x04006A4D RID: 27213
		protected string ritualExplanation;

		// Token: 0x04006A4E RID: 27214
		protected string ritualLabel;

		// Token: 0x04006A4F RID: 27215
		protected string okButtonText;

		// Token: 0x04006A50 RID: 27216
		protected string confirmText;

		// Token: 0x04006A51 RID: 27217
		private static List<Precept_Role> cachedRoles = new List<Precept_Role>();

		// Token: 0x04006A52 RID: 27218
		protected const float RoleHeight = 40f;

		// Token: 0x04006A53 RID: 27219
		protected const float TargetIconSize = 32f;

		// Token: 0x02003919 RID: 14617
		// (Invoke) Token: 0x06014CA1 RID: 85153
		public delegate bool ActionCallback(RitualRoleAssignments assignments);

		// Token: 0x0200391A RID: 14618
		// (Invoke) Token: 0x06014CA5 RID: 85157
		public delegate bool PawnFilter(Pawn pawn, bool voluntary, bool allowOtherIdeos);
	}
}
