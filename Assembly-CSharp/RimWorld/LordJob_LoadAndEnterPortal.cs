using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C72 RID: 3186
	public class LordJob_LoadAndEnterPortal : LordJob
	{
		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x06005A62 RID: 23138 RVA: 0x002144B5 File Offset: 0x002126B5
		public override bool AllowStartNewGatherings
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x06005A63 RID: 23139 RVA: 0x002144B8 File Offset: 0x002126B8
		public override bool AllowStartNewRituals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06005A64 RID: 23140 RVA: 0x002144BB File Offset: 0x002126BB
		public override bool AddFleeToil
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06005A65 RID: 23141 RVA: 0x002144BE File Offset: 0x002126BE
		public LordJob_LoadAndEnterPortal()
		{
		}

		// Token: 0x06005A66 RID: 23142 RVA: 0x002144C6 File Offset: 0x002126C6
		public LordJob_LoadAndEnterPortal(MapPortal portal)
		{
			this.portal = portal;
		}

		// Token: 0x06005A67 RID: 23143 RVA: 0x002144D5 File Offset: 0x002126D5
		public override void ExposeData()
		{
			Scribe_References.Look<MapPortal>(ref this.portal, "portal", false);
		}

		// Token: 0x06005A68 RID: 23144 RVA: 0x002144E8 File Offset: 0x002126E8
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_LoadAndEnterPortal startingToil = new LordToil_LoadAndEnterPortal(this.portal);
			stateGraph.StartingToil = startingToil;
			LordToil_End toil = new LordToil_End();
			stateGraph.AddToil(toil);
			return stateGraph;
		}

		// Token: 0x040033DC RID: 13276
		public MapPortal portal;
	}
}
