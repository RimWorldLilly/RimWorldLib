using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000914 RID: 2324
	public class AttackTargetsCache
	{
		// Token: 0x17000D20 RID: 3360
		// (get) Token: 0x060046C6 RID: 18118 RVA: 0x001BCDD0 File Offset: 0x001BAFD0
		public HashSet<IAttackTarget> TargetsHostileToColony
		{
			get
			{
				return this.TargetsHostileToFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x001BCDE0 File Offset: 0x001BAFE0
		public AttackTargetsCache(Map map)
		{
			this.map = map;
			map.events.FactionAdded += this.Notify_FactionAdded;
			map.events.FactionRemoved += this.Notify_FactionRemoved;
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x001BCE54 File Offset: 0x001BB054
		public static void AttackTargetsCacheStaticUpdate()
		{
			AttackTargetsCache.targets.Clear();
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x001BCE60 File Offset: 0x001BB060
		public void UpdateTarget(IAttackTarget t)
		{
			if (!this.allTargets.Contains(t))
			{
				return;
			}
			this.DeregisterTarget(t);
			Thing thing = t.Thing;
			if (thing.Spawned && thing.Map == this.map)
			{
				this.RegisterTarget(t);
			}
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x001BCEA8 File Offset: 0x001BB0A8
		public List<IAttackTarget> GetPotentialTargetsFor(IAttackTargetSearcher th)
		{
			Thing thing = th.Thing;
			AttackTargetsCache.targets.Clear();
			Faction faction = thing.Faction;
			if (faction != null)
			{
				foreach (IAttackTarget attackTarget in this.TargetsHostileToFaction(faction))
				{
					if (thing.HostileTo(attackTarget.Thing))
					{
						AttackTargetsCache.targets.Add(attackTarget);
					}
				}
			}
			foreach (Pawn pawn in this.pawnsInAggroMentalState)
			{
				if (thing.HostileTo(pawn))
				{
					AttackTargetsCache.targets.Add(pawn);
				}
			}
			foreach (Pawn pawn2 in this.factionlessHumanlikes)
			{
				if (thing.HostileTo(pawn2))
				{
					AttackTargetsCache.targets.Add(pawn2);
				}
			}
			Pawn pawn3 = th as Pawn;
			if (pawn3 != null && PrisonBreakUtility.IsPrisonBreaking(pawn3))
			{
				Faction hostFaction = pawn3.guest.HostFaction;
				List<Pawn> list = this.map.mapPawns.SpawnedPawnsInFaction(hostFaction);
				for (int i = 0; i < list.Count; i++)
				{
					if (thing.HostileTo(list[i]))
					{
						AttackTargetsCache.targets.Add(list[i]);
					}
				}
			}
			if (pawn3 != null && ModsConfig.IdeologyActive && SlaveRebellionUtility.IsRebelling(pawn3))
			{
				Faction faction2 = pawn3.Faction;
				List<Pawn> list2 = this.map.mapPawns.SpawnedPawnsInFaction(faction2);
				for (int j = 0; j < list2.Count; j++)
				{
					if (thing.HostileTo(list2[j]))
					{
						AttackTargetsCache.targets.Add(list2[j]);
					}
				}
			}
			return AttackTargetsCache.targets;
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x001BD0AC File Offset: 0x001BB2AC
		public HashSet<IAttackTarget> TargetsHostileToFaction(Faction f)
		{
			if (f == null)
			{
				Log.Warning("Called TargetsHostileToFaction with null faction.");
				return AttackTargetsCache.emptySet;
			}
			HashSet<IAttackTarget> result;
			if (this.targetsHostileToFaction.TryGetValue(f, out result))
			{
				return result;
			}
			return AttackTargetsCache.emptySet;
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x001BD0E4 File Offset: 0x001BB2E4
		public void Notify_ThingSpawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.RegisterTarget(attackTarget);
			}
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x001BD104 File Offset: 0x001BB304
		public void Notify_ThingDespawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.DeregisterTarget(attackTarget);
			}
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x001BD124 File Offset: 0x001BB324
		public void Notify_FactionHostilityChanged(Faction f1, Faction f2)
		{
			AttackTargetsCache.tmpTargets.Clear();
			foreach (IAttackTarget attackTarget in this.allTargets)
			{
				Thing thing = attackTarget.Thing;
				Pawn pawn = thing as Pawn;
				if (thing.Faction == f1 || thing.Faction == f2 || (pawn != null && pawn.HostFaction == f1) || (pawn != null && pawn.HostFaction == f2))
				{
					AttackTargetsCache.tmpTargets.Add(attackTarget);
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpTargets.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpTargets[i]);
			}
			AttackTargetsCache.tmpTargets.Clear();
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x001BD1F4 File Offset: 0x001BB3F4
		private void RegisterTarget(IAttackTarget target)
		{
			if (this.allTargets.Contains(target))
			{
				string str = "Tried to register the same target twice ";
				string str2 = target.ToStringSafe<IAttackTarget>();
				string str3 = " in ";
				Type type = base.GetType();
				Log.Warning(str + str2 + str3 + ((type != null) ? type.ToString() : null));
				return;
			}
			Thing thing = target.Thing;
			if (!thing.Spawned)
			{
				string str4 = "Tried to register unspawned thing ";
				string str5 = thing.ToStringSafe<Thing>();
				string str6 = " in ";
				Type type2 = base.GetType();
				Log.Warning(str4 + str5 + str6 + ((type2 != null) ? type2.ToString() : null));
				return;
			}
			if (thing.Map != this.map)
			{
				Log.Warning("Tried to register attack target " + thing.ToStringSafe<Thing>() + " but its Map is not this one.");
				return;
			}
			this.allTargets.Add(target);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (thing.HostileTo(allFactionsListForReading[i]))
				{
					if (!this.targetsHostileToFaction.ContainsKey(allFactionsListForReading[i]))
					{
						this.targetsHostileToFaction.Add(allFactionsListForReading[i], new HashSet<IAttackTarget>());
					}
					this.targetsHostileToFaction[allFactionsListForReading[i]].Add(target);
				}
			}
			Pawn pawn = target as Pawn;
			if (pawn != null)
			{
				if (pawn.InAggroMentalState)
				{
					this.pawnsInAggroMentalState.Add(pawn);
				}
				if (pawn.Faction == null && pawn.RaceProps.Humanlike)
				{
					this.factionlessHumanlikes.Add(pawn);
				}
			}
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x001BD360 File Offset: 0x001BB560
		private void DeregisterTarget(IAttackTarget target)
		{
			if (!this.allTargets.Contains(target))
			{
				string str = "Tried to deregister ";
				string str2 = (target != null) ? target.ToString() : null;
				string str3 = " but it's not in ";
				Type type = base.GetType();
				Log.Warning(str + str2 + str3 + ((type != null) ? type.ToString() : null));
				return;
			}
			this.allTargets.Remove(target);
			foreach (KeyValuePair<Faction, HashSet<IAttackTarget>> keyValuePair in this.targetsHostileToFaction)
			{
				keyValuePair.Value.Remove(target);
			}
			Pawn pawn = target as Pawn;
			if (pawn != null)
			{
				this.pawnsInAggroMentalState.Remove(pawn);
				this.factionlessHumanlikes.Remove(pawn);
			}
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x001BD430 File Offset: 0x001BB630
		private void Notify_FactionAdded(Faction faction)
		{
			if (!this.targetsHostileToFaction.ContainsKey(faction))
			{
				this.targetsHostileToFaction.Add(faction, new HashSet<IAttackTarget>());
			}
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x001BD451 File Offset: 0x001BB651
		private void Notify_FactionRemoved(Faction faction)
		{
			if (this.targetsHostileToFaction.ContainsKey(faction))
			{
				this.targetsHostileToFaction.Remove(faction);
			}
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x001BD470 File Offset: 0x001BB670
		private void Debug_AssertHostile(Faction f, HashSet<IAttackTarget> targets)
		{
			AttackTargetsCache.tmpToUpdate.Clear();
			foreach (IAttackTarget attackTarget in targets)
			{
				if (!attackTarget.Thing.HostileTo(f))
				{
					AttackTargetsCache.tmpToUpdate.Add(attackTarget);
					Log.Error(string.Concat(new string[]
					{
						"Target ",
						attackTarget.ToStringSafe<IAttackTarget>(),
						" is not hostile to ",
						f.ToStringSafe<Faction>(),
						" (in ",
						base.GetType().Name,
						") but it's in the list (forgot to update the target somewhere?). Trying to update the target..."
					}));
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpToUpdate.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpToUpdate[i]);
			}
			AttackTargetsCache.tmpToUpdate.Clear();
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x001BD55C File Offset: 0x001BB75C
		public bool Debug_CheckIfInAllTargets(IAttackTarget t)
		{
			return t != null && this.allTargets.Contains(t);
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x001BD56F File Offset: 0x001BB76F
		public bool Debug_CheckIfHostileToFaction(Faction f, IAttackTarget t)
		{
			return f != null && t != null && this.targetsHostileToFaction[f].Contains(t);
		}

		// Token: 0x04002E0D RID: 11789
		private Map map;

		// Token: 0x04002E0E RID: 11790
		private HashSet<IAttackTarget> allTargets = new HashSet<IAttackTarget>();

		// Token: 0x04002E0F RID: 11791
		private Dictionary<Faction, HashSet<IAttackTarget>> targetsHostileToFaction = new Dictionary<Faction, HashSet<IAttackTarget>>();

		// Token: 0x04002E10 RID: 11792
		private HashSet<Pawn> pawnsInAggroMentalState = new HashSet<Pawn>();

		// Token: 0x04002E11 RID: 11793
		private HashSet<Pawn> factionlessHumanlikes = new HashSet<Pawn>();

		// Token: 0x04002E12 RID: 11794
		private static List<IAttackTarget> targets = new List<IAttackTarget>();

		// Token: 0x04002E13 RID: 11795
		private static readonly HashSet<IAttackTarget> emptySet = new HashSet<IAttackTarget>();

		// Token: 0x04002E14 RID: 11796
		private static readonly List<IAttackTarget> tmpTargets = new List<IAttackTarget>();

		// Token: 0x04002E15 RID: 11797
		private static readonly List<IAttackTarget> tmpToUpdate = new List<IAttackTarget>();
	}
}
