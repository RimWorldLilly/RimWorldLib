using System;
using System.Collections.Generic;
using LudeonTK;
using Verse;

namespace RimWorld
{
	// Token: 0x020013CB RID: 5067
	public static class AbilityUtility
	{
		// Token: 0x06007E0E RID: 32270 RVA: 0x002D1250 File Offset: 0x002CF450
		public static bool ValidateIsConscious(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (!targetPawn.health.capacities.CanBeAwake)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyUnconscious".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E0F RID: 32271 RVA: 0x002D128B File Offset: 0x002CF48B
		public static bool ValidateIsAwake(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (!targetPawn.Awake())
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyAsleep".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E10 RID: 32272 RVA: 0x002D12BC File Offset: 0x002CF4BC
		public static bool ValidateNoMentalState(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.InMentalState)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyToMentallyBroken".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E11 RID: 32273 RVA: 0x002D12F0 File Offset: 0x002CF4F0
		public static bool ValidateIsMaddened(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (!targetPawn.InMentalState || targetPawn.MentalStateDef != MentalStateDefOf.Manhunter)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage((targetPawn.health.hediffSet.HasHediff(HediffDefOf.Scaria, false) ? "AbilityCantApplyToScaria" : "AbilityCanOnlyApplyToManhunter").Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E12 RID: 32274 RVA: 0x002D1358 File Offset: 0x002CF558
		public static bool ValidateCanWalk(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.Downed)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyDowned".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E13 RID: 32275 RVA: 0x002D138C File Offset: 0x002CF58C
		public static bool ValidateHasMentalState(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (!targetPawn.InMentalState && !targetPawn.health.hediffSet.HasHediff(HediffDefOf.CatatonicBreakdown, false))
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCanOnlyApplyToMentallyBroken".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E14 RID: 32276 RVA: 0x002D13E0 File Offset: 0x002CF5E0
		public static bool ValidateHasResistance(Pawn targetPawn, bool showMessages, Ability ability)
		{
			Pawn_GuestTracker guest = targetPawn.guest;
			if (guest != null && guest.resistance <= 1E-45f)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyNoResistance".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E15 RID: 32277 RVA: 0x002D1432 File Offset: 0x002CF632
		public static bool ValidateNoInspiration(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.Inspiration != null)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityAlreadyInspired".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E16 RID: 32278 RVA: 0x002D1463 File Offset: 0x002CF663
		public static bool ValidateCanGetInspiration(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.mindState.inspirationHandler.GetRandomAvailableInspirationDef() == null)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantGetInspiredNow".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E17 RID: 32279 RVA: 0x002D149E File Offset: 0x002CF69E
		public static bool ValidateMustBeHuman(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.NonHumanlikeOrWildMan())
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage((targetPawn.IsWildMan() ? "AbilityMustBeHumanNonWild" : "AbilityMustBeHuman").Translate(), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E18 RID: 32280 RVA: 0x002D14D8 File Offset: 0x002CF6D8
		public static bool ValidateMustBeHumanOrWildMan(Pawn targetPawn, bool showMessage, Ability ability)
		{
			if (!targetPawn.RaceProps.Humanlike)
			{
				if (showMessage)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeHuman".Translate(), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E19 RID: 32281 RVA: 0x002D1508 File Offset: 0x002CF708
		public static bool ValidateMustNotBeBaby(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.DevelopmentalStage.Baby())
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityCantApplyOnBaby".Translate(), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E1A RID: 32282 RVA: 0x002D1538 File Offset: 0x002CF738
		public static bool ValidateMustBeAnimal(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (!targetPawn.IsAnimal)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeAnimal".Translate(), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E1B RID: 32283 RVA: 0x002D1564 File Offset: 0x002CF764
		public static bool ValidateNotSameIdeo(Pawn casterPawn, Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (casterPawn.Ideo != null && casterPawn.Ideo == targetPawn.Ideo)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeNotSameIdeo".Translate(targetPawn, casterPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E1C RID: 32284 RVA: 0x002D15B4 File Offset: 0x002CF7B4
		public static bool ValidateSameIdeo(Pawn casterPawn, Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (casterPawn.Ideo != targetPawn.Ideo)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeSameIdeo".Translate(targetPawn, casterPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E1D RID: 32285 RVA: 0x002D15F4 File Offset: 0x002CF7F4
		public static bool ValidateSickOrInjured(Pawn targetPawn, bool showMessage, Ability ability)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (Hediff hediff in targetPawn.health.hediffSet.hediffs)
			{
				flag |= (hediff.TryGetComp<HediffComp_Immunizable>() != null);
				bool flag3 = flag2;
				Hediff_Injury hediff_Injury = hediff as Hediff_Injury;
				flag2 = (flag3 | (hediff_Injury != null && !hediff_Injury.IsPermanent() && hediff_Injury.CanHealNaturally()));
			}
			if (!flag && !flag2)
			{
				if (showMessage)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeSickOrInjured".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E1E RID: 32286 RVA: 0x002D16AC File Offset: 0x002CF8AC
		public static bool ValidateNotGuilty(Pawn targetPawn, bool showMessages, Ability ability)
		{
			if (targetPawn.guilt != null && targetPawn.guilt.IsGuilty)
			{
				if (showMessages)
				{
					AbilityUtility.SendPostProcessedMessage("AbilityMustBeNotGuilty".Translate(targetPawn), targetPawn, ability);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007E1F RID: 32287 RVA: 0x002D16EC File Offset: 0x002CF8EC
		public static bool ValidateHasTendableWound(Pawn targetPawn, bool showMessages, Ability ability)
		{
			List<Hediff> hediffs = targetPawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if ((hediffs[i] is Hediff_Injury || hediffs[i] is Hediff_MissingPart) && hediffs[i].TendableNow(false))
				{
					return true;
				}
			}
			if (showMessages)
			{
				AbilityUtility.SendPostProcessedMessage("AbilityMustHaveTendableWound".Translate(targetPawn), targetPawn, ability);
			}
			return false;
		}

		// Token: 0x06007E20 RID: 32288 RVA: 0x002D1770 File Offset: 0x002CF970
		private static void SendPostProcessedMessage(string message, LookTargets targets, Ability ability)
		{
			if (ability != null)
			{
				message = "CannotUseAbility".Translate(ability.def.label) + ": " + message;
			}
			Messages.Message(message, targets, MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x06007E21 RID: 32289 RVA: 0x002D17BE File Offset: 0x002CF9BE
		public static void DoClamor(IntVec3 cell, float radius, Thing source, ClamorDef clamor)
		{
			if (clamor != null)
			{
				GenClamor.DoClamor(source, cell, radius, clamor);
			}
		}

		// Token: 0x06007E22 RID: 32290 RVA: 0x002D17CC File Offset: 0x002CF9CC
		public static bool IsIncendiary(this Ability ability)
		{
			return ability.def.ai_IsIncendiary;
		}

		// Token: 0x06007E23 RID: 32291 RVA: 0x002D17D9 File Offset: 0x002CF9D9
		public static Ability MakeAbility(AbilityDef def, Pawn pawn)
		{
			return Activator.CreateInstance(def.abilityClass, new object[]
			{
				pawn,
				def
			}) as Ability;
		}

		// Token: 0x06007E24 RID: 32292 RVA: 0x002D17F9 File Offset: 0x002CF9F9
		public static Ability MakeAbility(AbilityDef def, Pawn pawn, Precept sourcePrecept)
		{
			return Activator.CreateInstance(def.abilityClass, new object[]
			{
				pawn,
				sourcePrecept,
				def
			}) as Ability;
		}

		// Token: 0x06007E25 RID: 32293 RVA: 0x002D1820 File Offset: 0x002CFA20
		public static void DoTable_AbilityCosts()
		{
			List<TableDataGetter<AbilityDef>> list = new List<TableDataGetter<AbilityDef>>();
			list.Add(new TableDataGetter<AbilityDef>("name", (AbilityDef a) => a.LabelCap));
			list.Add(new TableDataGetter<AbilityDef>("level", (AbilityDef a) => a.level));
			list.Add(new TableDataGetter<AbilityDef>("heat cost", (AbilityDef a) => a.EntropyGain));
			list.Add(new TableDataGetter<AbilityDef>("psyfocus cost", delegate(AbilityDef a)
			{
				if (a.PsyfocusCostRange.Span > 1E-45f)
				{
					return (a.PsyfocusCostRange.min * 100f).ToString() + "-" + (a.PsyfocusCostRange.max * 100f).ToString() + "%";
				}
				return a.PsyfocusCostRange.max.ToStringPercent();
			}));
			list.Add(new TableDataGetter<AbilityDef>("max psyfocus recovery time days", delegate(AbilityDef a)
			{
				if (a.PsyfocusCostRange.Span <= 1E-45f)
				{
					return a.PsyfocusCostRange.min / StatDefOf.MeditationFocusGain.defaultBaseValue;
				}
				return (a.PsyfocusCostRange.min / StatDefOf.MeditationFocusGain.defaultBaseValue).ToString() + "-" + (a.PsyfocusCostRange.max / StatDefOf.MeditationFocusGain.defaultBaseValue).ToString();
			}));
			DebugTables.MakeTablesDialog<AbilityDef>(DefDatabase<AbilityDef>.AllDefsListForReading, list.ToArray());
		}
	}
}
