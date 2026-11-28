using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001A6C RID: 6764
	public abstract class Building_TrapReleaseEntity : Building_Trap
	{
		// Token: 0x17001DE5 RID: 7653
		// (get) Token: 0x0600AA08 RID: 43528
		protected abstract PawnKindDef PawnToSpawn { get; }

		// Token: 0x17001DE6 RID: 7654
		// (get) Token: 0x0600AA09 RID: 43529
		protected abstract int CountToSpawn { get; }

		// Token: 0x17001DE7 RID: 7655
		// (get) Token: 0x0600AA0A RID: 43530 RVA: 0x003C4076 File Offset: 0x003C2276
		public override bool ShouldShowTrapDamageStat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600AA0B RID: 43531 RVA: 0x003C407C File Offset: 0x003C227C
		protected override void Tick()
		{
			base.Tick();
			if (!base.Spawned || base.IsStunned || !this.IsHashIntervalTick(60))
			{
				return;
			}
			Map map = base.Map;
			IEnumerable<Pawn> enumerable = from x in map.mapPawns.AllPawnsSpawned
			where !x.IsPsychologicallyInvisible() && x.HostileTo(this) && x.Position.DistanceTo(base.Position) <= 20f
			select x;
			IntVec3 position = base.Position;
			foreach (Pawn pawn in enumerable)
			{
				if ((base.Faction != Faction.OfPlayer || (!pawn.IsPrisoner && !pawn.Downed)) && GenSight.LineOfSight(position, pawn.Position, map))
				{
					base.Spring(pawn);
					break;
				}
			}
		}

		// Token: 0x0600AA0C RID: 43532 RVA: 0x003C413C File Offset: 0x003C233C
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (base.IsStunned)
			{
				base.Kill(dinfo, exactCulprit);
				return;
			}
			base.Spring(((dinfo != null) ? dinfo.GetValueOrDefault().Instigator : null) as Pawn);
		}

		// Token: 0x0600AA0D RID: 43533 RVA: 0x003C4180 File Offset: 0x003C2380
		protected override void SpringSub(Pawn p)
		{
			if (!base.Spawned)
			{
				return;
			}
			SoundDefOf.DroneTrapSpring.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			if (base.Faction != Faction.OfPlayer)
			{
				Messages.Message("MessageTrapSprung".Translate(this.Named("TRAP")), new LookTargets(base.Position, base.Map), MessageTypeDefOf.NegativeEvent, true);
			}
			for (int i = 0; i < this.CountToSpawn; i++)
			{
				PawnKindDef pawnToSpawn = this.PawnToSpawn;
				Faction faction = base.Faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				float? fixedBiologicalAge = new float?(0f);
				float? fixedChronologicalAge = new float?(0f);
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnToSpawn, faction, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, fixedChronologicalAge, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				pawn.mindState.enemyTarget = p;
				GenSpawn.Spawn(pawn, base.Position, base.Map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600AA0E RID: 43534 RVA: 0x003C42D3 File Offset: 0x003C24D3
		protected override float SpringChance(Pawn p)
		{
			return 0f;
		}
	}
}
