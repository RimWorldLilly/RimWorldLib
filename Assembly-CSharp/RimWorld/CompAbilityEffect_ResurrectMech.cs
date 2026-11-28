using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200143E RID: 5182
	public class CompAbilityEffect_ResurrectMech : CompAbilityEffect
	{
		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x06007FA3 RID: 32675 RVA: 0x002D8E5E File Offset: 0x002D705E
		public new CompProperties_ResurrectMech Props
		{
			get
			{
				return (CompProperties_ResurrectMech)this.props;
			}
		}

		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x06007FA4 RID: 32676 RVA: 0x002D8E6B File Offset: 0x002D706B
		public int ChargesRemaining
		{
			get
			{
				return this.resurrectCharges;
			}
		}

		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x06007FA5 RID: 32677 RVA: 0x002D8E73 File Offset: 0x002D7073
		public override bool CanCast
		{
			get
			{
				return this.resurrectCharges > 0;
			}
		}

		// Token: 0x06007FA6 RID: 32678 RVA: 0x002D8E80 File Offset: 0x002D7080
		public override void Initialize(AbilityCompProperties props)
		{
			base.Initialize(props);
			this.costsByWeightClass = new Dictionary<MechWeightClassDef, int>();
			for (int i = 0; i < this.Props.costs.Count; i++)
			{
				this.costsByWeightClass[this.Props.costs[i].weightClass] = this.Props.costs[i].cost;
			}
		}

		// Token: 0x06007FA7 RID: 32679 RVA: 0x002D8EF4 File Offset: 0x002D70F4
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!base.CanApplyOn(target, dest))
			{
				return false;
			}
			if (target.HasThing)
			{
				Corpse corpse = target.Thing as Corpse;
				if (corpse != null)
				{
					return this.CanResurrect(corpse);
				}
			}
			return false;
		}

		// Token: 0x06007FA8 RID: 32680 RVA: 0x002D8F34 File Offset: 0x002D7134
		private bool TryGetResurrectCost(Corpse corpse, out int cost)
		{
			if (corpse.InnerPawn.RaceProps.mechWeightClass != null && this.costsByWeightClass.ContainsKey(corpse.InnerPawn.RaceProps.mechWeightClass))
			{
				cost = this.costsByWeightClass[corpse.InnerPawn.RaceProps.mechWeightClass];
				return true;
			}
			cost = -1;
			return false;
		}

		// Token: 0x06007FA9 RID: 32681 RVA: 0x002D8F94 File Offset: 0x002D7194
		private bool CanResurrect(Corpse corpse)
		{
			int num;
			return corpse.InnerPawn.RaceProps.IsMechanoid && corpse.InnerPawn.RaceProps.mechWeightClass != null && corpse.InnerPawn.RaceProps.mechWeightClass.canResurrect && corpse.InnerPawn.Faction == this.parent.pawn.Faction && (corpse.InnerPawn.kindDef.abilities == null || !corpse.InnerPawn.kindDef.abilities.Contains(AbilityDefOf.ResurrectionMech)) && corpse.timeOfDeath >= Find.TickManager.TicksGame - this.Props.maxCorpseAgeTicks && this.TryGetResurrectCost(corpse, out num) && num <= this.resurrectCharges;
		}

		// Token: 0x06007FAA RID: 32682 RVA: 0x002D9064 File Offset: 0x002D7264
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Corpse corpse = (Corpse)target.Thing;
			if (!this.CanResurrect(corpse))
			{
				return;
			}
			int num;
			if (this.TryGetResurrectCost(corpse, out num))
			{
				Pawn innerPawn = corpse.InnerPawn;
				this.resurrectCharges -= num;
				ResurrectionUtility.TryResurrect(innerPawn, null);
				if (this.Props.appliedEffecterDef != null)
				{
					Effecter effecter = this.Props.appliedEffecterDef.SpawnAttached(innerPawn, innerPawn.MapHeld, 1f);
					effecter.Trigger(innerPawn, innerPawn, -1);
					effecter.Cleanup();
				}
				innerPawn.stances.stagger.StaggerFor(60, 0.17f);
			}
		}

		// Token: 0x06007FAB RID: 32683 RVA: 0x002D9112 File Offset: 0x002D7312
		public override bool GizmoDisabled(out string reason)
		{
			reason = null;
			return false;
		}

		// Token: 0x06007FAC RID: 32684 RVA: 0x002D9118 File Offset: 0x002D7318
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompAbilityEffect_ResurrectMech.<CompGetGizmosExtra>d__15 <CompGetGizmosExtra>d__ = new CompAbilityEffect_ResurrectMech.<CompGetGizmosExtra>d__15(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06007FAD RID: 32685 RVA: 0x002D9128 File Offset: 0x002D7328
		public override IEnumerable<Mote> CustomWarmupMotes(LocalTargetInfo target)
		{
			CompAbilityEffect_ResurrectMech.<CustomWarmupMotes>d__16 <CustomWarmupMotes>d__ = new CompAbilityEffect_ResurrectMech.<CustomWarmupMotes>d__16(-2);
			<CustomWarmupMotes>d__.<>4__this = this;
			<CustomWarmupMotes>d__.<>3__target = target;
			return <CustomWarmupMotes>d__;
		}

		// Token: 0x06007FAE RID: 32686 RVA: 0x002D9140 File Offset: 0x002D7340
		public override void PostApplied(List<LocalTargetInfo> targets, Map map)
		{
			Vector3 vector = Vector3.zero;
			foreach (LocalTargetInfo localTargetInfo in targets)
			{
				vector += localTargetInfo.Cell.ToVector3Shifted();
			}
			vector /= (float)targets.Count;
			IntVec3 intVec = vector.ToIntVec3();
			EffecterDefOf.ApocrionAoeResolve.Spawn(intVec, map, 1f).EffectTick(new TargetInfo(intVec, map, false), new TargetInfo(intVec, map, false));
		}

		// Token: 0x06007FAF RID: 32687 RVA: 0x002D91E0 File Offset: 0x002D73E0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.resurrectCharges, "resurrectCharges", 30, false);
		}

		// Token: 0x04004FE2 RID: 20450
		[LoadAlias("charges")]
		private int resurrectCharges = 30;

		// Token: 0x04004FE3 RID: 20451
		private Dictionary<MechWeightClassDef, int> costsByWeightClass;

		// Token: 0x04004FE4 RID: 20452
		private Gizmo gizmo;
	}
}
