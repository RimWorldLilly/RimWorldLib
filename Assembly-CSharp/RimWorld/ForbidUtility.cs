using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001898 RID: 6296
	public static class ForbidUtility
	{
		// Token: 0x06009DBB RID: 40379 RVA: 0x0038AAA4 File Offset: 0x00388CA4
		public static void TrySetForbidden(this Thing t, bool value)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			CompForbiddable compForbiddable;
			if (thingWithComps != null && thingWithComps.TryGetComp(out compForbiddable))
			{
				compForbiddable.Forbidden = value;
			}
		}

		// Token: 0x06009DBC RID: 40380 RVA: 0x0038AACC File Offset: 0x00388CCC
		public static void SetForbidden(this Thing t, bool value, bool warnOnFail = true)
		{
			if (t == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on null Thing.");
				}
				return;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on non-ThingWithComps Thing " + ((t != null) ? t.ToString() : null));
				}
				return;
			}
			CompForbiddable comp = thingWithComps.GetComp<CompForbiddable>();
			if (comp == null)
			{
				if (warnOnFail)
				{
					Log.Error("Tried to SetForbidden on non-Forbiddable Thing " + ((t != null) ? t.ToString() : null));
				}
				return;
			}
			comp.Forbidden = value;
		}

		// Token: 0x06009DBD RID: 40381 RVA: 0x0038AB48 File Offset: 0x00388D48
		public static void SetForbiddenIfOutsideHomeArea(this Thing t)
		{
			if (!t.Spawned)
			{
				Log.Error("SetForbiddenIfOutsideHomeArea unspawned thing " + ((t != null) ? t.ToString() : null));
			}
			if (t.Position.InBounds(t.Map) && !t.Map.areaManager.Home[t.Position])
			{
				t.SetForbidden(true, false);
			}
		}

		// Token: 0x06009DBE RID: 40382 RVA: 0x0038ABB4 File Offset: 0x00388DB4
		public static bool CaresAboutForbidden(Pawn pawn, bool cellTarget, bool bypassDraftedCheck = false)
		{
			return (pawn.HostFaction == null || (pawn.HostFaction == Faction.OfPlayer && pawn.Spawned && !pawn.Map.IsPlayerHome && (pawn.GetRoom(RegionType.Set_All) == null || !pawn.GetRoom(RegionType.Set_All).IsPrisonCell) && (!pawn.IsPrisoner || pawn.guest.PrisonerIsSecure))) && (bypassDraftedCheck || !pawn.Drafted) && !pawn.InMentalState && !SlaveRebellionUtility.IsRebelling(pawn) && (!cellTarget || !ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(pawn)) && !pawn.IsColonyMechRequiringMechanitor() && (!ModsConfig.AnomalyActive || pawn.kindDef != PawnKindDefOf.Revenant);
		}

		// Token: 0x06009DBF RID: 40383 RVA: 0x0038AC70 File Offset: 0x00388E70
		public static bool InAllowedArea(this IntVec3 c, Pawn forPawn)
		{
			if (forPawn.playerSettings != null)
			{
				Area effectiveAreaRestrictionInPawnCurrentMap = forPawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (effectiveAreaRestrictionInPawnCurrentMap != null && effectiveAreaRestrictionInPawnCurrentMap.TrueCount > 0 && !effectiveAreaRestrictionInPawnCurrentMap[c])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009DC0 RID: 40384 RVA: 0x0038ACA9 File Offset: 0x00388EA9
		public static bool IsForbiddenHeld(this Thing t, Pawn pawn)
		{
			return t.SpawnedParentOrMe.IsForbidden(pawn);
		}

		// Token: 0x06009DC1 RID: 40385 RVA: 0x0038ACB8 File Offset: 0x00388EB8
		public static bool IsForbidden(this Thing t, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, false, false))
			{
				return false;
			}
			if ((t.Spawned || t.SpawnedParentOrMe != pawn) && t.PositionHeld.IsForbidden(pawn))
			{
				return true;
			}
			if (t.IsForbidden(pawn.Faction) || t.IsForbidden(pawn.HostFaction))
			{
				return true;
			}
			Lord lord = pawn.GetLord();
			if (lord != null && lord.extraForbiddenThings.Contains(t))
			{
				return true;
			}
			foreach (Lord lord2 in pawn.MapHeld.lordManager.lords)
			{
				LordToil_Ritual lordToil_Ritual = lord2.CurLordToil as LordToil_Ritual;
				if (lordToil_Ritual != null && lordToil_Ritual.ReservedThings.Contains(t) && lord2 != lord)
				{
					return true;
				}
				LordToil_PsychicRitual lordToil_PsychicRitual = lord2.CurLordToil as LordToil_PsychicRitual;
				if (lordToil_PsychicRitual != null)
				{
					PsychicRitualDef_InvocationCircle psychicRitualDef_InvocationCircle = lordToil_PsychicRitual.RitualData.psychicRitual.def as PsychicRitualDef_InvocationCircle;
					if (psychicRitualDef_InvocationCircle != null && psychicRitualDef_InvocationCircle.TargetRole != null && lordToil_PsychicRitual.RitualData.psychicRitual.assignments.FirstAssignedPawn(psychicRitualDef_InvocationCircle.TargetRole) == t && !(lordToil_PsychicRitual.RitualData.CurPsychicRitualToil is PsychicRitualToil_TargetCleanup) && lord2 != lord)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06009DC2 RID: 40386 RVA: 0x0038AE20 File Offset: 0x00389020
		public static bool IsForbiddenToPass(this Building_Door t, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, false, true) && t.IsForbidden(pawn.Faction);
		}

		// Token: 0x06009DC3 RID: 40387 RVA: 0x0038AE40 File Offset: 0x00389040
		public static bool IsForbidden(this IntVec3 c, Pawn pawn)
		{
			return ForbidUtility.CaresAboutForbidden(pawn, true, false) && (!c.InAllowedArea(pawn) || (pawn.mindState.maxDistToSquadFlag > 0f && !c.InHorDistOf(pawn.DutyLocation(), pawn.mindState.maxDistToSquadFlag)));
		}

		// Token: 0x06009DC4 RID: 40388 RVA: 0x0038AE94 File Offset: 0x00389094
		public static bool IsForbiddenEntirely(this Region r, Pawn pawn)
		{
			if (!ForbidUtility.CaresAboutForbidden(pawn, true, false))
			{
				return false;
			}
			if (pawn.playerSettings != null)
			{
				Area effectiveAreaRestrictionInPawnCurrentMap = pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (effectiveAreaRestrictionInPawnCurrentMap != null && effectiveAreaRestrictionInPawnCurrentMap.TrueCount > 0 && effectiveAreaRestrictionInPawnCurrentMap.Map == r.Map && r.OverlapWith(effectiveAreaRestrictionInPawnCurrentMap) == AreaOverlap.None)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009DC5 RID: 40389 RVA: 0x0038AEE8 File Offset: 0x003890E8
		public static bool IsForbidden(this Thing t, Faction faction)
		{
			if (faction == null)
			{
				return false;
			}
			if (faction != Faction.OfPlayer)
			{
				return false;
			}
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				return false;
			}
			CompForbiddable compForbiddable = thingWithComps.compForbiddable;
			return compForbiddable != null && compForbiddable.Forbidden;
		}
	}
}
