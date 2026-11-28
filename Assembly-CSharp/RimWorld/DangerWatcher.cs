using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200120B RID: 4619
	public class DangerWatcher
	{
		// Token: 0x17001386 RID: 4998
		// (get) Token: 0x060072B7 RID: 29367 RVA: 0x00281B91 File Offset: 0x0027FD91
		public StoryDanger DangerRating
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastUpdateTick + 101)
				{
					this.dangerRatingInt = this.CalculateDangerRating();
					this.lastUpdateTick = Find.TickManager.TicksGame;
				}
				return this.dangerRatingInt;
			}
		}

		// Token: 0x060072B8 RID: 29368 RVA: 0x00281BCC File Offset: 0x0027FDCC
		private StoryDanger CalculateDangerRating()
		{
			float num = this.map.attackTargetsCache.TargetsHostileToColony.Where(DangerWatcher.CachedAffectsStoryDangerDelegate).Sum(delegate(IAttackTarget t)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null)
				{
					return pawn.kindDef.combatPower;
				}
				Building_TurretGun building_TurretGun = t as Building_TurretGun;
				if (building_TurretGun != null && building_TurretGun.def.building.IsMortar && !building_TurretGun.IsMannable)
				{
					return building_TurretGun.def.building.combatPower;
				}
				return 0f;
			});
			if (num == 0f)
			{
				return StoryDanger.None;
			}
			int num2 = this.map.mapPawns.FreeColonistsSpawned.Count((Pawn p) => !p.Downed);
			if (num < 150f && num <= (float)num2 * 18f)
			{
				return StoryDanger.Low;
			}
			if (num > 400f)
			{
				return StoryDanger.High;
			}
			if (this.lastColonistHarmedTick > Find.TickManager.TicksGame - 900)
			{
				return StoryDanger.High;
			}
			foreach (Lord lord in this.map.lordManager.lords)
			{
				if (lord.faction.HostileTo(Faction.OfPlayer) && lord.CurLordToil.ForceHighStoryDanger && lord.AnyActivePawn)
				{
					return StoryDanger.High;
				}
			}
			return StoryDanger.Low;
		}

		// Token: 0x060072B9 RID: 29369 RVA: 0x00281D0C File Offset: 0x0027FF0C
		public DangerWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x060072BA RID: 29370 RVA: 0x00281D31 File Offset: 0x0027FF31
		public void Notify_ColonistHarmedExternally()
		{
			this.lastColonistHarmedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060072BB RID: 29371 RVA: 0x00281D44 File Offset: 0x0027FF44
		private static bool AffectsStoryDanger(IAttackTarget t)
		{
			Pawn pawn = t.Thing as Pawn;
			if (pawn != null)
			{
				Lord lord = pawn.GetLord();
				if (lord != null && (lord.LordJob is LordJob_DefendPoint || lord.LordJob is LordJob_MechanoidDefendBase) && pawn.CurJobDef != JobDefOf.AttackMelee && pawn.CurJobDef != JobDefOf.AttackStatic)
				{
					return false;
				}
				CompCanBeDormant comp = pawn.GetComp<CompCanBeDormant>();
				if (comp != null && !comp.Awake)
				{
					return false;
				}
			}
			return GenHostility.IsActiveThreatToPlayer(t, false);
		}

		// Token: 0x04004887 RID: 18567
		private Map map;

		// Token: 0x04004888 RID: 18568
		private StoryDanger dangerRatingInt;

		// Token: 0x04004889 RID: 18569
		private int lastUpdateTick = -10000;

		// Token: 0x0400488A RID: 18570
		private int lastColonistHarmedTick = -10000;

		// Token: 0x0400488B RID: 18571
		private const int UpdateInterval = 101;

		// Token: 0x0400488C RID: 18572
		private static readonly Func<IAttackTarget, bool> CachedAffectsStoryDangerDelegate = new Func<IAttackTarget, bool>(DangerWatcher.AffectsStoryDanger);
	}
}
