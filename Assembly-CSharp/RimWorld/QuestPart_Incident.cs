using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111C RID: 4380
	public class QuestPart_Incident : QuestPart
	{
		// Token: 0x170012C9 RID: 4809
		// (get) Token: 0x06006D85 RID: 28037 RVA: 0x00265F25 File Offset: 0x00264125
		// (set) Token: 0x06006D86 RID: 28038 RVA: 0x00265F2D File Offset: 0x0026412D
		public MapParent MapParent
		{
			get
			{
				return this.mapParent;
			}
			set
			{
				this.mapParent = value;
			}
		}

		// Token: 0x170012CA RID: 4810
		// (get) Token: 0x06006D87 RID: 28039 RVA: 0x00265F36 File Offset: 0x00264136
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_Incident.<get_QuestLookTargets>d__8 <get_QuestLookTargets>d__ = new QuestPart_Incident.<get_QuestLookTargets>d__8(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006D88 RID: 28040 RVA: 0x00265F48 File Offset: 0x00264148
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.incidentParms != null)
			{
				if (!this.incidentParms.forced)
				{
					string str = "QuestPart incident should always be forced but it's not. incident=";
					IncidentDef incidentDef = this.incident;
					Log.Error(str + ((incidentDef != null) ? incidentDef.ToString() : null));
					this.incidentParms.forced = true;
				}
				this.incidentParms.quest = this.quest;
				if (this.incidentParms.target == null && (this.mapParent == null || !this.mapParent.HasMap || !this.quest.IsParentSuitableForQuest(this.mapParent)))
				{
					MapParent mapParent;
					if ((mapParent = this.quest.TryFindNewSuitableMapParentForRetarget()) == null)
					{
						Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
						mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
					}
					this.mapParent = mapParent;
				}
				if (this.mapParent != null)
				{
					if (!this.mapParent.HasMap)
					{
						return;
					}
					this.incidentParms.target = this.mapParent.Map;
					if (this.incident.Worker.CanFireNow(this.incidentParms))
					{
						this.incident.Worker.TryExecute(this.incidentParms);
					}
					this.incidentParms.target = null;
					return;
				}
				else if (this.incidentParms.target != null && this.incident.Worker.CanFireNow(this.incidentParms))
				{
					this.incident.Worker.TryExecute(this.incidentParms);
				}
			}
		}

		// Token: 0x06006D89 RID: 28041 RVA: 0x002660C8 File Offset: 0x002642C8
		public void SetIncidentParmsAndRemoveTarget(IncidentParms value)
		{
			this.incidentParms = value;
			Map map = this.incidentParms.target as Map;
			if (map != null)
			{
				this.mapParent = map.Parent;
				this.incidentParms.target = null;
				return;
			}
			this.mapParent = null;
		}

		// Token: 0x06006D8A RID: 28042 RVA: 0x00266110 File Offset: 0x00264310
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			Scribe_Deep.Look<IncidentParms>(ref this.incidentParms, "incidentParms", Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x06006D8B RID: 28043 RVA: 0x0026616C File Offset: 0x0026436C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.incident = IncidentDefOf.RaidEnemy;
				this.SetIncidentParmsAndRemoveTarget(new IncidentParms
				{
					target = Find.RandomPlayerHomeMap,
					points = 500f
				});
			}
		}

		// Token: 0x040045C6 RID: 17862
		public string inSignal;

		// Token: 0x040045C7 RID: 17863
		public IncidentDef incident;

		// Token: 0x040045C8 RID: 17864
		private IncidentParms incidentParms;

		// Token: 0x040045C9 RID: 17865
		private MapParent mapParent;
	}
}
