using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001559 RID: 5465
	public static class SlaveRebellionUtility
	{
		// Token: 0x060085A3 RID: 34211 RVA: 0x00303980 File Offset: 0x00301B80
		public static float InitiateSlaveRebellionMtbDays(Pawn pawn)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame >= SlaveRebellionUtility.pawnMtbCacheNextWipeTick)
			{
				SlaveRebellionUtility.cachedSlaveRebellionMTB.Clear();
				SlaveRebellionUtility.pawnMtbCacheNextWipeTick = ticksGame + 60000;
			}
			if (!pawn.IsSlave)
			{
				return -1f;
			}
			CachedRebellionMtb cachedRebellionMtb = SlaveRebellionUtility.cachedSlaveRebellionMTB.TryGetValue(pawn, null);
			if (cachedRebellionMtb != null && cachedRebellionMtb.gameTick == ticksGame)
			{
				return cachedRebellionMtb.mtb;
			}
			if (cachedRebellionMtb == null)
			{
				cachedRebellionMtb = new CachedRebellionMtb();
				SlaveRebellionUtility.cachedSlaveRebellionMTB[pawn] = cachedRebellionMtb;
			}
			cachedRebellionMtb.gameTick = ticksGame;
			cachedRebellionMtb.mtb = SlaveRebellionUtility.InitiateSlaveRebellionMtbDaysHelper(pawn);
			return cachedRebellionMtb.mtb;
		}

		// Token: 0x060085A4 RID: 34212 RVA: 0x00303A14 File Offset: 0x00301C14
		private static float InitiateSlaveRebellionMtbDaysHelper(Pawn pawn)
		{
			if (!SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn))
			{
				return -1f;
			}
			Need_Suppression need_Suppression;
			if (!pawn.needs.TryGetNeed<Need_Suppression>(out need_Suppression))
			{
				return -1f;
			}
			float num = 45f;
			num /= SlaveRebellionUtility.MovingCapacityFactorCurve.Evaluate(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
			num /= SlaveRebellionUtility.SuppresionRebellionFactorCurve.Evaluate(need_Suppression.CurLevelPercentage);
			num /= SlaveRebellionUtility.SlaveCountFactorCurve.Evaluate((float)pawn.Map.mapPawns.SlavesOfColonySpawned.Count);
			if (pawn.needs.mood != null)
			{
				num /= SlaveRebellionUtility.MoodRebellionFactorCurve.Evaluate(pawn.needs.mood.CurLevelPercentage);
			}
			if (SlaveRebellionUtility.InRoomTouchingMapEdge(pawn))
			{
				num /= 1.7f;
			}
			if (SlaveRebellionUtility.CanApplyWeaponFactor(pawn))
			{
				num /= 4f;
			}
			if (SlaveRebellionUtility.IsUnattendedByColonists(pawn.Map))
			{
				num /= 20f;
			}
			if (SlaveRebellionUtility.BlissLobotomized(pawn))
			{
				num *= 10f;
			}
			return num;
		}

		// Token: 0x060085A5 RID: 34213 RVA: 0x00303B14 File Offset: 0x00301D14
		public static void IncrementInteractionSuppression(Pawn initiator, Pawn recipient)
		{
			if (initiator == null || recipient == null)
			{
				return;
			}
			Need_Suppression need_Suppression;
			if (recipient.needs == null || !recipient.needs.TryGetNeed<Need_Suppression>(out need_Suppression))
			{
				return;
			}
			float statValue = initiator.GetStatValue(StatDefOf.SuppressionPower, true, -1);
			float num = SlaveRebellionUtility.CurrentSuppressionFactorCurve.Evaluate(need_Suppression.CurLevel);
			SlaveRebellionUtility.IncrementSuppression(need_Suppression, initiator, recipient, statValue * num * need_Suppression.MaxLevel);
		}

		// Token: 0x060085A6 RID: 34214 RVA: 0x00303B74 File Offset: 0x00301D74
		public static void IncrementMeleeSuppression(Pawn initiator, Pawn recipient, float damageDealt)
		{
			if (initiator == null || recipient == null)
			{
				return;
			}
			Need_Suppression need_Suppression;
			if (recipient.needs == null || !recipient.needs.TryGetNeed<Need_Suppression>(out need_Suppression))
			{
				return;
			}
			float num = SlaveRebellionUtility.CurrentSuppressionFactorCurve.Evaluate(need_Suppression.CurLevel);
			float num2 = 0.2f * damageDealt;
			SlaveRebellionUtility.IncrementSuppression(need_Suppression, initiator, recipient, 0.02f * num * need_Suppression.MaxLevel * num2);
		}

		// Token: 0x060085A7 RID: 34215 RVA: 0x00303BD4 File Offset: 0x00301DD4
		public static void IncrementSuppression(Need_Suppression suppressionNeed, Pawn initiator, Pawn recipient, float suppressionIncrementPct)
		{
			if (suppressionNeed == null || initiator == null || recipient == null)
			{
				return;
			}
			suppressionNeed.CurLevelPercentage += suppressionIncrementPct;
			TaggedString taggedString = "TextMote_SuppressionIncreased".Translate(suppressionNeed.CurLevel.ToStringPercent());
			MoteMaker.ThrowText((initiator.DrawPos + recipient.DrawPos) / 2f, initiator.Map, taggedString, 4f);
		}

		// Token: 0x060085A8 RID: 34216 RVA: 0x00303C48 File Offset: 0x00301E48
		private static bool InRoomTouchingMapEdge(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			return room != null && room.TouchesMapEdge;
		}

		// Token: 0x060085A9 RID: 34217 RVA: 0x00303C6C File Offset: 0x00301E6C
		private static bool CanApplyWeaponFactor(Pawn pawn)
		{
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return false;
			}
			ThingWithComps primary = pawn.equipment.Primary;
			return (primary != null && primary.def.IsWeapon && SlaveRebellionUtility.WeaponUsableInRebellion(primary)) || SlaveRebellionUtility.GoodWeaponInSameRoom(pawn);
		}

		// Token: 0x060085AA RID: 34218 RVA: 0x00303CB0 File Offset: 0x00301EB0
		public static bool IsUnattendedByColonists(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (!pawn.IsSlave && !pawn.Downed && !pawn.Dead)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060085AB RID: 34219 RVA: 0x00303D20 File Offset: 0x00301F20
		public static bool BlissLobotomized(Pawn p)
		{
			return ModsConfig.AnomalyActive && p.health.hediffSet.HasHediff(HediffDefOf.BlissLobotomy, false);
		}

		// Token: 0x060085AC RID: 34220 RVA: 0x00303D44 File Offset: 0x00301F44
		public static string GetAnySlaveRebellionExplanation(Pawn pawn)
		{
			int count = pawn.Map.mapPawns.SlavesOfColonySpawned.Count;
			if (count > 1 && SlaveRebellionUtility.RebellionForAnySlaveInMapMtbDays(pawn.Map) != -1f)
			{
				int numTicks = (int)(SlaveRebellionUtility.RebellionForAnySlaveInMapMtbDays(pawn.Map) * 60000f);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 1);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("SuppressionRebellionAnySlave".Translate(count, numTicks.ToStringTicksToPeriod(true, false, true, true, false)));
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			return "";
		}

		// Token: 0x060085AD RID: 34221 RVA: 0x00303DDC File Offset: 0x00301FDC
		public static string GetSlaveRebellionMtbCalculationExplanation(Pawn pawn)
		{
			Need_Suppression need_Suppression;
			if (!pawn.needs.TryGetNeed<Need_Suppression>(out need_Suppression) || !SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("SuppressionBaseInterval".Translate());
			defaultInterpolatedStringHandler.AppendLiteral(": ");
			defaultInterpolatedStringHandler.AppendFormatted(2700000.ToStringTicksToPeriodVague(true, true));
			stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			float f = 1f / SlaveRebellionUtility.MovingCapacityFactorCurve.Evaluate(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
			StringBuilder stringBuilder3 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 2);
			defaultInterpolatedStringHandler2.AppendFormatted<TaggedString>("SuppressionMovingCapacityFactor".Translate());
			defaultInterpolatedStringHandler2.AppendLiteral(": x");
			defaultInterpolatedStringHandler2.AppendFormatted(f.ToStringPercent());
			stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
			float f2 = 1f / SlaveRebellionUtility.SuppresionRebellionFactorCurve.Evaluate(need_Suppression.CurLevelPercentage);
			StringBuilder stringBuilder4 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(3, 2);
			defaultInterpolatedStringHandler3.AppendFormatted<TaggedString>("SuppressionFactor".Translate());
			defaultInterpolatedStringHandler3.AppendLiteral(": x");
			defaultInterpolatedStringHandler3.AppendFormatted(f2.ToStringPercent());
			stringBuilder4.AppendLine(defaultInterpolatedStringHandler3.ToStringAndClear());
			float f3 = 1f / SlaveRebellionUtility.MoodRebellionFactorCurve.Evaluate(pawn.needs.mood.CurLevelPercentage);
			StringBuilder stringBuilder5 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(3, 2);
			defaultInterpolatedStringHandler4.AppendFormatted<TaggedString>("SuppressionMoodFactor".Translate());
			defaultInterpolatedStringHandler4.AppendLiteral(": x");
			defaultInterpolatedStringHandler4.AppendFormatted(f3.ToStringPercent());
			stringBuilder5.AppendLine(defaultInterpolatedStringHandler4.ToStringAndClear());
			int count = pawn.Map.mapPawns.SlavesOfColonySpawned.Count;
			float f4 = 1f / SlaveRebellionUtility.SlaveCountFactorCurve.Evaluate((float)count);
			StringBuilder stringBuilder6 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(3, 2);
			defaultInterpolatedStringHandler5.AppendFormatted<TaggedString>("SuppressionSlaveCountFactor".Translate());
			defaultInterpolatedStringHandler5.AppendLiteral(": x");
			defaultInterpolatedStringHandler5.AppendFormatted(f4.ToStringPercent());
			stringBuilder6.AppendLine(defaultInterpolatedStringHandler5.ToStringAndClear());
			if (SlaveRebellionUtility.InRoomTouchingMapEdge(pawn))
			{
				float f5 = 0.58823526f;
				StringBuilder stringBuilder7 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler6.AppendFormatted<TaggedString>("SuppressionEscapeFactor".Translate());
				defaultInterpolatedStringHandler6.AppendLiteral(": x");
				defaultInterpolatedStringHandler6.AppendFormatted(f5.ToStringPercent());
				stringBuilder7.AppendLine(defaultInterpolatedStringHandler6.ToStringAndClear());
			}
			if (SlaveRebellionUtility.CanApplyWeaponFactor(pawn))
			{
				float f6 = 0.25f;
				StringBuilder stringBuilder8 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler7 = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler7.AppendFormatted<TaggedString>("SuppressionWeaponProximityFactor".Translate());
				defaultInterpolatedStringHandler7.AppendLiteral(": x");
				defaultInterpolatedStringHandler7.AppendFormatted(f6.ToStringPercent());
				stringBuilder8.AppendLine(defaultInterpolatedStringHandler7.ToStringAndClear());
			}
			if (SlaveRebellionUtility.IsUnattendedByColonists(pawn.Map))
			{
				float f7 = 0.05f;
				StringBuilder stringBuilder9 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler8 = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler8.AppendFormatted<TaggedString>("SuppressionUnattendedByColonists".Translate());
				defaultInterpolatedStringHandler8.AppendLiteral(": x");
				defaultInterpolatedStringHandler8.AppendFormatted(f7.ToStringPercent());
				stringBuilder9.AppendLine(defaultInterpolatedStringHandler8.ToStringAndClear());
			}
			if (SlaveRebellionUtility.BlissLobotomized(pawn))
			{
				StringBuilder stringBuilder10 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler9 = new DefaultInterpolatedStringHandler(3, 2);
				defaultInterpolatedStringHandler9.AppendFormatted<TaggedString>("BlissLobotomy".Translate());
				defaultInterpolatedStringHandler9.AppendLiteral(": x");
				defaultInterpolatedStringHandler9.AppendFormatted(10f.ToStringPercent());
				stringBuilder10.AppendLine(defaultInterpolatedStringHandler9.ToStringAndClear());
			}
			StringBuilder stringBuilder11 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler10 = new DefaultInterpolatedStringHandler(2, 2);
			defaultInterpolatedStringHandler10.AppendFormatted<TaggedString>("SuppressionFinalInterval".Translate());
			defaultInterpolatedStringHandler10.AppendLiteral(": ");
			defaultInterpolatedStringHandler10.AppendFormatted(((int)(SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(pawn) * 60000f)).ToStringTicksToPeriod(true, false, true, true, false));
			stringBuilder11.Append(defaultInterpolatedStringHandler10.ToStringAndClear());
			return stringBuilder.ToString();
		}

		// Token: 0x060085AE RID: 34222 RVA: 0x00304184 File Offset: 0x00302384
		private static bool IsDamagelessWeapon(Thing t)
		{
			ProjectileProperties projectileProperties;
			if (t == null)
			{
				projectileProperties = null;
			}
			else
			{
				ThingDef def = t.def;
				if (def == null)
				{
					projectileProperties = null;
				}
				else
				{
					List<VerbProperties> verbs = def.Verbs;
					if (verbs == null)
					{
						projectileProperties = null;
					}
					else
					{
						VerbProperties verbProperties = verbs.FirstOrDefault((VerbProperties v) => v.isPrimary);
						if (verbProperties == null)
						{
							projectileProperties = null;
						}
						else
						{
							ThingDef defaultProjectile = verbProperties.defaultProjectile;
							projectileProperties = ((defaultProjectile != null) ? defaultProjectile.projectile : null);
						}
					}
				}
			}
			ProjectileProperties projectileProperties2 = projectileProperties;
			if (projectileProperties2 != null && projectileProperties2.GetDamageAmount(null, null) == 0)
			{
				return true;
			}
			if (projectileProperties2 == null)
			{
				return false;
			}
			DamageDef damageDef = projectileProperties2.damageDef;
			bool? flag = (damageDef != null) ? new bool?(damageDef.harmsHealth) : null;
			bool flag2 = false;
			return flag.GetValueOrDefault() == flag2 & flag != null;
		}

		// Token: 0x060085AF RID: 34223 RVA: 0x00304235 File Offset: 0x00302435
		public static bool WeaponUsableInRebellion(Thing weapon)
		{
			return weapon != null && !SlaveRebellionUtility.IsDamagelessWeapon(weapon) && weapon.def != ThingDefOf.WoodLog && !PawnWeaponGenerator.IsDerpWeapon(weapon.def, weapon.Stuff);
		}

		// Token: 0x060085B0 RID: 34224 RVA: 0x00304268 File Offset: 0x00302468
		public static float RebellionForAnySlaveInMapMtbDays(Map m)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame >= SlaveRebellionUtility.mapMtbCacheNextWipeTick)
			{
				SlaveRebellionUtility.cachedCombinedSlaveRebellionMTB.Clear();
				SlaveRebellionUtility.mapMtbCacheNextWipeTick = ticksGame + 60000;
			}
			CachedRebellionMtb cachedRebellionMtb = SlaveRebellionUtility.cachedCombinedSlaveRebellionMTB.TryGetValue(m, null);
			if (cachedRebellionMtb != null && cachedRebellionMtb.gameTick == ticksGame)
			{
				return cachedRebellionMtb.mtb;
			}
			if (cachedRebellionMtb == null)
			{
				cachedRebellionMtb = new CachedRebellionMtb();
				SlaveRebellionUtility.cachedCombinedSlaveRebellionMTB[m] = cachedRebellionMtb;
			}
			cachedRebellionMtb.gameTick = ticksGame;
			cachedRebellionMtb.mtb = SlaveRebellionUtility.RebellionForAnySlaveInMapMtbDaysHelper(m);
			return cachedRebellionMtb.mtb;
		}

		// Token: 0x060085B1 RID: 34225 RVA: 0x003042EC File Offset: 0x003024EC
		private static float RebellionForAnySlaveInMapMtbDaysHelper(Map m)
		{
			SlaveRebellionUtility.computeCombinedMTBs.Clear();
			foreach (Pawn pawn in m.mapPawns.SlavesOfColonySpawned)
			{
				float num = SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(pawn);
				if (num != -1f)
				{
					SlaveRebellionUtility.computeCombinedMTBs.Add(num);
				}
			}
			if (SlaveRebellionUtility.computeCombinedMTBs.Count <= 0)
			{
				return -1f;
			}
			return Rand.CombineMTBs(SlaveRebellionUtility.computeCombinedMTBs);
		}

		// Token: 0x060085B2 RID: 34226 RVA: 0x0030437C File Offset: 0x0030257C
		private static bool GoodWeaponInSameRoom(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room == null || room.PsychologicallyOutdoors)
			{
				return false;
			}
			ThingRequest thingReq = ThingRequest.ForGroup(ThingRequestGroup.Weapon);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, thingReq, PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 6.9f, (Thing t) => EquipmentUtility.CanEquip(t, pawn) && SlaveRebellionUtility.WeaponUsableInRebellion(t) && t.GetRoom(RegionType.Set_All) == room, null, 0, -1, false, RegionType.Set_Passable, false, false) != null;
		}

		// Token: 0x060085B3 RID: 34227 RVA: 0x00304414 File Offset: 0x00302614
		public static bool CanParticipateInSlaveRebellion(Pawn pawn)
		{
			return pawn.Spawned && (!pawn.Map.IsPocketMap || !pawn.Map.generatorDef.pocketMapProperties.preventPrisonerEscape) && !pawn.Map.Biome.inVacuum && (!pawn.Downed && pawn.Spawned && pawn.IsSlave && !pawn.InMentalState && pawn.Awake()) && !SlaveRebellionUtility.IsRebelling(pawn);
		}

		// Token: 0x060085B4 RID: 34228 RVA: 0x00304498 File Offset: 0x00302698
		public static bool IsRebelling(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_SlaveRebellion;
		}

		// Token: 0x060085B5 RID: 34229 RVA: 0x003044C0 File Offset: 0x003026C0
		public static bool StartSlaveRebellion(Pawn initiator, bool forceAggressive = false)
		{
			string str;
			string str2;
			LetterDef textLetterDef;
			LookTargets lookTargets;
			if (SlaveRebellionUtility.StartSlaveRebellion(initiator, out str, out str2, out textLetterDef, out lookTargets, forceAggressive))
			{
				Find.LetterStack.ReceiveLetter(str2, str, textLetterDef, lookTargets, null, null, null, null, 0, true);
				return true;
			}
			return false;
		}

		// Token: 0x060085B6 RID: 34230 RVA: 0x00304500 File Offset: 0x00302700
		public static bool StartSlaveRebellion(Pawn initiator, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets, bool forceAggressive = false)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			if (!ModLister.CheckIdeology("Slave rebellion"))
			{
				return false;
			}
			SlaveRebellionUtility.rebellingSlaves.Clear();
			SlaveRebellionUtility.rebellingSlaves.Add(initiator);
			SlaveRebellionUtility.allPossibleRebellingSlaves.Clear();
			List<Pawn> slavesOfColonySpawned = initiator.Map.mapPawns.SlavesOfColonySpawned;
			for (int i = 0; i < slavesOfColonySpawned.Count; i++)
			{
				Pawn pawn = slavesOfColonySpawned[i];
				if (pawn != initiator && SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn))
				{
					SlaveRebellionUtility.allPossibleRebellingSlaves.Add(pawn);
				}
			}
			SlaveRebellionUtility.SlaveRebellionType slaveRebellionType = SlaveRebellionUtility.DecideSlaveRebellionType();
			if (slaveRebellionType != SlaveRebellionUtility.SlaveRebellionType.GrandRebellion)
			{
				if (slaveRebellionType == SlaveRebellionUtility.SlaveRebellionType.LocalRebellion)
				{
					for (int j = 0; j < SlaveRebellionUtility.allPossibleRebellingSlaves.Count; j++)
					{
						Pawn pawn2 = SlaveRebellionUtility.allPossibleRebellingSlaves[j];
						if (initiator.Position.DistanceTo(pawn2.Position) <= 35f)
						{
							SlaveRebellionUtility.rebellingSlaves.Add(pawn2);
						}
					}
				}
			}
			else
			{
				for (int k = 0; k < SlaveRebellionUtility.allPossibleRebellingSlaves.Count; k++)
				{
					SlaveRebellionUtility.rebellingSlaves.Add(SlaveRebellionUtility.allPossibleRebellingSlaves[k]);
				}
			}
			if (SlaveRebellionUtility.rebellingSlaves.Count == 1)
			{
				slaveRebellionType = SlaveRebellionUtility.SlaveRebellionType.SingleRebellion;
			}
			else if (SlaveRebellionUtility.rebellingSlaves.Count == SlaveRebellionUtility.allPossibleRebellingSlaves.Count)
			{
				slaveRebellionType = SlaveRebellionUtility.SlaveRebellionType.GrandRebellion;
			}
			IntVec3 exitPoint;
			if (!RCellFinder.TryFindRandomExitSpot(initiator, out exitPoint, TraverseMode.PassDoors))
			{
				return false;
			}
			IntVec3 groupUpLoc;
			if (!PrisonBreakUtility.TryFindGroupUpLoc(SlaveRebellionUtility.rebellingSlaves, exitPoint, out groupUpLoc))
			{
				return false;
			}
			bool flag = forceAggressive || Rand.Chance(0.5f);
			switch (slaveRebellionType)
			{
			case SlaveRebellionUtility.SlaveRebellionType.GrandRebellion:
				if (flag)
				{
					letterLabel = "LetterLabelGrandSlaveRebellion".Translate();
					letterText = "LetterGrandSlaveRebellion".Translate(GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				else
				{
					letterLabel = "LetterLabelGrandSlaveEscape".Translate();
					letterText = "LetterGrandSlaveEscape".Translate(GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				break;
			case SlaveRebellionUtility.SlaveRebellionType.LocalRebellion:
				if (flag)
				{
					letterLabel = "LetterLabelLocalSlaveRebellion".Translate();
					letterText = "LetterLocalSlaveRebellion".Translate(initiator, GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				else
				{
					letterLabel = "LetterLabelLocalSlaveEscape".Translate();
					letterText = "LetterLocalSlaveEscape".Translate(initiator, GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				break;
			case SlaveRebellionUtility.SlaveRebellionType.SingleRebellion:
				if (flag)
				{
					letterLabel = "LetterLabelSingleSlaveRebellion".Translate() + (": " + initiator.LabelShort);
					letterText = "LetterSingleSlaveRebellion".Translate(initiator);
				}
				else
				{
					letterLabel = "LetterLabelSingleSlaveEscape".Translate() + (": " + initiator.LabelShort);
					letterText = "LetterSingleSlaveEscape".Translate(initiator);
				}
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unkown slave rebellion type ");
				defaultInterpolatedStringHandler.AppendFormatted<SlaveRebellionUtility.SlaveRebellionType>(slaveRebellionType);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				break;
			}
			}
			letterText += "\n\n" + "SlaveRebellionSuppressionExplanation".Translate();
			lookTargets = new LookTargets(SlaveRebellionUtility.rebellingSlaves);
			letterDef = LetterDefOf.ThreatBig;
			int sapperThingID = -1;
			if (Rand.Value < 0.5f)
			{
				sapperThingID = initiator.thingIDNumber;
			}
			for (int l = 0; l < SlaveRebellionUtility.rebellingSlaves.Count; l++)
			{
				Lord lord = SlaveRebellionUtility.rebellingSlaves[l].GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(SlaveRebellionUtility.rebellingSlaves[l], PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordMaker.MakeNewLord(SlaveRebellionUtility.rebellingSlaves[0].Faction, new LordJob_SlaveRebellion(groupUpLoc, exitPoint, sapperThingID, !flag), initiator.Map, SlaveRebellionUtility.rebellingSlaves);
			for (int m = 0; m < SlaveRebellionUtility.rebellingSlaves.Count; m++)
			{
				if (!SlaveRebellionUtility.rebellingSlaves[m].Awake())
				{
					RestUtility.WakeUp(SlaveRebellionUtility.rebellingSlaves[m], true);
				}
				SlaveRebellionUtility.rebellingSlaves[m].drafter.Drafted = false;
				if (SlaveRebellionUtility.rebellingSlaves[m].CurJob != null)
				{
					SlaveRebellionUtility.rebellingSlaves[m].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				SlaveRebellionUtility.rebellingSlaves[m].Map.attackTargetsCache.UpdateTarget(SlaveRebellionUtility.rebellingSlaves[m]);
				if (SlaveRebellionUtility.rebellingSlaves[m].carryTracker.CarriedThing != null)
				{
					Thing thing;
					SlaveRebellionUtility.rebellingSlaves[m].carryTracker.TryDropCarriedThing(SlaveRebellionUtility.rebellingSlaves[m].Position, ThingPlaceMode.Near, out thing, null);
				}
			}
			SlaveRebellionUtility.rebellingSlaves.Clear();
			return true;
		}

		// Token: 0x060085B7 RID: 34231 RVA: 0x00304A03 File Offset: 0x00302C03
		private static SlaveRebellionUtility.SlaveRebellionType DecideSlaveRebellionType()
		{
			return Enum.GetValues(typeof(SlaveRebellionUtility.SlaveRebellionType)).Cast<SlaveRebellionUtility.SlaveRebellionType>().RandomElement<SlaveRebellionUtility.SlaveRebellionType>();
		}

		// Token: 0x060085B8 RID: 34232 RVA: 0x00304A20 File Offset: 0x00302C20
		public static Pawn FindSlaveForRebellion(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			SlaveRebellionUtility.tmpSlaves.Clear();
			IReadOnlyList<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn2 = allPawnsSpawned[i];
				if (pawn2.IsSlave && pawn2.SlaveFaction == pawn.SlaveFaction && pawn2 != pawn && !pawn2.Downed && !pawn2.InMentalState && !pawn2.IsBurning() && pawn2.Awake() && SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn2) && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					SlaveRebellionUtility.tmpSlaves.Add(pawn2);
				}
			}
			if (!SlaveRebellionUtility.tmpSlaves.Any<Pawn>())
			{
				return null;
			}
			Pawn result = SlaveRebellionUtility.tmpSlaves.RandomElement<Pawn>();
			SlaveRebellionUtility.tmpSlaves.Clear();
			return result;
		}

		// Token: 0x060085B9 RID: 34233 RVA: 0x00304AED File Offset: 0x00302CED
		public static void ClearCache()
		{
			SlaveRebellionUtility.cachedSlaveRebellionMTB.Clear();
			SlaveRebellionUtility.cachedCombinedSlaveRebellionMTB.Clear();
			SlaveRebellionUtility.pawnMtbCacheNextWipeTick = 0;
			SlaveRebellionUtility.mapMtbCacheNextWipeTick = 0;
		}

		// Token: 0x0400524D RID: 21069
		private static List<float> computeCombinedMTBs = new List<float>();

		// Token: 0x0400524E RID: 21070
		private static Dictionary<Pawn, CachedRebellionMtb> cachedSlaveRebellionMTB = new Dictionary<Pawn, CachedRebellionMtb>();

		// Token: 0x0400524F RID: 21071
		private static Dictionary<Map, CachedRebellionMtb> cachedCombinedSlaveRebellionMTB = new Dictionary<Map, CachedRebellionMtb>();

		// Token: 0x04005250 RID: 21072
		private static int pawnMtbCacheNextWipeTick = 0;

		// Token: 0x04005251 RID: 21073
		private static int mapMtbCacheNextWipeTick = 0;

		// Token: 0x04005252 RID: 21074
		private const int cacheWipeIntervalTicks = 60000;

		// Token: 0x04005253 RID: 21075
		private const int MaxRegionsForDoorCount = 25;

		// Token: 0x04005254 RID: 21076
		private const float WontRebelMTB = -1f;

		// Token: 0x04005255 RID: 21077
		private const float BaseInitiateSlaveRebellionMtbDays = 45f;

		// Token: 0x04005256 RID: 21078
		private const float WeaponProximityMultiplier = 4f;

		// Token: 0x04005257 RID: 21079
		private const float WeaponMaxDistanceForProximityMultiplier = 6.9f;

		// Token: 0x04005258 RID: 21080
		private const float RoomTouchesMapEdge = 1.7f;

		// Token: 0x04005259 RID: 21081
		private const float UnattendedByColonistsMultiplier = 20f;

		// Token: 0x0400525A RID: 21082
		private const float BlissLobotomyMultiplier = 10f;

		// Token: 0x0400525B RID: 21083
		private static readonly SimpleCurve MovingCapacityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.01f),
				true
			},
			{
				new CurvePoint(0.5f, 0.5f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x0400525C RID: 21084
		private static readonly SimpleCurve SuppresionRebellionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 5f),
				true
			},
			{
				new CurvePoint(0.333f, 1.5f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.25f),
				true
			}
		};

		// Token: 0x0400525D RID: 21085
		private static readonly SimpleCurve MoodRebellionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1.5f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.8f),
				true
			}
		};

		// Token: 0x0400525E RID: 21086
		private static readonly SimpleCurve SlaveCountFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(2f, 0.75f),
				true
			},
			{
				new CurvePoint(5f, 0.5f),
				true
			},
			{
				new CurvePoint(10f, 0.3f),
				true
			},
			{
				new CurvePoint(20f, 0.2f),
				true
			}
		};

		// Token: 0x0400525F RID: 21087
		private const float SapperChance = 0.5f;

		// Token: 0x04005260 RID: 21088
		private const float LocalRebellionSearchDistance = 35f;

		// Token: 0x04005261 RID: 21089
		private const float AggressiveRebellionChance = 0.5f;

		// Token: 0x04005262 RID: 21090
		private const float BaseMeleeSlaveSuppressionPct = 0.02f;

		// Token: 0x04005263 RID: 21091
		private const float DamageSuppressionMultiplier = 0.2f;

		// Token: 0x04005264 RID: 21092
		private static readonly SimpleCurve CurrentSuppressionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.5f),
				true
			}
		};

		// Token: 0x04005265 RID: 21093
		private static List<Pawn> rebellingSlaves = new List<Pawn>();

		// Token: 0x04005266 RID: 21094
		private static List<Pawn> allPossibleRebellingSlaves = new List<Pawn>();

		// Token: 0x04005267 RID: 21095
		private static List<Pawn> tmpSlaves = new List<Pawn>();

		// Token: 0x02003374 RID: 13172
		private enum SlaveRebellionType
		{
			// Token: 0x0400CE86 RID: 52870
			GrandRebellion,
			// Token: 0x0400CE87 RID: 52871
			LocalRebellion,
			// Token: 0x0400CE88 RID: 52872
			SingleRebellion
		}
	}
}
