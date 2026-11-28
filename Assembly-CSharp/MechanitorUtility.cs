using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

// Token: 0x02000016 RID: 22
public static class MechanitorUtility
{
	// Token: 0x060000A7 RID: 167 RVA: 0x00007478 File Offset: 0x00005678
	public static bool IsMechanitor(Pawn pawn)
	{
		return MechanitorUtility.ShouldBeMechanitor(pawn) && pawn.mechanitor != null;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x0000748D File Offset: 0x0000568D
	public static bool ShouldBeMechanitor(Pawn pawn)
	{
		return ModsConfig.BiotechActive && pawn.Faction.IsPlayerSafe() && pawn.health.hediffSet.HasHediff(HediffDefOf.MechlinkImplant, false);
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x000074BB File Offset: 0x000056BB
	public static bool IsPlayerOverseerSubject(Pawn pawn)
	{
		return pawn.Faction.IsPlayerSafe() && pawn.OverseerSubject != null;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x000074D5 File Offset: 0x000056D5
	public static MechWorkModeDef GetMechWorkMode(this Pawn pawn)
	{
		MechanitorControlGroup mechControlGroup = pawn.GetMechControlGroup();
		if (mechControlGroup == null)
		{
			return null;
		}
		return mechControlGroup.WorkMode;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x000074E8 File Offset: 0x000056E8
	public static MechanitorControlGroup GetMechControlGroup(this Pawn pawn)
	{
		Pawn overseer = pawn.GetOverseer();
		if (overseer == null)
		{
			return null;
		}
		Pawn_MechanitorTracker mechanitor = overseer.mechanitor;
		if (mechanitor == null)
		{
			return null;
		}
		return mechanitor.GetControlGroup(pawn);
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00007507 File Offset: 0x00005707
	public static Pawn GetOverseer(this Pawn pawn)
	{
		if (!ModsConfig.BiotechActive)
		{
			return null;
		}
		Pawn_RelationsTracker relations = pawn.relations;
		if (relations == null)
		{
			return null;
		}
		return relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer, null);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00007529 File Offset: 0x00005729
	public static bool IsGestating(this Pawn pawn)
	{
		return pawn.ParentHolder is Building_MechGestator;
	}

	// Token: 0x060000AE RID: 174 RVA: 0x0000753C File Offset: 0x0000573C
	public static AcceptanceReport CanControlMech(Pawn pawn, Pawn mech)
	{
		if (pawn.mechanitor == null || !mech.IsColonyMech || mech.Downed || mech.Dead || mech.IsAttacking())
		{
			return false;
		}
		if (!MechanitorUtility.EverControllable(mech))
		{
			return "CannotControlMechNeverControllable".Translate();
		}
		if (mech.GetOverseer() == pawn)
		{
			return "CannotControlMechAlreadyControlled".Translate(pawn.LabelShort);
		}
		float num = (float)(pawn.mechanitor.TotalBandwidth - pawn.mechanitor.UsedBandwidth);
		float statValue = mech.GetStatValue(StatDefOf.BandwidthCost, true, -1);
		if (num < statValue)
		{
			return "CannotControlMechNotEnoughBandwidth".Translate();
		}
		return true;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x000075F3 File Offset: 0x000057F3
	public static bool EverControllable(Pawn mech)
	{
		return mech.OverseerSubject != null;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00007600 File Offset: 0x00005800
	public static bool IsColonyMechRequiringMechanitor(this Pawn mech)
	{
		if (!mech.IsColonyMech)
		{
			return false;
		}
		CompOverseerSubject overseerSubject = mech.OverseerSubject;
		return overseerSubject != null && overseerSubject.State != OverseerSubjectState.Overseen;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00007630 File Offset: 0x00005830
	public static void Notify_MechlinkQuestRewardAvailable(Quest quest, LookTargets lookTargets = null)
	{
		if (!Find.History.mechlinkEverAvailable)
		{
			List<Pawn> allMaps_FreeColonists = PawnsFinder.AllMaps_FreeColonists;
			bool flag = false;
			for (int i = 0; i < allMaps_FreeColonists.Count; i++)
			{
				if (MechanitorUtility.IsMechanitor(allMaps_FreeColonists[i]))
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelMechlinkAvailable".Translate(), "LetterMechlinkAvailable".Translate(quest.name), LetterDefOf.PositiveEvent, lookTargets, null, quest, null, null, 0, true);
			}
			Find.History.Notify_MechlinkAvailable();
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x000076B8 File Offset: 0x000058B8
	public static bool TryGetBandwidthLostFromDroppingThing(Pawn pawn, Thing thing, out int bandwidthLost)
	{
		bandwidthLost = -1;
		if (thing.def.equippedStatOffsets.NullOrEmpty<StatModifier>())
		{
			return false;
		}
		bandwidthLost = Mathf.RoundToInt(thing.def.equippedStatOffsets.GetStatOffsetFromList(StatDefOf.MechBandwidth));
		if (bandwidthLost <= 0)
		{
			return false;
		}
		int num = pawn.mechanitor.TotalBandwidth - pawn.mechanitor.UsedBandwidth;
		return bandwidthLost > num;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x0000771F File Offset: 0x0000591F
	public static bool TryGetMechsLostFromDroppingThing(Pawn pawn, Thing thing, out List<Pawn> lostMechs, out int bandwidthLost)
	{
		if (!MechanitorUtility.TryGetBandwidthLostFromDroppingThing(pawn, thing, out bandwidthLost))
		{
			lostMechs = null;
			return false;
		}
		return MechanitorUtility.TryGetMechsLostFromBandwidthReduction(pawn, bandwidthLost, out lostMechs);
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x0000773C File Offset: 0x0000593C
	public static bool TryGetMechsLostFromBandwidthReduction(Pawn pawn, int lostBandwidth, out List<Pawn> lostMechs)
	{
		lostMechs = new List<Pawn>();
		MechanitorUtility.tmpMechs.Clear();
		MechanitorUtility.GetMechsInAssignedOrder(pawn, ref MechanitorUtility.tmpMechs);
		int num = pawn.mechanitor.TotalBandwidth - lostBandwidth;
		int num2 = pawn.mechanitor.UsedBandwidth;
		int num3 = 0;
		while (num3 < MechanitorUtility.tmpMechs.Count && num2 > num)
		{
			int num4 = Mathf.RoundToInt(MechanitorUtility.tmpMechs[num3].GetStatValue(StatDefOf.BandwidthCost, true, -1));
			num2 -= num4;
			lostMechs.Add(MechanitorUtility.tmpMechs[num3]);
			num3++;
		}
		return MechanitorUtility.tmpMechs.Count > 0;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x000077D8 File Offset: 0x000059D8
	public static bool TryConfirmBandwidthLossFromDroppingThing(Pawn pawn, Thing thing, Action confirmAct)
	{
		List<Pawn> source;
		int num;
		if (MechanitorUtility.IsMechanitor(pawn) && pawn.apparel.Wearing(thing) && MechanitorUtility.TryGetMechsLostFromDroppingThing(pawn, thing, out source, out num))
		{
			int totalBandwidth = pawn.mechanitor.TotalBandwidth;
			string bandwidthLower = "BandwidthLower".Translate().ToString();
			Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation("DropThingBandwidthApparel".Translate(thing.LabelShort, pawn, totalBandwidth, totalBandwidth - num) + (":\n\n" + (from m in source
			select string.Concat(new string[]
			{
				m.LabelShortCap,
				" (",
				m.GetStatValue(StatDefOf.BandwidthCost, true, -1).ToString(),
				" ",
				bandwidthLower,
				")"
			})).ToLineList("- ", false)), confirmAct, false, null, WindowLayer.Dialog);
			dialog_MessageBox.buttonBText = "CancelButton".Translate();
			Find.WindowStack.Add(dialog_MessageBox);
			return true;
		}
		return false;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x000078C8 File Offset: 0x00005AC8
	public static void GetMechsInAssignedOrder(Pawn pawn, ref List<Pawn> mechs)
	{
		MechanitorUtility.tmpAssignedMechs.Clear();
		for (int i = 0; i < pawn.mechanitor.controlGroups.Count; i++)
		{
			MechanitorUtility.tmpAssignedMechs.AddRange(pawn.mechanitor.controlGroups[i].AssignedMechs);
		}
		MechanitorUtility.tmpAssignedMechs.SortByDescending((AssignedMech m) => m.tickAssigned);
		for (int j = 0; j < MechanitorUtility.tmpAssignedMechs.Count; j++)
		{
			mechs.Add(MechanitorUtility.tmpAssignedMechs[j].pawn);
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00007970 File Offset: 0x00005B70
	public static void ForceDisconnectMechFromOverseer(Pawn mech)
	{
		Pawn overseer = mech.GetOverseer();
		if (overseer != null && overseer.relations.TryRemoveDirectRelation(PawnRelationDefOf.Overseer, mech))
		{
			mech.OverseerSubject.Notify_DisconnectedFromOverseer();
			SoundDefOf.DisconnectedMech.PlayOneShot(new TargetInfo(overseer));
			Messages.Message("MessageMechanitorDisconnectedFromMech".Translate(overseer, mech), new LookTargets(new Pawn[]
			{
				mech,
				overseer
			}), MessageTypeDefOf.NeutralEvent, true);
		}
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x000079F4 File Offset: 0x00005BF4
	public static string GetMechGestationJobString(JobDriver_DoBill job, Pawn mechanitor, Bill_Mech bill)
	{
		switch (bill.State)
		{
		case FormingState.Gathering:
			if (job.AnyIngredientsQueued)
			{
				return "LoadingMechGestator".Translate() + ".";
			}
			if (job.AnyIngredientsQueued)
			{
				goto IL_7D;
			}
			break;
		case FormingState.Preparing:
		case FormingState.Forming:
			break;
		case FormingState.Formed:
			return "InitMechBirth".Translate() + ".";
		default:
			goto IL_7D;
		}
		return "InitMechGestationCycle".Translate() + ".";
		IL_7D:
		Log.Error("Unknown mech gestation job state.");
		return null;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00007A89 File Offset: 0x00005C89
	public static IEnumerable<Gizmo> GetMechGizmos(Pawn mech)
	{
		MechanitorUtility.<GetMechGizmos>d__21 <GetMechGizmos>d__ = new MechanitorUtility.<GetMechGizmos>d__21(-2);
		<GetMechGizmos>d__.<>3__mech = mech;
		return <GetMechGizmos>d__;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00007A9C File Offset: 0x00005C9C
	public static bool AnyMechanitorInPlayerFaction()
	{
		using (List<Pawn>.Enumerator enumerator = PawnsFinder.AllMaps_FreeColonists.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (MechanitorUtility.IsMechanitor(enumerator.Current))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00007AF4 File Offset: 0x00005CF4
	public static IEnumerable<Pawn> MechsInPlayerFaction()
	{
		return new MechanitorUtility.<MechsInPlayerFaction>d__23(-2);
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00007B00 File Offset: 0x00005D00
	public static bool AnyMechlinkInMap()
	{
		List<Map> maps = Find.Maps;
		for (int i = 0; i < maps.Count; i++)
		{
			if (maps[i].listerThings.ThingsOfDef(ThingDefOf.Mechlink).Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00007B48 File Offset: 0x00005D48
	public static List<ThingDefCountClass> IngredientsFromDisassembly(ThingDef mech)
	{
		MechanitorUtility.tmpIngredients.Clear();
		Predicate<ThingDefCountClass> <>9__0;
		foreach (RecipeDef recipeDef in DefDatabase<RecipeDef>.AllDefs)
		{
			if (!recipeDef.products.NullOrEmpty<ThingDefCountClass>())
			{
				List<ThingDefCountClass> products = recipeDef.products;
				Predicate<ThingDefCountClass> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDefCountClass x) => x.thingDef == mech));
				}
				if (products.Any(predicate))
				{
					for (int i = 0; i < recipeDef.ingredients.Count; i++)
					{
						ThingDef thingDef = recipeDef.ingredients[i].filter.AllowedThingDefs.FirstOrDefault<ThingDef>();
						int count = Mathf.Max(1, Mathf.RoundToInt(recipeDef.ingredients[i].GetBaseCount() * 0.4f));
						MechanitorUtility.tmpIngredients.Add(new ThingDefCountClass(thingDef, count));
					}
					break;
				}
			}
		}
		return MechanitorUtility.tmpIngredients;
	}

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x060000BE RID: 190 RVA: 0x00007C60 File Offset: 0x00005E60
	public static List<ThingDef> MechRechargers
	{
		get
		{
			if (MechanitorUtility.cachedRechargers == null)
			{
				MechanitorUtility.cachedRechargers = (from d in DefDatabase<ThingDef>.AllDefs
				where d.IsMechRecharger
				select d).ToList<ThingDef>();
			}
			return MechanitorUtility.cachedRechargers;
		}
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00007CAC File Offset: 0x00005EAC
	public static ThingDef RechargerForMech(ThingDef mech)
	{
		foreach (ThingDef thingDef in MechanitorUtility.MechRechargers)
		{
			if (Building_MechCharger.IsCompatibleWithCharger(thingDef, mech))
			{
				return thingDef;
			}
		}
		return null;
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x060000C0 RID: 192 RVA: 0x00007D08 File Offset: 0x00005F08
	public static List<RecipeDef> MechRecipes
	{
		get
		{
			if (MechanitorUtility.cachedMechRecipes == null)
			{
				MechanitorUtility.cachedMechRecipes = DefDatabase<RecipeDef>.AllDefs.Where(delegate(RecipeDef r)
				{
					ThingDef producedThingDef = r.ProducedThingDef;
					if (producedThingDef == null)
					{
						return false;
					}
					RaceProperties race = producedThingDef.race;
					return ((race != null) ? new bool?(race.IsMechanoid) : null).GetValueOrDefault();
				}).ToList<RecipeDef>();
			}
			return MechanitorUtility.cachedMechRecipes;
		}
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00007D54 File Offset: 0x00005F54
	public static void ClearCache()
	{
		MechanitorUtility.cachedMechRecipes = null;
		MechanitorUtility.cachedRechargers = null;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00007D64 File Offset: 0x00005F64
	public static bool InMechanitorCommandRange(Pawn mech, LocalTargetInfo target)
	{
		Pawn overseer = mech.GetOverseer();
		if (overseer != null)
		{
			if (mech.MapHeld != overseer.MapHeld)
			{
				return false;
			}
			if (overseer.mechanitor.CanCommandTo(target))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00007D9C File Offset: 0x00005F9C
	public static bool AnyPlayerMechCanDoWork(WorkTypeDef workType, int skillRequired, out Pawn pawn)
	{
		if (!ModsConfig.BiotechActive)
		{
			pawn = null;
			return false;
		}
		List<Pawn> list = Find.CurrentMap.mapPawns.PawnsInFaction(Faction.OfPlayer);
		for (int i = 0; i < list.Count; i++)
		{
			Pawn pawn2 = list[i];
			if (pawn2.IsColonyMech && pawn2.GetOverseer() != null && pawn2.RaceProps.mechEnabledWorkTypes.Contains(workType) && pawn2.RaceProps.mechFixedSkillLevel >= skillRequired)
			{
				pawn = pawn2;
				return true;
			}
		}
		pawn = null;
		return false;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00007E20 File Offset: 0x00006020
	public static AcceptanceReport CanDraftMech(Pawn mech)
	{
		if (mech.IsColonyMech)
		{
			if (mech.needs.energy != null && mech.needs.energy.IsLowEnergySelfShutdown)
			{
				return "IsLowEnergySelfShutdown".Translate(mech.Named("PAWN"));
			}
			Pawn overseer = mech.GetOverseer();
			if (overseer != null)
			{
				AcceptanceReport canControlMechs = overseer.mechanitor.CanControlMechs;
				if (!canControlMechs)
				{
					return canControlMechs;
				}
				if (!overseer.mechanitor.ControlledPawns.Contains(mech))
				{
					return "MechControllerInsufficientBandwidth".Translate(overseer.Named("PAWN"));
				}
				return true;
			}
		}
		return false;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00007ED0 File Offset: 0x000060D0
	public static void Notify_PawnGotoLeftMap(Pawn pawn, Map map)
	{
		if (pawn.mechanitor == null || !pawn.IsColonist || map == null)
		{
			return;
		}
		List<Pawn> overseenPawns = pawn.mechanitor.OverseenPawns;
		MechanitorUtility.tmpLeftBehind.Clear();
		for (int i = 0; i < overseenPawns.Count; i++)
		{
			if (overseenPawns[i].Map == map && overseenPawns[i].Spawned && !overseenPawns[i].Downed && overseenPawns[i].GetMechWorkMode() != MechWorkModeDefOf.Escort)
			{
				MechanitorUtility.tmpLeftBehind.Add(overseenPawns[i]);
			}
		}
		if (MechanitorUtility.tmpLeftBehind.Count <= 0)
		{
			return;
		}
		Dialog_MessageBox window = new Dialog_MessageBox("MechanitorLeftMapWithoutMechs".Translate(pawn.Named("PAWN"), (from m in MechanitorUtility.tmpLeftBehind
		select m.LabelShortCap).ToLineList(" - ", false).Named("MECHS")), "Confirm".Translate(), delegate()
		{
			for (int j = 0; j < MechanitorUtility.tmpLeftBehind.Count; j++)
			{
				MechanitorUtility.tmpLeftBehind[j].GetMechControlGroup().SetWorkMode(MechWorkModeDefOf.Escort);
			}
		}, "Cancel".Translate(), null, null, false, null, null, WindowLayer.Dialog);
		Find.WindowStack.Add(window);
	}

	// Token: 0x0400002A RID: 42
	private static CachedTexture SelectOverseerIcon = new CachedTexture("UI/Icons/SelectOverseer");

	// Token: 0x0400002B RID: 43
	private static List<Pawn> tmpMechs = new List<Pawn>();

	// Token: 0x0400002C RID: 44
	private static List<AssignedMech> tmpAssignedMechs = new List<AssignedMech>();

	// Token: 0x0400002D RID: 45
	private static List<ThingDefCountClass> tmpIngredients = new List<ThingDefCountClass>();

	// Token: 0x0400002E RID: 46
	private static List<ThingDef> cachedRechargers;

	// Token: 0x0400002F RID: 47
	private static List<RecipeDef> cachedMechRecipes;

	// Token: 0x04000030 RID: 48
	private static readonly List<Pawn> tmpLeftBehind = new List<Pawn>();
}
