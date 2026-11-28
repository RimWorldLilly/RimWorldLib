using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020004BF RID: 1215
	public class Hediff_Pregnant : HediffWithParents
	{
		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x060021D7 RID: 8663 RVA: 0x000C8EBC File Offset: 0x000C70BC
		// (set) Token: 0x060021D8 RID: 8664 RVA: 0x000C8EC4 File Offset: 0x000C70C4
		public float GestationProgress
		{
			get
			{
				return this.Severity;
			}
			private set
			{
				this.Severity = value;
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x060021D9 RID: 8665 RVA: 0x000C8ED0 File Offset: 0x000C70D0
		private bool IsSeverelyWounded
		{
			get
			{
				float num = 0f;
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i] is Hediff_Injury && !hediffs[i].IsPermanent())
					{
						num += hediffs[i].Severity;
					}
				}
				List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
				{
					if (missingPartsCommonAncestors[j].IsFreshNonSolidExtremity)
					{
						num += missingPartsCommonAncestors[j].Part.def.GetMaxHealth(this.pawn);
					}
				}
				return num > 38f * this.pawn.RaceProps.baseHealthScale;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x060021DA RID: 8666 RVA: 0x000C8FA8 File Offset: 0x000C71A8
		public PregnancyAttitude? Attitude
		{
			get
			{
				if (this.comps == null || !this.pawn.RaceProps.Humanlike)
				{
					return null;
				}
				for (int i = 0; i < this.comps.Count; i++)
				{
					HediffComp_PregnantHuman hediffComp_PregnantHuman = this.comps[i] as HediffComp_PregnantHuman;
					if (hediffComp_PregnantHuman != null)
					{
						return hediffComp_PregnantHuman.Attitude;
					}
				}
				return null;
			}
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x000C9014 File Offset: 0x000C7214
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.lastStage = this.CurStageIndex;
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x000C902C File Offset: 0x000C722C
		public override void TickInterval(int delta)
		{
			this.ageTicks += delta;
			if (this.CurStageIndex != this.lastStage)
			{
				this.NotifyPlayerOfTrimesterPassing();
				this.lastStage = this.CurStageIndex;
			}
			if ((!this.pawn.RaceProps.Humanlike || !Find.Storyteller.difficulty.babiesAreHealthy) && this.pawn.IsHashIntervalTick(1000, delta))
			{
				Need_Food food = this.pawn.needs.food;
				if (food != null && food.CurCategory == HungerCategory.Starving)
				{
					Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
					if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.1f && Rand.MTBEventOccurs(2f, 60000f, 1000f))
					{
						if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
						{
							string value = this.pawn.Name.Numerical ? this.pawn.LabelShort : (this.pawn.LabelShort + " (" + this.pawn.kindDef.label + ")");
							Messages.Message("MessageMiscarriedStarvation".Translate(value, this.pawn), this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
						}
						this.Miscarry();
						return;
					}
				}
				if (this.IsSeverelyWounded && Rand.MTBEventOccurs(2f, 60000f, 1000f))
				{
					if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						string value2 = this.pawn.Name.Numerical ? this.pawn.LabelShort : (this.pawn.LabelShort + " (" + this.pawn.kindDef.label + ")");
						Messages.Message("MessageMiscarriedPoorHealth".Translate(value2, this.pawn), this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					}
					this.Miscarry();
					return;
				}
			}
			float num = PawnUtility.BodyResourceGrowthSpeed(this.pawn) / (this.pawn.RaceProps.gestationPeriodDays * 60000f);
			this.GestationProgress += num * (float)delta;
			if (this.GestationProgress >= 1f)
			{
				if (!this.pawn.RaceProps.Humanlike)
				{
					if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						Messages.Message("MessageGaveBirth".Translate(this.pawn), this.pawn, MessageTypeDefOf.PositiveEvent, true);
					}
					Hediff_Pregnant.DoBirthSpawn(this.pawn, base.Father);
				}
				else
				{
					this.StartLabor();
				}
				this.pawn.health.RemoveHediff(this);
			}
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x000C9320 File Offset: 0x000C7520
		public void Miscarry()
		{
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.Miscarried, null, null);
						}
					}
				}
			}
			Pawn mother = base.Mother;
			Pawn father = base.Father;
			if (mother != this.pawn && mother != null)
			{
				Pawn_NeedsTracker needs2 = mother.needs;
				if (needs2 != null)
				{
					Need_Mood mood2 = needs2.mood;
					if (mood2 != null)
					{
						ThoughtHandler thoughts2 = mood2.thoughts;
						if (thoughts2 != null)
						{
							MemoryThoughtHandler memories2 = thoughts2.memories;
							if (memories2 != null)
							{
								memories2.TryGainMemory(ThoughtDefOf.PartnerMiscarried, this.pawn, null);
							}
						}
					}
				}
			}
			if (father != this.pawn && father != null)
			{
				Pawn_NeedsTracker needs3 = father.needs;
				if (needs3 != null)
				{
					Need_Mood mood3 = needs3.mood;
					if (mood3 != null)
					{
						ThoughtHandler thoughts3 = mood3.thoughts;
						if (thoughts3 != null)
						{
							MemoryThoughtHandler memories3 = thoughts3.memories;
							if (memories3 != null)
							{
								memories3.TryGainMemory(ThoughtDefOf.PartnerMiscarried, this.pawn, null);
							}
						}
					}
				}
			}
			this.pawn.health.RemoveHediff(this);
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x000C9420 File Offset: 0x000C7620
		public static void DoBirthSpawn(Pawn mother, Pawn father)
		{
			if (mother.RaceProps.Humanlike && !ModsConfig.BiotechActive)
			{
				return;
			}
			int num = (mother.RaceProps.litterSizeCurve != null) ? Mathf.RoundToInt(Rand.ByCurve(mother.RaceProps.litterSizeCurve)) : 1;
			if (num < 1)
			{
				num = 1;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, null, false, false, true, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false);
			Pawn pawn = null;
			for (int i = 0; i < num; i++)
			{
				pawn = PawnGenerator.GeneratePawn(request);
				if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, mother, null))
				{
					if (pawn.playerSettings != null && mother.playerSettings != null)
					{
						pawn.playerSettings.AreaRestrictionInPawnCurrentMap = mother.playerSettings.AreaRestrictionInPawnCurrentMap;
					}
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, mother);
						if (father != null)
						{
							pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
						}
					}
					if (mother.Spawned)
					{
						Lord lord = mother.GetLord();
						if (lord != null)
						{
							lord.AddPawn(pawn);
						}
					}
				}
				else
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				TaleRecorder.RecordTale(TaleDefOf.GaveBirth, new object[]
				{
					mother,
					pawn
				});
			}
			if (mother.Spawned)
			{
				FilthMaker.TryMakeFilth(mother.Position, mother.Map, ThingDefOf.Filth_AmnioticFluid, mother.LabelIndefinite(), 5, FilthSourceFlags.None);
				Pawn_CallTracker caller = mother.caller;
				if (caller != null)
				{
					caller.DoCall(false);
				}
				Pawn_CallTracker caller2 = pawn.caller;
				if (caller2 == null)
				{
					return;
				}
				caller2.DoCall(false);
			}
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x000C9620 File Offset: 0x000C7820
		public void StartLabor()
		{
			if (!ModLister.CheckBiotech("labor"))
			{
				return;
			}
			((Hediff_Labor)this.pawn.health.AddHediff(HediffDefOf.PregnancyLabor, null, null, null)).SetParents(base.Mother, base.Father, this.geneSet);
			Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.MorningSickness, false);
			if (firstHediffOfDef != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff firstHediffOfDef2 = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PregnancyMood, false);
			if (firstHediffOfDef2 != null)
			{
				this.pawn.health.RemoveHediff(firstHediffOfDef2);
			}
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Find.LetterStack.ReceiveLetter("LetterColonistPregnancyLaborLabel".Translate(this.pawn), "LetterColonistPregnancyLabor".Translate(this.pawn), LetterDefOf.NeutralEvent, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x000C972D File Offset: 0x000C792D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastStage, "lastStage", 0, false);
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x000C9748 File Offset: 0x000C7948
		private void NotifyPlayerOfTrimesterPassing()
		{
			if (this.pawn.RaceProps.Humanlike && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message(((this.lastStage == 0) ? "MessageColonistReaching2ndTrimesterPregnancy" : "MessageColonistReaching3rdTrimesterPregnancy").Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				if (this.lastStage == 1 && !Find.History.everThirdTrimesterPregnancy)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelThirdTrimester".Translate(this.pawn), "LetterTextThirdTrimester".Translate(this.pawn), LetterDefOf.PositiveEvent, this.pawn, null, null, null, null, 0, true);
					Find.History.everThirdTrimesterPregnancy = true;
				}
			}
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x000C982D File Offset: 0x000C7A2D
		public override void PostDebugAdd()
		{
			if (ModsConfig.BiotechActive && this.pawn.RaceProps.Humanlike)
			{
				base.SetParents(this.pawn, null, PregnancyUtility.GetInheritedGeneSet(null, this.pawn));
			}
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x000C9864 File Offset: 0x000C7A64
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.DebugString());
			stringBuilder.AppendLine("Gestation progress: " + this.GestationProgress.ToStringPercent());
			stringBuilder.AppendLine("Time left: " + ((int)((1f - this.GestationProgress) * this.pawn.RaceProps.gestationPeriodDays * 60000f)).ToStringTicksToPeriod(true, false, true, true, false));
			return stringBuilder.ToString();
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x000C98E3 File Offset: 0x000C7AE3
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Hediff_Pregnant.<GetGizmos>d__21 <GetGizmos>d__ = new Hediff_Pregnant.<GetGizmos>d__21(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0400199A RID: 6554
		private int lastStage;

		// Token: 0x0400199B RID: 6555
		private const int MiscarryCheckInterval = 1000;

		// Token: 0x0400199C RID: 6556
		private const float MTBMiscarryStarvingDays = 2f;

		// Token: 0x0400199D RID: 6557
		private const float MTBMiscarryWoundedDays = 2f;

		// Token: 0x0400199E RID: 6558
		private const float MalnutritionMinSeverityForMiscarry = 0.1f;
	}
}
