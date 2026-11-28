using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D2C RID: 3372
	public class Pawn_WorkSettings : IExposable
	{
		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x06005E6F RID: 24175 RVA: 0x00228315 File Offset: 0x00226515
		public bool EverWork
		{
			get
			{
				return this.priorities != null;
			}
		}

		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x06005E70 RID: 24176 RVA: 0x00228320 File Offset: 0x00226520
		public List<WorkGiver> WorkGiversInOrderNormal
		{
			get
			{
				if (this.workGiversDirty)
				{
					this.CacheWorkGiversInOrder();
				}
				return this.workGiversInOrderNormal;
			}
		}

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x06005E71 RID: 24177 RVA: 0x00228336 File Offset: 0x00226536
		public List<WorkGiver> WorkGiversInOrderEmergency
		{
			get
			{
				if (this.workGiversDirty)
				{
					this.CacheWorkGiversInOrder();
				}
				return this.workGiversInOrderEmerg;
			}
		}

		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x06005E72 RID: 24178 RVA: 0x0022834C File Offset: 0x0022654C
		public bool LimitInitialActiveWorks
		{
			get
			{
				return !this.pawn.RaceProps.IsMechanoid;
			}
		}

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x06005E73 RID: 24179 RVA: 0x00228361 File Offset: 0x00226561
		public bool Initialized
		{
			get
			{
				return this.priorities != null;
			}
		}

		// Token: 0x06005E74 RID: 24180 RVA: 0x0022836C File Offset: 0x0022656C
		public Pawn_WorkSettings()
		{
		}

		// Token: 0x06005E75 RID: 24181 RVA: 0x00228391 File Offset: 0x00226591
		public Pawn_WorkSettings(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06005E76 RID: 24182 RVA: 0x002283C0 File Offset: 0x002265C0
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<WorkTypeDef, int>>(ref this.priorities, "priorities", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.priorities != null)
			{
				this.pawn.Notify_DisabledWorkTypesChanged();
				List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
				for (int i = 0; i < disabledWorkTypes.Count; i++)
				{
					this.Disable(disabledWorkTypes[i]);
				}
			}
		}

		// Token: 0x06005E77 RID: 24183 RVA: 0x00228428 File Offset: 0x00226628
		public void EnableAndInitializeIfNotAlreadyInitialized()
		{
			if (this.priorities == null)
			{
				this.EnableAndInitialize();
			}
		}

		// Token: 0x06005E78 RID: 24184 RVA: 0x00228438 File Offset: 0x00226638
		public void EnableAndInitialize()
		{
			if (this.priorities == null)
			{
				this.priorities = new DefMap<WorkTypeDef, int>();
			}
			this.priorities.SetAll(0);
			int num = 0;
			foreach (WorkTypeDef w3 in (from w in DefDatabase<WorkTypeDef>.AllDefs
			where !w.alwaysStartActive && !this.pawn.WorkTypeIsDisabled(w)
			select w).OrderByDescending(delegate(WorkTypeDef w)
			{
				Pawn_SkillTracker skills = this.pawn.skills;
				if (skills == null)
				{
					return 1f;
				}
				return skills.AverageOfRelevantSkillsFor(w);
			}))
			{
				this.SetPriority(w3, 3);
				num++;
				if (this.LimitInitialActiveWorks && num >= 6)
				{
					break;
				}
			}
			foreach (WorkTypeDef w2 in from w in DefDatabase<WorkTypeDef>.AllDefs
			where w.alwaysStartActive
			select w)
			{
				if (!this.pawn.WorkTypeIsDisabled(w2))
				{
					this.SetPriority(w2, 3);
				}
			}
			if (ModsConfig.BiotechActive && this.pawn.RaceProps.IsMechanoid && !this.pawn.RaceProps.mechWorkTypePriorities.NullOrEmpty<MechWorkTypePriority>())
			{
				for (int i = 0; i < this.pawn.RaceProps.mechWorkTypePriorities.Count; i++)
				{
					MechWorkTypePriority mechWorkTypePriority = this.pawn.RaceProps.mechWorkTypePriorities[i];
					this.SetPriority(mechWorkTypePriority.def, mechWorkTypePriority.priority);
				}
			}
			List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
			for (int j = 0; j < disabledWorkTypes.Count; j++)
			{
				this.Disable(disabledWorkTypes[j]);
			}
		}

		// Token: 0x06005E79 RID: 24185 RVA: 0x002285FC File Offset: 0x002267FC
		private void ConfirmInitializedDebug()
		{
			if (this.priorities == null)
			{
				Pawn pawn = this.pawn;
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " did not have work settings initialized.");
				this.EnableAndInitialize();
			}
		}

		// Token: 0x06005E7A RID: 24186 RVA: 0x00228630 File Offset: 0x00226830
		public void SetPriority(WorkTypeDef w, int priority)
		{
			this.ConfirmInitializedDebug();
			if (priority != 0 && this.pawn.WorkTypeIsDisabled(w))
			{
				string str = "Tried to change priority on disabled worktype ";
				string str2 = (w != null) ? w.ToString() : null;
				string str3 = " for pawn ";
				Pawn pawn = this.pawn;
				Log.Error(str + str2 + str3 + ((pawn != null) ? pawn.ToString() : null));
				return;
			}
			if (priority < 0 || priority > 4)
			{
				Log.Message("Trying to set work to invalid priority " + priority.ToString());
			}
			this.priorities[w] = priority;
			this.workGiversDirty = true;
			if (priority == 0 && this.pawn.jobs != null)
			{
				this.pawn.jobs.Notify_WorkTypeDisabled(w);
			}
		}

		// Token: 0x06005E7B RID: 24187 RVA: 0x002286DC File Offset: 0x002268DC
		public int GetPriority(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			int num = this.priorities[w];
			if (this.pawn.RaceProps.Humanlike && num > 0 && !Find.PlaySettings.useWorkPriorities)
			{
				return 3;
			}
			return num;
		}

		// Token: 0x06005E7C RID: 24188 RVA: 0x00228721 File Offset: 0x00226921
		public bool WorkIsActive(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			return this.GetPriority(w) > 0;
		}

		// Token: 0x06005E7D RID: 24189 RVA: 0x00228733 File Offset: 0x00226933
		public void Disable(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			this.SetPriority(w, 0);
		}

		// Token: 0x06005E7E RID: 24190 RVA: 0x00228743 File Offset: 0x00226943
		public void DisableAll()
		{
			this.ConfirmInitializedDebug();
			this.priorities.SetAll(0);
			this.workGiversDirty = true;
		}

		// Token: 0x06005E7F RID: 24191 RVA: 0x0022875E File Offset: 0x0022695E
		public void Notify_UseWorkPrioritiesChanged()
		{
			this.workGiversDirty = true;
		}

		// Token: 0x06005E80 RID: 24192 RVA: 0x00228768 File Offset: 0x00226968
		public void Notify_DisabledWorkTypesChanged()
		{
			if (this.priorities == null)
			{
				return;
			}
			List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
			for (int i = 0; i < disabledWorkTypes.Count; i++)
			{
				this.Disable(disabledWorkTypes[i]);
			}
		}

		// Token: 0x06005E81 RID: 24193 RVA: 0x002287AC File Offset: 0x002269AC
		private void CacheWorkGiversInOrder()
		{
			Pawn_WorkSettings.wtsByPrio.Clear();
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			int num = 999;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				int priority = this.GetPriority(workTypeDef);
				if (priority > 0)
				{
					if (priority < num)
					{
						if (workTypeDef.workGiversByPriority.Any((WorkGiverDef wg) => !wg.emergency))
						{
							num = priority;
						}
					}
					Pawn_WorkSettings.wtsByPrio.Add(workTypeDef);
				}
			}
			Pawn_WorkSettings.wtsByPrio.InsertionSort(delegate(WorkTypeDef a, WorkTypeDef b)
			{
				float value = (float)(a.naturalPriority + (4 - this.GetPriority(a)) * 100000);
				return ((float)(b.naturalPriority + (4 - this.GetPriority(b)) * 100000)).CompareTo(value);
			});
			this.workGiversInOrderEmerg.Clear();
			for (int j = 0; j < Pawn_WorkSettings.wtsByPrio.Count; j++)
			{
				WorkTypeDef workTypeDef2 = Pawn_WorkSettings.wtsByPrio[j];
				for (int k = 0; k < workTypeDef2.workGiversByPriority.Count; k++)
				{
					WorkGiver worker = workTypeDef2.workGiversByPriority[k].Worker;
					if (worker.def.emergency && this.GetPriority(worker.def.workType) <= num)
					{
						this.workGiversInOrderEmerg.Add(worker);
					}
				}
			}
			this.workGiversInOrderNormal.Clear();
			for (int l = 0; l < Pawn_WorkSettings.wtsByPrio.Count; l++)
			{
				WorkTypeDef workTypeDef3 = Pawn_WorkSettings.wtsByPrio[l];
				for (int m = 0; m < workTypeDef3.workGiversByPriority.Count; m++)
				{
					WorkGiver worker2 = workTypeDef3.workGiversByPriority[m].Worker;
					if (!worker2.def.emergency || this.GetPriority(worker2.def.workType) > num)
					{
						this.workGiversInOrderNormal.Add(worker2);
					}
				}
			}
			this.workGiversDirty = false;
		}

		// Token: 0x06005E82 RID: 24194 RVA: 0x0022897C File Offset: 0x00226B7C
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			string str = "WorkSettings for ";
			Pawn pawn = this.pawn;
			stringBuilder2.AppendLine(str + ((pawn != null) ? pawn.ToString() : null));
			stringBuilder.AppendLine("Cached emergency WorkGivers in order:");
			for (int i = 0; i < this.WorkGiversInOrderEmergency.Count; i++)
			{
				stringBuilder.AppendLine("   " + i.ToString() + ": " + this.DebugStringFor(this.WorkGiversInOrderEmergency[i].def));
			}
			stringBuilder.AppendLine("Cached normal WorkGivers in order:");
			for (int j = 0; j < this.WorkGiversInOrderNormal.Count; j++)
			{
				stringBuilder.AppendLine("   " + j.ToString() + ": " + this.DebugStringFor(this.WorkGiversInOrderNormal[j].def));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005E83 RID: 24195 RVA: 0x00228A64 File Offset: 0x00226C64
		private string DebugStringFor(WorkGiverDef wg)
		{
			return string.Concat(new string[]
			{
				"[",
				this.GetPriority(wg.workType).ToString(),
				" ",
				wg.workType.defName,
				"] - ",
				wg.defName,
				" (",
				wg.priorityInType.ToString(),
				")"
			});
		}

		// Token: 0x04003585 RID: 13701
		private Pawn pawn;

		// Token: 0x04003586 RID: 13702
		private DefMap<WorkTypeDef, int> priorities;

		// Token: 0x04003587 RID: 13703
		private bool workGiversDirty = true;

		// Token: 0x04003588 RID: 13704
		private List<WorkGiver> workGiversInOrderEmerg = new List<WorkGiver>();

		// Token: 0x04003589 RID: 13705
		private List<WorkGiver> workGiversInOrderNormal = new List<WorkGiver>();

		// Token: 0x0400358A RID: 13706
		public const int LowestPriority = 4;

		// Token: 0x0400358B RID: 13707
		public const int DefaultPriority = 3;

		// Token: 0x0400358C RID: 13708
		private const int MaxInitialActiveWorks = 6;

		// Token: 0x0400358D RID: 13709
		private static List<WorkTypeDef> wtsByPrio = new List<WorkTypeDef>();
	}
}
