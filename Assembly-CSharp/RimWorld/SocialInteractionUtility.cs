using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D39 RID: 3385
	public static class SocialInteractionUtility
	{
		// Token: 0x06005EE3 RID: 24291 RVA: 0x0022AFF8 File Offset: 0x002291F8
		public static bool CanInitiateInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.interactions != null && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.Awake() && !pawn.IsBurning() && (!pawn.IsMutant || !pawn.mutant.Def.incapableOfSocialInteractions) && !pawn.IsInteractionBlocked(interactionDef, true, false);
		}

		// Token: 0x06005EE4 RID: 24292 RVA: 0x0022B068 File Offset: 0x00229268
		public static IntVec3 GetAdjacentInteractionCell(Pawn pawn, Thing entityThing, bool forced)
		{
			IntVec3 result;
			SocialInteractionUtility.TryGetAdjacentInteractionCell(pawn, entityThing, forced, out result);
			return result;
		}

		// Token: 0x06005EE5 RID: 24293 RVA: 0x0022B084 File Offset: 0x00229284
		public static bool TryGetAdjacentInteractionCell(Pawn pawn, Thing entityThing, bool forced, out IntVec3 cell)
		{
			if (entityThing is Pawn)
			{
				IntVec3 intVec;
				if (CellFinder.TryFindRandomCellNear(entityThing.Position, pawn.Map, 2, (IntVec3 x) => base.<TryGetAdjacentInteractionCell>g__CellIsValid|0(x, true), out intVec, -1) || CellFinder.TryFindRandomCellNear(entityThing.Position, pawn.Map, 2, (IntVec3 x) => base.<TryGetAdjacentInteractionCell>g__CellIsValid|0(x, false), out intVec, -1))
				{
					cell = intVec;
					return true;
				}
				cell = pawn.Position;
				return true;
			}
			else
			{
				IntVec3 intVec2;
				if ((from x in GenAdj.CellsAdjacentCardinal(entityThing)
				where base.<TryGetAdjacentInteractionCell>g__CellIsValid|0(x, true)
				select x).TryRandomElement(out intVec2) || (from x in GenAdj.CellsAdjacentCardinal(entityThing)
				where base.<TryGetAdjacentInteractionCell>g__CellIsValid|0(x, false)
				select x).TryRandomElement(out intVec2))
				{
					cell = intVec2;
					return true;
				}
				cell = pawn.Position;
				return false;
			}
		}

		// Token: 0x06005EE6 RID: 24294 RVA: 0x0022B174 File Offset: 0x00229374
		public static bool CanReceiveInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.Awake() && !pawn.IsBurning() && (!pawn.IsMutant || !pawn.mutant.Def.incapableOfSocialInteractions) && !pawn.IsInteractionBlocked(interactionDef, false, false);
		}

		// Token: 0x06005EE7 RID: 24295 RVA: 0x0022B1B4 File Offset: 0x002293B4
		public static bool CanInitiateRandomInteraction(Pawn p)
		{
			return SocialInteractionUtility.CanInitiateInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState && !p.IsInteractionBlocked(null, true, true) && p.Faction != null && p.ageTracker.CurLifeStage.canInitiateSocialInteraction && !p.Inhumanized();
		}

		// Token: 0x06005EE8 RID: 24296 RVA: 0x0022B21F File Offset: 0x0022941F
		public static bool CanReceiveRandomInteraction(Pawn p)
		{
			return SocialInteractionUtility.CanReceiveInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState;
		}

		// Token: 0x06005EE9 RID: 24297 RVA: 0x0022B24C File Offset: 0x0022944C
		public static bool IsGoodPositionForInteraction(Pawn p, Pawn recipient)
		{
			return SocialInteractionUtility.IsGoodPositionForInteraction(p.Position, recipient.Position, p.Map);
		}

		// Token: 0x06005EEA RID: 24298 RVA: 0x0022B265 File Offset: 0x00229465
		public static bool IsGoodPositionForInteraction(IntVec3 cell, IntVec3 recipientCell, Map map)
		{
			return cell.InHorDistOf(recipientCell, 6f) && GenSight.LineOfSight(cell, recipientCell, map, true, null, 0, 0);
		}

		// Token: 0x06005EEB RID: 24299 RVA: 0x0022B284 File Offset: 0x00229484
		public static IntVec3 BestInteractableCell(Pawn actor, Pawn targetPawn)
		{
			IntVec3 intVec = IntVec3.Invalid;
			int num = 0;
			while (num < 9 && (num != 8 || !intVec.IsValid))
			{
				IntVec3 intVec2 = targetPawn.Position + GenAdj.AdjacentCellsAndInside[num];
				if (intVec2.InBounds(actor.Map) && intVec2.Walkable(actor.Map) && intVec2 != actor.Position && SocialInteractionUtility.IsGoodPositionForInteraction(intVec2, targetPawn.Position, actor.Map) && actor.CanReach(intVec2, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn) && (!intVec.IsValid || actor.Position.DistanceToSquared(intVec2) < actor.Position.DistanceToSquared(intVec)))
				{
					intVec = intVec2;
				}
				num++;
			}
			return intVec;
		}

		// Token: 0x06005EEC RID: 24300 RVA: 0x0022B348 File Offset: 0x00229548
		public static bool HasAnyVerbForSocialFight(Pawn p)
		{
			if (p.Dead)
			{
				return false;
			}
			List<Verb> allVerbs = p.verbTracker.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].IsMeleeAttack && allVerbs[i].IsStillUsableBy(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005EED RID: 24301 RVA: 0x0022B39C File Offset: 0x0022959C
		public static bool TryGetRandomVerbForSocialFight(Pawn p, out Verb verb)
		{
			if (p.Dead)
			{
				verb = null;
				return false;
			}
			return (from x in p.verbTracker.AllVerbs
			where x.IsMeleeAttack && x.IsStillUsableBy(p)
			select x).TryRandomElementByWeight(delegate(Verb x)
			{
				float num = x.verbProps.AdjustedMeleeDamageAmount(x, p);
				Tool tool = x.tool;
				return num * ((tool != null) ? tool.chanceFactor : 1f);
			}, out verb);
		}

		// Token: 0x06005EEE RID: 24302 RVA: 0x0022B3FC File Offset: 0x002295FC
		public static void ImitateSocialInteractionWithManyPawns(Pawn initiator, List<Pawn> targets, InteractionDef intDef)
		{
			List<Pawn> list = targets.Except(initiator).ToList<Pawn>();
			if (targets.NullOrEmpty<Pawn>())
			{
				Log.Error(((initiator != null) ? initiator.ToString() : null) + " tried to do interaction " + ((intDef != null) ? intDef.ToString() : null) + " with no targets. ");
				return;
			}
			if (intDef.initiatorXpGainSkill != null)
			{
				initiator.skills.Learn(intDef.initiatorXpGainSkill, (float)intDef.initiatorXpGainAmount, false, false);
			}
			foreach (Pawn pawn in list)
			{
				if (initiator != pawn && initiator.interactions.CanInteractNowWith(pawn, intDef))
				{
					if (intDef.recipientThought != null && pawn.needs.mood != null)
					{
						Pawn_InteractionsTracker.AddInteractionThought(pawn, initiator, intDef.recipientThought);
					}
					if (intDef.recipientXpGainSkill != null && pawn.RaceProps.Humanlike)
					{
						pawn.skills.Learn(intDef.recipientXpGainSkill, (float)intDef.recipientXpGainAmount, false, false);
					}
				}
			}
			LogEntry entry;
			if (list.Count > 0)
			{
				entry = new PlayLogEntry_InteractionWithMany(intDef, initiator, list, null);
				MoteMaker.MakeInteractionBubble(initiator, list.RandomElement<Pawn>(), intDef.interactionMote, intDef.GetSymbol(initiator.Faction, initiator.Ideo), intDef.GetSymbolColor(initiator.Faction));
			}
			else
			{
				entry = new PlayLogEntry_InteractionSinglePawn(intDef, initiator, null);
				MoteMaker.MakeInteractionBubble(initiator, null, intDef.interactionMote, intDef.GetSymbol(null, null), intDef.GetSymbolColor(null));
			}
			Find.PlayLog.Add(entry);
		}

		// Token: 0x06005EEF RID: 24303 RVA: 0x0022B584 File Offset: 0x00229784
		public static void ImitateInteractionWithNoPawn(Pawn initiator, InteractionDef intDef)
		{
			MoteMaker.MakeInteractionBubble(initiator, null, intDef.interactionMote, intDef.GetSymbol(null, null), intDef.GetSymbolColor(null));
			Find.PlayLog.Add(new PlayLogEntry_InteractionSinglePawn(intDef, initiator, null));
		}

		// Token: 0x040035B8 RID: 13752
		public const float MaxInteractRange = 6f;
	}
}
