using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D2F RID: 3375
	public abstract class ShipJob : IExposable, ILoadReferenceable
	{
		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x06005EA5 RID: 24229
		protected abstract bool ShouldEnd { get; }

		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x06005EA6 RID: 24230 RVA: 0x00229E5A File Offset: 0x0022805A
		public virtual bool HasDestination
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x06005EA7 RID: 24231 RVA: 0x00229E5D File Offset: 0x0022805D
		public virtual bool Interruptible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x06005EA8 RID: 24232 RVA: 0x00229E60 File Offset: 0x00228060
		public virtual bool ShowGizmos
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005EA9 RID: 24233 RVA: 0x00229E63 File Offset: 0x00228063
		public ShipJob()
		{
		}

		// Token: 0x06005EAA RID: 24234 RVA: 0x00229E72 File Offset: 0x00228072
		public ShipJob(TransportShip transportShip)
		{
			this.loadID = Find.UniqueIDsManager.GetNextShipJobID();
			this.transportShip = transportShip;
		}

		// Token: 0x06005EAB RID: 24235 RVA: 0x00229E98 File Offset: 0x00228098
		public virtual bool TryStart()
		{
			if (this.transportShip == null)
			{
				Log.Error("Trying to start a ship job with a null ship object.");
				return false;
			}
			if (this.jobState == ShipJobState.Ended)
			{
				Log.Error("Trying to start an already ended ship job.");
				this.transportShip.RemoveJob(this);
				return false;
			}
			this.jobState = ShipJobState.Working;
			return true;
		}

		// Token: 0x06005EAC RID: 24236 RVA: 0x00229ED7 File Offset: 0x002280D7
		public virtual void End()
		{
			this.jobState = ShipJobState.Ended;
		}

		// Token: 0x06005EAD RID: 24237 RVA: 0x00229EE0 File Offset: 0x002280E0
		public virtual void TickInterval(int delta)
		{
			if (this.jobState != ShipJobState.Working)
			{
				Log.Error("Trying to tick " + this.jobState.ToString() + " job.");
			}
			if (this.ShouldEnd)
			{
				this.End();
			}
		}

		// Token: 0x06005EAE RID: 24238 RVA: 0x00229F1E File Offset: 0x0022811E
		public virtual IEnumerable<Gizmo> GetJobGizmos()
		{
			return null;
		}

		// Token: 0x06005EAF RID: 24239 RVA: 0x00229F21 File Offset: 0x00228121
		public virtual string GetJobInfo()
		{
			return base.GetType().Name;
		}

		// Token: 0x06005EB0 RID: 24240 RVA: 0x00229F2E File Offset: 0x0022812E
		public virtual string ShipThingExtraInspectString()
		{
			return null;
		}

		// Token: 0x06005EB1 RID: 24241 RVA: 0x00229F34 File Offset: 0x00228134
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ShipJobDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<ShipJobState>(ref this.jobState, "jobState", ShipJobState.Uninitialized, false);
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
		}

		// Token: 0x06005EB2 RID: 24242 RVA: 0x00229F86 File Offset: 0x00228186
		public string GetUniqueLoadID()
		{
			return "ShipJob_" + this.loadID.ToString();
		}

		// Token: 0x04003597 RID: 13719
		public ShipJobDef def;

		// Token: 0x04003598 RID: 13720
		public int loadID = -1;

		// Token: 0x04003599 RID: 13721
		public ShipJobState jobState;

		// Token: 0x0400359A RID: 13722
		public TransportShip transportShip;
	}
}
