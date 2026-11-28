using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001761 RID: 5985
	public class TransportShip : IExposable, ILoadReferenceable
	{
		// Token: 0x17001972 RID: 6514
		// (get) Token: 0x060094E1 RID: 38113 RVA: 0x00363CAA File Offset: 0x00361EAA
		public bool ShipExistsAndIsSpawned
		{
			get
			{
				return this.shipThing != null && this.shipThing.Spawned;
			}
		}

		// Token: 0x17001973 RID: 6515
		// (get) Token: 0x060094E2 RID: 38114 RVA: 0x00363CC1 File Offset: 0x00361EC1
		public bool Disposed
		{
			get
			{
				return this.disposed;
			}
		}

		// Token: 0x17001974 RID: 6516
		// (get) Token: 0x060094E3 RID: 38115 RVA: 0x00363CC9 File Offset: 0x00361EC9
		public bool Waiting
		{
			get
			{
				return this.ShipExistsAndIsSpawned && (this.curJob == null || typeof(ShipJob_Wait).IsAssignableFrom(this.curJob.GetType()));
			}
		}

		// Token: 0x17001975 RID: 6517
		// (get) Token: 0x060094E4 RID: 38116 RVA: 0x00363CF9 File Offset: 0x00361EF9
		public bool ShowGizmos
		{
			get
			{
				return this.curJob != null && this.curJob.ShowGizmos;
			}
		}

		// Token: 0x17001976 RID: 6518
		// (get) Token: 0x060094E5 RID: 38117 RVA: 0x00363D10 File Offset: 0x00361F10
		private bool CanDispose
		{
			get
			{
				if (this.started && this.curJob == null)
				{
					Thing thing = this.shipThing;
					if (thing == null || !thing.Spawned)
					{
						thing = this.shipThing;
						if (thing == null || !(thing.ParentHolder is ActiveTransporterInfo))
						{
							Thing thing2 = this.shipThing;
							if (thing2 == null || !thing2.IsInCaravan())
							{
								if (!this.shipJobs.Any((ShipJob x) => x.def.blocksDisposalIfQueuedUnspawned) && !Find.World.worldObjects.TravellingTransporters.Any(delegate(TravellingTransporters x)
								{
									TransportersArrivalAction_TransportShip transportersArrivalAction_TransportShip = x.arrivalAction as TransportersArrivalAction_TransportShip;
									return transportersArrivalAction_TransportShip != null && transportersArrivalAction_TransportShip.transportShip == this;
								}) && !Find.QuestManager.IsReservedByAnyQuest(this))
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001977 RID: 6519
		// (get) Token: 0x060094E6 RID: 38118 RVA: 0x00363DD1 File Offset: 0x00361FD1
		public CompTransporter TransporterComp
		{
			get
			{
				if (this.cachedCompTransporter == null)
				{
					this.cachedCompTransporter = this.shipThing.TryGetComp<CompTransporter>();
				}
				return this.cachedCompTransporter;
			}
		}

		// Token: 0x17001978 RID: 6520
		// (get) Token: 0x060094E7 RID: 38119 RVA: 0x00363DF2 File Offset: 0x00361FF2
		public CompShuttle ShuttleComp
		{
			get
			{
				if (this.cachedCompShuttle == null)
				{
					this.cachedCompShuttle = this.shipThing.TryGetComp<CompShuttle>();
				}
				return this.cachedCompShuttle;
			}
		}

		// Token: 0x17001979 RID: 6521
		// (get) Token: 0x060094E8 RID: 38120 RVA: 0x00363E14 File Offset: 0x00362014
		public bool LeavingSoonAutomatically
		{
			get
			{
				if (this.curJob != null && this.curJob.def == ShipJobDefOf.FlyAway)
				{
					return true;
				}
				foreach (ShipJob shipJob in this.shipJobs)
				{
					if (shipJob.Interruptible)
					{
						return false;
					}
					if (shipJob.def == ShipJobDefOf.FlyAway)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700197A RID: 6522
		// (get) Token: 0x060094E9 RID: 38121 RVA: 0x00363E9C File Offset: 0x0036209C
		public bool HasPredeterminedDestination
		{
			get
			{
				if (this.curJob != null && this.curJob.HasDestination)
				{
					return true;
				}
				using (List<ShipJob>.Enumerator enumerator = this.shipJobs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasDestination)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x060094EA RID: 38122 RVA: 0x00363F0C File Offset: 0x0036210C
		public TransportShip()
		{
		}

		// Token: 0x060094EB RID: 38123 RVA: 0x00363F34 File Offset: 0x00362134
		public TransportShip(TransportShipDef def)
		{
			this.def = def;
			this.loadID = Find.UniqueIDsManager.GetNextTransportShipID();
			Find.TransportShipManager.RegisterShipObject(this);
		}

		// Token: 0x060094EC RID: 38124 RVA: 0x00363F86 File Offset: 0x00362186
		public void Start()
		{
			if (this.started)
			{
				return;
			}
			this.started = true;
			this.TryGetNextJob();
		}

		// Token: 0x060094ED RID: 38125 RVA: 0x00363FA0 File Offset: 0x003621A0
		public void Tick()
		{
			if (!this.started)
			{
				return;
			}
			if (this.curJob == null || this.curJob.jobState == ShipJobState.Ended)
			{
				this.TryGetNextJob();
			}
			if (this.curJob != null)
			{
				this.curJob.TickInterval(1);
				return;
			}
			if (this.CanDispose)
			{
				this.Dispose();
			}
		}

		// Token: 0x060094EE RID: 38126 RVA: 0x00363FF5 File Offset: 0x003621F5
		public void ForceJob(ShipJob shipJob)
		{
			if (this.curJob != null && this.curJob.Interruptible)
			{
				this.EndCurrentJob();
			}
			this.shipJobs.Clear();
			this.AddJob(shipJob);
			this.TryGetNextJob();
		}

		// Token: 0x060094EF RID: 38127 RVA: 0x0036402A File Offset: 0x0036222A
		public void ForceJob(ShipJobDef def)
		{
			this.ForceJob(ShipJobMaker.MakeShipJob(def));
		}

		// Token: 0x060094F0 RID: 38128 RVA: 0x00364038 File Offset: 0x00362238
		public void SetNextJob(ShipJob shipJob)
		{
			if (this.Disposed)
			{
				Log.Error("Trying to add a job to a disposed transport ship. id=" + this.GetUniqueLoadID());
				return;
			}
			shipJob.transportShip = this;
			this.shipJobs.Insert(0, shipJob);
		}

		// Token: 0x060094F1 RID: 38129 RVA: 0x0036406C File Offset: 0x0036226C
		public void ForceJob_DelayCurrent(ShipJob shipJob)
		{
			if (this.curJob != null)
			{
				this.shipJobs.Insert(0, this.curJob);
				this.curJob = null;
			}
			if (!this.started)
			{
				this.Start();
			}
			this.SetNextJob(shipJob);
			this.TryGetNextJob();
		}

		// Token: 0x060094F2 RID: 38130 RVA: 0x003640AA File Offset: 0x003622AA
		public void AddJob(ShipJob shipJob)
		{
			if (this.Disposed)
			{
				Log.Error("Trying to add a job to a disposed transport ship. id=" + this.GetUniqueLoadID());
				return;
			}
			shipJob.transportShip = this;
			this.shipJobs.Add(shipJob);
		}

		// Token: 0x060094F3 RID: 38131 RVA: 0x003640DD File Offset: 0x003622DD
		public void AddJob(ShipJobDef def)
		{
			this.AddJob(ShipJobMaker.MakeShipJob(def));
		}

		// Token: 0x060094F4 RID: 38132 RVA: 0x003640EC File Offset: 0x003622EC
		public void AddJobs(params ShipJobDef[] defs)
		{
			if (defs == null)
			{
				return;
			}
			for (int i = 0; i < defs.Length; i++)
			{
				this.AddJob(defs[i]);
			}
		}

		// Token: 0x060094F5 RID: 38133 RVA: 0x00364114 File Offset: 0x00362314
		public void EndCurrentJob()
		{
			if (this.curJob != null)
			{
				this.curJob.End();
			}
			this.curJob = null;
		}

		// Token: 0x060094F6 RID: 38134 RVA: 0x00364130 File Offset: 0x00362330
		public void TryGetNextJob()
		{
			this.EndCurrentJob();
			if (this.shipJobs.NullOrEmpty<ShipJob>())
			{
				return;
			}
			ShipJob shipJob = this.shipJobs.First<ShipJob>();
			if (shipJob.TryStart())
			{
				this.curJob = shipJob;
				this.shipJobs.Remove(shipJob);
			}
		}

		// Token: 0x060094F7 RID: 38135 RVA: 0x00364179 File Offset: 0x00362379
		public void RemoveJob(ShipJob job)
		{
			if (this.curJob == job)
			{
				this.EndCurrentJob();
				return;
			}
			if (this.shipJobs.Contains(job))
			{
				this.shipJobs.Remove(job);
			}
		}

		// Token: 0x060094F8 RID: 38136 RVA: 0x003641A8 File Offset: 0x003623A8
		public void ArriveAt(IntVec3 cell, MapParent mapParent)
		{
			if (this.curJob != null && this.curJob.Interruptible)
			{
				this.EndCurrentJob();
			}
			ShipJob_Arrive shipJob_Arrive = (ShipJob_Arrive)ShipJobMaker.MakeShipJob(ShipJobDefOf.Arrive);
			shipJob_Arrive.cell = cell;
			shipJob_Arrive.mapParent = mapParent;
			this.SetNextJob(shipJob_Arrive);
			this.Start();
			if (this.curJob == null || this.curJob.Interruptible)
			{
				this.TryGetNextJob();
			}
		}

		// Token: 0x060094F9 RID: 38137 RVA: 0x00364218 File Offset: 0x00362418
		public void Dispose()
		{
			if (this.def.playerShuttle)
			{
				this.shipThing.TryGetComp<CompShuttle>().shipParent = null;
			}
			else
			{
				Thing thing = this.shipThing;
				if (thing != null && !thing.Destroyed)
				{
					this.shipThing.Destroy(DestroyMode.QuestLogic);
				}
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Disposed", this.Named("SUBJECT"));
			Find.TransportShipManager.DeregisterShipObject(this);
		}

		// Token: 0x060094FA RID: 38138 RVA: 0x0036428C File Offset: 0x0036248C
		public void LogJobs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Jobs for " + this.GetUniqueLoadID() + ":");
			if (this.curJob == null)
			{
				stringBuilder.AppendLine("  - CurJob: null");
			}
			else
			{
				stringBuilder.AppendLine("  - CurJob: " + this.curJob.GetJobInfo());
			}
			foreach (ShipJob shipJob in this.shipJobs)
			{
				stringBuilder.AppendLine("  - " + shipJob.GetJobInfo());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060094FB RID: 38139 RVA: 0x00364350 File Offset: 0x00362550
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<bool>(ref this.disposed, "disposed", false, false);
			Scribe_Values.Look<bool>(ref this.started, "started", false, false);
			Scribe_Defs.Look<TransportShipDef>(ref this.def, "def");
			Scribe_Deep.Look<ShipJob>(ref this.curJob, "curJob", Array.Empty<object>());
			Scribe_Collections.Look<ShipJob>(ref this.shipJobs, "shipJobs", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.shipThing != null && this.shipThing.Destroyed)
				{
					this.shipThing = null;
				}
				this.shipThingSpawnedOrHasOwner = (this.shipThing != null && (this.shipThing.SpawnedOrAnyParentSpawned || this.shipThing.ParentHolder != null));
			}
			Scribe_Values.Look<bool>(ref this.shipThingSpawnedOrHasOwner, "shipThingSpawned", false, false);
			if (this.shipThingSpawnedOrHasOwner)
			{
				Scribe_References.Look<Thing>(ref this.shipThing, "shipThing", false);
				return;
			}
			Scribe_Deep.Look<Thing>(ref this.shipThing, "shipThing", Array.Empty<object>());
		}

		// Token: 0x060094FC RID: 38140 RVA: 0x00364478 File Offset: 0x00362678
		public string GetUniqueLoadID()
		{
			return "TransportShip_" + this.loadID.ToString();
		}

		// Token: 0x040059C4 RID: 22980
		public int loadID = -1;

		// Token: 0x040059C5 RID: 22981
		public TransportShipDef def;

		// Token: 0x040059C6 RID: 22982
		public ShipJob curJob;

		// Token: 0x040059C7 RID: 22983
		public Thing shipThing;

		// Token: 0x040059C8 RID: 22984
		private List<ShipJob> shipJobs = new List<ShipJob>();

		// Token: 0x040059C9 RID: 22985
		public List<string> questTags = new List<string>();

		// Token: 0x040059CA RID: 22986
		public bool started;

		// Token: 0x040059CB RID: 22987
		private bool disposed;

		// Token: 0x040059CC RID: 22988
		private bool shipThingSpawnedOrHasOwner;

		// Token: 0x040059CD RID: 22989
		[Unsaved(false)]
		private CompShuttle cachedCompShuttle;

		// Token: 0x040059CE RID: 22990
		[Unsaved(false)]
		private CompTransporter cachedCompTransporter;
	}
}
