using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02000939 RID: 2361
	[StaticConstructorOnStartup]
	public class Lord : IExposable, ILoadReferenceable, ISignalReceiver, IDisposable
	{
		// Token: 0x17000D3C RID: 3388
		// (get) Token: 0x0600483C RID: 18492 RVA: 0x001C5D3D File Offset: 0x001C3F3D
		public Map Map
		{
			get
			{
				return this.lordManager.map;
			}
		}

		// Token: 0x17000D3D RID: 3389
		// (get) Token: 0x0600483D RID: 18493 RVA: 0x001C5D4A File Offset: 0x001C3F4A
		public StateGraph Graph
		{
			get
			{
				return this.graph;
			}
		}

		// Token: 0x17000D3E RID: 3390
		// (get) Token: 0x0600483E RID: 18494 RVA: 0x001C5D52 File Offset: 0x001C3F52
		public LordToil CurLordToil
		{
			get
			{
				return this.curLordToil;
			}
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x0600483F RID: 18495 RVA: 0x001C5D5A File Offset: 0x001C3F5A
		public LordJob LordJob
		{
			get
			{
				return this.curJob;
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x06004840 RID: 18496 RVA: 0x001C5D62 File Offset: 0x001C3F62
		private bool CanExistWithoutPawns
		{
			get
			{
				return this.curJob.ShouldExistWithoutPawns || this.Map.deferredSpawner.GetRequestByLord(this) != null;
			}
		}

		// Token: 0x17000D41 RID: 3393
		// (get) Token: 0x06004841 RID: 18497 RVA: 0x001C5D87 File Offset: 0x001C3F87
		private bool ShouldExist
		{
			get
			{
				return this.ownedPawns.Count > 0 || this.CanExistWithoutPawns || (this.ownedBuildings.Count > 0 && this.curJob.KeepExistingWhileHasAnyBuilding);
			}
		}

		// Token: 0x17000D42 RID: 3394
		// (get) Token: 0x06004842 RID: 18498 RVA: 0x001C5DBC File Offset: 0x001C3FBC
		public bool AnyActivePawn
		{
			get
			{
				for (int i = 0; i < this.ownedPawns.Count; i++)
				{
					if (this.ownedPawns[i].mindState != null && this.ownedPawns[i].mindState.Active)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x001C5E0D File Offset: 0x001C400D
		private void Init()
		{
			this.initialized = true;
			this.initialColonyHealthTotal = this.Map.wealthWatcher.HealthTotal;
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x001C5E2C File Offset: 0x001C402C
		public string GetUniqueLoadID()
		{
			return "Lord_" + this.loadID.ToString();
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x001C5E44 File Offset: 0x001C4044
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.extraForbiddenThings.RemoveAll((Thing x) => x.DestroyedOrNull());
				this.ownedPawns.RemoveAll((Pawn x) => x.DestroyedOrNull());
				this.ownedBuildings.RemoveAll((Building x) => x.DestroyedOrNull());
				this.ownedCorpses.RemoveAll((Corpse x) => x.DestroyedOrNull());
			}
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Collections.Look<Thing>(ref this.extraForbiddenThings, "extraForbiddenThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.ownedPawns, "ownedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Building>(ref this.ownedBuildings, "ownedBuildings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Corpse>(ref this.ownedCorpses, "ownedCorpses", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<LordJob>(ref this.curJob, "lordJob", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.initialized, "initialized", true, false);
			Scribe_Values.Look<int>(ref this.ticksInToil, "ticksInToil", 0, false);
			Scribe_Values.Look<int>(ref this.numPawnsEverGained, "numPawnsEverGained", 0, false);
			Scribe_Values.Look<int>(ref this.numPawnsLostViolently, "numPawnsLostViolently", 0, false);
			Scribe_Values.Look<int>(ref this.initialColonyHealthTotal, "initialColonyHealthTotal", 0, false);
			Scribe_Values.Look<int>(ref this.lastPawnHarmTick, "lastPawnHarmTick", -99999, false);
			Scribe_Values.Look<string>(ref this.inSignalLeave, "inSignalLeave", null, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.extraForbiddenThings.RemoveAll((Thing x) => x == null);
				this.ownedPawns.RemoveAll((Pawn x) => x == null);
				this.ownedBuildings.RemoveAll((Building x) => x == null);
				this.ownedCorpses.RemoveAll((Corpse x) => x == null);
				this.ownedPawns.ForEach(delegate(Pawn p)
				{
					p.lord = this;
				});
			}
			this.ExposeData_StateGraph();
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x001C6104 File Offset: 0x001C4304
		public AcceptanceReport AllowsDrafting(Pawn pawn)
		{
			if (this.curJob == null)
			{
				return false;
			}
			return this.curJob.AllowsDrafting(pawn);
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x001C6121 File Offset: 0x001C4321
		public AcceptanceReport AllowsFloatMenu(Pawn pawn)
		{
			if (this.curJob == null)
			{
				return true;
			}
			return this.curJob.AllowsFloatMenu(pawn);
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x001C613E File Offset: 0x001C433E
		public bool BlocksSocialInteraction(Pawn pawn)
		{
			LordJob lordJob = this.curJob;
			return lordJob != null && lordJob.BlocksSocialInteraction(pawn);
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x001C6152 File Offset: 0x001C4352
		public bool PrisonerSecure(Pawn pawn)
		{
			LordJob lordJob = this.curJob;
			return lordJob != null && lordJob.PrisonerSecure(pawn);
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x001C6166 File Offset: 0x001C4366
		public AcceptanceReport AbilityAllowed(Ability ability)
		{
			if (this.curJob == null)
			{
				return true;
			}
			return this.curJob.AbilityAllowed(ability);
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x001C6183 File Offset: 0x001C4383
		public ThinkResult Notify_DutyConstantResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			if (this.curJob == null)
			{
				return result;
			}
			return this.curJob.Notify_DutyConstantResult(result, pawn, issueParams);
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x001C619D File Offset: 0x001C439D
		public ThinkResult Notify_DutyResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			if (this.curJob == null)
			{
				return result;
			}
			return this.curJob.Notify_DutyResult(result, pawn, issueParams);
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x001C61B8 File Offset: 0x001C43B8
		private void ExposeData_StateGraph()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpLordToilData.Clear();
				for (int i = 0; i < this.graph.lordToils.Count; i++)
				{
					if (this.graph.lordToils[i].data != null)
					{
						this.tmpLordToilData.Add(i, this.graph.lordToils[i].data);
					}
				}
				this.tmpTriggerData.Clear();
				int num = 0;
				for (int j = 0; j < this.graph.transitions.Count; j++)
				{
					for (int k = 0; k < this.graph.transitions[j].triggers.Count; k++)
					{
						if (this.graph.transitions[j].triggers[k].data != null)
						{
							this.tmpTriggerData.Add(num, this.graph.transitions[j].triggers[k].data);
						}
						num++;
					}
				}
				this.tmpCurLordToilIdx = this.graph.lordToils.IndexOf(this.curLordToil);
			}
			Scribe_Collections.Look<int, LordToilData>(ref this.tmpLordToilData, "lordToilData", LookMode.Value, LookMode.Deep);
			Scribe_Collections.Look<int, TriggerData>(ref this.tmpTriggerData, "triggerData", LookMode.Value, LookMode.Deep);
			Scribe_Values.Look<int>(ref this.tmpCurLordToilIdx, "curLordToilIdx", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.curJob.LostImportantReferenceDuringLoading)
				{
					this.lordManager.RemoveLord(this);
					return;
				}
				LordJob lordJob = this.curJob;
				this.curJob = null;
				this.SetJob(lordJob, true);
				foreach (KeyValuePair<int, LordToilData> keyValuePair in this.tmpLordToilData)
				{
					if (keyValuePair.Key < 0 || keyValuePair.Key >= this.graph.lordToils.Count)
					{
						string[] array = new string[6];
						array[0] = "Could not find lord toil for lord toil data of type \"";
						int num2 = 1;
						Type type = keyValuePair.Value.GetType();
						array[num2] = ((type != null) ? type.ToString() : null);
						array[2] = "\" (lord job: \"";
						int num3 = 3;
						Type type2 = this.curJob.GetType();
						array[num3] = ((type2 != null) ? type2.ToString() : null);
						array[4] = "\"), because lord toil index is out of bounds: ";
						array[5] = keyValuePair.Key.ToString();
						Log.Error(string.Concat(array));
					}
					else
					{
						this.graph.lordToils[keyValuePair.Key].data = keyValuePair.Value;
					}
				}
				this.tmpLordToilData.Clear();
				foreach (KeyValuePair<int, TriggerData> keyValuePair2 in this.tmpTriggerData)
				{
					Trigger triggerByIndex = this.GetTriggerByIndex(keyValuePair2.Key);
					if (triggerByIndex == null)
					{
						string[] array2 = new string[6];
						array2[0] = "Could not find trigger for trigger data of type \"";
						int num4 = 1;
						Type type3 = keyValuePair2.Value.GetType();
						array2[num4] = ((type3 != null) ? type3.ToString() : null);
						array2[2] = "\" (lord job: \"";
						int num5 = 3;
						Type type4 = this.curJob.GetType();
						array2[num5] = ((type4 != null) ? type4.ToString() : null);
						array2[4] = "\"), because trigger index is out of bounds: ";
						array2[5] = keyValuePair2.Key.ToString();
						Log.Error(string.Concat(array2));
					}
					else
					{
						triggerByIndex.data = keyValuePair2.Value;
					}
				}
				this.tmpTriggerData.Clear();
				if (this.tmpCurLordToilIdx < 0 || this.tmpCurLordToilIdx >= this.graph.lordToils.Count)
				{
					string str = "Current lord toil index out of bounds (lord job: \"";
					Type type5 = this.curJob.GetType();
					Log.Error(str + ((type5 != null) ? type5.ToString() : null) + "\"): " + this.tmpCurLordToilIdx.ToString());
					return;
				}
				this.curLordToil = this.graph.lordToils[this.tmpCurLordToilIdx];
			}
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x001C65C4 File Offset: 0x001C47C4
		public void SetJob(LordJob lordJob, bool loading = false)
		{
			if (this.curJob != null)
			{
				this.curJob.Cleanup();
			}
			this.curJob = lordJob;
			this.curLordToil = null;
			lordJob.lord = this;
			if (!loading)
			{
				lordJob.Notify_AddedToLord();
			}
			Rand.PushState();
			Rand.Seed = this.loadID * 193;
			this.graph = lordJob.CreateGraph();
			Rand.PopState();
			this.graph.ErrorCheck();
			if (this.faction != null && !this.faction.IsPlayer && this.faction.def.autoFlee && !this.faction.neverFlee && lordJob.AddFleeToil && lordJob.lord.Map.CanEverExit)
			{
				LordToil_PanicFlee lordToil_PanicFlee = new LordToil_PanicFlee();
				lordToil_PanicFlee.useAvoidGrid = true;
				Action <>9__0;
				for (int i = 0; i < this.graph.lordToils.Count; i++)
				{
					Transition transition = new Transition(this.graph.lordToils[i], lordToil_PanicFlee, false, true);
					transition.AddPreAction(new TransitionAction_Message("MessageFightersFleeing".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
					transition.AddTrigger(new Trigger_FractionPawnsLost(this.faction.def.attackersDownPercentageRangeForAutoFlee.RandomInRangeSeeded(this.loadID)));
					Transition transition2 = transition;
					Action action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate()
						{
							QuestUtility.SendQuestTargetSignals(lordJob.lord.questTags, "Fleeing", lordJob.lord.Named("SUBJECT"));
						});
					}
					transition2.AddPostAction(new TransitionAction_Custom(action));
					this.graph.AddTransition(transition, true);
				}
				this.graph.AddToil(lordToil_PanicFlee);
			}
			for (int j = 0; j < this.graph.lordToils.Count; j++)
			{
				this.graph.lordToils[j].lord = this;
			}
			for (int k = 0; k < this.ownedPawns.Count; k++)
			{
				this.Map.attackTargetsCache.UpdateTarget(this.ownedPawns[k]);
			}
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x001C682C File Offset: 0x001C4A2C
		public void Cleanup()
		{
			try
			{
				this.curJob.Cleanup();
			}
			catch (Exception ex)
			{
				string str = "Error in LordJob.Cleanup(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			if (this.curLordToil != null)
			{
				try
				{
					this.curLordToil.Cleanup();
				}
				catch (Exception ex3)
				{
					string str2 = "Error in LordToil.Cleanup(): ";
					Exception ex4 = ex3;
					Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (this.ownedPawns[i].mindState != null)
				{
					this.ownedPawns[i].mindState.duty = null;
				}
				if (this.ownedPawns[i].lord == this)
				{
					this.ownedPawns[i].lord = null;
				}
				this.Map.attackTargetsCache.UpdateTarget(this.ownedPawns[i]);
				if (this.curJob.EndPawnJobOnCleanup(this.ownedPawns[i]) && this.ownedPawns[i].Spawned && this.ownedPawns[i].CurJob != null && (!this.curJob.DontInterruptLayingPawnsOnCleanup || !RestUtility.IsLayingForJobCleanup(this.ownedPawns[i])))
				{
					this.ownedPawns[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			try
			{
				this.curJob.PostCleanup();
			}
			catch (Exception ex5)
			{
				string str3 = "Error in LordJob.PostCleanup(): ";
				Exception ex6 = ex5;
				Log.Error(str3 + ((ex6 != null) ? ex6.ToString() : null));
			}
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x001C69F0 File Offset: 0x001C4BF0
		public bool CanAddPawn(Pawn p)
		{
			return this.curLordToil.CanAddPawn(p);
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x001C6A00 File Offset: 0x001C4C00
		public void AddPawns(IEnumerable<Pawn> pawns, bool updateDuties = true)
		{
			foreach (Pawn p in pawns)
			{
				this.AddPawnInternal(p, false);
			}
			if (updateDuties)
			{
				try
				{
					this.curLordToil.UpdateAllDuties();
				}
				catch (Exception ex)
				{
					string str = "Error in LordToil.UpdateAllDuties(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x001C6A84 File Offset: 0x001C4C84
		public void AddPawn(Pawn p)
		{
			if (!this.CanAddPawn(p))
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to add pawn ",
					(p != null) ? p.ToString() : null,
					" to lord ",
					(this != null) ? this.ToString() : null,
					" but this pawn can't be added to this lord."
				}));
				return;
			}
			this.AddPawnInternal(p, true);
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x001C6AEC File Offset: 0x001C4CEC
		private void AddPawnInternal(Pawn p, bool updateDuties)
		{
			if (this.ownedPawns.Contains(p))
			{
				Log.Error(string.Concat(new string[]
				{
					"Lord for ",
					this.faction.ToStringSafe<Faction>(),
					" tried to add ",
					(p != null) ? p.ToString() : null,
					" whom it already controls."
				}));
				return;
			}
			if (p.GetLord() != null)
			{
				string[] array = new string[7];
				array[0] = "Tried to add pawn ";
				array[1] = ((p != null) ? p.ToString() : null);
				array[2] = " to lord with lord job";
				int num = 3;
				LordJob lordJob = this.LordJob;
				array[num] = ((lordJob != null) ? lordJob.ToString() : null);
				array[4] = " but this pawn is already a member of lord with lord job";
				int num2 = 5;
				LordJob lordJob2 = p.GetLord().LordJob;
				array[num2] = ((lordJob2 != null) ? lordJob2.ToString() : null);
				array[6] = ". Pawns can't be members of more than one lord at the same time.";
				Log.Error(string.Concat(array));
				return;
			}
			this.ownedPawns.Add(p);
			p.lord = this;
			this.numPawnsEverGained++;
			this.Map.attackTargetsCache.UpdateTarget(p);
			if (updateDuties)
			{
				try
				{
					this.curLordToil.UpdateAllDuties();
				}
				catch (Exception ex)
				{
					string str = "Error in LordToil.UpdateAllDuties(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			try
			{
				LordJob lordJob3 = this.curJob;
				if (lordJob3 != null)
				{
					lordJob3.Notify_PawnAdded(p);
				}
			}
			catch (Exception ex3)
			{
				string str2 = "Error in LordJob.Notify_PawnAdded(): ";
				Exception ex4 = ex3;
				Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
			}
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x001C6C78 File Offset: 0x001C4E78
		public void AddBuilding(Building b)
		{
			if (this.ownedBuildings.Contains(b))
			{
				Log.Error(string.Concat(new string[]
				{
					"Lord for ",
					this.faction.ToStringSafe<Faction>(),
					" tried to add ",
					(b != null) ? b.ToString() : null,
					" which it already controls."
				}));
				return;
			}
			this.ownedBuildings.Add(b);
			try
			{
				this.curLordToil.UpdateAllDuties();
			}
			catch (Exception ex)
			{
				string str = "Error in LordToil.UpdateAllDuties(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			try
			{
				this.curJob.Notify_BuildingAdded(b);
			}
			catch (Exception ex3)
			{
				string str2 = "Error in LordJob.Notify_BuildingAdded(): ";
				Exception ex4 = ex3;
				Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
			}
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x001C6D5C File Offset: 0x001C4F5C
		public void AddCorpse(Corpse c)
		{
			if (this.ownedCorpses.Contains(c))
			{
				Log.Error(string.Concat(new string[]
				{
					"Lord for ",
					this.faction.ToStringSafe<Faction>(),
					" tried to add ",
					(c != null) ? c.ToString() : null,
					" which it already controls."
				}));
				return;
			}
			this.ownedCorpses.Add(c);
			try
			{
				this.curJob.Notify_CorpseAdded(c);
			}
			catch (Exception ex)
			{
				string str = "Error in LordJob.Notify_CorpseAdded(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x001C6E08 File Offset: 0x001C5008
		public void RemovePawn(Pawn p)
		{
			this.ownedPawns.Remove(p);
			if (p.mindState != null)
			{
				p.mindState.duty = null;
			}
			if (p.lord == this)
			{
				p.lord = null;
			}
			this.Map.attackTargetsCache.UpdateTarget(p);
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x001C6E58 File Offset: 0x001C5058
		public void RemovePawns(List<Pawn> pawns)
		{
			foreach (Pawn p in pawns)
			{
				this.RemovePawn(p);
			}
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x001C6EA8 File Offset: 0x001C50A8
		public void RemoveAllPawns(bool interruptPawnJobs = true)
		{
			foreach (Pawn pawn in this.ownedPawns)
			{
				if (pawn.mindState != null)
				{
					pawn.mindState.duty = null;
					if (interruptPawnJobs)
					{
						Pawn_JobTracker jobs = pawn.jobs;
						Lord lord;
						if (jobs == null)
						{
							lord = null;
						}
						else
						{
							Job job = jobs.curJob;
							lord = ((job != null) ? job.lord : null);
						}
						if (lord == this)
						{
							pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
						}
					}
				}
				this.Map.attackTargetsCache.UpdateTarget(pawn);
				if (pawn.lord == this)
				{
					pawn.lord = null;
				}
			}
			this.ownedPawns.Clear();
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x001C6F68 File Offset: 0x001C5168
		public void RemoveAllBuildings()
		{
			this.ownedBuildings.Clear();
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x001C6F78 File Offset: 0x001C5178
		public void GotoToil(LordToil newLordToil)
		{
			LordToil previousToil = this.curLordToil;
			if (this.curLordToil != null)
			{
				try
				{
					this.curLordToil.Cleanup();
				}
				catch (Exception ex)
				{
					string str = "Error in LordToil.Cleanup(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			this.curLordToil = newLordToil;
			this.ticksInToil = 0;
			if (this.curLordToil.lord != this)
			{
				Log.Error("curLordToil lord is " + ((this.curLordToil.lord == null) ? "null (forgot to add toil to graph?)" : this.curLordToil.lord.ToString()));
				this.curLordToil.lord = this;
			}
			try
			{
				this.curLordToil.Init();
			}
			catch (Exception ex3)
			{
				string str2 = "Error in LordToil.Init(): ";
				Exception ex4 = ex3;
				Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
			}
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				if (this.graph.transitions[i].sources.Contains(this.curLordToil))
				{
					this.graph.transitions[i].SourceToilBecameActive(this.graph.transitions[i], previousToil);
				}
			}
			try
			{
				this.curLordToil.UpdateAllDuties();
			}
			catch (Exception ex5)
			{
				string str3 = "Error in LordToil.UpdateAllDuties(): ";
				Exception ex6 = ex5;
				Log.Error(str3 + ((ex6 != null) ? ex6.ToString() : null));
			}
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x001C7108 File Offset: 0x001C5308
		public void LordTick()
		{
			if (this.ticksInToil % 60 == 0)
			{
				for (int i = 0; i < this.ownedPawns.Count; i++)
				{
					Pawn pawn = this.ownedPawns[i];
					if (Find.WorldPawns.GetSituation(pawn) == WorldPawnSituation.Free)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(76, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Lord ");
						defaultInterpolatedStringHandler.AppendFormatted<Lord>(this);
						defaultInterpolatedStringHandler.AppendLiteral(" (");
						defaultInterpolatedStringHandler.AppendFormatted<LordJob>(this.curJob);
						defaultInterpolatedStringHandler.AppendLiteral(") owns a free world pawn ");
						defaultInterpolatedStringHandler.AppendFormatted(pawn.LabelShort);
						defaultInterpolatedStringHandler.AppendLiteral(". Is there WorldPawnSituation to be defined?");
						Log.ErrorOnce(defaultInterpolatedStringHandler.ToStringAndClear(), this.loadID ^ 1357461551);
					}
				}
			}
			if (!this.initialized)
			{
				this.Init();
			}
			this.curJob.LordJobTick();
			this.curLordToil.LordToilTick();
			this.CheckTransitionOnSignal(TriggerSignal.ForTick);
			this.ticksInToil++;
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x001C720C File Offset: 0x001C540C
		private Trigger GetTriggerByIndex(int index)
		{
			int num = 0;
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				for (int j = 0; j < this.graph.transitions[i].triggers.Count; j++)
				{
					if (num == index)
					{
						return this.graph.transitions[i].triggers[j];
					}
					num++;
				}
			}
			return null;
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x001C7282 File Offset: 0x001C5482
		public void ReceiveMemo(string memo)
		{
			this.CheckTransitionOnSignal(TriggerSignal.ForMemo(memo));
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x001C7294 File Offset: 0x001C5494
		public void Notify_FactionRelationsChanged(Faction otherFaction, FactionRelationKind previousRelationKind)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.FactionRelationsChanged,
				faction = otherFaction,
				previousRelationKind = new FactionRelationKind?(previousRelationKind)
			});
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (this.ownedPawns[i].Spawned)
				{
					this.ownedPawns[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x0600485F RID: 18527 RVA: 0x001C7311 File Offset: 0x001C5511
		private void Destroy()
		{
			this.lordManager.RemoveLord(this);
			this.curJob.Notify_LordDestroyed();
			if (this.faction != null)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "AllEnemiesDefeated");
			}
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x001C7344 File Offset: 0x001C5544
		public void Notify_PawnJobDone(Pawn pawn, JobCondition condition)
		{
			try
			{
				this.curJob.Notify_PawnJobDone(pawn, condition);
			}
			catch (Exception ex)
			{
				string str = "Error in LordJob.Notify_PawnJobDone(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			try
			{
				this.curLordToil.Notify_PawnJobDone(pawn, condition);
			}
			catch (Exception ex3)
			{
				string str2 = "Error in LordToil.Notify_PawnJobDone(): ";
				Exception ex4 = ex3;
				Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
			}
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x001C73CC File Offset: 0x001C55CC
		public void Notify_PawnLost(Pawn pawn, PawnLostCondition cond, DamageInfo? dinfo = null)
		{
			if (!this.curJob.ShouldRemovePawn(pawn, cond))
			{
				return;
			}
			if (this.ownedPawns.Contains(pawn))
			{
				this.RemovePawn(pawn);
				if (cond == PawnLostCondition.Incapped || cond == PawnLostCondition.Killed || cond == PawnLostCondition.MadePrisoner)
				{
					this.numPawnsLostViolently++;
				}
				try
				{
					this.curJob.Notify_PawnLost(pawn, cond);
				}
				catch (Exception ex)
				{
					string str = "Error in LordJob.Notify_PawnLost(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				if (this.lordManager.lords.Contains(this))
				{
					if (!this.ShouldExist)
					{
						this.Destroy();
						return;
					}
					try
					{
						this.curLordToil.Notify_PawnLost(pawn, cond);
					}
					catch (Exception ex3)
					{
						string str2 = "Error in LordToil.Notify_PawnLost(): ";
						Exception ex4 = ex3;
						Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
					}
					TriggerSignal signal = default(TriggerSignal);
					signal.type = TriggerSignalType.PawnLost;
					signal.thing = pawn;
					signal.condition = cond;
					if (dinfo != null)
					{
						signal.dinfo = dinfo.Value;
					}
					this.CheckTransitionOnSignal(signal);
				}
				return;
			}
			Log.Error("Lord lost pawn " + ((pawn != null) ? pawn.ToString() : null) + " it didn't have. Condition=" + cond.ToString());
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x001C7524 File Offset: 0x001C5724
		public void Notify_InMentalState(Pawn pawn, MentalStateDef def)
		{
			this.curJob.Notify_InMentalState(pawn, def);
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x001C7534 File Offset: 0x001C5734
		public void Notify_BuildingLost(Building building, DamageInfo? dinfo = null)
		{
			if (this.ownedBuildings.Contains(building))
			{
				this.ownedBuildings.Remove(building);
				this.curJob.Notify_BuildingLost(building);
				if (this.lordManager.lords.Contains(this))
				{
					if (!this.ShouldExist)
					{
						this.Destroy();
						return;
					}
					this.curLordToil.Notify_BuildingLost(building);
					TriggerSignal signal = default(TriggerSignal);
					signal.type = TriggerSignalType.BuildingLost;
					signal.thing = building;
					if (dinfo != null)
					{
						signal.dinfo = dinfo.Value;
					}
					this.CheckTransitionOnSignal(signal);
				}
				return;
			}
			Log.Error("Lord lost building " + ((building != null) ? building.ToString() : null) + " it didn't have.");
		}

		// Token: 0x06004864 RID: 18532 RVA: 0x001C75F4 File Offset: 0x001C57F4
		public void Notify_CorpseLost(Corpse corpse, DamageInfo? dinfo = null)
		{
			if (this.ownedCorpses.Contains(corpse))
			{
				this.ownedCorpses.Remove(corpse);
				this.curJob.Notify_CorpseLost(corpse);
				if (this.lordManager.lords.Contains(this))
				{
					if (!this.ShouldExist)
					{
						this.Destroy();
						return;
					}
					this.curLordToil.Notify_CorpseLost(corpse);
					TriggerSignal signal = default(TriggerSignal);
					signal.type = TriggerSignalType.CorpseLost;
					signal.thing = corpse;
					if (dinfo != null)
					{
						signal.dinfo = dinfo.Value;
					}
					this.CheckTransitionOnSignal(signal);
				}
				return;
			}
			Log.Error("Lord lost corpse " + ((corpse != null) ? corpse.ToString() : null) + " it didn't have.");
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x001C76B4 File Offset: 0x001C58B4
		public void Notify_BuildingDamaged(Building building, DamageInfo dinfo)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.BuildingDamaged,
				thing = building,
				dinfo = dinfo
			});
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x001C76EC File Offset: 0x001C58EC
		public void Notify_PawnDamaged(Pawn victim, DamageInfo dinfo)
		{
			LordToil lordToil = this.curLordToil;
			if (lordToil != null)
			{
				lordToil.Notify_PawnDamaged(victim, dinfo);
			}
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.PawnDamaged,
				thing = victim,
				dinfo = dinfo
			});
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x001C7734 File Offset: 0x001C5934
		public void Notify_PawnAttemptArrested(Pawn victim)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.PawnArrestAttempted,
				thing = victim
			});
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x001C7764 File Offset: 0x001C5964
		public void Notify_Clamor(Thing source, ClamorDef clamorType)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.Clamor,
				thing = source,
				clamorType = clamorType
			});
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x001C779C File Offset: 0x001C599C
		public void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
		{
			LordToil lordToil = this.curLordToil;
			if (lordToil != null)
			{
				lordToil.Notify_PawnAcquiredTarget(detector, newTarg);
			}
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.AcquiredTarget,
				thing = detector,
				otherThing = newTarg
			});
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x001C77E5 File Offset: 0x001C59E5
		public void Notify_BuildingSpawnedOnMap(Building b)
		{
			LordToil lordToil = this.curLordToil;
			if (lordToil == null)
			{
				return;
			}
			lordToil.Notify_BuildingSpawnedOnMap(b);
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x001C77F8 File Offset: 0x001C59F8
		public void Notify_BuildingDespawnedOnMap(Building b)
		{
			LordToil lordToil = this.curLordToil;
			if (lordToil == null)
			{
				return;
			}
			lordToil.Notify_BuildingDespawnedOnMap(b);
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x001C780B File Offset: 0x001C5A0B
		public void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.curLordToil.Notify_ReachedDutyLocation(pawn);
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x001C7819 File Offset: 0x001C5A19
		public void Notify_MapRemoved()
		{
			LordJob lordJob = this.curJob;
			if (lordJob == null)
			{
				return;
			}
			lordJob.Notify_MapRemoved();
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x001C782B File Offset: 0x001C5A2B
		public void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			this.curLordToil.Notify_ConstructionFailed(pawn, frame, newBlueprint);
		}

		// Token: 0x0600486F RID: 18543 RVA: 0x001C783B File Offset: 0x001C5A3B
		public void Notify_ConstructionCompleted(Pawn pawn, Building building)
		{
			this.curLordToil.Notify_ConstructionCompleted(pawn, building);
		}

		// Token: 0x06004870 RID: 18544 RVA: 0x001C784C File Offset: 0x001C5A4C
		public void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalLeave)
			{
				if (this.ownedPawns.Any<Pawn>() && this.faction != null)
				{
					Messages.Message("MessagePawnsLeaving".Translate(this.faction.def.pawnsPlural), this.ownedPawns, MessageTypeDefOf.NeutralEvent, true);
				}
				LordToil lordToil = this.Graph.lordToils.Find((LordToil st) => st is LordToil_PanicFlee);
				if (lordToil != null)
				{
					this.GotoToil(lordToil);
				}
				else
				{
					this.lordManager.RemoveLord(this);
				}
			}
			this.CheckTransitionOnSignal(TriggerSignal.ForSignal(signal));
		}

		// Token: 0x06004871 RID: 18545 RVA: 0x001C7914 File Offset: 0x001C5B14
		public void Notify_DormancyWakeup()
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.DormancyWakeup
			});
		}

		// Token: 0x06004872 RID: 18546 RVA: 0x001C793C File Offset: 0x001C5B3C
		public void Notify_MechClusterDefeated()
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.MechClusterDefeated
			});
		}

		// Token: 0x06004873 RID: 18547 RVA: 0x001C7962 File Offset: 0x001C5B62
		public void Notify_PawnUndowned(Pawn pawn)
		{
			this.curJob.Notify_PawnUndowned(pawn);
		}

		// Token: 0x06004874 RID: 18548 RVA: 0x001C7970 File Offset: 0x001C5B70
		public void Notify_PawnDowned(Pawn pawn)
		{
			this.curJob.Notify_PawnDowned(pawn);
		}

		// Token: 0x06004875 RID: 18549 RVA: 0x001C7980 File Offset: 0x001C5B80
		private bool CheckTransitionOnSignal(TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal))
			{
				this.lastPawnHarmTick = Find.TickManager.TicksGame;
			}
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				if (this.graph.transitions[i].sources.Contains(this.curLordToil) && this.graph.transitions[i].CheckSignal(this, signal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004876 RID: 18550 RVA: 0x001C7A00 File Offset: 0x001C5C00
		private Vector3 DebugCenter()
		{
			Vector3 result = this.Map.Center.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			if (this.ownedPawns.Any((Pawn p) => p.Spawned))
			{
				result.x = (from p in this.ownedPawns
				where p.Spawned
				select p).Average((Pawn p) => p.DrawPos.x);
				result.z = (from p in this.ownedPawns
				where p.Spawned
				select p).Average((Pawn p) => p.DrawPos.z);
			}
			return result;
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x001C7B00 File Offset: 0x001C5D00
		public void DebugDraw()
		{
			Vector3 a = this.DebugCenter();
			IntVec3 flagLoc = this.curLordToil.FlagLoc;
			if (flagLoc.IsValid)
			{
				Graphics.DrawMesh(MeshPool.plane14, flagLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Building), Quaternion.identity, Lord.FlagTex, 0);
			}
			GenDraw.DrawLineBetween(a, flagLoc.ToVector3Shifted(), SimpleColor.Red, 0.2f);
			foreach (Pawn pawn in this.ownedPawns)
			{
				SimpleColor color = (!pawn.InMentalState) ? SimpleColor.White : SimpleColor.Yellow;
				GenDraw.DrawLineBetween(a, pawn.DrawPos, color, 0.2f);
			}
		}

		// Token: 0x06004878 RID: 18552 RVA: 0x001C7BBC File Offset: 0x001C5DBC
		public void DebugOnGUI()
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			string label;
			if (this.CurLordToil != null)
			{
				string str = "toil ";
				string str2 = this.graph.lordToils.IndexOf(this.CurLordToil).ToString();
				string str3 = "\n";
				LordToil lordToil = this.CurLordToil;
				label = str + str2 + str3 + ((lordToil != null) ? lordToil.ToString() : null);
			}
			else
			{
				label = "toil=NULL";
			}
			Vector2 vector = this.DebugCenter().MapToUIPosition();
			Widgets.Label(new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f), label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x001C7C64 File Offset: 0x001C5E64
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Start steal threshold: " + StealAIUtility.StartStealingMarketValueThreshold(this).ToString("F0"));
			stringBuilder.AppendLine("Duties:");
			foreach (Pawn pawn in this.ownedPawns)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "   ";
				string labelCap = pawn.LabelCap;
				string str2 = " - ";
				PawnDuty duty = pawn.mindState.duty;
				stringBuilder2.AppendLine(str + labelCap + str2 + ((duty != null) ? duty.ToString() : null));
			}
			stringBuilder.AppendLine("Raw save data:");
			stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(this));
			return stringBuilder.ToString();
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x001C7D40 File Offset: 0x001C5F40
		private bool ShouldDoDebugOutput()
		{
			IntVec3 a = UI.MouseCell();
			IntVec3 flagLoc = this.curLordToil.FlagLoc;
			if (flagLoc.IsValid && a == flagLoc)
			{
				return true;
			}
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (a == this.ownedPawns[i].Position)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600487B RID: 18555 RVA: 0x001C7DA5 File Offset: 0x001C5FA5
		public void Dispose()
		{
			StateGraph stateGraph = this.graph;
			if (stateGraph != null)
			{
				stateGraph.Dispose();
			}
			LordJob lordJob = this.curJob;
			if (lordJob == null)
			{
				return;
			}
			lordJob.Dispose();
		}

		// Token: 0x04002EBC RID: 11964
		public LordManager lordManager;

		// Token: 0x04002EBD RID: 11965
		private LordToil curLordToil;

		// Token: 0x04002EBE RID: 11966
		private StateGraph graph;

		// Token: 0x04002EBF RID: 11967
		public int loadID = -1;

		// Token: 0x04002EC0 RID: 11968
		private LordJob curJob;

		// Token: 0x04002EC1 RID: 11969
		public Faction faction;

		// Token: 0x04002EC2 RID: 11970
		public List<Pawn> ownedPawns = new List<Pawn>();

		// Token: 0x04002EC3 RID: 11971
		public List<Building> ownedBuildings = new List<Building>();

		// Token: 0x04002EC4 RID: 11972
		public List<Corpse> ownedCorpses = new List<Corpse>();

		// Token: 0x04002EC5 RID: 11973
		public List<Thing> extraForbiddenThings = new List<Thing>();

		// Token: 0x04002EC6 RID: 11974
		public List<string> questTags;

		// Token: 0x04002EC7 RID: 11975
		public string inSignalLeave;

		// Token: 0x04002EC8 RID: 11976
		private bool initialized;

		// Token: 0x04002EC9 RID: 11977
		public int ticksInToil;

		// Token: 0x04002ECA RID: 11978
		public int numPawnsLostViolently;

		// Token: 0x04002ECB RID: 11979
		public int numPawnsEverGained;

		// Token: 0x04002ECC RID: 11980
		public int initialColonyHealthTotal;

		// Token: 0x04002ECD RID: 11981
		public int lastPawnHarmTick = -99999;

		// Token: 0x04002ECE RID: 11982
		private static readonly Material FlagTex = MaterialPool.MatFrom("UI/Overlays/SquadFlag");

		// Token: 0x04002ECF RID: 11983
		private int tmpCurLordToilIdx = -1;

		// Token: 0x04002ED0 RID: 11984
		private Dictionary<int, LordToilData> tmpLordToilData = new Dictionary<int, LordToilData>();

		// Token: 0x04002ED1 RID: 11985
		private Dictionary<int, TriggerData> tmpTriggerData = new Dictionary<int, TriggerData>();
	}
}
