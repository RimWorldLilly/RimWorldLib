using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200180A RID: 6154
	public abstract class CompAncientVent : ThingComp
	{
		// Token: 0x17001A19 RID: 6681
		// (get) Token: 0x060099A2 RID: 39330 RVA: 0x00378F71 File Offset: 0x00377171
		public CompProperties_AncientVentEmitter Props
		{
			get
			{
				return (CompProperties_AncientVentEmitter)this.props;
			}
		}

		// Token: 0x17001A1A RID: 6682
		// (get) Token: 0x060099A3 RID: 39331 RVA: 0x00378F7E File Offset: 0x0037717E
		protected virtual bool AppliesEffectsToPawns
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001A1B RID: 6683
		// (get) Token: 0x060099A4 RID: 39332 RVA: 0x00378F81 File Offset: 0x00377181
		public bool VentOn
		{
			get
			{
				return this.ventOn;
			}
		}

		// Token: 0x060099A5 RID: 39333 RVA: 0x00378F8C File Offset: 0x0037718C
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Destroyed)
			{
				return;
			}
			if (this.CheckShouldToggleAndHandleMapState())
			{
				this.ventOn = !this.ventOn;
				this.ToggleIndividualVent(this.ventOn);
			}
			if (!this.ventOn)
			{
				Sustainer sustainer = this.sustainer;
				if (sustainer != null)
				{
					sustainer.End();
				}
				this.sustainer = null;
				return;
			}
			if (this.sustainer == null)
			{
				this.sustainer = this.Props.activeSound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.PerTick));
			}
			this.sustainer.Maintain();
			if (this.parent.IsHashIntervalTick(20) && this.Props.heatToPush != 0)
			{
				GenTemperature.PushHeat(this.parent, (float)this.Props.heatToPush);
			}
			if (!this.AppliesEffectsToPawns)
			{
				return;
			}
			Map map = this.parent.Map;
			IntVec3 position = this.parent.Position;
			int num = Mathf.FloorToInt((float)this.parent.def.size.x / 2f);
			for (int i = 0; i < map.mapPawns.AllPawnsSpawned.Count; i++)
			{
				Pawn pawn = map.mapPawns.AllPawnsSpawned[i];
				if (pawn.Position.DistanceTo(position) <= (float)num)
				{
					this.ApplyPawnEffect(pawn, 1);
				}
			}
		}

		// Token: 0x060099A6 RID: 39334 RVA: 0x003790E9 File Offset: 0x003772E9
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.ventOn, "ventOn", false, false);
		}

		// Token: 0x060099A7 RID: 39335 RVA: 0x00379104 File Offset: 0x00377304
		private bool CheckShouldToggleAndHandleMapState()
		{
			int num;
			if ((CompAncientVent.sharedLastCheckMtbTickPerMap.TryGetValue(this.parent.Map.uniqueID, out num) ? num : 0) == GenTicks.TicksGame)
			{
				bool flag;
				return CompAncientVent.sharedLastMtbCheckDidOccurPerMap.TryGetValue(this.parent.Map.uniqueID, out flag) && flag;
			}
			int num2 = this.ventOn ? this.Props.onDurationMtbDays : this.Props.offDurationMtbDays;
			CompAncientVent.sharedLastCheckMtbTickPerMap[this.parent.Map.uniqueID] = GenTicks.TicksGame;
			bool flag2 = CompAncientVent.sharedLastMtbCheckDidOccurPerMap[this.parent.Map.uniqueID] = Rand.MTBEventOccurs((float)num2, 60000f, 1f);
			if (flag2)
			{
				this.ToggleMapState();
			}
			return flag2;
		}

		// Token: 0x060099A8 RID: 39336 RVA: 0x003791D0 File Offset: 0x003773D0
		private void ToggleMapState()
		{
			if (this.ventOn)
			{
				if (this.Props.conditionToCause != null)
				{
					GameCondition activeCondition = this.parent.Map.gameConditionManager.GetActiveCondition(this.Props.conditionToCause);
					if (activeCondition != null)
					{
						activeCondition.End();
					}
				}
				this.SendToggleAlert(false);
				return;
			}
			if (this.Props.conditionToCause != null)
			{
				if (!this.parent.Map.gameConditionManager.ConditionIsActive(this.Props.conditionToCause))
				{
					GameCondition gameCondition = GameConditionMaker.MakeConditionPermanent(this.Props.conditionToCause);
					gameCondition.suppressEndMessage = true;
					gameCondition.conditionCauser = this.parent;
					gameCondition.forceDisplayAsDuration = true;
					this.parent.Map.gameConditionManager.RegisterCondition(gameCondition);
				}
				this.SendToggleAlert(true);
			}
		}

		// Token: 0x060099A9 RID: 39337 RVA: 0x0037929C File Offset: 0x0037749C
		public override string CompInspectStringExtra()
		{
			if (!this.ventOn)
			{
				return "DormantCompInactive".Translate();
			}
			return this.Props.activeInspectStringKey.Translate().CapitalizeFirst();
		}

		// Token: 0x060099AA RID: 39338 RVA: 0x003792DE File Offset: 0x003774DE
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompAncientVent.<CompGetGizmosExtra>d__16 <CompGetGizmosExtra>d__ = new CompAncientVent.<CompGetGizmosExtra>d__16(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x060099AB RID: 39339 RVA: 0x003792F0 File Offset: 0x003774F0
		private void DevToggleVent()
		{
			this.ToggleMapState();
			bool on = !this.ventOn;
			foreach (Building thing in this.parent.Map.listerBuildings.AllBuildingsNonColonistOfDef(this.parent.def))
			{
				CompAncientVent compAncientVent = thing.TryGetComp<CompAncientVent>();
				if (compAncientVent != null)
				{
					compAncientVent.ventOn = on;
					compAncientVent.ToggleIndividualVent(on);
				}
			}
		}

		// Token: 0x060099AC RID: 39340 RVA: 0x00379378 File Offset: 0x00377578
		protected void SendToggleAlert(bool goingToBeOn)
		{
			Messages.Message((goingToBeOn ? this.Props.startupMessageKey : this.Props.shutdownMessageKey).Translate(), new LookTargets(this.parent.Map.listerBuildings.AllBuildingsNonColonistOfDef(this.parent.def)), MessageTypeDefOf.NeutralEvent, false);
		}

		// Token: 0x060099AD RID: 39341
		protected abstract void ToggleIndividualVent(bool on);

		// Token: 0x060099AE RID: 39342 RVA: 0x003793DA File Offset: 0x003775DA
		protected virtual void ApplyPawnEffect(Pawn pawn, int delta)
		{
		}

		// Token: 0x04005BA3 RID: 23459
		private static Dictionary<int, int> sharedLastCheckMtbTickPerMap = new Dictionary<int, int>();

		// Token: 0x04005BA4 RID: 23460
		private static Dictionary<int, bool> sharedLastMtbCheckDidOccurPerMap = new Dictionary<int, bool>();

		// Token: 0x04005BA5 RID: 23461
		private const int PushHeatInterval = 20;

		// Token: 0x04005BA6 RID: 23462
		private bool ventOn;

		// Token: 0x04005BA7 RID: 23463
		private Sustainer sustainer;
	}
}
