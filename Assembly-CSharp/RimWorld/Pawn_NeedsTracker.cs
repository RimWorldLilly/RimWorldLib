using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200158D RID: 5517
	public class Pawn_NeedsTracker : IExposable
	{
		// Token: 0x1700171A RID: 5914
		// (get) Token: 0x06008724 RID: 34596 RVA: 0x0030D4AB File Offset: 0x0030B6AB
		public List<Need> AllNeeds
		{
			get
			{
				return this.needs;
			}
		}

		// Token: 0x1700171B RID: 5915
		// (get) Token: 0x06008725 RID: 34597 RVA: 0x0030D4B3 File Offset: 0x0030B6B3
		public List<Need> MiscNeeds
		{
			get
			{
				return this.needsMisc;
			}
		}

		// Token: 0x06008726 RID: 34598 RVA: 0x0030D4BB File Offset: 0x0030B6BB
		public Pawn_NeedsTracker()
		{
		}

		// Token: 0x06008727 RID: 34599 RVA: 0x0030D4DA File Offset: 0x0030B6DA
		public Pawn_NeedsTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.AddOrRemoveNeedsAsAppropriate();
		}

		// Token: 0x06008728 RID: 34600 RVA: 0x0030D508 File Offset: 0x0030B708
		public void ExposeData()
		{
			Scribe_Collections.Look<Need>(ref this.needs, "needs", LookMode.Deep, new object[]
			{
				this.pawn
			});
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.needs.RemoveAll((Need x) => ((x != null) ? x.def : null) == null) != 0)
				{
					Log.Error("Pawn " + this.pawn.ToStringSafe<Pawn>() + " had some null needs after loading.");
				}
				this.BindDirectNeedFields();
				this.CacheMiscNeeds();
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x06008729 RID: 34601 RVA: 0x0030D5A4 File Offset: 0x0030B7A4
		public void BindDirectNeedFields()
		{
			this.TryGetNeed<Need_Mood>(out this.mood);
			this.TryGetNeed<Need_Food>(out this.food);
			this.TryGetNeed<Need_MechEnergy>(out this.energy);
			this.TryGetNeed<Need_Rest>(out this.rest);
			this.TryGetNeed<Need_Joy>(out this.joy);
			this.TryGetNeed<Need_Beauty>(out this.beauty);
			this.TryGetNeed<Need_Comfort>(out this.comfort);
			this.TryGetNeed<Need_RoomSize>(out this.roomsize);
			this.TryGetNeed<Need_Outdoors>(out this.outdoors);
			this.TryGetNeed<Need_Indoors>(out this.indoors);
			this.TryGetNeed<Need_Chemical_Any>(out this.drugsDesire);
			this.TryGetNeed<Need_Learning>(out this.learning);
			this.TryGetNeed<Need_Play>(out this.play);
		}

		// Token: 0x0600872A RID: 34602 RVA: 0x0030D65A File Offset: 0x0030B85A
		private void CacheMiscNeeds()
		{
			this.needsMisc.Clear();
		}

		// Token: 0x0600872B RID: 34603 RVA: 0x0030D668 File Offset: 0x0030B868
		public void NeedsTrackerTickInterval(int delta)
		{
			if (!this.pawn.IsHashIntervalTick(150, delta))
			{
				return;
			}
			for (int i = 0; i < this.needs.Count; i++)
			{
				this.needs[i].NeedInterval();
			}
		}

		// Token: 0x0600872C RID: 34604 RVA: 0x0030D6B0 File Offset: 0x0030B8B0
		public T TryGetNeed<T>() where T : Need
		{
			T result;
			this.TryGetNeed<T>(out result);
			return result;
		}

		// Token: 0x0600872D RID: 34605 RVA: 0x0030D6C8 File Offset: 0x0030B8C8
		public bool TryGetNeed<T>(out T need) where T : Need
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				if (this.needs[i].GetType() == typeof(T))
				{
					need = (T)((object)this.needs[i]);
					return true;
				}
			}
			need = default(T);
			return false;
		}

		// Token: 0x0600872E RID: 34606 RVA: 0x0030D730 File Offset: 0x0030B930
		public Need TryGetNeed(NeedDef def)
		{
			Need result;
			this.TryGetNeed(def, out result);
			return result;
		}

		// Token: 0x0600872F RID: 34607 RVA: 0x0030D748 File Offset: 0x0030B948
		public bool TryGetNeed(NeedDef def, out Need need)
		{
			for (int i = 0; i < this.needs.Count; i++)
			{
				if (this.needs[i].def == def)
				{
					need = this.needs[i];
					return true;
				}
			}
			need = null;
			return false;
		}

		// Token: 0x06008730 RID: 34608 RVA: 0x0030D794 File Offset: 0x0030B994
		public void SetInitialLevels()
		{
			this.pawn.GetStatValue(StatDefOf.MaxNutrition, true, 0);
			for (int i = 0; i < this.needs.Count; i++)
			{
				this.needs[i].SetInitialLevel();
			}
		}

		// Token: 0x06008731 RID: 34609 RVA: 0x0030D7DC File Offset: 0x0030B9DC
		public void AddOrRemoveNeedsAsAppropriate()
		{
			List<NeedDef> allDefsListForReading = DefDatabase<NeedDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				try
				{
					NeedDef needDef = allDefsListForReading[i];
					Need need;
					if (this.ShouldHaveNeed(needDef))
					{
						if (!this.TryGetNeed(needDef, out need))
						{
							this.AddNeed(needDef);
						}
					}
					else if (this.TryGetNeed(needDef, out need))
					{
						this.RemoveNeed(needDef);
					}
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Error while determining if ");
					defaultInterpolatedStringHandler.AppendFormatted(this.pawn.ToStringSafe<Pawn>());
					defaultInterpolatedStringHandler.AppendLiteral(" should have Need ");
					defaultInterpolatedStringHandler.AppendFormatted(allDefsListForReading[i].ToStringSafe<NeedDef>());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x1700171C RID: 5916
		// (get) Token: 0x06008732 RID: 34610 RVA: 0x0030D8BC File Offset: 0x0030BABC
		public bool PrefersOutdoors
		{
			get
			{
				Need need;
				return !this.TryGetNeed(NeedDefOf.Indoors, out need);
			}
		}

		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x06008733 RID: 34611 RVA: 0x0030D8DC File Offset: 0x0030BADC
		public bool PrefersIndoors
		{
			get
			{
				Need need;
				return this.TryGetNeed(NeedDefOf.Indoors, out need);
			}
		}

		// Token: 0x06008734 RID: 34612 RVA: 0x0030D8F8 File Offset: 0x0030BAF8
		private bool ShouldHaveNeed(NeedDef nd)
		{
			if (this.pawn.RaceProps.intelligence < nd.minIntelligence)
			{
				return false;
			}
			if (!nd.developmentalStageFilter.Has(this.pawn.DevelopmentalStage))
			{
				return false;
			}
			if (nd.colonistsOnly && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer))
			{
				return false;
			}
			if (nd.playerMechsOnly && (!this.pawn.RaceProps.IsMechanoid || this.pawn.Faction != Faction.OfPlayer || this.pawn.OverseerSubject == null))
			{
				return false;
			}
			if (nd.colonistAndPrisonersOnly && (this.pawn.Faction == null || !this.pawn.Faction.IsPlayer) && (this.pawn.HostFaction == null || this.pawn.HostFaction != Faction.OfPlayer))
			{
				return false;
			}
			if (this.pawn.health.hediffSet.DisablesNeed(nd))
			{
				return false;
			}
			if (ModsConfig.BiotechActive && this.pawn.genes != null && this.pawn.genes.DisablesNeed(nd))
			{
				return false;
			}
			Pawn_StoryTracker story = this.pawn.story;
			if (((story != null) ? story.traits : null) != null && this.pawn.story.traits.DisablesNeed(nd))
			{
				return false;
			}
			if (this.pawn.Ideo != null && this.pawn.Ideo.DisablesNeed(nd))
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			if (nd.onlyIfCausedByHediff)
			{
				flag = true;
				if (this.pawn.health.hediffSet.EnablesNeed(nd))
				{
					flag2 = true;
				}
			}
			if (ModsConfig.BiotechActive && nd.onlyIfCausedByGene)
			{
				flag = true;
				if (this.pawn.genes != null && this.pawn.genes.EnablesNeed(nd))
				{
					flag2 = true;
				}
			}
			if (ModsConfig.IdeologyActive && nd.onlyIfCausedByIdeo)
			{
				flag = true;
				if (this.pawn.Ideo != null && this.pawn.Ideo.EnablesNeed(nd))
				{
					flag2 = true;
				}
			}
			if (nd.onlyIfCausedByTrait)
			{
				flag = true;
				Pawn_StoryTracker story2 = this.pawn.story;
				if (((story2 != null) ? story2.traits : null) != null && this.pawn.story.traits.EnablesNeed(nd))
				{
					flag2 = true;
				}
			}
			if (flag && !flag2)
			{
				return false;
			}
			if (nd.neverOnPrisoner && this.pawn.IsPrisoner)
			{
				return false;
			}
			if (nd.neverOnSlave && this.pawn.IsSlave)
			{
				return false;
			}
			if (this.pawn.IsMutant && this.pawn.mutant.Def.disableNeeds && (this.pawn.mutant.Def.needWhitelist == null || !this.pawn.mutant.Def.needWhitelist.Contains(nd)))
			{
				return false;
			}
			if (nd.titleRequiredAny != null)
			{
				if (this.pawn.royalty == null)
				{
					return false;
				}
				bool flag3 = false;
				foreach (RoyalTitle royalTitle in this.pawn.royalty.AllTitlesInEffectForReading)
				{
					if (nd.titleRequiredAny.Contains(royalTitle.def))
					{
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					return false;
				}
			}
			if (nd.nullifyingPrecepts != null && this.pawn.Ideo != null)
			{
				bool flag4 = false;
				foreach (PreceptDef preceptDef in nd.nullifyingPrecepts)
				{
					if (this.pawn.Ideo.HasPrecept(preceptDef))
					{
						flag4 = true;
						break;
					}
				}
				if (flag4)
				{
					return false;
				}
			}
			if (nd.hediffRequiredAny != null)
			{
				bool flag5 = false;
				foreach (HediffDef def in nd.hediffRequiredAny)
				{
					if (this.pawn.health.hediffSet.HasHediff(def, false))
					{
						flag5 = true;
						break;
					}
				}
				if (!flag5)
				{
					return false;
				}
			}
			if (nd.defName == "Authority")
			{
				return false;
			}
			if (nd.slavesOnly && !this.pawn.IsSlave)
			{
				return false;
			}
			if (ModsConfig.AnomalyActive && nd.requiredComps != null)
			{
				foreach (CompProperties properties in nd.requiredComps)
				{
					if (this.pawn.TryGetComp(properties) == null)
					{
						return false;
					}
				}
			}
			if (nd == NeedDefOf.Food)
			{
				return this.pawn.RaceProps.EatsFood;
			}
			return nd != NeedDefOf.Rest || this.pawn.RaceProps.needsRest;
		}

		// Token: 0x06008735 RID: 34613 RVA: 0x0030DE04 File Offset: 0x0030C004
		private void AddNeed(NeedDef nd)
		{
			Need need = (Need)Activator.CreateInstance(nd.needClass, new object[]
			{
				this.pawn
			});
			need.def = nd;
			this.needs.Add(need);
			need.SetInitialLevel();
			this.BindDirectNeedFields();
		}

		// Token: 0x06008736 RID: 34614 RVA: 0x0030DE50 File Offset: 0x0030C050
		private void RemoveNeed(NeedDef nd)
		{
			Need need;
			if (!this.TryGetNeed(nd, out need))
			{
				return;
			}
			need.OnNeedRemoved();
			this.needs.Remove(need);
			this.BindDirectNeedFields();
		}

		// Token: 0x06008737 RID: 34615 RVA: 0x0030DE82 File Offset: 0x0030C082
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_NeedsTracker.<GetGizmos>d__39 <GetGizmos>d__ = new Pawn_NeedsTracker.<GetGizmos>d__39(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0400538C RID: 21388
		private Pawn pawn;

		// Token: 0x0400538D RID: 21389
		private List<Need> needs = new List<Need>();

		// Token: 0x0400538E RID: 21390
		public Need_Mood mood;

		// Token: 0x0400538F RID: 21391
		public Need_Food food;

		// Token: 0x04005390 RID: 21392
		public Need_MechEnergy energy;

		// Token: 0x04005391 RID: 21393
		public Need_Rest rest;

		// Token: 0x04005392 RID: 21394
		public Need_Joy joy;

		// Token: 0x04005393 RID: 21395
		public Need_Beauty beauty;

		// Token: 0x04005394 RID: 21396
		public Need_RoomSize roomsize;

		// Token: 0x04005395 RID: 21397
		public Need_Outdoors outdoors;

		// Token: 0x04005396 RID: 21398
		public Need_Indoors indoors;

		// Token: 0x04005397 RID: 21399
		public Need_Chemical_Any drugsDesire;

		// Token: 0x04005398 RID: 21400
		public Need_Comfort comfort;

		// Token: 0x04005399 RID: 21401
		public Need_Learning learning;

		// Token: 0x0400539A RID: 21402
		public Need_Play play;

		// Token: 0x0400539B RID: 21403
		private List<Need> needsMisc = new List<Need>(0);
	}
}
