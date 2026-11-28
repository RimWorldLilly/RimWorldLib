using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200174D RID: 5965
	public class Precept_RoleMulti : Precept_Role
	{
		// Token: 0x0600948C RID: 38028 RVA: 0x003628CA File Offset: 0x00360ACA
		public override IEnumerable<Pawn> ChosenPawns()
		{
			Precept_RoleMulti.<ChosenPawns>d__2 <ChosenPawns>d__ = new Precept_RoleMulti.<ChosenPawns>d__2(-2);
			<ChosenPawns>d__.<>4__this = this;
			return <ChosenPawns>d__;
		}

		// Token: 0x0600948D RID: 38029 RVA: 0x003628DA File Offset: 0x00360ADA
		public override Pawn ChosenPawnSingle()
		{
			return null;
		}

		// Token: 0x0600948E RID: 38030 RVA: 0x003628DD File Offset: 0x00360ADD
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, generatingFor);
			this.active = true;
		}

		// Token: 0x0600948F RID: 38031 RVA: 0x003628F0 File Offset: 0x00360AF0
		public override void Assign(Pawn p, bool addThoughts)
		{
			if (!this.IsAssigned(p))
			{
				IdeoRoleInstance ideoRoleInstance = this.chosenPawnsCache.FirstOrDefault((IdeoRoleInstance c) => c.pawn == p);
				if (ideoRoleInstance != null)
				{
					this.chosenPawnsCache.Remove(ideoRoleInstance);
					this.chosenPawns.Add(ideoRoleInstance);
				}
				else
				{
					this.chosenPawns.Add(new IdeoRoleInstance(this)
					{
						pawn = p
					});
					this.FillOrUpdateAbilities();
				}
				base.Notify_PawnAssigned(p);
			}
		}

		// Token: 0x06009490 RID: 38032 RVA: 0x00362980 File Offset: 0x00360B80
		public override void FillOrUpdateAbilities()
		{
			foreach (IdeoRoleInstance ideoRoleInstance in this.chosenPawns)
			{
				ideoRoleInstance.abilities = base.FillOrUpdateAbilityList(ideoRoleInstance.pawn, ideoRoleInstance.abilities);
			}
		}

		// Token: 0x06009491 RID: 38033 RVA: 0x003629E4 File Offset: 0x00360BE4
		public override List<Ability> AbilitiesFor(Pawn p)
		{
			for (int i = 0; i < this.chosenPawns.Count; i++)
			{
				if (this.chosenPawns[i].pawn == p)
				{
					return this.chosenPawns[i].abilities;
				}
			}
			return null;
		}

		// Token: 0x06009492 RID: 38034 RVA: 0x00362A30 File Offset: 0x00360C30
		public override bool IsAssigned(Pawn p)
		{
			for (int i = 0; i < this.chosenPawns.Count; i++)
			{
				if (this.chosenPawns[i].pawn == p)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009493 RID: 38035 RVA: 0x00362A6C File Offset: 0x00360C6C
		public override void Unassign(Pawn p, bool generateThoughts)
		{
			if (!this.IsAssigned(p))
			{
				return;
			}
			IdeoRoleInstance ideoRoleInstance = this.chosenPawns.FirstOrDefault((IdeoRoleInstance c) => c.pawn == p);
			if (ideoRoleInstance != null)
			{
				this.chosenPawns.Remove(ideoRoleInstance);
				this.chosenPawnsCache.Add(ideoRoleInstance);
				Pawn_NeedsTracker needs = p.needs;
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
								memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleLost, this), null);
							}
						}
					}
				}
				base.Notify_PawnUnassigned(p);
			}
		}

		// Token: 0x06009494 RID: 38036 RVA: 0x00362B18 File Offset: 0x00360D18
		public override void RecacheActivity()
		{
			this.pawnsToRemove.Clear();
			foreach (IdeoRoleInstance ideoRoleInstance in this.chosenPawns)
			{
				if (ideoRoleInstance.pawn == null || !base.ValidatePawn(ideoRoleInstance.pawn))
				{
					this.pawnsToRemove.Add(ideoRoleInstance);
				}
			}
			foreach (IdeoRoleInstance ideoRoleInstance2 in this.pawnsToRemove)
			{
				this.Unassign(ideoRoleInstance2.pawn, false);
				if (this.chosenPawns.Contains(ideoRoleInstance2))
				{
					this.chosenPawns.Remove(ideoRoleInstance2);
				}
			}
		}

		// Token: 0x06009495 RID: 38037 RVA: 0x00362BF4 File Offset: 0x00360DF4
		public override void ExposeData()
		{
			base.ExposeData();
			if (!GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
			{
				Scribe_Collections.Look<IdeoRoleInstance>(ref this.chosenPawns, "chosenPawns", LookMode.Deep, new object[]
				{
					this
				});
				Scribe_Collections.Look<IdeoRoleInstance>(ref this.chosenPawnsCache, "chosenPawnsCache", LookMode.Deep, new object[]
				{
					this
				});
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.chosenPawns.RemoveAll((IdeoRoleInstance c) => c.pawn == null || !base.ValidatePawn(c.pawn));
				this.chosenPawnsCache.RemoveAll((IdeoRoleInstance c) => c.pawn == null);
				foreach (IdeoRoleInstance ideoRoleInstance in this.chosenPawns)
				{
					ideoRoleInstance.sourceRole = this;
				}
			}
		}

		// Token: 0x06009496 RID: 38038 RVA: 0x00362CD4 File Offset: 0x00360ED4
		public override void CopyTo(Precept precept)
		{
			base.CopyTo(precept);
			Precept_RoleMulti precept_RoleMulti = (Precept_RoleMulti)precept;
			precept_RoleMulti.chosenPawns.Clear();
			precept_RoleMulti.chosenPawns.AddRange(this.chosenPawns);
			precept_RoleMulti.chosenPawnsCache.Clear();
			precept_RoleMulti.chosenPawnsCache.AddRange(this.chosenPawnsCache);
		}

		// Token: 0x0400599B RID: 22939
		public List<IdeoRoleInstance> chosenPawns = new List<IdeoRoleInstance>();

		// Token: 0x0400599C RID: 22940
		private List<IdeoRoleInstance> chosenPawnsCache = new List<IdeoRoleInstance>();

		// Token: 0x0400599D RID: 22941
		private List<IdeoRoleInstance> pawnsToRemove = new List<IdeoRoleInstance>();
	}
}
