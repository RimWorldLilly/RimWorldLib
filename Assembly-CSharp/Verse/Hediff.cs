using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200040A RID: 1034
	public class Hediff : IExposable, ILoadReferenceable
	{
		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001E36 RID: 7734 RVA: 0x000BA781 File Offset: 0x000B8981
		public virtual string LabelBase
		{
			get
			{
				HediffStage curStage = this.CurStage;
				return ((curStage != null) ? curStage.overrideLabel : null) ?? this.def.label;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001E37 RID: 7735 RVA: 0x000BA7A4 File Offset: 0x000B89A4
		public string LabelBaseCap
		{
			get
			{
				return this.LabelBase.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001E38 RID: 7736 RVA: 0x000BA7B8 File Offset: 0x000B89B8
		public virtual string Label
		{
			get
			{
				string labelInBrackets = this.LabelInBrackets;
				return this.LabelBase + (labelInBrackets.NullOrEmpty() ? "" : (" (" + labelInBrackets + ")"));
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001E39 RID: 7737 RVA: 0x000BA7F6 File Offset: 0x000B89F6
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001E3A RID: 7738 RVA: 0x000BA809 File Offset: 0x000B8A09
		public virtual Color LabelColor
		{
			get
			{
				return this.def.defaultLabelColor;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001E3B RID: 7739 RVA: 0x000BA816 File Offset: 0x000B8A16
		public virtual string LabelInBrackets
		{
			get
			{
				if (this.CurStage != null && !this.CurStage.label.NullOrEmpty())
				{
					return this.CurStage.label;
				}
				return null;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001E3C RID: 7740 RVA: 0x000BA83F File Offset: 0x000B8A3F
		public virtual string SeverityLabel
		{
			get
			{
				if (!this.IsLethal && !this.def.alwaysShowSeverity)
				{
					return null;
				}
				return (this.Severity / Mathf.Abs(this.def.lethalSeverity)).ToStringPercent();
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x000BA874 File Offset: 0x000B8A74
		public virtual int UIGroupKey
		{
			get
			{
				return this.Label.GetHashCode();
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001E3E RID: 7742 RVA: 0x000BA884 File Offset: 0x000B8A84
		public virtual string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (StatDrawEntry statDrawEntry in HediffStatsUtility.SpecialDisplayStats(this.CurStage, this))
				{
					if (statDrawEntry.ShouldDisplay(null))
					{
						stringBuilder.Append("  - " + statDrawEntry.LabelCap + ": " + statDrawEntry.ValueString);
						HediffStage curStage = this.CurStage;
						if (((curStage != null) ? curStage.statOffsetEffectMultiplier : null) != null)
						{
							StringBuilder stringBuilder2 = stringBuilder;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
							defaultInterpolatedStringHandler.AppendLiteral(" x ");
							defaultInterpolatedStringHandler.AppendFormatted<TaggedString>(this.CurStage.statOffsetEffectMultiplier.LabelCap);
							stringBuilder2.Append(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						else
						{
							HediffStage curStage2 = this.CurStage;
							if (((curStage2 != null) ? curStage2.statFactorEffectMultiplier : null) != null)
							{
								StringBuilder stringBuilder3 = stringBuilder;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 1);
								defaultInterpolatedStringHandler2.AppendLiteral(" x ");
								defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>(this.CurStage.statFactorEffectMultiplier.LabelCap);
								stringBuilder3.Append(defaultInterpolatedStringHandler2.ToStringAndClear());
							}
						}
						stringBuilder.AppendLine();
					}
				}
				if (ModsConfig.AnomalyActive && !this.def.aptitudes.NullOrEmpty<Aptitude>())
				{
					stringBuilder.AppendLine((from x in this.def.aptitudes
					select x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList("  - ", true));
				}
				HediffStage stage = this.CurStage;
				if (stage != null)
				{
					if (!stage.enablesNeeds.NullOrEmpty<NeedDef>())
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendLine(("AddsNeeds".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
						stringBuilder.AppendLine((from x in stage.enablesNeeds
						select x.LabelCap.ToString()).ToLineList("  - ", false));
					}
					if (!stage.disablesNeeds.NullOrEmpty<NeedDef>())
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendLine(("DisabledNeedsLabel".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
						stringBuilder.AppendLine((from x in stage.disablesNeeds
						select x.LabelCap.ToString()).ToLineList("  - ", false));
					}
					if (stage.disabledWorkTags != WorkTags.None)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendLine(("DisabledWorkLabel".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
						IEnumerable<string> items = from x in DefDatabase<WorkTypeDef>.AllDefsListForReading
						where (stage.disabledWorkTags & x.workTags) > WorkTags.None
						select x.labelShort;
						stringBuilder.Append("  - " + items.ToCommaList(false, false).CapitalizeFirst());
					}
				}
				if (this.def.CompProps<HediffCompProperties_GiveLovinMTBFactor>() != null)
				{
					stringBuilder.AppendLine("  - " + "IncreasesChanceOfLovin".Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001E3F RID: 7743 RVA: 0x000BAC24 File Offset: 0x000B8E24
		public virtual HediffStage CurStage
		{
			get
			{
				if (!this.def.stages.NullOrEmpty<HediffStage>())
				{
					return this.def.stages[this.CurStageIndex];
				}
				return null;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001E40 RID: 7744 RVA: 0x000BAC50 File Offset: 0x000B8E50
		public virtual bool ShouldRemove
		{
			get
			{
				return this.Severity <= 0f;
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x06001E41 RID: 7745 RVA: 0x000BAC62 File Offset: 0x000B8E62
		public virtual bool Visible
		{
			get
			{
				return this.visible || this.CurStage == null || this.CurStage.becomeVisible;
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x06001E42 RID: 7746 RVA: 0x000BAC81 File Offset: 0x000B8E81
		public virtual float BleedRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001E43 RID: 7747 RVA: 0x000BAC88 File Offset: 0x000B8E88
		public virtual float BleedRateScaled
		{
			get
			{
				return this.BleedRate / this.pawn.HealthScale;
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001E44 RID: 7748 RVA: 0x000BAC9C File Offset: 0x000B8E9C
		public bool Bleeding
		{
			get
			{
				return this.BleedRate > 1E-05f;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001E45 RID: 7749 RVA: 0x000BACAB File Offset: 0x000B8EAB
		public virtual float PainOffset
		{
			get
			{
				if (this.CurStage != null && !this.causesNoPain)
				{
					return this.CurStage.painOffset;
				}
				return 0f;
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001E46 RID: 7750 RVA: 0x000BACCE File Offset: 0x000B8ECE
		public virtual float PainFactor
		{
			get
			{
				HediffStage curStage = this.CurStage;
				if (curStage == null)
				{
					return 1f;
				}
				return curStage.painFactor;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001E47 RID: 7751 RVA: 0x000BACE5 File Offset: 0x000B8EE5
		public List<PawnCapacityModifier> CapMods
		{
			get
			{
				HediffStage curStage = this.CurStage;
				if (curStage == null)
				{
					return null;
				}
				return curStage.capMods;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001E48 RID: 7752 RVA: 0x000BACF8 File Offset: 0x000B8EF8
		public virtual float SummaryHealthPercentImpact
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001E49 RID: 7753 RVA: 0x000BAD00 File Offset: 0x000B8F00
		public virtual float TendPriority
		{
			get
			{
				float num = 0f;
				HediffStage curStage = this.CurStage;
				if (curStage != null && curStage.lifeThreatening)
				{
					num = Mathf.Max(num, 1f);
				}
				num = Mathf.Max(num, this.BleedRate * 1.5f);
				HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && hediffComp_TendDuration.TProps.severityPerDayTended < 0f)
				{
					num = Mathf.Max(num, 0.025f);
				}
				return num;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06001E4A RID: 7754 RVA: 0x000BAD6D File Offset: 0x000B8F6D
		public virtual TextureAndColor StateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06001E4B RID: 7755 RVA: 0x000BAD74 File Offset: 0x000B8F74
		public virtual int CurStageIndex
		{
			get
			{
				return this.def.StageAtSeverity(this.Severity);
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001E4C RID: 7756 RVA: 0x000BAD87 File Offset: 0x000B8F87
		// (set) Token: 0x06001E4D RID: 7757 RVA: 0x000BAD90 File Offset: 0x000B8F90
		public virtual float Severity
		{
			get
			{
				return this.severityInt;
			}
			set
			{
				bool flag = false;
				if (this.IsLethal && value >= this.def.lethalSeverity)
				{
					value = this.def.lethalSeverity;
					flag = true;
				}
				bool flag2 = this is Hediff_Injury && value > this.severityInt && Mathf.RoundToInt(value) != Mathf.RoundToInt(this.severityInt);
				int curStageIndex = this.CurStageIndex;
				this.severityInt = Mathf.Clamp(value, this.def.minSeverity, this.def.maxSeverity);
				if (this.CurStageIndex != curStageIndex)
				{
					this.OnStageIndexChanged(this.CurStageIndex);
				}
				if ((this.CurStageIndex != curStageIndex || flag || flag2) && this.pawn.health.hediffSet.hediffs.Contains(this))
				{
					this.pawn.health.Notify_HediffChanged(this);
					if (!this.pawn.Dead && this.pawn.needs.mood != null)
					{
						this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
					}
				}
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001E4E RID: 7758 RVA: 0x000BAEAB File Offset: 0x000B90AB
		// (set) Token: 0x06001E4F RID: 7759 RVA: 0x000BAEB3 File Offset: 0x000B90B3
		public BodyPartRecord Part
		{
			get
			{
				return this.part;
			}
			set
			{
				if (this.pawn == null && value != null)
				{
					Log.Error("Hediff: Cannot set Part without setting pawn first.");
					return;
				}
				this.part = value;
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x06001E50 RID: 7760 RVA: 0x000BAED2 File Offset: 0x000B90D2
		public bool IsLethal
		{
			get
			{
				return this.def.lethalSeverity > 0f && this.canBeThreateningToPart;
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x000BAEEE File Offset: 0x000B90EE
		public bool IsCurrentlyLifeThreatening
		{
			get
			{
				return this.IsStageLifeThreatening(this.CurStage);
			}
		}

		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06001E52 RID: 7762 RVA: 0x000BAEFC File Offset: 0x000B90FC
		public List<Ability> AllAbilitiesForReading
		{
			get
			{
				if (this.abilities == null && !this.def.abilities.NullOrEmpty<AbilityDef>())
				{
					this.abilities = new List<Ability>();
					foreach (AbilityDef abilityDef in this.def.abilities)
					{
						this.abilities.Add(AbilityUtility.MakeAbility(abilityDef, this.pawn));
					}
				}
				return this.abilities;
			}
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x000BAF90 File Offset: 0x000B9190
		public virtual bool TendableNow(bool ignoreTimer = false)
		{
			if (!this.def.tendable || this.Severity <= 0f || this.FullyImmune() || !this.Visible || this.IsPermanent() || !this.pawn.RaceProps.IsFlesh || (this is Hediff_Injury && !this.pawn.health.CanBleed))
			{
				return false;
			}
			if (!ignoreTimer)
			{
				HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && !hediffComp_TendDuration.AllowTend)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x000BB015 File Offset: 0x000B9215
		public bool IsStageLifeThreatening(HediffStage stage)
		{
			return stage != null && stage.lifeThreatening && this.canBeThreateningToPart;
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x000BB02C File Offset: 0x000B922C
		public bool IsAnyStageLifeThreatening()
		{
			if (this.def.stages == null || !this.canBeThreateningToPart)
			{
				return false;
			}
			for (int i = 0; i < this.def.stages.Count; i++)
			{
				if (this.def.stages[i].lifeThreatening)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x000BB086 File Offset: 0x000B9286
		public bool CanEverKill()
		{
			return this.IsLethal || this.IsAnyStageLifeThreatening();
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000BB098 File Offset: 0x000B9298
		public void SetVisible()
		{
			this.visible = true;
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000BB0A1 File Offset: 0x000B92A1
		protected virtual void OnStageIndexChanged(int stageIndex)
		{
			if (this.CurStage.pctConditionalThoughtsNullified > 0f || this.CurStage.pctAllThoughtNullification > 0f)
			{
				this.pawn.health.hediffSet.CacheThoughtsNullified();
			}
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x000BB0DC File Offset: 0x000B92DC
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.combatLogEntry != null)
			{
				LogEntry target = this.combatLogEntry.Target;
				if (target == null || !Current.Game.battleLog.IsEntryActive(target))
				{
					this.combatLogEntry = null;
				}
			}
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
			Scribe_Values.Look<int>(ref this.tickAdded, "tickAdded", 0, false);
			Scribe_Values.Look<bool>(ref this.visible, "visible", false, false);
			Scribe_Values.Look<float>(ref this.severityInt, "severity", 0f, false);
			Scribe_Values.Look<bool>(ref this.recordedTale, "recordedTale", false, false);
			Scribe_Values.Look<bool>(ref this.causesNoPain, "causesNoPain", false, false);
			Scribe_Values.Look<string>(ref this.combatLogText, "combatLogText", null, false);
			Scribe_Values.Look<bool>(ref this.canBeThreateningToPart, "canBeThreateningToPart", false, false);
			Scribe_Defs.Look<HediffDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.sourceDef, "source");
			Scribe_Defs.Look<HediffDef>(ref this.sourceHediffDef, "sourceHediffDef");
			Scribe_Defs.Look<BodyPartGroupDef>(ref this.sourceBodyPartGroup, "sourceBodyPartGroup");
			Scribe_Values.Look<string>(ref this.sourceLabel, "sourceLabel", null, false);
			Scribe_Values.Look<string>(ref this.sourceToolLabel, "sourceToolLabel", null, false);
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_References.Look<LogEntry>(ref this.combatLogEntry, "combatLogEntry", false);
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.abilities != null)
			{
				foreach (Ability ability in this.abilities)
				{
					ability.pawn = this.pawn;
					ability.verb.caster = this.pawn;
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x000BB2D4 File Offset: 0x000B94D4
		public virtual void Tick()
		{
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x000BB2D8 File Offset: 0x000B94D8
		public virtual void TickInterval(int delta)
		{
			this.ageTicks += delta;
			if (this.def.hediffGivers != null && this.pawn.IsHashIntervalTick(60, delta))
			{
				for (int i = 0; i < this.def.hediffGivers.Count; i++)
				{
					this.def.hediffGivers[i].OnIntervalPassed(this.pawn, this);
				}
			}
			if (this.Visible && !this.visible)
			{
				this.visible = true;
				if (this.def.taleOnVisible != null)
				{
					TaleRecorder.RecordTale(this.def.taleOnVisible, new object[]
					{
						this.pawn,
						this.def
					});
				}
			}
			HediffStage curStage = this.CurStage;
			if (curStage != null)
			{
				if (curStage.hediffGivers != null && this.pawn.IsHashIntervalTick(60, delta))
				{
					for (int j = 0; j < curStage.hediffGivers.Count; j++)
					{
						curStage.hediffGivers[j].OnIntervalPassed(this.pawn, this);
					}
				}
				if (curStage.mentalStateGivers != null && this.pawn.IsHashIntervalTick(60, delta) && !this.pawn.InMentalState)
				{
					for (int k = 0; k < curStage.mentalStateGivers.Count; k++)
					{
						MentalStateGiver mentalStateGiver = curStage.mentalStateGivers[k];
						if (Rand.MTBEventOccurs(mentalStateGiver.mtbDays, 60000f, 60f))
						{
							this.pawn.mindState.mentalStateHandler.TryStartMentalState(mentalStateGiver.mentalState, "MentalStateReason_Hediff".Translate(this.Label), false, false, false, null, false, false, false);
						}
					}
				}
				if (curStage.mentalBreakMtbDays > 0f && this.pawn.IsHashIntervalTick(60, delta) && !this.pawn.InMentalState && !this.pawn.Downed && Rand.MTBEventOccurs(curStage.mentalBreakMtbDays, 60000f, 60f))
				{
					this.TryDoRandomMentalBreak();
				}
				if (curStage.vomitMtbDays > 0f && this.pawn.IsHashIntervalTick(600, delta) && Rand.MTBEventOccurs(curStage.vomitMtbDays, 60000f, 600f) && this.pawn.Spawned && this.pawn.Awake() && this.pawn.RaceProps.IsFlesh)
				{
					this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
				}
				if (curStage.forgetMemoryThoughtMtbDays > 0f)
				{
					Pawn_NeedsTracker needs = this.pawn.needs;
					Thought_Memory th;
					if (((needs != null) ? needs.mood : null) != null && this.pawn.IsHashIntervalTick(400, delta) && Rand.MTBEventOccurs(curStage.forgetMemoryThoughtMtbDays, 60000f, 400f) && this.pawn.needs.mood.thoughts.memories.Memories.TryRandomElement(out th))
					{
						this.pawn.needs.mood.thoughts.memories.RemoveMemory(th);
					}
				}
				if (!this.recordedTale && curStage.tale != null)
				{
					TaleRecorder.RecordTale(curStage.tale, new object[]
					{
						this.pawn
					});
					this.recordedTale = true;
				}
				if (curStage.destroyPart && this.Part != null && this.Part != this.pawn.RaceProps.body.corePart)
				{
					this.pawn.health.AddHediff(HediffDefOf.MissingBodyPart, this.Part, null, null);
				}
				if (curStage.deathMtbDays > 0f && this.pawn.IsHashIntervalTick(200, delta) && Rand.MTBEventOccurs(curStage.deathMtbDays, 60000f, 200f))
				{
					this.DoMTBDeath();
				}
			}
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x000BB6E0 File Offset: 0x000B98E0
		private void DoMTBDeath()
		{
			HediffStage curStage = this.CurStage;
			if (!curStage.mtbDeathDestroysBrain && ModsConfig.BiotechActive)
			{
				Pawn_GeneTracker genes = this.pawn.genes;
				if (genes != null && genes.HasActiveGene(GeneDefOf.Deathless))
				{
					return;
				}
			}
			this.pawn.Kill(null, this);
			if (curStage.mtbDeathDestroysBrain)
			{
				BodyPartRecord brain = this.pawn.health.hediffSet.GetBrain();
				if (brain != null)
				{
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, brain);
					this.pawn.health.AddHediff(hediff, brain, null, null);
				}
			}
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x000BB788 File Offset: 0x000B9988
		private void TryDoRandomMentalBreak()
		{
			HediffStage curStage = this.CurStage;
			if (curStage == null)
			{
				return;
			}
			TaggedString taggedString = "MentalStateReason_Hediff".Translate(this.Label);
			if (!curStage.mentalBreakExplanation.NullOrEmpty())
			{
				taggedString += "\n\n" + curStage.mentalBreakExplanation.Formatted(this.pawn.Named("PAWN"));
			}
			if (this.pawn.NonHumanlikeOrWildMan())
			{
				this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, taggedString, false, false, false, null, false, false, false);
				return;
			}
			MentalBreakDef mentalBreakDef;
			if ((from x in DefDatabase<MentalBreakDef>.AllDefsListForReading
			where x.Worker.BreakCanOccur(this.pawn) && (curStage.allowedMentalBreakIntensities == null || curStage.allowedMentalBreakIntensities.Contains(x.intensity))
			select x).TryRandomElementByWeight((MentalBreakDef x) => x.Worker.CommonalityFor(this.pawn, false), out mentalBreakDef))
			{
				mentalBreakDef.Worker.TryStart(this.pawn, taggedString.Resolve(), false);
			}
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000BB88C File Offset: 0x000B9A8C
		public virtual void PostMake()
		{
			this.Severity = Mathf.Max(this.Severity, this.def.initialSeverity);
			this.causesNoPain = (Rand.Value < this.def.chanceToCauseNoPain);
			if (this.def.onlyLifeThreateningTo != null)
			{
				bool flag = false;
				for (int i = 0; i < this.def.onlyLifeThreateningTo.Count; i++)
				{
					if (this.Part.def == this.def.onlyLifeThreateningTo[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.canBeThreateningToPart = false;
				}
			}
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000BB924 File Offset: 0x000B9B24
		public virtual void PostAdd(DamageInfo? dinfo)
		{
			this.tickAdded = Find.TickManager.TicksGame;
			if (!this.def.abilities.NullOrEmpty<AbilityDef>())
			{
				Pawn_AbilityTracker pawn_AbilityTracker = this.pawn.abilities;
				if (pawn_AbilityTracker != null)
				{
					pawn_AbilityTracker.Notify_TemporaryAbilitiesChanged();
				}
			}
			if (!this.def.removeWithTags.NullOrEmpty<string>())
			{
				for (int i = this.pawn.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
				{
					Hediff hediff = this.pawn.health.hediffSet.hediffs[i];
					if (hediff != this && !hediff.def.tags.NullOrEmpty<string>())
					{
						for (int j = 0; j < this.def.removeWithTags.Count; j++)
						{
							if (hediff.def.tags.Contains(this.def.removeWithTags[j]))
							{
								this.pawn.health.RemoveHediff(hediff);
								break;
							}
						}
					}
				}
			}
			if (!this.def.aptitudes.NullOrEmpty<Aptitude>())
			{
				this.pawn.skills.DirtyAptitudes();
			}
			if (this.def.clearsEgo)
			{
				this.pawn.everLostEgo = true;
			}
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000BBA69 File Offset: 0x000B9C69
		public virtual void PreRemoved()
		{
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000BBA6C File Offset: 0x000B9C6C
		public virtual void PostRemoved()
		{
			HediffStage curStage = this.CurStage;
			if (!this.pawn.Dead && this.def.chemicalNeed != null)
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs != null)
				{
					needs.AddOrRemoveNeedsAsAppropriate();
				}
			}
			else if (curStage != null && !this.pawn.Dead && (!curStage.disablesNeeds.NullOrEmpty<NeedDef>() || !curStage.enablesNeeds.NullOrEmpty<NeedDef>()))
			{
				Pawn_NeedsTracker needs2 = this.pawn.needs;
				if (needs2 != null)
				{
					needs2.AddOrRemoveNeedsAsAppropriate();
				}
			}
			if (!this.def.abilities.NullOrEmpty<AbilityDef>())
			{
				Pawn_AbilityTracker pawn_AbilityTracker = this.pawn.abilities;
				if (pawn_AbilityTracker != null)
				{
					pawn_AbilityTracker.Notify_TemporaryAbilitiesChanged();
				}
			}
			if (!this.def.aptitudes.NullOrEmpty<Aptitude>())
			{
				this.pawn.skills.DirtyAptitudes();
			}
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x000BBB3C File Offset: 0x000B9D3C
		public virtual void PostTick()
		{
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x000BBB3E File Offset: 0x000B9D3E
		public virtual void PostTickInterval(int delta)
		{
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x000BBB40 File Offset: 0x000B9D40
		public virtual void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x000BBB42 File Offset: 0x000B9D42
		public virtual void Heal(float amount)
		{
			if (amount <= 0f)
			{
				return;
			}
			this.Severity -= amount;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x000BBB6C File Offset: 0x000B9D6C
		public virtual void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x000BBB6E File Offset: 0x000B9D6E
		public virtual bool TryMergeWith(Hediff other)
		{
			if (other == null || other.def != this.def || other.Part != this.Part)
			{
				return false;
			}
			this.Severity += other.Severity;
			this.ageTicks = 0;
			return true;
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x000BBBAC File Offset: 0x000B9DAC
		public virtual bool CauseDeathNow()
		{
			if (this.IsLethal)
			{
				bool flag = this.Severity >= this.def.lethalSeverity;
				if (flag && DebugViewSettings.logCauseOfDeath)
				{
					Log.Message("CauseOfDeath: lethal severity exceeded " + this.Severity.ToString() + " >= " + this.def.lethalSeverity.ToString());
				}
				return flag;
			}
			return false;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x000BBC15 File Offset: 0x000B9E15
		public virtual void Notify_Downed()
		{
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x000BBC17 File Offset: 0x000B9E17
		public virtual void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x000BBC19 File Offset: 0x000B9E19
		public virtual void Notify_PawnKilled()
		{
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x000BBC1B File Offset: 0x000B9E1B
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06001E6D RID: 7789 RVA: 0x000BBC1D File Offset: 0x000B9E1D
		public virtual void Notify_PawnDamagedThing(Thing thing, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
		}

		// Token: 0x06001E6E RID: 7790 RVA: 0x000BBC1F File Offset: 0x000B9E1F
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo targets)
		{
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x000BBC21 File Offset: 0x000B9E21
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x000BBC23 File Offset: 0x000B9E23
		public virtual void Notify_RelationAdded(Pawn otherPawn, PawnRelationDef relationDef)
		{
		}

		// Token: 0x06001E71 RID: 7793 RVA: 0x000BBC25 File Offset: 0x000B9E25
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x000BBC27 File Offset: 0x000B9E27
		public virtual void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
		{
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000BBC29 File Offset: 0x000B9E29
		public virtual void Notify_IngestedThing(Thing thing, int amount)
		{
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x000BBC2B File Offset: 0x000B9E2B
		public virtual void Notify_Resurrected()
		{
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x000BBC2D File Offset: 0x000B9E2D
		public virtual void Notify_PawnCorpseSpawned()
		{
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x000BBC2F File Offset: 0x000B9E2F
		public virtual void Notify_PawnCorpseDestroyed()
		{
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000BBC31 File Offset: 0x000B9E31
		public virtual void Notify_Regenerated(float hp)
		{
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000BBC33 File Offset: 0x000B9E33
		public virtual void Notify_SurgicallyRemoved(Pawn surgeon)
		{
			if (this.def.HasDefinedGraphicProperties || this.def.forceRenderTreeRecache)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000BBC64 File Offset: 0x000B9E64
		public virtual void Notify_SurgicallyReplaced(Pawn surgeon)
		{
			if (this.def.HasDefinedGraphicProperties || this.def.forceRenderTreeRecache)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000BBC95 File Offset: 0x000B9E95
		public virtual void Notify_Spawned()
		{
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000BBC97 File Offset: 0x000B9E97
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			return null;
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000BBC9A File Offset: 0x000B9E9A
		public virtual string GetInspectString()
		{
			return this.def.inspectString ?? string.Empty;
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06001E7D RID: 7805 RVA: 0x000BBCB0 File Offset: 0x000B9EB0
		public virtual string Description
		{
			get
			{
				return this.def.Description;
			}
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000BBCC0 File Offset: 0x000B9EC0
		public virtual string GetTooltip(Pawn pawn, bool showHediffsDebugInfo)
		{
			Hediff.tipSb.Clear();
			HediffStage curStage = this.CurStage;
			if (!this.LabelCap.NullOrEmpty())
			{
				Hediff.tipSb.AppendTagged(this.LabelCap.Colorize(ColoredText.TipSectionTitleColor));
			}
			string severityLabel = this.SeverityLabel;
			if (!severityLabel.NullOrEmpty())
			{
				Hediff.tipSb.Append(": ").Append(severityLabel);
			}
			Hediff.tipSb.AppendLine();
			if (!this.def.overrideTooltip.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLineTagged(this.def.overrideTooltip.Formatted(pawn.Named("PAWN")));
			}
			else if (curStage != null && !curStage.overrideTooltip.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLineTagged(curStage.overrideTooltip.Formatted(pawn.Named("PAWN")));
			}
			else
			{
				string description = this.Description;
				if (!description.NullOrEmpty())
				{
					Hediff.tipSb.AppendLine().AppendLine(description);
				}
			}
			if (!this.def.extraTooltip.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLineTagged(this.def.extraTooltip.Formatted(pawn.Named("PAWN")));
			}
			if (curStage != null && !curStage.extraTooltip.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLineTagged(curStage.extraTooltip.Formatted(pawn.Named("PAWN")));
			}
			string tipStringExtra = this.TipStringExtra;
			if (!tipStringExtra.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLine(tipStringExtra.TrimEndNewlines());
			}
			TaggedString t;
			LogEntry logEntry;
			if (HealthCardUtility.GetCombatLogInfo(Gen.YieldSingle<Hediff>(this), out t, out logEntry) && !t.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLineTagged(("Cause".Translate() + ": " + t).Colorize(ColoredText.SubtleGrayColor));
			}
			if (showHediffsDebugInfo && !this.DebugString().NullOrEmpty() && !this.DebugString().NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLine(this.DebugString().TrimEndNewlines());
			}
			return Hediff.tipSb.ToString().TrimEnd();
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000BBF08 File Offset: 0x000BA108
		public virtual void CopyFrom(Hediff other)
		{
			this.ageTicks = other.ageTicks;
			this.sourceLabel = other.sourceLabel;
			this.sourceDef = other.sourceDef;
			this.sourceBodyPartGroup = other.sourceBodyPartGroup;
			this.severityInt = other.severityInt;
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x000BBF46 File Offset: 0x000BA146
		public virtual void PostDebugAdd()
		{
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x000BBF48 File Offset: 0x000BA148
		public virtual string DebugString()
		{
			string text = "";
			if (!this.Visible)
			{
				text += "hidden\n";
			}
			text = text + "severity: " + this.Severity.ToString("F3") + ((this.Severity >= this.def.maxSeverity) ? " (reached max)" : "");
			if (this.TendableNow(false))
			{
				text = text + "\ntend priority: " + this.TendPriority.ToString();
			}
			return text.Indented("    ");
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x000BBFDB File Offset: 0x000BA1DB
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			Hediff.<SpecialDisplayStats>d__125 <SpecialDisplayStats>d__ = new Hediff.<SpecialDisplayStats>d__125(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000BBFF4 File Offset: 0x000BA1F4
		public override string ToString()
		{
			string[] array = new string[6];
			array[0] = "(";
			int num = 1;
			HediffDef hediffDef = this.def;
			array[num] = (((hediffDef != null) ? hediffDef.defName : null) ?? base.GetType().Name);
			array[2] = ((this.part != null) ? (" " + this.part.Label) : "");
			array[3] = " ticksSinceCreation=";
			array[4] = this.ageTicks.ToString();
			array[5] = ")";
			return string.Concat(array);
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x000BC07D File Offset: 0x000BA27D
		public string GetUniqueLoadID()
		{
			return "Hediff_" + this.loadID.ToString();
		}

		// Token: 0x040017FB RID: 6139
		public HediffDef def;

		// Token: 0x040017FC RID: 6140
		public int ageTicks;

		// Token: 0x040017FD RID: 6141
		public int tickAdded = -1;

		// Token: 0x040017FE RID: 6142
		private BodyPartRecord part;

		// Token: 0x040017FF RID: 6143
		public string sourceLabel;

		// Token: 0x04001800 RID: 6144
		public ThingDef sourceDef;

		// Token: 0x04001801 RID: 6145
		public BodyPartGroupDef sourceBodyPartGroup;

		// Token: 0x04001802 RID: 6146
		public string sourceToolLabel;

		// Token: 0x04001803 RID: 6147
		public HediffDef sourceHediffDef;

		// Token: 0x04001804 RID: 6148
		public int loadID = -1;

		// Token: 0x04001805 RID: 6149
		protected float severityInt;

		// Token: 0x04001806 RID: 6150
		private bool recordedTale;

		// Token: 0x04001807 RID: 6151
		protected bool causesNoPain;

		// Token: 0x04001808 RID: 6152
		private bool visible;

		// Token: 0x04001809 RID: 6153
		public WeakReference<LogEntry> combatLogEntry;

		// Token: 0x0400180A RID: 6154
		public string combatLogText;

		// Token: 0x0400180B RID: 6155
		public int temp_partIndexToSetLater = -1;

		// Token: 0x0400180C RID: 6156
		public bool canBeThreateningToPart = true;

		// Token: 0x0400180D RID: 6157
		private List<Ability> abilities;

		// Token: 0x0400180E RID: 6158
		[Unsaved(false)]
		public Pawn pawn;

		// Token: 0x0400180F RID: 6159
		private static StringBuilder tipSb = new StringBuilder();
	}
}
