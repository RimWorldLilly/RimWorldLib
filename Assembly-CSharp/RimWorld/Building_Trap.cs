using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001A6A RID: 6762
	public abstract class Building_Trap : Building, IPathFindCostProvider
	{
		// Token: 0x17001DE2 RID: 7650
		// (get) Token: 0x0600A9F0 RID: 43504 RVA: 0x003C3A6A File Offset: 0x003C1C6A
		private bool CanSetAutoRearm
		{
			get
			{
				return base.Faction == Faction.OfPlayer && this.def.blueprintDef != null && this.def.IsResearchFinished;
			}
		}

		// Token: 0x17001DE3 RID: 7651
		// (get) Token: 0x0600A9F1 RID: 43505 RVA: 0x003C3A93 File Offset: 0x003C1C93
		public virtual bool ShouldShowTrapDamageStat { get; } = 1;

		// Token: 0x17001DE4 RID: 7652
		// (get) Token: 0x0600A9F2 RID: 43506 RVA: 0x003C3A9C File Offset: 0x003C1C9C
		protected bool IsStunned
		{
			get
			{
				if (!this.triedGettingStunner)
				{
					CompStunnable comp = base.GetComp<CompStunnable>();
					this.stunner = ((comp != null) ? comp.StunHandler : null);
					this.triedGettingStunner = true;
				}
				StunHandler stunHandler = this.stunner;
				return stunHandler != null && stunHandler.Stunned;
			}
		}

		// Token: 0x0600A9F3 RID: 43507 RVA: 0x003C3AE4 File Offset: 0x003C1CE4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.autoRearm, "autoRearm", false, false);
			Scribe_Collections.Look<Pawn>(ref this.touchingPawns, "testees", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.touchingPawns.RemoveAll((Pawn x) => x == null) != 0)
				{
					Log.Error("Removed null pawns from touchingPawns.");
				}
			}
		}

		// Token: 0x0600A9F4 RID: 43508 RVA: 0x003C3B5D File Offset: 0x003C1D5D
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.autoRearm = (this.CanSetAutoRearm && map.areaManager.Home[base.Position]);
			}
		}

		// Token: 0x0600A9F5 RID: 43509 RVA: 0x003C3B9C File Offset: 0x003C1D9C
		protected override void Tick()
		{
			if (base.Spawned && !this.IsStunned)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null && !pawn.Flying && !this.touchingPawns.Contains(pawn))
					{
						this.touchingPawns.Add(pawn);
						this.CheckSpring(pawn);
					}
				}
				for (int j = this.touchingPawns.Count - 1; j >= 0; j--)
				{
					Pawn pawn2 = this.touchingPawns[j];
					if (pawn2 == null || !pawn2.Spawned || pawn2.Flying || pawn2.Position != base.Position)
					{
						this.touchingPawns.Remove(pawn2);
					}
				}
			}
			base.Tick();
		}

		// Token: 0x0600A9F6 RID: 43510 RVA: 0x003C3C80 File Offset: 0x003C1E80
		private void CheckSpring(Pawn p)
		{
			if (Rand.Chance(this.SpringChance(p)))
			{
				Map map = base.Map;
				this.Spring(p);
				if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterFriendlyTrapSprungLabel".Translate(p.LabelShort, p).CapitalizeFirst(), "LetterFriendlyTrapSprung".Translate(p.LabelShort, p).CapitalizeFirst(), LetterDefOf.NegativeEvent, new TargetInfo(base.Position, map, false), null, null, null, null, 0, true);
				}
			}
		}

		// Token: 0x0600A9F7 RID: 43511 RVA: 0x003C3D34 File Offset: 0x003C1F34
		protected virtual float SpringChance(Pawn p)
		{
			float num = 1f;
			if (p.kindDef.immuneToTraps)
			{
				return 0f;
			}
			if (this.KnowsOfTrap(p))
			{
				if (p.Faction == null)
				{
					if (p.IsAnimal)
					{
						num = 0.2f;
						num *= this.def.building.trapPeacefulWildAnimalsSpringChanceFactor;
					}
					else
					{
						num = 0.3f;
					}
				}
				else if (p.Faction == base.Faction)
				{
					num = 0.005f;
				}
				else
				{
					num = 0f;
				}
			}
			num *= this.GetStatValue(StatDefOf.TrapSpringChance, true, -1) * p.GetStatValue(StatDefOf.PawnTrapSpringChance, true, -1);
			return Mathf.Clamp01(num);
		}

		// Token: 0x0600A9F8 RID: 43512 RVA: 0x003C3DD8 File Offset: 0x003C1FD8
		public bool KnowsOfTrap(Pawn p)
		{
			if (p.Faction != null && !p.Faction.HostileTo(base.Faction))
			{
				return true;
			}
			if (p.Faction == null && p.IsAnimal && !p.InAggroMentalState)
			{
				return true;
			}
			if (p.guest != null && p.guest.Released)
			{
				return true;
			}
			if (!p.IsPrisoner && base.Faction != null && p.HostFaction == base.Faction)
			{
				return true;
			}
			if (p.RaceProps.Humanlike && p.IsFormingCaravan())
			{
				return true;
			}
			if (p.pather.cachedReturningToCell && base.Faction == p.HostFaction)
			{
				return true;
			}
			if (p.Faction == null && p.RaceProps.Humanlike)
			{
				return true;
			}
			Lord lord = p.lord;
			ILordAvoidTraps lordAvoidTraps = ((lord != null) ? lord.LordJob : null) as ILordAvoidTraps;
			if (lordAvoidTraps != null)
			{
				Faction faction = p.Faction;
				if (Rand.ValueAsync(HashCode.Combine<int, int>((faction != null) ? faction.loadID : p.lord.loadID, this.thingIDNumber)) < lordAvoidTraps.AvoidTrapRatio)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A9F9 RID: 43513 RVA: 0x003C3EF0 File Offset: 0x003C20F0
		public CellRect GetOccupiedRect()
		{
			return this.OccupiedRect();
		}

		// Token: 0x0600A9FA RID: 43514 RVA: 0x003C3EF8 File Offset: 0x003C20F8
		public ushort PathFindCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 800;
		}

		// Token: 0x0600A9FB RID: 43515 RVA: 0x003C3F0A File Offset: 0x003C210A
		public override ushort PathWalkCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 40;
		}

		// Token: 0x0600A9FC RID: 43516 RVA: 0x003C3F19 File Offset: 0x003C2119
		public override bool IsDangerousFor(Pawn p)
		{
			return this.KnowsOfTrap(p);
		}

		// Token: 0x0600A9FD RID: 43517 RVA: 0x003C3F24 File Offset: 0x003C2124
		public void Spring(Pawn p)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			this.SpringSub(p);
			if (this.def.building.trapDestroyOnSpring)
			{
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
				if (spawned)
				{
					this.CheckAutoRebuild(map);
				}
			}
		}

		// Token: 0x0600A9FE RID: 43518 RVA: 0x003C3F74 File Offset: 0x003C2174
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Kill(dinfo, exactCulprit);
			if (spawned)
			{
				this.CheckAutoRebuild(map);
			}
		}

		// Token: 0x0600A9FF RID: 43519
		protected abstract void SpringSub(Pawn p);

		// Token: 0x0600AA00 RID: 43520 RVA: 0x003C3FA0 File Offset: 0x003C21A0
		private void CheckAutoRebuild(Map map)
		{
			if (this.autoRearm && this.CanSetAutoRearm && map != null && GenConstruct.CanPlaceBlueprintAt(this.def, base.Position, base.Rotation, map, false, null, null, base.Stuff, false, false, false).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(this.def, base.Position, map, base.Rotation, Faction.OfPlayer, base.Stuff, null, null, true);
			}
		}

		// Token: 0x0600AA01 RID: 43521 RVA: 0x003C4015 File Offset: 0x003C2215
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_Trap.<GetGizmos>d__31 <GetGizmos>d__ = new Building_Trap.<GetGizmos>d__31(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0400636B RID: 25451
		private bool autoRearm;

		// Token: 0x0400636C RID: 25452
		private List<Pawn> touchingPawns = new List<Pawn>();

		// Token: 0x0400636D RID: 25453
		private StunHandler stunner;

		// Token: 0x0400636E RID: 25454
		private bool triedGettingStunner;

		// Token: 0x0400636F RID: 25455
		private const float KnowerSpringChanceFactorSameFaction = 0.005f;

		// Token: 0x04006370 RID: 25456
		private const float KnowerSpringChanceFactorWildAnimal = 0.2f;

		// Token: 0x04006371 RID: 25457
		private const float KnowerSpringChanceFactorFactionlessHuman = 0.3f;

		// Token: 0x04006372 RID: 25458
		private const float KnowerSpringChanceFactorOther = 0f;

		// Token: 0x04006373 RID: 25459
		private const ushort KnowerPathFindCost = 800;

		// Token: 0x04006374 RID: 25460
		private const ushort KnowerPathWalkCost = 40;
	}
}
