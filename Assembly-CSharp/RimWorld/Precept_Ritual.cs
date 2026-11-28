using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001654 RID: 5716
	[StaticConstructorOnStartup]
	public class Precept_Ritual : Precept
	{
		// Token: 0x170018D8 RID: 6360
		// (get) Token: 0x06008F94 RID: 36756 RVA: 0x0034BBB7 File Offset: 0x00349DB7
		public float RepeatQualityPenalty
		{
			get
			{
				return Mathf.Lerp(-0.95f, 0f, this.RepeatPenaltyProgress);
			}
		}

		// Token: 0x170018D9 RID: 6361
		// (get) Token: 0x06008F95 RID: 36757 RVA: 0x0034BBD0 File Offset: 0x00349DD0
		public override string UIInfoFirstLine
		{
			get
			{
				if (!this.ShortDescOverrideCap.NullOrEmpty())
				{
					return this.ShortDescOverrideCap;
				}
				return this.def.LabelCap.Resolve();
			}
		}

		// Token: 0x170018DA RID: 6362
		// (get) Token: 0x06008F96 RID: 36758 RVA: 0x0034BC04 File Offset: 0x00349E04
		public override string UIInfoSecondLine
		{
			get
			{
				return base.LabelCap;
			}
		}

		// Token: 0x170018DB RID: 6363
		// (get) Token: 0x06008F97 RID: 36759 RVA: 0x0034BC0C File Offset: 0x00349E0C
		public bool RepeatPenaltyActive
		{
			get
			{
				return this.isAnytime && this.lastFinishedTick != -1 && this.def.useRepeatPenalty && this.TicksSinceLastPerformed < 1200000;
			}
		}

		// Token: 0x170018DC RID: 6364
		// (get) Token: 0x06008F98 RID: 36760 RVA: 0x0034BC3B File Offset: 0x00349E3B
		public int TicksSinceLastPerformed
		{
			get
			{
				return GenTicks.TicksGame - this.lastFinishedTick;
			}
		}

		// Token: 0x170018DD RID: 6365
		// (get) Token: 0x06008F99 RID: 36761 RVA: 0x0034BC49 File Offset: 0x00349E49
		public float RepeatPenaltyProgress
		{
			get
			{
				return (float)this.TicksSinceLastPerformed / 1200000f;
			}
		}

		// Token: 0x170018DE RID: 6366
		// (get) Token: 0x06008F9A RID: 36762 RVA: 0x0034BC58 File Offset: 0x00349E58
		public string RepeatPenaltyTimeLeft
		{
			get
			{
				return (1200000 - this.TicksSinceLastPerformed).ToStringTicksToPeriod(true, false, true, true, false);
			}
		}

		// Token: 0x170018DF RID: 6367
		// (get) Token: 0x06008F9B RID: 36763 RVA: 0x0034BC70 File Offset: 0x00349E70
		public bool SupportsAttachableOutcomeEffect
		{
			get
			{
				RitualOutcomeEffectWorker ritualOutcomeEffectWorker = this.outcomeEffect;
				return ritualOutcomeEffectWorker != null && ritualOutcomeEffectWorker.SupportsAttachableOutcomeEffect;
			}
		}

		// Token: 0x170018E0 RID: 6368
		// (get) Token: 0x06008F9C RID: 36764 RVA: 0x0034BC83 File Offset: 0x00349E83
		public bool IsDateTriggered
		{
			get
			{
				if (this.obligationTriggers != null)
				{
					return this.obligationTriggers.Any((RitualObligationTrigger o) => o is RitualObligationTrigger_Date);
				}
				return false;
			}
		}

		// Token: 0x170018E1 RID: 6369
		// (get) Token: 0x06008F9D RID: 36765 RVA: 0x0034BCB9 File Offset: 0x00349EB9
		public string ShortDescOverrideCap
		{
			get
			{
				if (this.shortDescOverrideCap.NullOrEmpty() && this.shortDescOverride != null)
				{
					this.shortDescOverrideCap = this.shortDescOverride.CapitalizeFirst();
				}
				return this.shortDescOverrideCap;
			}
		}

		// Token: 0x170018E2 RID: 6370
		// (get) Token: 0x06008F9E RID: 36766 RVA: 0x0034BCE8 File Offset: 0x00349EE8
		public override Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					string text = this.iconPathOverride ?? this.def.iconPath;
					if (text != null)
					{
						this.icon = ContentFinder<Texture2D>.Get(text, true);
					}
					else
					{
						this.icon = this.ideo.Icon;
					}
				}
				return this.icon;
			}
		}

		// Token: 0x170018E3 RID: 6371
		// (get) Token: 0x06008F9F RID: 36767 RVA: 0x0034BD44 File Offset: 0x00349F44
		public virtual Texture2D CancelIcon
		{
			get
			{
				if (this.cancelIcon == null)
				{
					string text = this.cancelIconPathOverride ?? this.def.iconPath;
					if (text != null)
					{
						this.cancelIcon = ContentFinder<Texture2D>.Get(text, true);
					}
					else
					{
						this.cancelIcon = this.ideo.Icon;
					}
				}
				return this.cancelIcon;
			}
		}

		// Token: 0x170018E4 RID: 6372
		// (get) Token: 0x06008FA0 RID: 36768 RVA: 0x0034BDA0 File Offset: 0x00349FA0
		public override string TipLabel
		{
			get
			{
				if (this.tipLabelCached == null)
				{
					this.tipLabelCached = ((!this.def.tipLabelOverride.NullOrEmpty()) ? this.def.tipLabelOverride : base.LabelCap);
					if (!this.ShortDescOverrideCap.NullOrEmpty() && this.ShortDescOverrideCap != this.tipLabelCached)
					{
						this.tipLabelCached = this.tipLabelCached + "\n" + this.ShortDescOverrideCap;
					}
					if (this.isAnytime)
					{
						this.tipLabelCached += "\n" + "RitualStartAnyTime".Translate();
					}
					else
					{
						RitualObligationTrigger_Date ritualObligationTrigger_Date = this.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>();
						if (ritualObligationTrigger_Date != null)
						{
							this.tipLabelCached = this.tipLabelCached + "\n" + ritualObligationTrigger_Date.DateString;
						}
						else
						{
							this.tipLabelCached += "\n" + "RitualStartEvent".Translate();
						}
					}
				}
				return this.tipLabelCached;
			}
		}

		// Token: 0x170018E5 RID: 6373
		// (get) Token: 0x06008FA1 RID: 36769 RVA: 0x0034BEB5 File Offset: 0x0034A0B5
		public override bool UsesGeneratedName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170018E6 RID: 6374
		// (get) Token: 0x06008FA2 RID: 36770 RVA: 0x0034BEB8 File Offset: 0x0034A0B8
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170018E7 RID: 6375
		// (get) Token: 0x06008FA3 RID: 36771 RVA: 0x0034BEBB File Offset: 0x0034A0BB
		public static Texture2D DateRitualTex
		{
			get
			{
				if (Precept_Ritual.dayRitualTex == null)
				{
					Precept_Ritual.dayRitualTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/DateRitual", true);
				}
				return Precept_Ritual.dayRitualTex;
			}
		}

		// Token: 0x170018E8 RID: 6376
		// (get) Token: 0x06008FA4 RID: 36772 RVA: 0x0034BEDF File Offset: 0x0034A0DF
		public static Texture2D AnytimeRitualTex
		{
			get
			{
				if (Precept_Ritual.anytimeRitualTex == null)
				{
					Precept_Ritual.anytimeRitualTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/AnytimeRitual", true);
				}
				return Precept_Ritual.anytimeRitualTex;
			}
		}

		// Token: 0x170018E9 RID: 6377
		// (get) Token: 0x06008FA5 RID: 36773 RVA: 0x0034BF03 File Offset: 0x0034A103
		public static Texture2D EventRitualTex
		{
			get
			{
				if (Precept_Ritual.eventRitualTex == null)
				{
					Precept_Ritual.eventRitualTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/EventRitual", true);
				}
				return Precept_Ritual.eventRitualTex;
			}
		}

		// Token: 0x06008FA6 RID: 36774 RVA: 0x0034BF28 File Offset: 0x0034A128
		public TaggedString GetBeginRitualText(RitualObligation obligation = null)
		{
			RitualObligationTargetFilter ritualObligationTargetFilter = this.obligationTargetFilter;
			string text = ((ritualObligationTargetFilter != null) ? ritualObligationTargetFilter.LabelExtraPart(obligation) : null) ?? "";
			if (!string.IsNullOrEmpty(this.def.ritualPatternBase.beginRitualOverride))
			{
				return new TaggedString(this.def.ritualPatternBase.beginRitualOverride).Formatted(this.Label, text);
			}
			if (text.NullOrEmpty() || this.mergeGizmosForObligations)
			{
				return "BeginRitual".Translate(this.Label);
			}
			return "BeginRitualFor".Translate(this.Label, text);
		}

		// Token: 0x06008FA7 RID: 36775 RVA: 0x0034BFD6 File Offset: 0x0034A1D6
		public override void ClearTipCache()
		{
			base.ClearTipCache();
			this.tipLabelCached = null;
		}

		// Token: 0x06008FA8 RID: 36776 RVA: 0x0034BFE5 File Offset: 0x0034A1E5
		public override string GetTip()
		{
			return this.TipMainPart() + this.TipExtraPart();
		}

		// Token: 0x06008FA9 RID: 36777 RVA: 0x0034BFF8 File Offset: 0x0034A1F8
		public string TipMainPart()
		{
			Precept.tmpCompsDesc.Clear();
			if (this.RepeatPenaltyActive)
			{
				float value = (float)Mathf.RoundToInt(this.RepeatPenaltyProgress * 20f * 10f) / 10f;
				float value2 = (float)Mathf.RoundToInt((1f - this.RepeatPenaltyProgress) * 20f * 10f) / 10f;
				Precept.tmpCompsDesc.AppendLine(base.ColorizeWarning("RitualRepeatPenaltyTip".Translate(20, value, this.RepeatQualityPenalty.ToStringPercent(), value2)));
				Precept.tmpCompsDesc.AppendLine();
			}
			if (!this.DescriptionForTip.NullOrEmpty())
			{
				string value3 = this.DescriptionForTip;
				RitualBehaviorWorker ritualBehaviorWorker = this.behavior;
				if (((ritualBehaviorWorker != null) ? ritualBehaviorWorker.descriptionOverride : null) != null)
				{
					RitualBehaviorWorker ritualBehaviorWorker2 = this.behavior;
					value3 = ((ritualBehaviorWorker2 != null) ? ritualBehaviorWorker2.descriptionOverride : null);
				}
				Precept.tmpCompsDesc.Append(value3);
			}
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				if (ritualObligationTrigger.TriggerExtraDesc != null)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(ritualObligationTrigger.TriggerExtraDesc);
				}
			}
			if (this.outcomeEffect != null)
			{
				StringBuilder stringBuilder = new StringBuilder(this.outcomeEffect.Description);
				if (!this.outcomeEffect.def.extraPredictedOutcomeDescriptions.NullOrEmpty<string>())
				{
					foreach (string str in this.outcomeEffect.def.extraPredictedOutcomeDescriptions)
					{
						stringBuilder.Append(" " + str.Formatted(this.shortDescOverride ?? this.def.label));
					}
				}
				if (this.attachableOutcomeEffect != null)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendInNewLine(this.attachableOutcomeEffect.DescriptionForRitualValidated(this));
				}
				if (stringBuilder.Length > 0)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(stringBuilder.ToString());
				}
			}
			return Precept.tmpCompsDesc.ToString();
		}

		// Token: 0x06008FAA RID: 36778 RVA: 0x0034C268 File Offset: 0x0034A468
		public string TipExtraPart()
		{
			this.tmpTipExtraSb.Clear();
			if (this.obligationTargetFilter != null && this.obligationTargetFilter.GetTargetInfos(null).Any<string>())
			{
				this.tmpTipExtraSb.AppendLine();
				this.tmpTipExtraSb.AppendInNewLine(base.ColorizeDescTitle("RitualFocusObjects".Translate() + ":"));
				this.tmpTipExtraSb.AppendInNewLine(this.obligationTargetFilter.GetTargetInfos(null).ToLineList("  - ", true));
			}
			string text = this.RolesDescription();
			if (text != null)
			{
				this.tmpTipExtraSb.Append(base.ColorizeDescTitle("\n\n" + "RitualParticipatingRoles".Translate() + ":" + "\n"));
				this.tmpTipExtraSb.Append(text);
			}
			if (this.ideo.Fluid)
			{
				SimpleCurve developmentPointsOverOutcomeIndexCurveForRitual = IdeoDevelopmentUtility.GetDevelopmentPointsOverOutcomeIndexCurveForRitual(this.ideo, this);
				if (developmentPointsOverOutcomeIndexCurveForRitual != null)
				{
					float y = developmentPointsOverOutcomeIndexCurveForRitual.MinBy((CurvePoint p) => p.y).y;
					float y2 = developmentPointsOverOutcomeIndexCurveForRitual.MaxBy((CurvePoint p) => p.y).y;
					this.tmpTipExtraSb.Append(base.ColorizeDescTitle("\n\n" + "RitualDevelopmentPoints".Translate() + ":" + "\n"));
					this.tmpTipExtraSb.Append("RitualDevelopmentPointsTip".Translate(y, y2));
				}
			}
			return this.tmpTipExtraSb.ToString();
		}

		// Token: 0x06008FAB RID: 36779 RVA: 0x0034C430 File Offset: 0x0034A630
		public string RolesDescription()
		{
			if (this.behavior.def.roles.NullOrEmpty<RitualRole>())
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (RitualRole ritualRole in this.behavior.def.roles)
			{
				Precept_Role precept_Role = ritualRole.FindInstance(this.ideo);
				TaggedString t = ritualRole.LabelCap;
				if (precept_Role != null)
				{
					t = precept_Role.LabelCap;
					if (precept_Role != null)
					{
						PreceptDef def = precept_Role.def;
						if (((def != null) ? new bool?(def.leaderRole) : null).GetValueOrDefault())
						{
							Pawn pawn = precept_Role.ChosenPawnSingle();
							if (pawn != null)
							{
								t = precept_Role.LabelForPawn(pawn).CapitalizeFirst();
							}
						}
					}
				}
				stringBuilder.AppendInNewLine("  - " + t + " (" + ((ritualRole.required && !ritualRole.substitutable) ? "Required".Translate().ToLower() : "Optional".Translate()) + ")");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06008FAC RID: 36780 RVA: 0x0034C584 File Offset: 0x0034A784
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			base.RegenerateName();
		}

		// Token: 0x06008FAD RID: 36781 RVA: 0x0034C594 File Offset: 0x0034A794
		public override void PostMake()
		{
			this.sourcePattern = this.def.ritualPatternBase;
		}

		// Token: 0x06008FAE RID: 36782 RVA: 0x0034C5A8 File Offset: 0x0034A7A8
		public override string GenerateNameRaw()
		{
			if (this.nameMaker == null)
			{
				return this.def.label;
			}
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(this.nameMaker);
			base.AddIdeoRulesTo(ref request);
			if (!request.Rules.Any<Rule>())
			{
				return this.def.label;
			}
			if (this.ideo.culture.festivalNameMaker != null)
			{
				request.Includes.Add(this.ideo.culture.festivalNameMaker);
			}
			List<string> list = new List<string>();
			string text = GrammarResolver.Resolve("r_ritualName", request, null, false, null, null, list, false);
			this.usesDefiniteArticle = !list.Contains("noArticle");
			if (this.def.capitalizeAsTitle)
			{
				text = GenText.CapitalizeAsTitle(text);
			}
			return text;
		}

		// Token: 0x06008FAF RID: 36783 RVA: 0x0034C674 File Offset: 0x0034A874
		public void AddObligation(RitualObligation obligation)
		{
			if (!this.ideo.ObligationsActive && !this.def.allowOptionalRitualObligations)
			{
				return;
			}
			if (!ModLister.CheckIdeology("Ritual obligations"))
			{
				return;
			}
			bool flag = this.def.notifyPlayerOnOpportunity && (float)Find.TickManager.TicksGame >= 60000f * this.def.skipOpportunityLettersBeforeDay;
			foreach (RitualRole ritualRole in this.behavior.def.RequiredRoles())
			{
				Precept_Role precept_Role = ritualRole.FindInstance(this.ideo);
				if (precept_Role != null && !precept_Role.Active)
				{
					if (flag)
					{
						RitualObligationTrigger_Date ritualObligationTrigger_Date = this.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>();
						if (ritualObligationTrigger_Date != null)
						{
							Find.LetterStack.ReceiveLetter("LetterObligationRoleInactive".Translate(base.LabelCap), "LetterObligationRoleInactiveDateDesc".Translate(ritualObligationTrigger_Date.DateString, base.LabelCap, this.ideo.memberName, precept_Role.Named("ROLE")), LetterDefOf.NeutralEvent, null, 0, true);
						}
					}
					return;
				}
			}
			if (this.activeObligations == null)
			{
				this.activeObligations = new List<RitualObligation>();
			}
			if (this.completedObligations == null)
			{
				this.completedObligations = new List<RitualObligation>();
			}
			if (obligation.onlyForPawns == null)
			{
				obligation.onlyForPawns = new List<Pawn>();
			}
			foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
			{
				if (pawn.Ideo == this.ideo)
				{
					obligation.onlyForPawns.Add(pawn);
				}
			}
			this.activeObligations.Add(obligation);
			if (obligation.sendLetter && flag && this.def.notifyPlayerOnOpportunity)
			{
				LookTargets lookTargets = LookTargets.Invalid;
				TargetInfo firstValidTarget = obligation.FirstValidTarget;
				Map map = Find.AnyPlayerHomeMap;
				if (firstValidTarget.IsValid)
				{
					map = firstValidTarget.Map;
				}
				if (map != null)
				{
					IEnumerable<TargetInfo> targets = this.obligationTargetFilter.GetTargets(obligation, map);
					if (targets.Any<TargetInfo>())
					{
						lookTargets = targets.First<TargetInfo>();
					}
					else
					{
						lookTargets = firstValidTarget;
					}
				}
				Find.LetterStack.ReceiveLetter(obligation.LetterLabel, obligation.LetterText.Resolve(), LetterDefOf.NeutralEvent, lookTargets, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06008FB0 RID: 36784 RVA: 0x0034C90C File Offset: 0x0034AB0C
		public void RemoveObligation(RitualObligation obligation, bool completed = false)
		{
			this.activeObligations.Remove(obligation);
			if (completed)
			{
				this.completedObligations.Add(obligation);
			}
		}

		// Token: 0x06008FB1 RID: 36785 RVA: 0x0034C92C File Offset: 0x0034AB2C
		public bool ShouldShowGizmo(TargetInfo target)
		{
			if (this.activeObligations != null)
			{
				for (int i = 0; i < this.activeObligations.Count; i++)
				{
					if (this.obligationTargetFilter.CanUseTarget(target, this.activeObligations[i]).ShouldShowGizmo)
					{
						return true;
					}
				}
			}
			return (this.obligationTriggers.FirstOrDefault((RitualObligationTrigger o) => o is RitualObligationTrigger_Date) != null || this.isAnytime) && this.obligationTargetFilter.CanUseTarget(target, null).ShouldShowGizmo;
		}

		// Token: 0x06008FB2 RID: 36786 RVA: 0x0034C9C6 File Offset: 0x0034ABC6
		public RitualTargetUseReport CanUseTarget(TargetInfo target, RitualObligation obligation)
		{
			return this.obligationTargetFilter.CanUseTarget(target, obligation);
		}

		// Token: 0x06008FB3 RID: 36787 RVA: 0x0034C9D8 File Offset: 0x0034ABD8
		public override void Tick()
		{
			base.Tick();
			if (!this.activeObligations.NullOrEmpty<RitualObligation>())
			{
				for (int i = this.activeObligations.Count - 1; i >= 0; i--)
				{
					if (!this.activeObligations[i].StillValid || !this.obligationTargetFilter.ObligationTargetsValid(this.activeObligations[i]))
					{
						this.RemoveObligation(this.activeObligations[i], false);
					}
				}
			}
			for (int j = 0; j < this.obligationTriggers.Count; j++)
			{
				try
				{
					this.obligationTriggers[j].Tick();
				}
				catch (Exception ex)
				{
					string str = "Error while ticking a ritual obligation trigger: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06008FB4 RID: 36788 RVA: 0x0034CAA8 File Offset: 0x0034ACA8
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			Precept_Ritual.<EditFloatMenuOptions>d__95 <EditFloatMenuOptions>d__ = new Precept_Ritual.<EditFloatMenuOptions>d__95(-2);
			<EditFloatMenuOptions>d__.<>4__this = this;
			return <EditFloatMenuOptions>d__;
		}

		// Token: 0x06008FB5 RID: 36789 RVA: 0x0034CAB8 File Offset: 0x0034ACB8
		public override void Notify_MemberDied(Pawn p)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberDied(p);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberDied(p);
			}
		}

		// Token: 0x06008FB6 RID: 36790 RVA: 0x0034CB1C File Offset: 0x0034AD1C
		public override void Notify_MemberCorpseDestroyed(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberCorpseDestroyed(p);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberCorpseDestroyed(p);
			}
		}

		// Token: 0x06008FB7 RID: 36791 RVA: 0x0034CB84 File Offset: 0x0034AD84
		public override void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			}
		}

		// Token: 0x06008FB8 RID: 36792 RVA: 0x0034CBEC File Offset: 0x0034ADEC
		public override void Notify_MemberLost(Pawn pawn)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberLost(pawn);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberLost(pawn);
			}
		}

		// Token: 0x06008FB9 RID: 36793 RVA: 0x0034CC50 File Offset: 0x0034AE50
		public override void Notify_MemberGained(Pawn pawn)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberGained(pawn);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberGained(pawn);
			}
		}

		// Token: 0x06008FBA RID: 36794 RVA: 0x0034CCB4 File Offset: 0x0034AEB4
		public override void Notify_MemberGenerated(Pawn pawn, bool newborn, bool ignoreApparel = false)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			base.Notify_MemberGenerated(pawn, newborn, ignoreApparel);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberGenerated(pawn);
			}
		}

		// Token: 0x06008FBB RID: 36795 RVA: 0x0034CD18 File Offset: 0x0034AF18
		public override void Notify_MemberGuestStatusChanged(Pawn pawn)
		{
			base.Notify_MemberGuestStatusChanged(pawn);
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_MemberGuestStatusChanged(pawn);
			}
		}

		// Token: 0x06008FBC RID: 36796 RVA: 0x0034CD70 File Offset: 0x0034AF70
		public override void Notify_GameStarted()
		{
			base.Notify_GameStarted();
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_GameStarted();
			}
		}

		// Token: 0x06008FBD RID: 36797 RVA: 0x0034CDC8 File Offset: 0x0034AFC8
		public override void Notify_IdeoReformed()
		{
			base.Notify_IdeoReformed();
			foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
			{
				ritualObligationTrigger.Notify_IdeoReformed();
			}
		}

		// Token: 0x06008FBE RID: 36798 RVA: 0x0034CE20 File Offset: 0x0034B020
		public IEnumerable<Gizmo> GetGizmoFor(TargetInfo t)
		{
			Precept_Ritual.<GetGizmoFor>d__105 <GetGizmoFor>d__ = new Precept_Ritual.<GetGizmoFor>d__105(-2);
			<GetGizmoFor>d__.<>4__this = this;
			<GetGizmoFor>d__.<>3__t = t;
			return <GetGizmoFor>d__;
		}

		// Token: 0x06008FBF RID: 36799 RVA: 0x0034CE38 File Offset: 0x0034B038
		public void ShowRitualBeginWindow(TargetInfo targetInfo, RitualObligation forObligation = null, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			RitualObligation ritualObligation = forObligation;
			if (ritualObligation == null)
			{
				ritualObligation = ((this.activeObligations != null) ? this.activeObligations.FirstOrDefault((RitualObligation o) => this.obligationTargetFilter.CanUseTarget(targetInfo, o).canUse) : null);
			}
			Window ritualBeginWindow = this.GetRitualBeginWindow(targetInfo, ritualObligation, null, null, forcedForRole, selectedPawn);
			if (ritualBeginWindow != null)
			{
				Find.WindowStack.Add(ritualBeginWindow);
			}
		}

		// Token: 0x06008FC0 RID: 36800 RVA: 0x0034CEA4 File Offset: 0x0034B0A4
		public virtual Window GetRitualBeginWindow(TargetInfo targetInfo, RitualObligation obligation = null, Action onConfirm = null, Pawn organizer = null, Dictionary<string, Pawn> forcedForRole = null, Pawn selectedPawn = null)
		{
			string text = this.behavior.CanStartRitualNow(targetInfo, this, selectedPawn, forcedForRole);
			if (!string.IsNullOrEmpty(text))
			{
				Messages.Message(text, targetInfo, MessageTypeDefOf.RejectInput, false);
			}
			List<string> list = new List<string>();
			if (this.outcomeEffect != null)
			{
				if (!this.outcomeEffect.def.extraInfoLines.NullOrEmpty<string>())
				{
					foreach (string item in this.outcomeEffect.def.extraInfoLines)
					{
						list.Add(item);
					}
				}
				if (!this.outcomeEffect.def.extraPredictedOutcomeDescriptions.NullOrEmpty<string>())
				{
					foreach (string str in this.outcomeEffect.def.extraPredictedOutcomeDescriptions)
					{
						list.Add(str.Formatted(this.shortDescOverride ?? this.def.label));
					}
				}
				if (this.attachableOutcomeEffect != null)
				{
					list.Add(this.attachableOutcomeEffect.DescriptionForRitualValidated(this, targetInfo.Map));
				}
			}
			string ritualLabel = this.Label.CapitalizeFirst();
			TargetInfo targetInfo2 = targetInfo;
			Map map = targetInfo.Map;
			Dialog_BeginRitual.ActionCallback action = delegate(RitualRoleAssignments assignments)
			{
				this.behavior.TryExecuteOn(targetInfo, organizer, this, obligation, assignments, true);
				Action onConfirm2 = onConfirm;
				if (onConfirm2 != null)
				{
					onConfirm2();
				}
				return true;
			};
			Pawn organizer2 = organizer;
			RitualObligation obligation2 = obligation;
			Dialog_BeginRitual.PawnFilter filter = (Pawn pawn, bool voluntary, bool allowOtherIdeos) => pawn.GetLord() == null && (!pawn.RaceProps.Animal || this.behavior.def.roles.Any(delegate(RitualRole r)
			{
				string text2;
				return r.AppliesToPawn(pawn, out text2, targetInfo, null, null, null, true);
			})) && !pawn.IsSubhuman && (!this.ritualOnlyForIdeoMembers || this.def.allowSpectatorsFromOtherIdeos || (pawn.Ideo == this.ideo || !voluntary || allowOtherIdeos) || pawn.IsPrisonerOfColony || pawn.RaceProps.Animal || (!forcedForRole.NullOrEmpty<string, Pawn>() && forcedForRole.ContainsValue(pawn)));
			string okButtonText = "Begin".Translate();
			object requiredPawns;
			if (organizer == null)
			{
				requiredPawns = null;
			}
			else
			{
				(requiredPawns = new List<Pawn>()).Add(organizer);
			}
			List<string> extraInfoText = list;
			return new Dialog_BeginRitual(ritualLabel, this, targetInfo2, map, action, organizer2, obligation2, filter, okButtonText, requiredPawns, forcedForRole, null, extraInfoText, selectedPawn);
		}

		// Token: 0x06008FC1 RID: 36801 RVA: 0x0034D0C4 File Offset: 0x0034B2C4
		public void Notify_CooldownFromAbilityStarted(int cooldown)
		{
			this.abilityOnCooldownUntilTick = Find.TickManager.TicksGame + cooldown;
		}

		// Token: 0x06008FC2 RID: 36802 RVA: 0x0034D0D8 File Offset: 0x0034B2D8
		public override void DrawIcon(Rect rect)
		{
			GUI.color = this.ideo.Color;
			GUI.DrawTexture(rect, this.Icon);
			GUI.color = Color.white;
		}

		// Token: 0x06008FC3 RID: 36803 RVA: 0x0034D100 File Offset: 0x0034B300
		public override void DrawPreceptBox(Rect preceptBox, IdeoEditMode editMode, bool forceHighlight = false)
		{
			base.DrawPreceptBox(preceptBox, editMode, forceHighlight);
			Rect position = new Rect(preceptBox.xMax - (float)this.DateIconSize.x, preceptBox.yMin, (float)this.DateIconSize.x, (float)this.DateIconSize.z);
			if (this.isAnytime)
			{
				GUI.DrawTexture(position, Precept_Ritual.AnytimeRitualTex);
				return;
			}
			if (this.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>() != null)
			{
				GUI.DrawTexture(position, Precept_Ritual.DateRitualTex);
				return;
			}
			GUI.DrawTexture(position, Precept_Ritual.EventRitualTex);
		}

		// Token: 0x06008FC4 RID: 36804 RVA: 0x0034D18D File Offset: 0x0034B38D
		public override void Notify_RecachedPrecepts()
		{
			this.tipCached = null;
		}

		// Token: 0x06008FC5 RID: 36805 RVA: 0x0034D198 File Offset: 0x0034B398
		public override bool TryGetLostByReformingWarning(out string warning)
		{
			warning = null;
			IEnumerable<LordJob_Ritual> activeRitualsForPrecept = RitualUtility.GetActiveRitualsForPrecept(this);
			if (activeRitualsForPrecept.Any<LordJob_Ritual>())
			{
				foreach (LordJob_Ritual lordJob_Ritual in activeRitualsForPrecept)
				{
					if (!warning.NullOrEmpty())
					{
						warning += "\n\n";
					}
					warning += "ReformIdeoCancelRituals".Translate(lordJob_Ritual.RitualLabel);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06008FC6 RID: 36806 RVA: 0x0034D228 File Offset: 0x0034B428
		public override void Notify_RemovedByReforming()
		{
			this.tmpActiveRituals.Clear();
			this.tmpActiveRituals.AddRange(RitualUtility.GetActiveRitualsForPrecept(this));
			for (int i = 0; i < this.tmpActiveRituals.Count; i++)
			{
				this.tmpActiveRituals[i].Cancel();
			}
			this.tmpActiveRituals.Clear();
		}

		// Token: 0x06008FC7 RID: 36807 RVA: 0x0034D284 File Offset: 0x0034B484
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.nameMaker, "nameMaker");
			Scribe_Values.Look<bool>(ref this.ritualOnlyForIdeoMembers, "ritualOnlyForIdeoMembers", false, false);
			Scribe_Values.Look<string>(ref this.ritualExpectedDesc, "ritualExpectedDesc", null, false);
			Scribe_Values.Look<string>(ref this.ritualExpectedDescNoAdjective, "ritualExpectedDescNoAdjective", null, false);
			Scribe_Values.Look<string>(ref this.shortDescOverride, "shortDescOverride", null, false);
			Scribe_Values.Look<string>(ref this.iconPathOverride, "iconPathOverride", null, false);
			Scribe_Values.Look<string>(ref this.patternGroupTag, "patternGroupTag", null, false);
			Scribe_Values.Look<TechLevel>(ref this.minTechLevel, "minTechLevel", TechLevel.Undefined, false);
			Scribe_Values.Look<TechLevel>(ref this.maxTechLevel, "maxTechLevel", TechLevel.Undefined, false);
			Scribe_Values.Look<bool>(ref this.isAnytime, "isAnytime", false, false);
			Scribe_Values.Look<bool>(ref this.canBeAnytime, "canBeAnytime", false, false);
			Scribe_Values.Look<bool>(ref this.allowOtherInstances, "allowOtherInstances", false, false);
			Scribe_Values.Look<bool>(ref this.playsIdeoMusic, "playsIdeoMusic", false, false);
			Scribe_Values.Look<string>(ref this.ritualExplanation, "ritualExplanation", null, false);
			Scribe_Values.Look<bool>(ref this.mergeGizmosForObligations, "mergeGizmosForObligations", false, false);
			Scribe_Values.Look<bool>(ref this.canMergeGizmosFromDifferentIdeos, "canMergeGizmosFromDifferentIdeos", true, false);
			Scribe_Values.Look<bool>(ref this.generatedAttachedReward, "generatedAttachedReward", false, false);
			Scribe_Defs.Look<RitualPatternDef>(ref this.sourcePattern, "sourcePattern");
			Scribe_Values.Look<bool>(ref this.ignoreExtremeTemperatures, "ignoreExtremeTemperatures", false, false);
			Scribe_Deep.Look<RitualObligationTargetFilter>(ref this.obligationTargetFilter, "obligationTargetFilter", Array.Empty<object>());
			Scribe_Deep.Look<RitualTargetFilter>(ref this.targetFilter, "targetFilter", Array.Empty<object>());
			Scribe_Deep.Look<RitualBehaviorWorker>(ref this.behavior, "behavior", Array.Empty<object>());
			Scribe_Collections.Look<RitualObligationTrigger>(ref this.obligationTriggers, "triggers", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<RitualAttachableOutcomeEffectDef>(ref this.attachableOutcomeEffect, "attachableOutcomeEffect");
			Scribe_Deep.Look<RitualOutcomeEffectWorker>(ref this.outcomeEffect, "outcomeEffect", Array.Empty<object>());
			Scribe_Collections.Look<PlanetLayerDef>(ref this.layerWhitelist, "layerWhitelist", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<PlanetLayerDef>(ref this.layerBlacklist, "layerBlacklist", LookMode.Undefined, Array.Empty<object>());
			if (!GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
			{
				Scribe_Collections.Look<RitualObligation>(ref this.activeObligations, "activeObligations", LookMode.Deep, Array.Empty<object>());
				Scribe_Collections.Look<RitualObligation>(ref this.completedObligations, "completedObligations", LookMode.Deep, Array.Empty<object>());
				Scribe_Values.Look<int>(ref this.lastFinishedTick, "lastFinishedTick", 0, false);
				Scribe_Values.Look<int>(ref this.abilityOnCooldownUntilTick, "abilityOnCooldownUntilTick", -1, false);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.activeObligations.NullOrEmpty<RitualObligation>())
				{
					if (this.activeObligations.RemoveAll((RitualObligation t) => t == null) != 0)
					{
						Log.Warning("Some activeObligations were null.");
					}
					foreach (RitualObligation ritualObligation in this.activeObligations)
					{
						ritualObligation.precept = this;
					}
				}
				if (!this.obligationTriggers.NullOrEmpty<RitualObligationTrigger>())
				{
					if (this.obligationTriggers.RemoveAll((RitualObligationTrigger t) => t == null) != 0)
					{
						Log.Warning("Some obligationTriggers were null.");
					}
					foreach (RitualObligationTrigger ritualObligationTrigger in this.obligationTriggers)
					{
						ritualObligationTrigger.ritual = this;
					}
				}
				if (this.obligationTargetFilter != null)
				{
					this.obligationTargetFilter.parent = this;
				}
				if (this.attachableOutcomeEffect == null && !this.generatedAttachedReward && this.SupportsAttachableOutcomeEffect)
				{
					this.attachableOutcomeEffect = (from d in DefDatabase<RitualAttachableOutcomeEffectDef>.AllDefs
					where d.CanAttachToRitual(this)
					select d).RandomElementWithFallback(null);
					this.generatedAttachedReward = true;
				}
				this.cancelIconPathOverride = this.sourcePattern.cancelIconPathOverride;
			}
		}

		// Token: 0x06008FC8 RID: 36808 RVA: 0x0034D664 File Offset: 0x0034B864
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			Precept_Ritual precept_Ritual = (Precept_Ritual)other;
			if (this.activeObligations != null)
			{
				precept_Ritual.activeObligations = new List<RitualObligation>();
				foreach (RitualObligation ritualObligation in this.activeObligations)
				{
					RitualObligation ritualObligation2 = new RitualObligation();
					ritualObligation.CopyTo(ritualObligation2);
					precept_Ritual.activeObligations.Add(ritualObligation2);
				}
			}
			precept_Ritual.nameMaker = this.nameMaker;
			precept_Ritual.obligationTriggers.Clear();
			for (int i = 0; i < this.obligationTriggers.Count; i++)
			{
				RitualObligationTrigger ritualObligationTrigger = (RitualObligationTrigger)Activator.CreateInstance(this.obligationTriggers[i].GetType());
				this.obligationTriggers[i].CopyTo(ritualObligationTrigger);
				ritualObligationTrigger.ritual = precept_Ritual;
				precept_Ritual.obligationTriggers.Add(ritualObligationTrigger);
			}
			if (this.obligationTargetFilter != null)
			{
				precept_Ritual.obligationTargetFilter = this.obligationTargetFilter;
				precept_Ritual.obligationTargetFilter.parent = precept_Ritual;
			}
			precept_Ritual.outcomeEffect = this.outcomeEffect;
			precept_Ritual.attachableOutcomeEffect = this.attachableOutcomeEffect;
			precept_Ritual.targetFilter = this.targetFilter;
			precept_Ritual.behavior = this.behavior;
			precept_Ritual.ritualOnlyForIdeoMembers = this.ritualOnlyForIdeoMembers;
			precept_Ritual.isAnytime = this.isAnytime;
			precept_Ritual.canBeAnytime = this.canBeAnytime;
			precept_Ritual.playsIdeoMusic = this.playsIdeoMusic;
			precept_Ritual.mergeGizmosForObligations = this.mergeGizmosForObligations;
			precept_Ritual.canMergeGizmosFromDifferentIdeos = this.canMergeGizmosFromDifferentIdeos;
			precept_Ritual.lastFinishedTick = this.lastFinishedTick;
			precept_Ritual.abilityOnCooldownUntilTick = this.abilityOnCooldownUntilTick;
			precept_Ritual.ritualExpectedDesc = this.ritualExpectedDesc;
			precept_Ritual.ritualExpectedDescNoAdjective = this.ritualExpectedDescNoAdjective;
			precept_Ritual.shortDescOverride = this.shortDescOverride;
			precept_Ritual.ritualExplanation = this.ritualExplanation;
			precept_Ritual.iconPathOverride = this.iconPathOverride;
			precept_Ritual.patternGroupTag = this.patternGroupTag;
			precept_Ritual.minTechLevel = this.minTechLevel;
			precept_Ritual.maxTechLevel = this.maxTechLevel;
			precept_Ritual.generatedAttachedReward = this.generatedAttachedReward;
			precept_Ritual.sourcePattern = this.sourcePattern;
		}

		// Token: 0x06008FCA RID: 36810 RVA: 0x0034D8E4 File Offset: 0x0034BAE4
		[CompilerGenerated]
		private Command_Ritual <GetGizmoFor>g__CommandForObligation|105_0(RitualObligation obligation, ref Precept_Ritual.<>c__DisplayClass105_0 A_2)
		{
			RitualTargetUseReport ritualTargetUseReport = this.CanUseTarget(A_2.t, obligation);
			if (ritualTargetUseReport.canUse)
			{
				return new Command_Ritual(this, A_2.t, obligation, null);
			}
			if (!ritualTargetUseReport.failReason.NullOrEmpty())
			{
				return new Command_Ritual(this, A_2.t, obligation, null)
				{
					disabledReason = ritualTargetUseReport.failReason,
					Disabled = true
				};
			}
			return null;
		}

		// Token: 0x040057A2 RID: 22434
		public List<RitualObligation> activeObligations;

		// Token: 0x040057A3 RID: 22435
		public List<RitualObligation> completedObligations;

		// Token: 0x040057A4 RID: 22436
		public RulePackDef nameMaker;

		// Token: 0x040057A5 RID: 22437
		public List<RitualObligationTrigger> obligationTriggers = new List<RitualObligationTrigger>();

		// Token: 0x040057A6 RID: 22438
		public RitualOutcomeEffectWorker outcomeEffect;

		// Token: 0x040057A7 RID: 22439
		public RitualAttachableOutcomeEffectDef attachableOutcomeEffect;

		// Token: 0x040057A8 RID: 22440
		public RitualObligationTargetFilter obligationTargetFilter;

		// Token: 0x040057A9 RID: 22441
		public RitualTargetFilter targetFilter;

		// Token: 0x040057AA RID: 22442
		public RitualBehaviorWorker behavior;

		// Token: 0x040057AB RID: 22443
		public bool ritualOnlyForIdeoMembers;

		// Token: 0x040057AC RID: 22444
		public bool isAnytime;

		// Token: 0x040057AD RID: 22445
		public bool canBeAnytime;

		// Token: 0x040057AE RID: 22446
		public bool allowOtherInstances;

		// Token: 0x040057AF RID: 22447
		public bool playsIdeoMusic;

		// Token: 0x040057B0 RID: 22448
		public bool mergeGizmosForObligations;

		// Token: 0x040057B1 RID: 22449
		public bool canMergeGizmosFromDifferentIdeos = true;

		// Token: 0x040057B2 RID: 22450
		public int lastFinishedTick = -1;

		// Token: 0x040057B3 RID: 22451
		public int abilityOnCooldownUntilTick = -1;

		// Token: 0x040057B4 RID: 22452
		public bool ignoreExtremeTemperatures;

		// Token: 0x040057B5 RID: 22453
		public string ritualExpectedDesc;

		// Token: 0x040057B6 RID: 22454
		public string ritualExpectedDescNoAdjective;

		// Token: 0x040057B7 RID: 22455
		public string shortDescOverride;

		// Token: 0x040057B8 RID: 22456
		public string ritualExplanation;

		// Token: 0x040057B9 RID: 22457
		public string iconPathOverride;

		// Token: 0x040057BA RID: 22458
		public string cancelIconPathOverride;

		// Token: 0x040057BB RID: 22459
		public string patternGroupTag;

		// Token: 0x040057BC RID: 22460
		public TechLevel minTechLevel;

		// Token: 0x040057BD RID: 22461
		public TechLevel maxTechLevel;

		// Token: 0x040057BE RID: 22462
		public bool generatedAttachedReward;

		// Token: 0x040057BF RID: 22463
		public bool showIdeoIconsInDialog = true;

		// Token: 0x040057C0 RID: 22464
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x040057C1 RID: 22465
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x040057C2 RID: 22466
		public RitualPatternDef sourcePattern;

		// Token: 0x040057C3 RID: 22467
		private string shortDescOverrideCap;

		// Token: 0x040057C4 RID: 22468
		public const float RepeatQualityPenaltyMax = -0.95f;

		// Token: 0x040057C5 RID: 22469
		public const int RepeatPenaltyDurationDays = 20;

		// Token: 0x040057C6 RID: 22470
		public const float LowQualityWarningThreshold = 0.25f;

		// Token: 0x040057C7 RID: 22471
		private readonly IntVec2 DateIconSize = new IntVec2(28, 28);

		// Token: 0x040057C8 RID: 22472
		private Texture2D icon;

		// Token: 0x040057C9 RID: 22473
		private Texture2D cancelIcon;

		// Token: 0x040057CA RID: 22474
		private string tipLabelCached;

		// Token: 0x040057CB RID: 22475
		private static Texture2D dayRitualTex;

		// Token: 0x040057CC RID: 22476
		private static Texture2D anytimeRitualTex;

		// Token: 0x040057CD RID: 22477
		private static Texture2D eventRitualTex;

		// Token: 0x040057CE RID: 22478
		private StringBuilder tmpTipExtraSb = new StringBuilder();

		// Token: 0x040057CF RID: 22479
		private List<LordJob_Ritual> tmpActiveRituals = new List<LordJob_Ritual>();
	}
}
