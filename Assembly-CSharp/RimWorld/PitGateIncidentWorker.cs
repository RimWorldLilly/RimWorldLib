using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC0 RID: 4032
	public abstract class PitGateIncidentWorker : IExposable, ILoadReferenceable
	{
		// Token: 0x06006668 RID: 26216 RVA: 0x00245D67 File Offset: 0x00243F67
		protected PitGateIncidentWorker()
		{
			this.loadID = Find.UniqueIDsManager.GetNextPitGateIncidentID();
		}

		// Token: 0x06006669 RID: 26217 RVA: 0x00245D91 File Offset: 0x00243F91
		public virtual void Setup(float points)
		{
			this.fireTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600666A RID: 26218 RVA: 0x00245DA3 File Offset: 0x00243FA3
		public virtual bool CanFireNow()
		{
			return this.pitGate != null && !this.pitGate.Destroyed;
		}

		// Token: 0x0600666B RID: 26219 RVA: 0x00245DBD File Offset: 0x00243FBD
		public virtual void Tick()
		{
		}

		// Token: 0x0600666C RID: 26220 RVA: 0x00245DBF File Offset: 0x00243FBF
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<PitGateIncidentDef>(ref this.def, "def");
			Scribe_References.Look<Building>(ref this.pitGate, "pitGate", false);
		}

		// Token: 0x0600666D RID: 26221 RVA: 0x00245DF4 File Offset: 0x00243FF4
		public string GetUniqueLoadID()
		{
			return "PitGateIncident_" + this.def.defName + "_" + this.loadID.ToString();
		}

		// Token: 0x04003ED3 RID: 16083
		private int loadID = -1;

		// Token: 0x04003ED4 RID: 16084
		public PitGateIncidentDef def;

		// Token: 0x04003ED5 RID: 16085
		public Building pitGate;

		// Token: 0x04003ED6 RID: 16086
		public int fireTick = -99999;
	}
}
