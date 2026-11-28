using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020001F1 RID: 497
	public class GameInitData
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000E24 RID: 3620 RVA: 0x000499DF File Offset: 0x00047BDF
		public bool QuickStarted
		{
			get
			{
				return this.gameToLoad.NullOrEmpty() && !this.startedFromEntry;
			}
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x000499F9 File Offset: 0x00047BF9
		public void ChooseRandomStartingTile()
		{
			this.startingTile = TileFinder.RandomStartingTile();
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00049A06 File Offset: 0x00047C06
		public void ResetWorldRelatedMapInitData()
		{
			Current.Game.World = null;
			this.startingAndOptionalPawns.Clear();
			this.playerFaction = null;
			this.startingTile = PlanetTile.Invalid;
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x00049A30 File Offset: 0x00047C30
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 2);
			defaultInterpolatedStringHandler.AppendLiteral("startedFromEntry: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(this.startedFromEntry);
			defaultInterpolatedStringHandler.AppendLiteral("\nstartingAndOptionalPawns: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.startingAndOptionalPawns.Count);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x00049A88 File Offset: 0x00047C88
		public void PrepForMapGen()
		{
			while (this.startingAndOptionalPawns.Count > this.startingPawnCount)
			{
				PawnComponentsUtility.RemoveComponentsOnDespawned(this.startingAndOptionalPawns[this.startingPawnCount]);
				this.startingAndOptionalPawns[this.startingPawnCount].wasLeftBehindStartingPawn = true;
				Find.WorldPawns.PassToWorld(this.startingAndOptionalPawns[this.startingPawnCount], PawnDiscardDecideMode.KeepForever);
				Pawn pawn = this.startingAndOptionalPawns[this.startingPawnCount];
				this.startingAndOptionalPawns.Remove(pawn);
				this.startingPossessions.Remove(pawn);
			}
			List<Pawn> list = this.startingAndOptionalPawns;
			foreach (Pawn pawn2 in list)
			{
				pawn2.SetFactionDirect(Faction.OfPlayer);
				PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn2, false);
			}
			foreach (Pawn pawn3 in list)
			{
				pawn3.workSettings.DisableAll();
			}
			using (IEnumerator<WorkTypeDef> enumerator2 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					WorkTypeDef w = enumerator2.Current;
					if (w.alwaysStartActive)
					{
						IEnumerable<Pawn> source = list;
						Func<Pawn, bool> predicate;
						Func<Pawn, bool> <>9__0;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((Pawn col) => !col.WorkTypeIsDisabled(w)));
						}
						using (IEnumerator<Pawn> enumerator3 = source.Where(predicate).GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Pawn pawn4 = enumerator3.Current;
								pawn4.workSettings.SetPriority(w, 3);
							}
							continue;
						}
					}
					bool flag = false;
					foreach (Pawn pawn5 in list)
					{
						if (!pawn5.WorkTypeIsDisabled(w) && pawn5.skills.AverageOfRelevantSkillsFor(w) >= 6f)
						{
							pawn5.workSettings.SetPriority(w, 3);
							flag = true;
						}
					}
					if (!flag)
					{
						IEnumerable<Pawn> source2 = from col in list
						where !col.WorkTypeIsDisabled(w)
						select col;
						if (source2.Any<Pawn>())
						{
							source2.InRandomOrder(null).MaxBy((Pawn c) => c.skills.AverageOfRelevantSkillsFor(w)).workSettings.SetPriority(w, 3);
						}
					}
				}
			}
			if (ModsConfig.BiotechActive)
			{
				for (int i = 0; i < this.startingAndOptionalPawns.Count; i++)
				{
					CustomXenotype forcedCustomXenotype = StartingPawnUtility.GetGenerationRequest(i).ForcedCustomXenotype;
					if (forcedCustomXenotype != null && !Current.Game.customXenotypeDatabase.customXenotypes.Contains(forcedCustomXenotype))
					{
						Current.Game.customXenotypeDatabase.customXenotypes.Add(forcedCustomXenotype);
					}
				}
			}
		}

		// Token: 0x04000E14 RID: 3604
		public PlanetTile startingTile = PlanetTile.Invalid;

		// Token: 0x04000E15 RID: 3605
		public MapGeneratorDef mapGeneratorDef;

		// Token: 0x04000E16 RID: 3606
		public int mapSize = 250;

		// Token: 0x04000E17 RID: 3607
		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		// Token: 0x04000E18 RID: 3608
		public Dictionary<Pawn, List<ThingDefCount>> startingPossessions = new Dictionary<Pawn, List<ThingDefCount>>();

		// Token: 0x04000E19 RID: 3609
		public int startingPawnCount = -1;

		// Token: 0x04000E1A RID: 3610
		public Faction playerFaction;

		// Token: 0x04000E1B RID: 3611
		public Season startingSeason;

		// Token: 0x04000E1C RID: 3612
		public bool permadeathChosen;

		// Token: 0x04000E1D RID: 3613
		public bool permadeath;

		// Token: 0x04000E1E RID: 3614
		public PawnKindDef startingPawnKind;

		// Token: 0x04000E1F RID: 3615
		public List<PawnKindCount> startingPawnsRequired;

		// Token: 0x04000E20 RID: 3616
		public List<XenotypeCount> startingXenotypesRequired;

		// Token: 0x04000E21 RID: 3617
		public List<MutantCount> startingMutantsRequired;

		// Token: 0x04000E22 RID: 3618
		public DevelopmentalStage allowedDevelopmentalStages = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04000E23 RID: 3619
		public List<SkillDef> startingSkillsRequired;

		// Token: 0x04000E24 RID: 3620
		public bool startedFromEntry;

		// Token: 0x04000E25 RID: 3621
		public string gameToLoad;

		// Token: 0x04000E26 RID: 3622
		public const int DefaultMapSize = 250;
	}
}
