using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001B23 RID: 6947
	public class Hive : Building, IAttackTarget, ILoadReferenceable
	{
		// Token: 0x17002016 RID: 8214
		// (get) Token: 0x0600B1FF RID: 45567 RVA: 0x003EAEDE File Offset: 0x003E90DE
		public CompCanBeDormant CompDormant
		{
			get
			{
				return base.GetComp<CompCanBeDormant>();
			}
		}

		// Token: 0x17002017 RID: 8215
		// (get) Token: 0x0600B200 RID: 45568 RVA: 0x003EAEE6 File Offset: 0x003E90E6
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17002018 RID: 8216
		// (get) Token: 0x0600B201 RID: 45569 RVA: 0x003EAEE9 File Offset: 0x003E90E9
		public float TargetPriorityFactor
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x17002019 RID: 8217
		// (get) Token: 0x0600B202 RID: 45570 RVA: 0x003EAEF0 File Offset: 0x003E90F0
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x1700201A RID: 8218
		// (get) Token: 0x0600B203 RID: 45571 RVA: 0x003EAEF7 File Offset: 0x003E90F7
		public CompSpawnerPawn PawnSpawner
		{
			get
			{
				return base.GetComp<CompSpawnerPawn>();
			}
		}

		// Token: 0x0600B204 RID: 45572 RVA: 0x003EAF00 File Offset: 0x003E9100
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!base.Spawned)
			{
				return true;
			}
			CompCanBeDormant comp = base.GetComp<CompCanBeDormant>();
			return comp != null && !comp.Awake;
		}

		// Token: 0x0600B205 RID: 45573 RVA: 0x003EAF2C File Offset: 0x003E912C
		public static void ResetStaticData()
		{
			Hive.spawnablePawnKinds.Clear();
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megascarab);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Spelopede);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megaspider);
			if (ModsConfig.OdysseyActive)
			{
				Hive.spawnablePawnKinds.Add(PawnKindDefOf.Locust);
			}
		}

		// Token: 0x0600B206 RID: 45574 RVA: 0x003EAF86 File Offset: 0x003E9186
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction == null)
			{
				this.SetFaction(Faction.OfInsects, null);
			}
		}

		// Token: 0x0600B207 RID: 45575 RVA: 0x003EAFA4 File Offset: 0x003E91A4
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (base.Spawned && !this.CompDormant.Awake && !base.Position.Fogged(base.Map))
			{
				this.CompDormant.WakeUp();
			}
		}

		// Token: 0x0600B208 RID: 45576 RVA: 0x003EAFE0 File Offset: 0x003E91E0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (HiveUtility.TotalSpawnedHivesCount(map, true) == 0)
			{
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (needs != null)
					{
						Need_Mood mood = needs.mood;
						if (mood != null)
						{
							ThoughtHandler thoughts = mood.thoughts;
							if (thoughts != null)
							{
								MemoryThoughtHandler memories = thoughts.memories;
								if (memories != null)
								{
									memories.TryGainMemory(ThoughtDefOf.DefeatedInsectHive, null, null);
								}
							}
						}
					}
				}
			}
			if (map.generatorDef != MapGeneratorDefOf.InsectLair)
			{
				List<Lord> lords = map.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					lords[i].ReceiveMemo(Hive.MemoDeSpawned);
				}
			}
			HiveUtility.Notify_HiveDespawned(this, map);
		}

		// Token: 0x0600B209 RID: 45577 RVA: 0x003EB0C4 File Offset: 0x003E92C4
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!this.questTags.NullOrEmpty<string>())
			{
				bool flag = false;
				List<Thing> list = base.Map.listerThings.ThingsOfDef(this.def);
				for (int i = 0; i < list.Count; i++)
				{
					Hive hive = list[i] as Hive;
					if (hive != null && hive != this && hive.CompDormant.Awake && !hive.questTags.NullOrEmpty<string>() && QuestUtility.AnyMatchingTags(hive.questTags, this.questTags))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "AllHivesDestroyed");
				}
			}
			base.Destroy(mode);
		}

		// Token: 0x0600B20A RID: 45578 RVA: 0x003EB16C File Offset: 0x003E936C
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (dinfo.Def.ExternalViolenceFor(this) && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
				Lord lord = base.GetComp<CompSpawnerPawn>().Lord;
				if (lord != null)
				{
					lord.ReceiveMemo(Hive.MemoAttackedByEnemy);
				}
			}
			if (dinfo.Def == DamageDefOf.Flame && (float)this.HitPoints < (float)base.MaxHitPoints * 0.3f)
			{
				Lord lord2 = base.GetComp<CompSpawnerPawn>().Lord;
				if (lord2 != null)
				{
					lord2.ReceiveMemo(Hive.MemoBurnedBadly);
				}
			}
			base.PostApplyDamage(dinfo, totalDamageDealt);
		}

		// Token: 0x0600B20B RID: 45579 RVA: 0x003EB200 File Offset: 0x003E9400
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (base.Spawned && (dinfo == null || dinfo.Value.Category != DamageInfo.SourceCategory.Collapse))
			{
				List<Lord> lords = base.Map.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					lords[i].ReceiveMemo(Hive.MemoDestroyedNonRoofCollapse);
				}
			}
			base.Kill(dinfo, exactCulprit);
		}

		// Token: 0x0600B20C RID: 45580 RVA: 0x003EB270 File Offset: 0x003E9470
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (this.PawnSpawner.spawnedPawns.Count > 0)
			{
				if (this.PawnSpawner.spawnedPawns.Any((Pawn p) => !p.Downed))
				{
					reason = this.def.label;
					return true;
				}
			}
			reason = null;
			return false;
		}

		// Token: 0x0600B20D RID: 45581 RVA: 0x003EB2D4 File Offset: 0x003E94D4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Hive.<GetGizmos>d__27 <GetGizmos>d__ = new Hive.<GetGizmos>d__27(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B20E RID: 45582 RVA: 0x003EB2E4 File Offset: 0x003E94E4
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				bool flag = false;
				Scribe_Values.Look<bool>(ref flag, "active", false, false);
				if (flag)
				{
					this.CompDormant.WakeUp();
				}
			}
		}

		// Token: 0x040066B2 RID: 26290
		public const int PawnSpawnRadius = 2;

		// Token: 0x040066B3 RID: 26291
		public const float MaxSpawnedPawnsPoints = 500f;

		// Token: 0x040066B4 RID: 26292
		public const float InitialPawnsPoints = 200f;

		// Token: 0x040066B5 RID: 26293
		public static List<PawnKindDef> spawnablePawnKinds = new List<PawnKindDef>();

		// Token: 0x040066B6 RID: 26294
		public static readonly string MemoAttackedByEnemy = "HiveAttacked";

		// Token: 0x040066B7 RID: 26295
		public static readonly string MemoDeSpawned = "HiveDeSpawned";

		// Token: 0x040066B8 RID: 26296
		public static readonly string MemoBurnedBadly = "HiveBurnedBadly";

		// Token: 0x040066B9 RID: 26297
		public static readonly string MemoDestroyedNonRoofCollapse = "HiveDestroyedNonRoofCollapse";
	}
}
