using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001422 RID: 5154
	public class CompAbilityEffect_Neuroquake : CompAbilityEffect
	{
		// Token: 0x170015C8 RID: 5576
		// (get) Token: 0x06007F51 RID: 32593 RVA: 0x002D7980 File Offset: 0x002D5B80
		public new CompProperties_AbilityNeuroquake Props
		{
			get
			{
				return (CompProperties_AbilityNeuroquake)this.props;
			}
		}

		// Token: 0x06007F52 RID: 32594 RVA: 0x002D7990 File Offset: 0x002D5B90
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.affectedFactions == null)
			{
				this.affectedFactions = new Dictionary<Faction, Pair<bool, Pawn>>();
			}
			else
			{
				this.affectedFactions.Clear();
			}
			this.giveMentalStateTo.Clear();
			foreach (Pawn pawn in this.parent.pawn.Map.mapPawns.AllPawnsSpawned)
			{
				if (this.CanApplyEffects(pawn) && !pawn.Fogged())
				{
					bool flag = !pawn.Spawned || pawn.Position.InHorDistOf(this.parent.pawn.Position, this.parent.def.EffectRadius) || !pawn.Position.InHorDistOf(this.parent.pawn.Position, this.Props.mentalStateRadius);
					this.AffectGoodwill(pawn.HomeFaction, !flag, pawn);
					if (!flag)
					{
						this.giveMentalStateTo.Add(pawn);
					}
					else
					{
						this.GiveNeuroquakeThought(pawn);
					}
				}
			}
			foreach (Map map in Find.Maps)
			{
				if (map != this.parent.pawn.Map && Find.WorldGrid.TraversalDistanceBetween(map.Tile, this.parent.pawn.Map.Tile, true, this.Props.worldRangeTiles + 1, false) <= this.Props.worldRangeTiles)
				{
					foreach (Pawn p in map.mapPawns.AllPawns)
					{
						if (this.CanApplyEffects(p))
						{
							this.GiveNeuroquakeThought(p);
						}
					}
				}
			}
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				if (Find.WorldGrid.TraversalDistanceBetween(caravan.Tile, this.parent.pawn.Map.Tile, true, this.Props.worldRangeTiles + 1, false) <= this.Props.worldRangeTiles)
				{
					foreach (Pawn p2 in caravan.pawns)
					{
						if (this.CanApplyEffects(p2))
						{
							this.GiveNeuroquakeThought(p2);
						}
					}
				}
			}
			foreach (Pawn pawn2 in this.giveMentalStateTo)
			{
				MentalStateDef def;
				if (!pawn2.RaceProps.IsMechanoid)
				{
					def = MentalStateDefOf.Berserk;
				}
				else
				{
					def = MentalStateDefOf.BerserkMechanoid;
				}
				CompAbilityEffect_GiveMentalState.TryGiveMentalState(def, pawn2, this.parent.def, StatDefOf.PsychicSensitivity, this.parent.pawn, false);
				RestUtility.WakeUp(pawn2, true);
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.IsPlayer && !faction.defeated && !faction.HostileTo(Faction.OfPlayer))
				{
					this.AffectGoodwill(faction, false, null);
				}
			}
			if (this.parent.pawn.Faction == Faction.OfPlayer)
			{
				foreach (KeyValuePair<Faction, Pair<bool, Pawn>> keyValuePair in this.affectedFactions)
				{
					Faction key = keyValuePair.Key;
					bool first = keyValuePair.Value.First;
					Pawn second = keyValuePair.Value.Second;
					int goodwillChange = first ? this.Props.goodwillImpactForBerserk : this.Props.goodwillImpactForNeuroquake;
					Faction.OfPlayer.TryAffectGoodwillWith(key, goodwillChange, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
			base.Apply(target, dest);
			this.affectedFactions.Clear();
			this.giveMentalStateTo.Clear();
		}

		// Token: 0x06007F53 RID: 32595 RVA: 0x002D7E5C File Offset: 0x002D605C
		private void AffectGoodwill(Faction faction, bool gaveMentalBreak, Pawn p = null)
		{
			Pair<bool, Pawn> pair;
			if (faction != null && !faction.IsPlayer && !faction.HostileTo(Faction.OfPlayer) && (p == null || !p.IsSlaveOfColony) && (!this.affectedFactions.TryGetValue(faction, out pair) || (!pair.First && gaveMentalBreak)))
			{
				this.affectedFactions[faction] = new Pair<bool, Pawn>(gaveMentalBreak, p);
			}
		}

		// Token: 0x06007F54 RID: 32596 RVA: 0x002D7EBD File Offset: 0x002D60BD
		private void GiveNeuroquakeThought(Pawn p)
		{
			Pawn_NeedsTracker needs = p.needs;
			if (needs == null)
			{
				return;
			}
			Need_Mood mood = needs.mood;
			if (mood == null)
			{
				return;
			}
			mood.thoughts.memories.TryGainMemory(ThoughtDefOf.NeuroquakeEcho, null, null);
		}

		// Token: 0x06007F55 RID: 32597 RVA: 0x002D7EEA File Offset: 0x002D60EA
		private bool CanApplyEffects(Pawn p)
		{
			return !p.kindDef.isBoss && (!p.Dead && !p.Suspended) && p.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > float.Epsilon;
		}

		// Token: 0x06007F56 RID: 32598 RVA: 0x002D7F24 File Offset: 0x002D6124
		public override void OnGizmoUpdate()
		{
			if (CompAbilityEffect_Neuroquake.cachedRadiusCellsTarget == null || CompAbilityEffect_Neuroquake.cachedRadiusCellsTarget.Value == this.parent.pawn.Position || CompAbilityEffect_Neuroquake.cachedRadiusCellsMap != this.parent.pawn.Map)
			{
				CompAbilityEffect_Neuroquake.cachedRadiusCells.Clear();
				foreach (IntVec3 item in this.parent.pawn.Map.AllCells)
				{
					if (item.InHorDistOf(this.parent.pawn.Position, this.Props.mentalStateRadius))
					{
						CompAbilityEffect_Neuroquake.cachedRadiusCells.Add(item);
					}
				}
				CompAbilityEffect_Neuroquake.cachedRadiusCellsTarget = new IntVec3?(this.parent.pawn.Position);
			}
			GenDraw.DrawFieldEdges(CompAbilityEffect_Neuroquake.cachedRadiusCells, 2900);
		}

		// Token: 0x04004FC5 RID: 20421
		private Dictionary<Faction, Pair<bool, Pawn>> affectedFactions;

		// Token: 0x04004FC6 RID: 20422
		private List<Pawn> giveMentalStateTo = new List<Pawn>();

		// Token: 0x04004FC7 RID: 20423
		private static List<IntVec3> cachedRadiusCells = new List<IntVec3>();

		// Token: 0x04004FC8 RID: 20424
		private static IntVec3? cachedRadiusCellsTarget = null;

		// Token: 0x04004FC9 RID: 20425
		private static Map cachedRadiusCellsMap = null;
	}
}
