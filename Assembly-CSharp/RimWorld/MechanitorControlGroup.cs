using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001560 RID: 5472
	public class MechanitorControlGroup : IExposable
	{
		// Token: 0x170016A8 RID: 5800
		// (get) Token: 0x06008601 RID: 34305 RVA: 0x00306BE0 File Offset: 0x00304DE0
		public List<Pawn> MechsForReading
		{
			get
			{
				if (this.mechsForReading == null)
				{
					this.mechsForReading = new List<Pawn>();
				}
				this.mechsForReading.Clear();
				for (int i = 0; i < this.assignedMechs.Count; i++)
				{
					if (!this.assignedMechs[i].pawn.IsGestating())
					{
						this.mechsForReading.Add(this.assignedMechs[i].pawn);
					}
				}
				return this.mechsForReading;
			}
		}

		// Token: 0x170016A9 RID: 5801
		// (get) Token: 0x06008602 RID: 34306 RVA: 0x00306C5B File Offset: 0x00304E5B
		public List<AssignedMech> AssignedMechs
		{
			get
			{
				return this.assignedMechs;
			}
		}

		// Token: 0x170016AA RID: 5802
		// (get) Token: 0x06008603 RID: 34307 RVA: 0x00306C63 File Offset: 0x00304E63
		public Pawn_MechanitorTracker Tracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x170016AB RID: 5803
		// (get) Token: 0x06008604 RID: 34308 RVA: 0x00306C6B File Offset: 0x00304E6B
		public MechWorkModeDef WorkMode
		{
			get
			{
				return this.workMode;
			}
		}

		// Token: 0x170016AC RID: 5804
		// (get) Token: 0x06008605 RID: 34309 RVA: 0x00306C73 File Offset: 0x00304E73
		public GlobalTargetInfo Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x170016AD RID: 5805
		// (get) Token: 0x06008606 RID: 34310 RVA: 0x00306C7B File Offset: 0x00304E7B
		public int Index
		{
			get
			{
				return this.Tracker.controlGroups.IndexOf(this) + 1;
			}
		}

		// Token: 0x170016AE RID: 5806
		// (get) Token: 0x06008607 RID: 34311 RVA: 0x00306C90 File Offset: 0x00304E90
		public string LabelIndexWithWorkMode
		{
			get
			{
				return this.Index.ToString() + " (" + this.WorkMode.LabelCap.ToString() + ")";
			}
		}

		// Token: 0x06008608 RID: 34312 RVA: 0x00306CD3 File Offset: 0x00304ED3
		public MechanitorControlGroup(Pawn_MechanitorTracker tracker)
		{
			this.tracker = tracker;
			this.workMode = MechWorkModeDefOf.Work;
		}

		// Token: 0x06008609 RID: 34313 RVA: 0x00306D10 File Offset: 0x00304F10
		public bool TryUnassign(Pawn pawn)
		{
			return this.assignedMechs.RemoveAll((AssignedMech x) => x.pawn == pawn) > 0;
		}

		// Token: 0x0600860A RID: 34314 RVA: 0x00306D44 File Offset: 0x00304F44
		public void Assign(Pawn pawn)
		{
			foreach (MechanitorControlGroup mechanitorControlGroup in this.tracker.controlGroups)
			{
				mechanitorControlGroup.TryUnassign(pawn);
			}
			this.assignedMechs.Add(new AssignedMech(pawn));
			this.SetWorkModeForPawn(pawn, this.workMode);
		}

		// Token: 0x0600860B RID: 34315 RVA: 0x00306DBC File Offset: 0x00304FBC
		public void SetWorkMode(MechWorkModeDef workMode)
		{
			this.SetWorkMode(workMode, GlobalTargetInfo.Invalid);
		}

		// Token: 0x0600860C RID: 34316 RVA: 0x00306DCC File Offset: 0x00304FCC
		public void SetWorkMode(MechWorkModeDef workMode, GlobalTargetInfo target)
		{
			if (this.workMode == workMode && this.target == target)
			{
				return;
			}
			this.workMode = workMode;
			this.target = target;
			foreach (AssignedMech assignedMech in this.assignedMechs)
			{
				this.SetWorkModeForPawn(assignedMech.pawn, workMode);
			}
			Dictionary<Pawn, string> dictionary = this.tags;
			if (dictionary != null)
			{
				dictionary.Clear();
			}
			if (workMode == MechWorkModeDefOf.Escort)
			{
				Pawn pawn = this.Tracker.Pawn;
				if (pawn.IsFormingCaravan())
				{
					Lord formAndSendCaravanLord = CaravanFormingUtility.GetFormAndSendCaravanLord(pawn);
					List<Pawn> list = Dialog_FormCaravan.AllSendablePawns(pawn.Map, false);
					for (int i = 0; i < this.assignedMechs.Count; i++)
					{
						Pawn pawn2 = this.assignedMechs[i].pawn;
						if (list.Contains(pawn2) && CaravanFormingUtility.GetFormAndSendCaravanLord(pawn2) != formAndSendCaravanLord)
						{
							Messages.Message("MessageCaravanAddingEscortingMech".Translate(pawn2.Named("MECH"), pawn.Named("OVERSEER")), pawn2, MessageTypeDefOf.RejectInput, false);
							CaravanFormingUtility.LateJoinFormingCaravan(pawn2, formAndSendCaravanLord);
						}
					}
				}
			}
		}

		// Token: 0x0600860D RID: 34317 RVA: 0x00306F18 File Offset: 0x00305118
		private void SetWorkModeForPawn(Pawn pawn, MechWorkModeDef workMode)
		{
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			if (workMode != MechWorkModeDefOf.Recharge && pawn.CurJobDef == JobDefOf.MechCharge && pawn.IsCharging())
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			CompCanBeDormant compCanBeDormant = pawn.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null)
			{
				compCanBeDormant.WakeUp();
			}
			Pawn_JobTracker jobs = pawn.jobs;
			if (jobs == null)
			{
				return;
			}
			jobs.CheckForJobOverride(0f, true);
		}

		// Token: 0x0600860E RID: 34318 RVA: 0x00306F7F File Offset: 0x0030517F
		public void SetTag(Pawn pawn, string tag)
		{
			if (this.tags == null)
			{
				this.tags = new Dictionary<Pawn, string>();
			}
			this.tags.Add(pawn, tag);
		}

		// Token: 0x0600860F RID: 34319 RVA: 0x00306FA4 File Offset: 0x003051A4
		public string GetTag(Pawn pawn)
		{
			string result;
			if (this.tags == null || !this.tags.TryGetValue(pawn, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06008610 RID: 34320 RVA: 0x00306FCC File Offset: 0x003051CC
		public void ExposeData()
		{
			Scribe_Collections.Look<AssignedMech>(ref this.assignedMechs, "assignedMechs", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<MechWorkModeDef>(ref this.workMode, "workMode");
			Scribe_TargetInfo.Look(ref this.target, "target");
			Scribe_Collections.Look<Pawn, string>(ref this.tags, "tags", LookMode.Reference, LookMode.Value, ref this.tmpTagPawns, ref this.tmpTags, true, false, false);
			Scribe_Values.Look<FloatRange>(ref this.mechRechargeThresholds, "mechRechargeThresholds", MechanitorControlGroup.DefaultMechRechargeThresholds, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedMechs.RemoveAll((AssignedMech x) => x.pawn == null);
			}
		}

		// Token: 0x0400528E RID: 21134
		public static readonly FloatRange DefaultMechRechargeThresholds = new FloatRange(0.05f, 1f);

		// Token: 0x0400528F RID: 21135
		private List<AssignedMech> assignedMechs = new List<AssignedMech>();

		// Token: 0x04005290 RID: 21136
		private Pawn_MechanitorTracker tracker;

		// Token: 0x04005291 RID: 21137
		private MechWorkModeDef workMode;

		// Token: 0x04005292 RID: 21138
		private GlobalTargetInfo target = GlobalTargetInfo.Invalid;

		// Token: 0x04005293 RID: 21139
		private Dictionary<Pawn, string> tags;

		// Token: 0x04005294 RID: 21140
		public FloatRange mechRechargeThresholds = MechanitorControlGroup.DefaultMechRechargeThresholds;

		// Token: 0x04005295 RID: 21141
		private List<Pawn> tmpTagPawns;

		// Token: 0x04005296 RID: 21142
		private List<string> tmpTags;

		// Token: 0x04005297 RID: 21143
		private List<Pawn> mechsForReading;
	}
}
