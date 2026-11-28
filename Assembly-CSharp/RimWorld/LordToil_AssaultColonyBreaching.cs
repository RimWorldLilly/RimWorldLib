using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CC4 RID: 3268
	public class LordToil_AssaultColonyBreaching : LordToil
	{
		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x06005CAD RID: 23725 RVA: 0x002205E0 File Offset: 0x0021E7E0
		public LordToilData_AssaultColonyBreaching Data
		{
			get
			{
				if (this.data == null)
				{
					this.data = new LordToilData_AssaultColonyBreaching(this.lord);
				}
				return (LordToilData_AssaultColonyBreaching)this.data;
			}
		}

		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x06005CAE RID: 23726 RVA: 0x00220606 File Offset: 0x0021E806
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x06005CAF RID: 23727 RVA: 0x00220609 File Offset: 0x0021E809
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005CB1 RID: 23729 RVA: 0x00220661 File Offset: 0x0021E861
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06005CB2 RID: 23730 RVA: 0x00220674 File Offset: 0x0021E874
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.Data.Reset();
			this.UpdateAllDuties();
		}

		// Token: 0x06005CB3 RID: 23731 RVA: 0x00220687 File Offset: 0x0021E887
		public override void Notify_BuildingSpawnedOnMap(Building b)
		{
			this.Data.breachingGrid.Notify_BuildingStateChanged(b);
		}

		// Token: 0x06005CB4 RID: 23732 RVA: 0x0022069A File Offset: 0x0021E89A
		public override void Notify_BuildingDespawnedOnMap(Building b)
		{
			this.Data.breachingGrid.Notify_BuildingStateChanged(b);
		}

		// Token: 0x06005CB5 RID: 23733 RVA: 0x002206AD File Offset: 0x0021E8AD
		public override void LordToilTick()
		{
			if (this.lord.ticksInToil % 300 == 0)
			{
				this.UpdateAllDuties();
			}
		}

		// Token: 0x06005CB6 RID: 23734 RVA: 0x002206C8 File Offset: 0x0021E8C8
		public override void UpdateAllDuties()
		{
			if (!this.lord.ownedPawns.Any<Pawn>())
			{
				return;
			}
			if (!this.Data.breachDest.IsValid)
			{
				this.Data.Reset();
				this.Data.preferMelee = Rand.Chance(0.5f);
				this.Data.breachStart = this.lord.ownedPawns[0].PositionHeld;
				this.Data.breachDest = GenAI.RandomRaidDest(this.Data.breachStart, base.Map);
				int breachRadius = Mathf.RoundToInt(LordToil_AssaultColonyBreaching.BreachRadiusFromNumRaiders.Evaluate((float)this.lord.ownedPawns.Count));
				int walkMargin = Mathf.RoundToInt(LordToil_AssaultColonyBreaching.WalkMarginFromNumRaiders.Evaluate((float)this.lord.ownedPawns.Count));
				this.Data.breachingGrid.CreateBreachPath(this.Data.breachStart, this.Data.breachDest, breachRadius, walkMargin, this.useAvoidGrid);
			}
			this.pawnsRangedDestructive.Clear();
			this.pawnsMeleeDestructive.Clear();
			this.pawnsRangedGeneral.Clear();
			this.pawnSoloAttackers.Clear();
			this.pawnsEscort.Clear();
			this.pawnsLost.Clear();
			this.Data.maxRange = 12f;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!pawn.CanReach(this.Data.breachStart, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					this.pawnsLost.Add(pawn);
				}
				else
				{
					Verb verb = BreachingUtility.FindVerbToUseForBreaching(pawn);
					if (verb == null)
					{
						this.pawnsEscort.Add(pawn);
					}
					else if (!pawn.RaceProps.IsMechanoid && BreachingUtility.IsSoloAttackVerb(verb))
					{
						this.pawnSoloAttackers.Add(pawn);
					}
					else if (verb.verbProps.ai_IsBuildingDestroyer)
					{
						if (verb.IsMeleeAttack)
						{
							this.pawnsMeleeDestructive.Add(pawn);
						}
						else
						{
							this.pawnsRangedDestructive.Add(pawn);
							this.Data.maxRange = Math.Min(this.Data.maxRange, verb.EffectiveRange);
						}
					}
					else if (verb.IsMeleeAttack)
					{
						this.pawnsEscort.Add(pawn);
					}
					else
					{
						this.pawnsRangedGeneral.Add(pawn);
					}
				}
			}
			bool flag = this.pawnsMeleeDestructive.Any<Pawn>();
			bool flag2 = this.pawnsRangedDestructive.Any<Pawn>();
			if (flag && (!flag2 || this.Data.preferMelee))
			{
				LordToil_AssaultColonyBreaching.BalanceAndSetDuties(this.Data.breachDest, this.pawnsMeleeDestructive, this.pawnSoloAttackers, this.pawnsRangedDestructive, this.pawnsRangedGeneral, this.pawnsEscort);
				LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
				return;
			}
			if (flag2)
			{
				LordToil_AssaultColonyBreaching.BalanceAndSetDuties(this.Data.breachDest, this.pawnsRangedDestructive, this.pawnSoloAttackers, this.pawnsRangedGeneral, this.pawnsMeleeDestructive, this.pawnsEscort);
				LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
				return;
			}
			if (this.pawnsRangedGeneral.Any<Pawn>())
			{
				LordToil_AssaultColonyBreaching.BalanceAndSetDuties(this.Data.breachDest, this.pawnsRangedGeneral, this.pawnSoloAttackers, this.pawnsMeleeDestructive, this.pawnsRangedDestructive, this.pawnsEscort);
				LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
				return;
			}
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsMeleeDestructive);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsRangedDestructive);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsRangedGeneral);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnSoloAttackers);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsEscort);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
		}

		// Token: 0x06005CB7 RID: 23735 RVA: 0x00220A70 File Offset: 0x0021EC70
		private static void BalanceAndSetDuties(IntVec3 breachDest, List<Pawn> breachers, List<Pawn> soloAttackers, List<Pawn> escorts1, List<Pawn> escorts2, List<Pawn> escorts3)
		{
			if (!escorts1.Any<Pawn>() && !escorts2.Any<Pawn>() && !escorts3.Any<Pawn>())
			{
				if (soloAttackers.Any<Pawn>())
				{
					escorts3.AddRange(soloAttackers);
					soloAttackers.Clear();
				}
				else if (breachers.Count > 1)
				{
					Pawn item = breachers.First<Pawn>();
					breachers.Remove(item);
					escorts3.Add(item);
				}
			}
			LordToil_AssaultColonyBreaching.SetBreachDuty(breachers, breachDest);
			LordToil_AssaultColonyBreaching.SetSoloAttackDuty(soloAttackers, breachDest);
			LordToil_AssaultColonyBreaching.SetEscortDuty(escorts1, breachers);
			LordToil_AssaultColonyBreaching.SetEscortDuty(escorts2, breachers);
			LordToil_AssaultColonyBreaching.SetEscortDuty(escorts3, breachers);
		}

		// Token: 0x06005CB8 RID: 23736 RVA: 0x00220AF4 File Offset: 0x0021ECF4
		private static void SetBackupDuty(List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				pawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
			}
		}

		// Token: 0x06005CB9 RID: 23737 RVA: 0x00220B30 File Offset: 0x0021ED30
		private static void SetEscortDuty(List<Pawn> escorts, List<Pawn> targets)
		{
			for (int i = 0; i < escorts.Count; i++)
			{
				Pawn pawn = escorts[i];
				pawn.mindState.duty = new PawnDuty(DutyDefOf.Escort, targets.RandomElement<Pawn>(), BreachingUtility.EscortRadius(pawn));
			}
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x00220B7C File Offset: 0x0021ED7C
		private static void SetBreachDuty(List<Pawn> breachers, IntVec3 breachDest)
		{
			for (int i = 0; i < breachers.Count; i++)
			{
				breachers[i].mindState.duty = new PawnDuty(DutyDefOf.Breaching, breachDest, -1f);
			}
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x00220BC0 File Offset: 0x0021EDC0
		private static void SetSoloAttackDuty(List<Pawn> breachers, IntVec3 breachDest)
		{
			for (int i = 0; i < breachers.Count; i++)
			{
				breachers[i].mindState.duty = new PawnDuty(DutyDefOf.Breaching, breachDest, -1f);
			}
		}

		// Token: 0x06005CBC RID: 23740 RVA: 0x00220C04 File Offset: 0x0021EE04
		public void UpdateCurrentBreachTarget()
		{
			if (this.Data.currentTarget != null && this.Data.currentTarget.Destroyed)
			{
				this.Data.currentTarget = null;
			}
			if (this.Data.soloAttacker != null && BreachingUtility.FindVerbToUseForBreaching(this.Data.soloAttacker) == null)
			{
				this.Data.currentTarget = null;
			}
			if (this.Data.currentTarget == null)
			{
				this.Data.currentTarget = this.Data.breachingGrid.FindBuildingToBreach();
				this.Data.soloAttacker = null;
				if (this.pawnSoloAttackers.Any<Pawn>() && BreachingUtility.CanSoloAttackTargetBuilding(this.Data.currentTarget) && Rand.Chance(0.2f))
				{
					Pawn pawn = this.pawnSoloAttackers.RandomElement<Pawn>();
					if (BreachingUtility.FindVerbToUseForBreaching(pawn) != null)
					{
						this.Data.soloAttacker = pawn;
					}
				}
			}
		}

		// Token: 0x040034D6 RID: 13526
		private const int UpdateIntervalTicks = 300;

		// Token: 0x040034D7 RID: 13527
		private const float PreferMeleeChance = 0.5f;

		// Token: 0x040034D8 RID: 13528
		private const float UseSoloAttackOnTargetChance = 0.2f;

		// Token: 0x040034D9 RID: 13529
		public const float MaxRangeForShooters = 12f;

		// Token: 0x040034DA RID: 13530
		private static readonly SimpleCurve BreachRadiusFromNumRaiders = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(120f, 2f),
				true
			}
		};

		// Token: 0x040034DB RID: 13531
		private static readonly SimpleCurve WalkMarginFromNumRaiders = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(60f, 4f),
				true
			}
		};

		// Token: 0x040034DC RID: 13532
		private List<Pawn> pawnsRangedDestructive = new List<Pawn>();

		// Token: 0x040034DD RID: 13533
		private List<Pawn> pawnsMeleeDestructive = new List<Pawn>();

		// Token: 0x040034DE RID: 13534
		private List<Pawn> pawnsRangedGeneral = new List<Pawn>();

		// Token: 0x040034DF RID: 13535
		private List<Pawn> pawnSoloAttackers = new List<Pawn>();

		// Token: 0x040034E0 RID: 13536
		private List<Pawn> pawnsEscort = new List<Pawn>();

		// Token: 0x040034E1 RID: 13537
		private List<Pawn> pawnsLost = new List<Pawn>();
	}
}
