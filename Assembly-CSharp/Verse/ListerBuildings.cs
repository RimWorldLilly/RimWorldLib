using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x0200028D RID: 653
	public sealed class ListerBuildings : IDisposable
	{
		// Token: 0x060013B1 RID: 5041 RVA: 0x0006FBE0 File Offset: 0x0006DDE0
		public ListerBuildings.TrackingScope Track(Predicate<Building> predicate = null)
		{
			return new ListerBuildings.TrackingScope(this, predicate);
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x0006FBE9 File Offset: 0x0006DDE9
		private void StartTracker(ListerBuildings.TrackingScope scope)
		{
			this.scopes.Add(scope);
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x0006FBF7 File Offset: 0x0006DDF7
		private void ReleaseTracker(ListerBuildings.TrackingScope scope)
		{
			this.scopes.Remove(scope);
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0006FC08 File Offset: 0x0006DE08
		public void Add(Building b)
		{
			if (b.def.building != null && b.def.building.isNaturalRock)
			{
				return;
			}
			if (b.Faction == Faction.OfPlayer)
			{
				this.allBuildingsColonist.Add(b);
				if (b is IAttackTarget)
				{
					this.allBuildingsColonistCombatTargets.Add(b);
				}
			}
			else
			{
				this.allBuildingsNonColonist.Add(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Add(b);
			}
			if (b.TryGetComp<CompAnimalPenMarker>() != null)
			{
				this.allBuildingsAnimalPenMarkers.Add(b);
			}
			if (b.def == ThingDefOf.CaravanPackingSpot)
			{
				this.allBuildingsHitchingPosts.Add(b);
			}
			foreach (ListerBuildings.TrackingScope trackingScope in this.scopes)
			{
				if ((trackingScope.Enabled && trackingScope.predicate == null) || trackingScope.predicate(b))
				{
					trackingScope.tracked.Add(b);
				}
			}
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0006FD30 File Offset: 0x0006DF30
		public void Remove(Building b)
		{
			this.allBuildingsColonist.Remove(b);
			this.allBuildingsNonColonist.Remove(b);
			if (b is IAttackTarget)
			{
				this.allBuildingsColonistCombatTargets.Remove(b);
			}
			CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
			if (compProperties != null && compProperties.shortCircuitInRain)
			{
				this.allBuildingsColonistElecFire.Remove(b);
			}
			this.allBuildingsAnimalPenMarkers.Remove(b);
			this.allBuildingsHitchingPosts.Remove(b);
			foreach (ListerBuildings.TrackingScope trackingScope in this.scopes)
			{
				if (trackingScope.Enabled)
				{
					trackingScope.tracked.Remove(b);
				}
			}
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0006FE00 File Offset: 0x0006E000
		public void RegisterInstallBlueprint(Blueprint_Install blueprint)
		{
			this.reinstallationMap.Add(blueprint.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified(), blueprint);
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0006FE1C File Offset: 0x0006E01C
		public void DeregisterInstallBlueprint(Blueprint_Install blueprint)
		{
			Thing miniToInstallOrBuildingToReinstall = blueprint.MiniToInstallOrBuildingToReinstall;
			Thing thing = (miniToInstallOrBuildingToReinstall != null) ? miniToInstallOrBuildingToReinstall.GetInnerIfMinified() : null;
			if (thing != null)
			{
				this.reinstallationMap.Remove(thing);
				return;
			}
			Thing thing2 = null;
			foreach (KeyValuePair<Thing, Blueprint_Install> keyValuePair in this.reinstallationMap)
			{
				if (keyValuePair.Value == blueprint)
				{
					thing2 = keyValuePair.Key;
					break;
				}
			}
			if (thing2 != null)
			{
				this.reinstallationMap.Remove(thing2);
			}
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x0006FEB4 File Offset: 0x0006E0B4
		public bool ColonistsHaveBuilding(ThingDef def)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0006FEF0 File Offset: 0x0006E0F0
		public bool ColonistsHaveBuilding(Func<Thing, bool> predicate)
		{
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (predicate(this.allBuildingsColonist[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0006FF2A File Offset: 0x0006E12A
		public bool ColonistsHaveResearchBench()
		{
			return this.AllBuildingsColonistOfClass<Building_ResearchBench>().Any<Building_ResearchBench>();
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x0006FF38 File Offset: 0x0006E138
		public bool ColonistsHaveBuildingWithPowerOn(ThingDef def)
		{
			List<Building> list = this.AllBuildingsColonistOfDef(def);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def == def)
				{
					CompPowerTrader compPowerTrader = list[i].TryGetComp<CompPowerTrader>();
					if (compPowerTrader == null || compPowerTrader.PowerOn)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x0006FF88 File Offset: 0x0006E188
		public List<Building> AllBuildingsColonistOfDef(ThingDef def)
		{
			ListerBuildings.allBuildingsColonistOfDefResult.Clear();
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (this.allBuildingsColonist[i].def == def)
				{
					ListerBuildings.allBuildingsColonistOfDefResult.Add(this.allBuildingsColonist[i]);
				}
			}
			return ListerBuildings.allBuildingsColonistOfDefResult;
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x0006FFE4 File Offset: 0x0006E1E4
		public List<Building> AllBuildingsColonistOfGroup(ThingRequestGroup group)
		{
			ListerBuildings.allBuildingsColonistOfGroupResult.Clear();
			for (int i = 0; i < this.allBuildingsColonist.Count; i++)
			{
				if (group.Includes(this.allBuildingsColonist[i].def))
				{
					ListerBuildings.allBuildingsColonistOfGroupResult.Add(this.allBuildingsColonist[i]);
				}
			}
			return ListerBuildings.allBuildingsColonistOfGroupResult;
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00070045 File Offset: 0x0006E245
		public IEnumerable<T> AllBuildingsColonistOfClass<T>() where T : Building
		{
			ListerBuildings.<AllBuildingsColonistOfClass>d__24<T> <AllBuildingsColonistOfClass>d__ = new ListerBuildings.<AllBuildingsColonistOfClass>d__24<T>(-2);
			<AllBuildingsColonistOfClass>d__.<>4__this = this;
			return <AllBuildingsColonistOfClass>d__;
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00070055 File Offset: 0x0006E255
		public IEnumerable<T> AllColonistBuildingsOfType<T>()
		{
			ListerBuildings.<AllColonistBuildingsOfType>d__25<T> <AllColonistBuildingsOfType>d__ = new ListerBuildings.<AllColonistBuildingsOfType>d__25<T>(-2);
			<AllColonistBuildingsOfType>d__.<>4__this = this;
			return <AllColonistBuildingsOfType>d__;
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x00070065 File Offset: 0x0006E265
		public IEnumerable<Building> AllBuildingsNonColonistOfDef(ThingDef def)
		{
			ListerBuildings.<AllBuildingsNonColonistOfDef>d__26 <AllBuildingsNonColonistOfDef>d__ = new ListerBuildings.<AllBuildingsNonColonistOfDef>d__26(-2);
			<AllBuildingsNonColonistOfDef>d__.<>4__this = this;
			<AllBuildingsNonColonistOfDef>d__.<>3__def = def;
			return <AllBuildingsNonColonistOfDef>d__;
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0007007C File Offset: 0x0006E27C
		public bool TryGetReinstallBlueprint(Thing building, out Blueprint_Install bp)
		{
			return this.reinstallationMap.TryGetValue(building, out bp);
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0007008C File Offset: 0x0006E28C
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.allBuildingsNonColonist.Count; i++)
			{
				if (this.allBuildingsNonColonist[i].Faction == faction)
				{
					this.allBuildingsNonColonist[i].SetFaction(null, null);
				}
			}
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x000700D8 File Offset: 0x0006E2D8
		public void Dispose()
		{
			this.allBuildingsColonist.Clear();
			this.allBuildingsNonColonist.Clear();
			this.allBuildingsColonistCombatTargets.Clear();
			this.allBuildingsColonistElecFire.Clear();
			this.allBuildingsAnimalPenMarkers.Clear();
			this.allBuildingsHitchingPosts.Clear();
			this.reinstallationMap.Clear();
		}

		// Token: 0x0400110A RID: 4362
		public readonly List<Building> allBuildingsColonist = new List<Building>();

		// Token: 0x0400110B RID: 4363
		public readonly List<Building> allBuildingsNonColonist = new List<Building>();

		// Token: 0x0400110C RID: 4364
		public readonly HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

		// Token: 0x0400110D RID: 4365
		public readonly HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();

		// Token: 0x0400110E RID: 4366
		public readonly HashSet<Building> allBuildingsAnimalPenMarkers = new HashSet<Building>();

		// Token: 0x0400110F RID: 4367
		public readonly HashSet<Building> allBuildingsHitchingPosts = new HashSet<Building>();

		// Token: 0x04001110 RID: 4368
		private readonly Dictionary<Thing, Blueprint_Install> reinstallationMap = new Dictionary<Thing, Blueprint_Install>();

		// Token: 0x04001111 RID: 4369
		private readonly List<ListerBuildings.TrackingScope> scopes = new List<ListerBuildings.TrackingScope>();

		// Token: 0x04001112 RID: 4370
		private static List<Building> allBuildingsColonistOfDefResult = new List<Building>();

		// Token: 0x04001113 RID: 4371
		private static List<Building> allBuildingsColonistOfGroupResult = new List<Building>();

		// Token: 0x020025EA RID: 9706
		public class TrackingScope : IDisposable, IEnumerable<Building>, IEnumerable
		{
			// Token: 0x17002A86 RID: 10886
			// (get) Token: 0x0600F145 RID: 61765 RVA: 0x0058AE01 File Offset: 0x00589001
			public bool Enabled
			{
				get
				{
					return this.enabled;
				}
			}

			// Token: 0x0600F146 RID: 61766 RVA: 0x0058AE09 File Offset: 0x00589009
			public TrackingScope(ListerBuildings parent, Predicate<Building> predicate = null)
			{
				this.tracked = SimplePool<HashSet<Building>>.Get();
				this.parent = parent;
				this.predicate = predicate;
				this.Start();
			}

			// Token: 0x0600F147 RID: 61767 RVA: 0x0058AE30 File Offset: 0x00589030
			public void Dispose()
			{
				this.tracked.Clear();
				SimplePool<HashSet<Building>>.Return(this.tracked);
				this.Stop();
			}

			// Token: 0x0600F148 RID: 61768 RVA: 0x0058AE4E File Offset: 0x0058904E
			public void Start()
			{
				if (this.enabled)
				{
					return;
				}
				this.enabled = true;
				this.parent.StartTracker(this);
			}

			// Token: 0x0600F149 RID: 61769 RVA: 0x0058AE6C File Offset: 0x0058906C
			public void Stop()
			{
				if (!this.enabled)
				{
					return;
				}
				this.enabled = false;
				this.parent.ReleaseTracker(this);
			}

			// Token: 0x0600F14A RID: 61770 RVA: 0x0058AE8A File Offset: 0x0058908A
			public IEnumerator<Building> GetEnumerator()
			{
				return this.tracked.GetEnumerator();
			}

			// Token: 0x0600F14B RID: 61771 RVA: 0x0058AE9C File Offset: 0x0058909C
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.tracked.GetEnumerator();
			}

			// Token: 0x04009B7B RID: 39803
			private bool enabled;

			// Token: 0x04009B7C RID: 39804
			private readonly ListerBuildings parent;

			// Token: 0x04009B7D RID: 39805
			public readonly HashSet<Building> tracked;

			// Token: 0x04009B7E RID: 39806
			public readonly Predicate<Building> predicate;
		}
	}
}
