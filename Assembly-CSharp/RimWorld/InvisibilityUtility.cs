using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014B3 RID: 5299
	public static class InvisibilityUtility
	{
		// Token: 0x060081B5 RID: 33205 RVA: 0x002E475C File Offset: 0x002E295C
		public static bool IsPsychologicallyInvisible(this Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffComp_Invisibility hediffComp_Invisibility = hediffs[i].TryGetComp<HediffComp_Invisibility>();
				if (hediffComp_Invisibility != null && !hediffComp_Invisibility.PsychologicallyVisible)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060081B6 RID: 33206 RVA: 0x002E47A8 File Offset: 0x002E29A8
		public static bool IsHiddenFromPlayer(this Pawn pawn)
		{
			if (DebugSettings.showHiddenPawns)
			{
				return false;
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return false;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffComp_Invisibility hediffComp_Invisibility = hediffs[i].TryGetComp<HediffComp_Invisibility>();
				if (hediffComp_Invisibility != null && !hediffComp_Invisibility.Props.visibleToPlayer && !hediffComp_Invisibility.PsychologicallyVisible)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060081B7 RID: 33207 RVA: 0x002E4818 File Offset: 0x002E2A18
		public static float GetAlpha(Pawn pawn)
		{
			if (DebugSettings.showHiddenPawns)
			{
				return 1f;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			if (hediffs.NullOrEmpty<Hediff>())
			{
				return 1f;
			}
			foreach (Hediff hd in hediffs)
			{
				HediffComp_Invisibility hediffComp_Invisibility = hd.TryGetComp<HediffComp_Invisibility>();
				if (hediffComp_Invisibility != null)
				{
					return hediffComp_Invisibility.GetAlpha();
				}
			}
			return 1f;
		}

		// Token: 0x060081B8 RID: 33208 RVA: 0x002E48A4 File Offset: 0x002E2AA4
		public static HediffComp_Invisibility GetInvisibilityComp(this Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			if (hediffs.NullOrEmpty<Hediff>())
			{
				return null;
			}
			foreach (Hediff hd in hediffs)
			{
				HediffComp_Invisibility hediffComp_Invisibility = hd.TryGetComp<HediffComp_Invisibility>();
				if (hediffComp_Invisibility != null)
				{
					return hediffComp_Invisibility;
				}
			}
			return null;
		}
	}
}
