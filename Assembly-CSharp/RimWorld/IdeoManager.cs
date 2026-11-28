using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001628 RID: 5672
	public class IdeoManager : IExposable
	{
		// Token: 0x1700188A RID: 6282
		// (get) Token: 0x06008DD9 RID: 36313 RVA: 0x0033C71B File Offset: 0x0033A91B
		public List<Ideo> IdeosListForReading
		{
			get
			{
				return this.ideos;
			}
		}

		// Token: 0x1700188B RID: 6283
		// (get) Token: 0x06008DDA RID: 36314 RVA: 0x0033C724 File Offset: 0x0033A924
		public IEnumerable<Ideo> IdeosInViewOrder
		{
			get
			{
				IEnumerable<Faction> factions = Find.FactionManager.AllFactionsInViewOrder;
				return (from i in this.IdeosListForReading
				where !i.hidden
				select i).OrderBy(delegate(Ideo ideo)
				{
					int num = 0;
					int num2 = int.MaxValue;
					foreach (Faction faction in factions)
					{
						if (faction.ideos != null && faction.ideos.IsPrimary(ideo) && !ideo.hidden)
						{
							if (faction.IsPlayer)
							{
								return int.MinValue;
							}
							num2 = Mathf.Min(num2, num);
						}
						num++;
					}
					return num2;
				});
			}
		}

		// Token: 0x1700188C RID: 6284
		// (get) Token: 0x06008DDB RID: 36315 RVA: 0x0033C784 File Offset: 0x0033A984
		public Ideo Horaxian
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return null;
				}
				if (this.horaxianInt == null)
				{
					for (int i = 0; i < this.ideos.Count; i++)
					{
						if (this.ideos[i].solid)
						{
							this.horaxianInt = this.ideos[i];
							break;
						}
					}
				}
				return this.horaxianInt;
			}
		}

		// Token: 0x06008DDC RID: 36316 RVA: 0x0033C7E5 File Offset: 0x0033A9E5
		public bool Add(Ideo ideo)
		{
			if (ideo == null)
			{
				Log.Error("Tried to add a null ideoligion.");
				return false;
			}
			if (this.ideos.Contains(ideo))
			{
				Log.Error("Tried to add the same ideoligion twice.");
				return false;
			}
			this.ideos.Add(ideo);
			return true;
		}

		// Token: 0x06008DDD RID: 36317 RVA: 0x0033C820 File Offset: 0x0033AA20
		public bool Remove(Ideo ideo)
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction.ideos != null && faction.ideos.AllIdeos.Contains(ideo))
				{
					Log.Error(string.Concat(new string[]
					{
						"Faction ",
						faction.Name,
						" contains ideo ",
						ideo.name,
						" which was removed!"
					}));
				}
			}
			if (this.ideos.Remove(ideo))
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
				{
					Pawn_IdeoTracker ideo2 = pawn.ideo;
					if (ideo2 != null)
					{
						ideo2.Notify_IdeoRemoved(ideo);
					}
				}
				Find.PlayLog.Notify_IdeoRemoved(ideo);
				return true;
			}
			return false;
		}

		// Token: 0x06008DDE RID: 36318 RVA: 0x0033C928 File Offset: 0x0033AB28
		public List<Faction> GetFactionsWithIdeo(Ideo ideo, bool onlyPrimary = false, bool onlyNpcFactions = false)
		{
			this.npcWithIdeoTmp.Clear();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if ((!onlyNpcFactions || faction != Faction.OfPlayer) && faction.ideos != null)
				{
					if (onlyPrimary)
					{
						if (faction.ideos.IsPrimary(ideo))
						{
							this.npcWithIdeoTmp.Add(faction);
						}
					}
					else if (faction.ideos.Has(ideo))
					{
						this.npcWithIdeoTmp.Add(faction);
					}
				}
			}
			return this.npcWithIdeoTmp;
		}

		// Token: 0x06008DDF RID: 36319 RVA: 0x0033C9D0 File Offset: 0x0033ABD0
		public List<LordJob_Ritual> GetActiveRituals(Map map)
		{
			this.activeRitualsTmp.Clear();
			if (map == null)
			{
				return this.activeRitualsTmp;
			}
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_Ritual lordJob_Ritual = lords[i].LordJob as LordJob_Ritual;
				if (lordJob_Ritual != null)
				{
					this.activeRitualsTmp.Add(lordJob_Ritual);
				}
			}
			return this.activeRitualsTmp;
		}

		// Token: 0x06008DE0 RID: 36320 RVA: 0x0033CA38 File Offset: 0x0033AC38
		public LordJob_Ritual GetActiveRitualOn(TargetInfo target)
		{
			if (target.HasThing && !target.Thing.Spawned)
			{
				return null;
			}
			foreach (LordJob_Ritual lordJob_Ritual in this.GetActiveRituals(target.Map))
			{
				if (lordJob_Ritual.selectedTarget == target)
				{
					return lordJob_Ritual;
				}
			}
			return null;
		}

		// Token: 0x06008DE1 RID: 36321 RVA: 0x0033CABC File Offset: 0x0033ACBC
		public void SortIdeos()
		{
			this.ideos.SortByDescending(delegate(Ideo x)
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				Ideo ideo;
				if (ofPlayerSilentFail == null)
				{
					ideo = null;
				}
				else
				{
					FactionIdeosTracker factionIdeosTracker = ofPlayerSilentFail.ideos;
					ideo = ((factionIdeosTracker != null) ? factionIdeosTracker.PrimaryIdeo : null);
				}
				if (x == ideo)
				{
					return 999;
				}
				int num = 0;
				List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
				for (int i = 0; i < allFactionsListForReading.Count; i++)
				{
					if (allFactionsListForReading[i].ideos != null && allFactionsListForReading[i].ideos.IsPrimary(x))
					{
						num++;
					}
				}
				return num;
			});
		}

		// Token: 0x06008DE2 RID: 36322 RVA: 0x0033CAE8 File Offset: 0x0033ACE8
		public void IdeoManagerTick()
		{
			for (int i = 0; i < this.ideos.Count; i++)
			{
				this.ideos[i].IdeoTick();
			}
			for (int j = this.toRemove.Count - 1; j >= 0; j--)
			{
				Ideo ideo = this.toRemove[j];
				this.toRemove.RemoveAt(j);
				this.Remove(ideo);
			}
		}

		// Token: 0x06008DE3 RID: 36323 RVA: 0x0033CB58 File Offset: 0x0033AD58
		public void RemoveUnusedStartingIdeos()
		{
			for (int i = this.ideos.Count - 1; i >= 0; i--)
			{
				if (this.CanRemoveIdeo(this.ideos[i]))
				{
					this.Remove(this.ideos[i]);
				}
			}
		}

		// Token: 0x06008DE4 RID: 36324 RVA: 0x0033CBA4 File Offset: 0x0033ADA4
		public void Notify_PawnKilled(Pawn pawn)
		{
			this.TryQueueIdeoRemoval(pawn.Ideo);
		}

		// Token: 0x06008DE5 RID: 36325 RVA: 0x0033CBB3 File Offset: 0x0033ADB3
		public void Notify_PawnLeftMap(Pawn pawn)
		{
			this.TryQueueIdeoRemoval(pawn.Ideo);
		}

		// Token: 0x06008DE6 RID: 36326 RVA: 0x0033CBC4 File Offset: 0x0033ADC4
		public void Notify_FactionRemoved(Faction faction)
		{
			if (faction.ideos != null)
			{
				foreach (Ideo ideo in faction.ideos.AllIdeos)
				{
					this.TryQueueIdeoRemoval(ideo);
				}
			}
		}

		// Token: 0x06008DE7 RID: 36327 RVA: 0x0033CC20 File Offset: 0x0033AE20
		private bool TryQueueIdeoRemoval(Ideo ideo)
		{
			if (ideo == null)
			{
				return false;
			}
			if (!this.CanRemoveIdeo(ideo))
			{
				return false;
			}
			if (!this.toRemove.Contains(ideo))
			{
				this.toRemove.Add(ideo);
			}
			return true;
		}

		// Token: 0x06008DE8 RID: 36328 RVA: 0x0033CC50 File Offset: 0x0033AE50
		private bool CanRemoveIdeo(Ideo ideo)
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (faction.ideos != null && faction.ideos.AllIdeos.Contains(ideo))
				{
					return false;
				}
			}
			foreach (Pawn pawn in PawnsFinder.AllMaps)
			{
				if (pawn.ideo != null && pawn.ideo.Ideo == ideo)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06008DE9 RID: 36329 RVA: 0x0033CD14 File Offset: 0x0033AF14
		public void Notify_GameStarted()
		{
			foreach (Ideo ideo in this.ideos)
			{
				ideo.Notify_GameStarted();
			}
		}

		// Token: 0x06008DEA RID: 36330 RVA: 0x0033CD64 File Offset: 0x0033AF64
		public void ExposeData()
		{
			Scribe_Collections.Look<Ideo>(ref this.ideos, "ideos", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Ideo>(ref this.toRemove, "toRemove", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.classicMode, "classicMode", false, false);
			Scribe_Collections.Look<StyleCategoryDef>(ref this.selectedStyleCategories, "selectedStyleCategories", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksToNextGauranlenSpawn, "ticksToNextGauranlenSpawn", 3600000, false);
			Scribe_Values.Look<int>(ref this.lastPsychicRitualPerformedTick, "lastPsychicRitualPerformedTick", 0, false);
			Scribe_Values.Look<int>(ref this.lastResettledTick, "lastResettledTick", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.ideos.RemoveAll((Ideo x) => x == null) != 0)
				{
					Log.Error("Some ideoligions were null after loading.");
				}
				if (this.toRemove == null)
				{
					this.toRemove = new List<Ideo>();
				}
				if (this.classicMode && this.selectedStyleCategories == null)
				{
					this.selectedStyleCategories = new List<StyleCategoryDef>();
				}
				BackCompatibility.IdeoManagerPostloadInit();
			}
		}

		// Token: 0x040056F6 RID: 22262
		private List<Faction> npcWithIdeoTmp = new List<Faction>();

		// Token: 0x040056F7 RID: 22263
		private List<LordJob_Ritual> activeRitualsTmp = new List<LordJob_Ritual>();

		// Token: 0x040056F8 RID: 22264
		[Unsaved(false)]
		private Ideo horaxianInt;

		// Token: 0x040056F9 RID: 22265
		private List<Ideo> ideos = new List<Ideo>();

		// Token: 0x040056FA RID: 22266
		private List<Ideo> toRemove = new List<Ideo>();

		// Token: 0x040056FB RID: 22267
		public bool classicMode;

		// Token: 0x040056FC RID: 22268
		public List<StyleCategoryDef> selectedStyleCategories = new List<StyleCategoryDef>();

		// Token: 0x040056FD RID: 22269
		public int ticksToNextGauranlenSpawn = 3600000;

		// Token: 0x040056FE RID: 22270
		public int lastPsychicRitualPerformedTick;

		// Token: 0x040056FF RID: 22271
		public int lastResettledTick;
	}
}
