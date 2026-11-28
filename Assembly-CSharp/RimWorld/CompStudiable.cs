using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200199C RID: 6556
	public class CompStudiable : ThingComp
	{
		// Token: 0x17001C97 RID: 7319
		// (get) Token: 0x0600A492 RID: 42130 RVA: 0x003ACFB4 File Offset: 0x003AB1B4
		public CompProperties_Studiable Props
		{
			get
			{
				return (CompProperties_Studiable)this.props;
			}
		}

		// Token: 0x17001C98 RID: 7320
		// (get) Token: 0x0600A493 RID: 42131 RVA: 0x003ACFC1 File Offset: 0x003AB1C1
		public Pawn Pawn
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x17001C99 RID: 7321
		// (get) Token: 0x0600A494 RID: 42132 RVA: 0x003ACFCE File Offset: 0x003AB1CE
		public float ProgressPercent
		{
			get
			{
				if (!this.Props.Completable)
				{
					return 0f;
				}
				return this.studyPoints / this.Props.studyAmountToComplete;
			}
		}

		// Token: 0x17001C9A RID: 7322
		// (get) Token: 0x0600A495 RID: 42133 RVA: 0x003ACFF5 File Offset: 0x003AB1F5
		public float AdjustedAnomalyKnowledgePerStudy
		{
			get
			{
				return this.AdjustedAnomalyKnowledge(null) / 5f;
			}
		}

		// Token: 0x17001C9B RID: 7323
		// (get) Token: 0x0600A496 RID: 42134 RVA: 0x003AD004 File Offset: 0x003AB204
		public bool Completed
		{
			get
			{
				return this.Props.Completable && this.ProgressPercent >= 1f;
			}
		}

		// Token: 0x17001C9C RID: 7324
		// (get) Token: 0x0600A497 RID: 42135 RVA: 0x003AD025 File Offset: 0x003AB225
		public int TicksTilNextStudy
		{
			get
			{
				return this.lastStudiedTick + this.Props.frequencyTicks - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001C9D RID: 7325
		// (get) Token: 0x0600A498 RID: 42136 RVA: 0x003AD044 File Offset: 0x003AB244
		private bool IsMutant
		{
			get
			{
				return this.Pawn != null && this.Pawn.IsMutant;
			}
		}

		// Token: 0x17001C9E RID: 7326
		// (get) Token: 0x0600A499 RID: 42137 RVA: 0x003AD05C File Offset: 0x003AB25C
		public virtual float AnomalyKnowledge
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return 0f;
				}
				if (this.KnowledgeCategory == null)
				{
					return 0f;
				}
				float num = 0f;
				CompStudyUnlocks compStudyUnlocks = this.compStudyUnlocks;
				if (compStudyUnlocks != null && compStudyUnlocks.StudyKnowledgeAmount != null)
				{
					num = this.compStudyUnlocks.StudyKnowledgeAmount.Value;
				}
				else if (this.Props.anomalyKnowledge != null)
				{
					num = this.Props.anomalyKnowledge.Value;
				}
				else if (this.Pawn != null)
				{
					num = (float)this.Pawn.RaceProps.anomalyKnowledge;
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(176, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Thing ");
					defaultInterpolatedStringHandler.AppendFormatted<ThingWithComps>(this.parent);
					defaultInterpolatedStringHandler.AppendLiteral(" has studiable comp but has no overriden anomaly knowledge amount, ensure this is on a pawn with setup race props vars, or provides a knowledge amount value in this comp.");
					Debug.LogError(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				CompStudyUnlocks compStudyUnlocks2 = this.compStudyUnlocks;
				if (compStudyUnlocks2 != null && compStudyUnlocks2.StudyKnowledgeAmount != null)
				{
					num = this.compStudyUnlocks.StudyKnowledgeAmount.Value;
				}
				if (this.IsMutant)
				{
					num += this.Pawn.mutant.Def.anomalyKnowledgeOffset;
				}
				return num;
			}
		}

		// Token: 0x17001C9F RID: 7327
		// (get) Token: 0x0600A49A RID: 42138 RVA: 0x003AD18C File Offset: 0x003AB38C
		public virtual KnowledgeCategoryDef KnowledgeCategory
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return null;
				}
				if (this.IsMutant)
				{
					return this.Pawn.mutant.Def.knowledgeCategory;
				}
				if (this.Props.knowledgeCategory != null)
				{
					return this.Props.knowledgeCategory;
				}
				Pawn pawn = this.Pawn;
				if (pawn == null)
				{
					return null;
				}
				return pawn.RaceProps.knowledgeCategory;
			}
		}

		// Token: 0x17001CA0 RID: 7328
		// (get) Token: 0x0600A49B RID: 42139 RVA: 0x003AD1EF File Offset: 0x003AB3EF
		public bool RequiresHoldingPlatform
		{
			get
			{
				return this.IsMutant || this.Props.requiresHoldingPlatform;
			}
		}

		// Token: 0x17001CA1 RID: 7329
		// (get) Token: 0x0600A49C RID: 42140 RVA: 0x003AD206 File Offset: 0x003AB406
		public bool RequiresImprisonment
		{
			get
			{
				return !this.IsMutant && this.Props.requiresImprisonment;
			}
		}

		// Token: 0x17001CA2 RID: 7330
		// (get) Token: 0x0600A49D RID: 42141 RVA: 0x003AD21D File Offset: 0x003AB41D
		public bool Deactivated
		{
			get
			{
				return this.Props.canBeActivityDeactivated && this.CompActivity.Deactivated;
			}
		}

		// Token: 0x17001CA3 RID: 7331
		// (get) Token: 0x0600A49E RID: 42142 RVA: 0x003AD239 File Offset: 0x003AB439
		private bool DisplayStudyInfoOnInspectPane
		{
			get
			{
				return !this.parent.Destroyed && !this.Completed && (!ModsConfig.AnomalyActive || Find.Anomaly.HighestLevelReached >= this.Props.minMonolithLevelForStudy);
			}
		}

		// Token: 0x17001CA4 RID: 7332
		// (get) Token: 0x0600A49F RID: 42143 RVA: 0x003AD278 File Offset: 0x003AB478
		private CompActivity CompActivity
		{
			get
			{
				CompActivity result;
				if ((result = this.compActivity) == null)
				{
					result = (this.compActivity = this.parent.GetComp<CompActivity>());
				}
				return result;
			}
		}

		// Token: 0x0600A4A0 RID: 42144 RVA: 0x003AD2A3 File Offset: 0x003AB4A3
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.studyEnabled = this.Props.studyEnabledByDefault;
		}

		// Token: 0x0600A4A1 RID: 42145 RVA: 0x003AD2BC File Offset: 0x003AB4BC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.compStudyUnlocks = this.parent.TryGetComp<CompStudyUnlocks>();
			StudyManager studyManager = Find.StudyManager;
			Thing parent = this.parent;
			Map map;
			if ((map = this.parent.Map) == null)
			{
				Thing thing = this.parent.ParentHolder as Thing;
				map = ((thing != null) ? thing.Map : null);
			}
			studyManager.UpdateStudiableCache(parent, map);
		}

		// Token: 0x0600A4A2 RID: 42146 RVA: 0x003AD315 File Offset: 0x003AB515
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			Find.StudyManager.UpdateStudiableCache(this.parent, map);
			base.PostDeSpawn(map, mode);
		}

		// Token: 0x0600A4A3 RID: 42147 RVA: 0x003AD330 File Offset: 0x003AB530
		public void SetStudyEnabled(bool enabled)
		{
			this.studyEnabled = enabled;
		}

		// Token: 0x0600A4A4 RID: 42148 RVA: 0x003AD33C File Offset: 0x003AB53C
		public bool CurrentlyStudiable()
		{
			string text;
			if (!this.EverStudiable(out text))
			{
				return false;
			}
			if (!this.studyEnabled)
			{
				return false;
			}
			if (this.Props.frequencyTicks > 0 && this.TicksTilNextStudy > 0)
			{
				return false;
			}
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				if (this.RequiresHoldingPlatform)
				{
					CompHoldingPlatformTarget compHoldingPlatformTarget = pawn.TryGetComp<CompHoldingPlatformTarget>();
					if (compHoldingPlatformTarget == null || !compHoldingPlatformTarget.CanStudy)
					{
						return false;
					}
				}
				if (this.RequiresImprisonment)
				{
					if (pawn.Downed || !pawn.Awake())
					{
						return false;
					}
					if (pawn.guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.Study))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600A4A5 RID: 42149 RVA: 0x003AD3D4 File Offset: 0x003AB5D4
		public bool EverStudiable()
		{
			string text;
			return this.EverStudiable(out text);
		}

		// Token: 0x0600A4A6 RID: 42150 RVA: 0x003AD3E9 File Offset: 0x003AB5E9
		public bool EverStudiable(out string reason)
		{
			reason = null;
			return this.StudyUnlocked() && this.EverStudiableCached(out reason);
		}

		// Token: 0x0600A4A7 RID: 42151 RVA: 0x003AD404 File Offset: 0x003AB604
		public bool StudyUnlocked()
		{
			return !this.parent.Destroyed && !this.Deactivated && (!ModsConfig.AnomalyActive || Find.Anomaly.HighestLevelReached >= this.Props.minMonolithLevelForStudy || !Find.Anomaly.GenerateMonolith);
		}

		// Token: 0x0600A4A8 RID: 42152 RVA: 0x003AD458 File Offset: 0x003AB658
		public bool EverStudiableCached()
		{
			string text;
			return this.EverStudiableCached(out text);
		}

		// Token: 0x0600A4A9 RID: 42153 RVA: 0x003AD470 File Offset: 0x003AB670
		public bool EverStudiableCached(out string reason)
		{
			reason = null;
			if (this.parent == null)
			{
				return false;
			}
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				if (this.RequiresHoldingPlatform && !(this.parent.ParentHolder is Building_HoldingPlatform))
				{
					reason = "RequiresHoldingPlatform".Translate();
					return false;
				}
				if (this.RequiresImprisonment && ((!pawn.Inhumanized() && !pawn.kindDef.studiableAsPrisoner) || !pawn.IsPrisonerOfColony))
				{
					reason = "RequiresImprisonment".Translate();
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600A4AA RID: 42154 RVA: 0x003AD500 File Offset: 0x003AB700
		public virtual void Study(Pawn studier, float studyAmount, float anomalyKnowledgeAmount = 0f)
		{
			bool completed = this.Completed;
			studyAmount *= Find.Storyteller.difficulty.researchSpeedFactor;
			studyAmount *= studier.GetStatValue(StatDefOf.ResearchSpeed, true, -1);
			this.anomalyKnowledgeGained += anomalyKnowledgeAmount;
			Find.StudyManager.Study(this.parent, studier, studyAmount);
			if (ModsConfig.AnomalyActive && anomalyKnowledgeAmount > 0f)
			{
				Find.StudyManager.StudyAnomaly(this.parent, studier, anomalyKnowledgeAmount, this.KnowledgeCategory);
			}
			if (studier != null)
			{
				studier.skills.Learn(SkillDefOf.Intellectual, 0.1f, false, false);
			}
			if (!completed && this.Completed)
			{
				QuestUtility.SendQuestTargetSignals(this.parent.questTags, "Researched", this.parent.Named("SUBJECT"), studier.Named("STUDIER"));
				if (!this.Props.completedMessage.NullOrEmpty())
				{
					Messages.Message(this.Props.completedMessage, this.parent, MessageTypeDefOf.NeutralEvent, true);
				}
				if (studier != null && !this.Props.completedLetterText.NullOrEmpty() && !this.Props.completedLetterTitle.NullOrEmpty())
				{
					Find.LetterStack.ReceiveLetter(this.Props.completedLetterTitle.Formatted(studier.Named("STUDIER"), this.parent.Named("PARENT")), this.Props.completedLetterText.Formatted(studier.Named("STUDIER"), this.parent.Named("PARENT")), this.Props.completedLetterDef ?? LetterDefOf.NeutralEvent, new List<Thing>
					{
						this.parent,
						studier
					}, null, null, null, null, 0, true);
				}
			}
		}

		// Token: 0x0600A4AB RID: 42155 RVA: 0x003AD6D2 File Offset: 0x003AB8D2
		public void Notify_ActivityDeactivated()
		{
			if (!this.Props.canBeActivityDeactivated)
			{
				return;
			}
			this.studyEnabled = false;
		}

		// Token: 0x0600A4AC RID: 42156 RVA: 0x003AD6E9 File Offset: 0x003AB8E9
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompStudiable.<CompGetGizmosExtra>d__48 <CompGetGizmosExtra>d__ = new CompStudiable.<CompGetGizmosExtra>d__48(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A4AD RID: 42157 RVA: 0x003AD6F9 File Offset: 0x003AB8F9
		private IEnumerable<Dialog_InfoCard.Hyperlink> GetRelatedQuestHyperlinks()
		{
			CompStudiable.<GetRelatedQuestHyperlinks>d__49 <GetRelatedQuestHyperlinks>d__ = new CompStudiable.<GetRelatedQuestHyperlinks>d__49(-2);
			<GetRelatedQuestHyperlinks>d__.<>4__this = this;
			return <GetRelatedQuestHyperlinks>d__;
		}

		// Token: 0x0600A4AE RID: 42158 RVA: 0x003AD70C File Offset: 0x003AB90C
		private float AdjustedAnomalyKnowledge(StringBuilder sb = null)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return 0f;
			}
			float num = this.AnomalyKnowledge;
			if (sb != null)
			{
				sb.AppendLine("StatsReport_BaseValue".Translate() + ": " + num.ToStringDecimalIfSmall());
			}
			bool flag = false;
			CompHoldingPlatformTarget compHoldingPlatformTarget;
			if (this.parent.TryGetComp(out compHoldingPlatformTarget) && compHoldingPlatformTarget.CurrentlyHeldOnPlatform)
			{
				CompEntityHolder comp = compHoldingPlatformTarget.HeldPlatform.GetComp<CompEntityHolder>();
				float studyKnowledgeAmountMultiplier = ContainmentUtility.GetStudyKnowledgeAmountMultiplier(this.parent, comp);
				if (!Mathf.Approximately(studyKnowledgeAmountMultiplier, 1f))
				{
					if (!flag)
					{
						if (sb != null)
						{
							sb.AppendLine();
						}
						flag = true;
					}
					num *= studyKnowledgeAmountMultiplier;
					if (sb != null)
					{
						sb.AppendLine("FactorContainmentStrength".Translate() + " (" + StatDefOf.ContainmentStrength.Worker.ValueToString(comp.ContainmentStrength, true, ToStringNumberSense.Absolute) + ")" + ": x" + studyKnowledgeAmountMultiplier.ToStringPercent());
					}
				}
				if (compHoldingPlatformTarget.HeldPlatform.HasAttachedElectroharvester)
				{
					if (!flag)
					{
						if (sb != null)
						{
							sb.AppendLine();
						}
						flag = true;
					}
					num *= 0.5f;
					if (sb != null)
					{
						sb.AppendLine(ThingDefOf.Electroharvester.LabelCap + ": x" + 0.5f.ToStringPercent());
					}
				}
			}
			CompActivity compActivity;
			if (this.parent.TryGetComp(out compActivity))
			{
				if (!flag)
				{
					if (sb != null)
					{
						sb.AppendLine();
					}
					flag = true;
				}
				num *= compActivity.ActivityResearchFactor;
				if (sb != null)
				{
					sb.AppendLine("FactorActivity".Translate() + ": x" + compActivity.ActivityResearchFactor.ToStringPercent());
				}
			}
			if (!Mathf.Approximately(this.Props.knowledgeFactorOutdoors, 1f) && this.parent.IsOutside())
			{
				if (!flag)
				{
					if (sb != null)
					{
						sb.AppendLine();
					}
					flag = true;
				}
				num *= this.Props.knowledgeFactorOutdoors;
				if (sb != null)
				{
					sb.AppendLine("Outdoors".Translate().CapitalizeFirst() + ": x" + this.Props.knowledgeFactorOutdoors.ToStringPercent());
				}
			}
			if (!Mathf.Approximately(Find.Storyteller.difficulty.studyEfficiencyFactor, 1f))
			{
				if (!flag)
				{
					if (sb != null)
					{
						sb.AppendLine();
					}
				}
				num *= Find.Storyteller.difficulty.studyEfficiencyFactor;
				if (sb != null)
				{
					sb.AppendLine("DifficultyLevel".Translate() + ": x" + Find.Storyteller.difficulty.studyEfficiencyFactor.ToStringPercent());
				}
			}
			if (sb != null)
			{
				sb.AppendLine();
			}
			if (sb != null)
			{
				sb.AppendLine("StatsReport_FinalValue".Translate() + ": " + num.ToStringDecimalIfSmall());
			}
			return num;
		}

		// Token: 0x0600A4AF RID: 42159 RVA: 0x003AD9FB File Offset: 0x003ABBFB
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompStudiable.<SpecialDisplayStats>d__52 <SpecialDisplayStats>d__ = new CompStudiable.<SpecialDisplayStats>d__52(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600A4B0 RID: 42160 RVA: 0x003ADA0C File Offset: 0x003ABC0C
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastStudiedTick, "lastStudiedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.studyEnabled, "studyEnabled", true, false);
			Scribe_Values.Look<float>(ref this.studyPoints, "studiedAmount", 0f, false);
			Scribe_Values.Look<float>(ref this.anomalyKnowledgeGained, "anomalyKnowledgeGained", 0f, false);
			Scribe_Values.Look<int>(ref this.studyInteractions, "studyInteractions", 0, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				float num = 0f;
				Scribe_Values.Look<float>(ref num, "progress", 0f, false);
				if (num > 0f)
				{
					float num2 = num / this.Props.studyAmountToComplete;
					this.studyPoints = this.Props.studyAmountToComplete * num2;
				}
			}
			float num3;
			if (Scribe.mode == LoadSaveMode.PostLoadInit && Find.StudyManager.backCompatStudyProgress.TryGetValue(this.parent.def, out num3))
			{
				this.studyPoints = this.Props.studyAmountToComplete * num3;
			}
		}

		// Token: 0x0600A4B1 RID: 42161 RVA: 0x003ADB00 File Offset: 0x003ABD00
		public override string CompInspectStringExtra()
		{
			string text = string.Empty;
			if (!this.EverStudiable())
			{
				return text;
			}
			if (this.Props.Completable)
			{
				text = text + ("StudyProgress".Translate() + ": " + this.studyPoints.ToString("F1") + " / ") + this.Props.studyAmountToComplete.ToString();
				if (this.Completed)
				{
					text += " (" + "StudyCompleted".Translate() + ")";
				}
			}
			else if (!this.RequiresHoldingPlatform && !(this.parent is Pawn))
			{
				string inspectStringExtraFor = CompStudiable.GetInspectStringExtraFor(this.parent);
				if (!inspectStringExtraFor.NullOrEmpty())
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += inspectStringExtraFor;
				}
			}
			return text;
		}

		// Token: 0x0600A4B2 RID: 42162 RVA: 0x003ADBF8 File Offset: 0x003ABDF8
		public static string GetInspectStringExtraFor(Thing thing)
		{
			CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
			if (compStudiable != null && compStudiable.DisplayStudyInfoOnInspectPane && compStudiable.EverStudiable())
			{
				TaggedString taggedString = TaggedString.Empty;
				if (ModsConfig.AnomalyActive && compStudiable.AnomalyKnowledge > 0f && compStudiable.KnowledgeCategory != null)
				{
					taggedString += "KnowledgeCategory".Translate() + ": " + compStudiable.KnowledgeCategory.LabelCap;
					if (compStudiable.TicksTilNextStudy <= 0)
					{
						TaggedString t = taggedString;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
						defaultInterpolatedStringHandler.AppendLiteral(" (");
						defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("ReadyToStudy".Translate());
						defaultInterpolatedStringHandler.AppendLiteral(")");
						taggedString = t + defaultInterpolatedStringHandler.ToStringAndClear();
					}
					else if (compStudiable.TicksTilNextStudy > 0)
					{
						TaggedString t2 = taggedString;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 1);
						defaultInterpolatedStringHandler2.AppendLiteral(" (");
						defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("CanBeStudiedInDuration".Translate(compStudiable.TicksTilNextStudy.ToStringTicksToPeriod(true, false, true, true, false)));
						defaultInterpolatedStringHandler2.AppendLiteral(")");
						taggedString = t2 + defaultInterpolatedStringHandler2.ToStringAndClear();
					}
					if (compStudiable.Props.knowledgeFactorOutdoors != 1f && compStudiable.parent.IsOutside())
					{
						TaggedString t3 = taggedString;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(4, 2);
						defaultInterpolatedStringHandler3.AppendLiteral("\n");
						defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("KnowledgeFactorOutdoors".Translate() + ": x" + compStudiable.Props.knowledgeFactorOutdoors.ToStringPercent());
						defaultInterpolatedStringHandler3.AppendLiteral(" (");
						defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("Outdoors".Translate());
						defaultInterpolatedStringHandler3.AppendLiteral(")");
						taggedString = t3 + defaultInterpolatedStringHandler3.ToStringAndClear();
					}
				}
				else if (compStudiable.TicksTilNextStudy <= 0)
				{
					taggedString += "ReadyToStudy".Translate().CapitalizeFirst();
				}
				else
				{
					taggedString += "CanBeStudiedInDuration".Translate(compStudiable.TicksTilNextStudy.ToStringTicksToPeriod(true, false, true, true, false));
				}
				return taggedString.Resolve();
			}
			return null;
		}

		// Token: 0x04006158 RID: 24920
		private static readonly CachedTexture StudyToggleIcon = new CachedTexture("UI/Icons/Study");

		// Token: 0x04006159 RID: 24921
		public int lastStudiedTick = -9999999;

		// Token: 0x0400615A RID: 24922
		public bool studyEnabled = true;

		// Token: 0x0400615B RID: 24923
		public float studyPoints;

		// Token: 0x0400615C RID: 24924
		public float anomalyKnowledgeGained;

		// Token: 0x0400615D RID: 24925
		public int studyInteractions;

		// Token: 0x0400615E RID: 24926
		private CompStudyUnlocks compStudyUnlocks;

		// Token: 0x0400615F RID: 24927
		private CompActivity compActivity;

		// Token: 0x04006160 RID: 24928
		private static StringBuilder knowledgeSb = new StringBuilder();
	}
}
