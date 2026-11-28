using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020004E4 RID: 1252
	public class Pawn_InfectionVectorTracker : IExposable
	{
		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06002511 RID: 9489 RVA: 0x000E186A File Offset: 0x000DFA6A
		public IEnumerable<InfectionPathway> Pathways
		{
			get
			{
				return this.pathways.Values;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06002512 RID: 9490 RVA: 0x000E1877 File Offset: 0x000DFA77
		public int PathwaysCount
		{
			get
			{
				return this.pathways.Count;
			}
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x000E1884 File Offset: 0x000DFA84
		public Pawn_InfectionVectorTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x000E18A0 File Offset: 0x000DFAA0
		public void NotifySpawned(bool respawningAfterLoad)
		{
			if (respawningAfterLoad || this.givenPrearrival)
			{
				return;
			}
			this.givenPrearrival = true;
			if (Find.GameInfo.startingAndOptionalPawns.Contains(this.pawn) || this.pawn.DevelopmentalStage.Baby() || this.pawn.dontGivePreArrivalPathway)
			{
				return;
			}
			this.AddInfectionVector(InfectionPathwayDefOf.PrearrivalGeneric, null);
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x000E1903 File Offset: 0x000DFB03
		public bool TryGetPathway(InfectionPathwayDef def, out InfectionPathway pathway)
		{
			return this.pathways.TryGetValue(def, out pathway);
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x000E1914 File Offset: 0x000DFB14
		public List<InfectionPathway> GetPathwaysForHediff(HediffDef hediff)
		{
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
			foreach (HediffInfectionPathway hediffInfectionPathway in hediff.possiblePathways)
			{
				InfectionPathway item;
				if (this.pathways.TryGetValue(hediffInfectionPathway.PathwayDef, out item))
				{
					Pawn_InfectionVectorTracker.TmpPathways.Add(item);
				}
			}
			return Pawn_InfectionVectorTracker.TmpPathways;
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x000E1990 File Offset: 0x000DFB90
		public bool AnyPathwayForHediff(HediffDef hediff)
		{
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
			foreach (HediffInfectionPathway hediffInfectionPathway in hediff.possiblePathways)
			{
				if (this.pathways.ContainsKey(hediffInfectionPathway.PathwayDef))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x000E1A00 File Offset: 0x000DFC00
		public void AddInfectionVector(InfectionPathwayDef def, Pawn source = null)
		{
			this.AddInfectionVector(new InfectionPathway(def, this.pawn, (source != null) ? source.kindDef : null));
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x000E1A20 File Offset: 0x000DFC20
		public void AddInfectionVector(InfectionPathway pathway)
		{
			if (this.pathways.ContainsKey(pathway.Def))
			{
				this.pathways[pathway.Def] = pathway;
				return;
			}
			if (this.pathways.Count >= 100)
			{
				this.ReducePathwaysForNewPathway();
			}
			this.pathways.Add(pathway.Def, pathway);
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x000E1A7C File Offset: 0x000DFC7C
		public void InfectionTickInterval(int delta)
		{
			foreach (KeyValuePair<InfectionPathwayDef, InfectionPathway> keyValuePair in this.pathways)
			{
				InfectionPathwayDef infectionPathwayDef;
				InfectionPathway infectionPathway;
				keyValuePair.Deconstruct(out infectionPathwayDef, out infectionPathway);
				infectionPathway.TickInterval(delta);
			}
			if (this.pawn.IsHashIntervalTick(600, delta))
			{
				this.RemoveExpiredPathways();
			}
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x000E1AF4 File Offset: 0x000DFCF4
		private void RemoveExpiredPathways()
		{
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
			foreach (KeyValuePair<InfectionPathwayDef, InfectionPathway> keyValuePair in this.pathways)
			{
				InfectionPathwayDef infectionPathwayDef;
				InfectionPathway infectionPathway;
				keyValuePair.Deconstruct(out infectionPathwayDef, out infectionPathway);
				InfectionPathway infectionPathway2 = infectionPathway;
				if (infectionPathway2.Expired)
				{
					Pawn_InfectionVectorTracker.TmpPathways.Add(infectionPathway2);
				}
			}
			foreach (InfectionPathway infectionPathway3 in Pawn_InfectionVectorTracker.TmpPathways)
			{
				this.pathways.Remove(infectionPathway3.Def);
			}
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x000E1BC4 File Offset: 0x000DFDC4
		private void ReducePathwaysForNewPathway()
		{
			if (this.pathways.Count <= 99)
			{
				return;
			}
			int num = this.pathways.Count - 100 + 1;
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
			foreach (KeyValuePair<InfectionPathwayDef, InfectionPathway> keyValuePair in this.pathways)
			{
				InfectionPathwayDef infectionPathwayDef;
				InfectionPathway infectionPathway;
				keyValuePair.Deconstruct(out infectionPathwayDef, out infectionPathway);
				InfectionPathway item = infectionPathway;
				Pawn_InfectionVectorTracker.TmpPathways.Add(item);
			}
			Pawn_InfectionVectorTracker.TmpPathways.SortBy((InfectionPathway pathway) => pathway.Def.ExpiryTicks - pathway.AgeTicks);
			for (int i = 0; i < num; i++)
			{
				this.pathways.Remove(Pawn_InfectionVectorTracker.TmpPathways[i].Def);
			}
			Pawn_InfectionVectorTracker.TmpPathways.Clear();
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000E1CB4 File Offset: 0x000DFEB4
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.givenPrearrival, "givenPrearrival", false, false);
			Scribe_Collections.Look<InfectionPathwayDef, InfectionPathway>(ref this.pathways, "pathways", LookMode.Def, LookMode.Deep);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.pathways == null)
			{
				this.pathways = new Dictionary<InfectionPathwayDef, InfectionPathway>(20);
			}
		}

		// Token: 0x04001ADD RID: 6877
		private readonly Pawn pawn;

		// Token: 0x04001ADE RID: 6878
		private Dictionary<InfectionPathwayDef, InfectionPathway> pathways = new Dictionary<InfectionPathwayDef, InfectionPathway>(20);

		// Token: 0x04001ADF RID: 6879
		private bool givenPrearrival;

		// Token: 0x04001AE0 RID: 6880
		private const int UpdateRateTicks = 600;

		// Token: 0x04001AE1 RID: 6881
		private const int MaximumPathways = 100;

		// Token: 0x04001AE2 RID: 6882
		private static readonly List<InfectionPathway> TmpPathways = new List<InfectionPathway>();
	}
}
