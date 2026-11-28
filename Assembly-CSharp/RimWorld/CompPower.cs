using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200135A RID: 4954
	public abstract class CompPower : ThingComp
	{
		// Token: 0x170014ED RID: 5357
		// (get) Token: 0x06007AAB RID: 31403 RVA: 0x002BAE5C File Offset: 0x002B905C
		public bool TransmitsPowerNow
		{
			get
			{
				return ((Building)this.parent).TransmitsPowerNow;
			}
		}

		// Token: 0x170014EE RID: 5358
		// (get) Token: 0x06007AAC RID: 31404 RVA: 0x002BAE6E File Offset: 0x002B906E
		public PowerNet PowerNet
		{
			get
			{
				if (this.transNet != null)
				{
					return this.transNet;
				}
				if (this.connectParent != null)
				{
					return this.connectParent.transNet;
				}
				return null;
			}
		}

		// Token: 0x170014EF RID: 5359
		// (get) Token: 0x06007AAD RID: 31405 RVA: 0x002BAE94 File Offset: 0x002B9094
		public CompProperties_Power Props
		{
			get
			{
				return (CompProperties_Power)this.props;
			}
		}

		// Token: 0x06007AAE RID: 31406 RVA: 0x002BAEA1 File Offset: 0x002B90A1
		public virtual void ResetPowerVars()
		{
			this.transNet = null;
			this.connectParent = null;
			this.connectChildren = null;
			CompPower.recentlyConnectedNets.Clear();
			CompPower.lastManualReconnector = null;
		}

		// Token: 0x06007AAF RID: 31407 RVA: 0x002BAEC8 File Offset: 0x002B90C8
		public virtual void SetUpPowerVars()
		{
		}

		// Token: 0x06007AB0 RID: 31408 RVA: 0x002BAECC File Offset: 0x002B90CC
		public override void PostExposeData()
		{
			Thing thing = null;
			if (Scribe.mode == LoadSaveMode.Saving && this.connectParent != null)
			{
				thing = this.connectParent.parent;
			}
			Scribe_References.Look<Thing>(ref thing, "parentThing", false);
			if (thing != null)
			{
				this.connectParent = ((ThingWithComps)thing).GetComp<CompPower>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.connectParent != null)
			{
				this.ConnectToTransmitter(this.connectParent, true);
			}
		}

		// Token: 0x06007AB1 RID: 31409 RVA: 0x002BAF38 File Offset: 0x002B9138
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.Props.transmitsPower || this.parent.def.ConnectToPower)
			{
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.PowerGrid, true, false);
				if (this.Props.transmitsPower)
				{
					this.parent.Map.powerNetManager.Notify_TransmitterSpawned(this);
				}
				if (this.parent.def.ConnectToPower)
				{
					this.parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
				}
				this.SetUpPowerVars();
			}
		}

		// Token: 0x06007AB2 RID: 31410 RVA: 0x002BAFE8 File Offset: 0x002B91E8
		public override void PostSwapMap()
		{
			if (this.connectParent != null && !this.connectParent.parent.Spawned)
			{
				this.LostConnectParent();
			}
		}

		// Token: 0x06007AB3 RID: 31411 RVA: 0x002BB00C File Offset: 0x002B920C
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.Props.transmitsPower || this.parent.def.ConnectToPower)
			{
				if (mode != DestroyMode.WillReplace || this.parent.BeingTransportedOnGravship)
				{
					if (this.Props.transmitsPower)
					{
						if (this.connectChildren != null)
						{
							for (int i = 0; i < this.connectChildren.Count; i++)
							{
								this.connectChildren[i].LostConnectParent();
							}
						}
						map.powerNetManager.Notify_TransmitterDespawned(this);
					}
					if (this.parent.def.ConnectToPower)
					{
						map.powerNetManager.Notify_ConnectorDespawned(this);
					}
				}
				map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.PowerGrid, true, false);
			}
		}

		// Token: 0x06007AB4 RID: 31412 RVA: 0x002BB0D4 File Offset: 0x002B92D4
		public virtual void LostConnectParent()
		{
			this.connectParent = null;
			if (this.parent.Spawned)
			{
				this.parent.Map.powerNetManager.Notify_ConnectorWantsConnect(this);
			}
		}

		// Token: 0x06007AB5 RID: 31413 RVA: 0x002BB100 File Offset: 0x002B9300
		public override void PostPrintOnto(SectionLayer layer)
		{
			base.PostPrintOnto(layer);
			if (this.connectParent != null && this.connectParent.parent.def != ThingDefOf.HiddenConduit)
			{
				PowerNetGraphics.PrintWirePieceConnecting(layer, this.parent, this.connectParent.parent, false);
			}
		}

		// Token: 0x06007AB6 RID: 31414 RVA: 0x002BB140 File Offset: 0x002B9340
		public override void CompPrintForPowerGrid(SectionLayer layer)
		{
			if (this.TransmitsPowerNow)
			{
				PowerOverlayMats.LinkedOverlayGraphic.Print(layer, this.parent, 0f);
			}
			Thing thing = this.parent;
			Building building = this.parent as Building;
			if (building != null && building.def.building.isAttachment)
			{
				thing = (GenConstruct.GetWallAttachedTo(this.parent) ?? this.parent);
			}
			if (this.parent.def.ConnectToPower)
			{
				PowerNetGraphics.PrintOverlayConnectorBaseFor(layer, thing);
			}
			if (this.connectParent != null)
			{
				PowerNetGraphics.PrintWirePieceConnecting(layer, thing, this.connectParent.parent, true);
			}
		}

		// Token: 0x06007AB7 RID: 31415 RVA: 0x002BB1DD File Offset: 0x002B93DD
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompPower.<CompGetGizmosExtra>d__21 <CompGetGizmosExtra>d__ = new CompPower.<CompGetGizmosExtra>d__21(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06007AB8 RID: 31416 RVA: 0x002BB1F0 File Offset: 0x002B93F0
		public void TryManualReconnect(bool dispayVisuals = true)
		{
			if (CompPower.lastManualReconnector != this)
			{
				CompPower.recentlyConnectedNets.Clear();
				CompPower.lastManualReconnector = this;
			}
			if (this.PowerNet != null)
			{
				CompPower.recentlyConnectedNets.Add(this.PowerNet);
			}
			IntVec3 connectorPos = this.parent.def.building.isAttachment ? GenConstruct.GetWallAttachedTo(this.parent).Position : this.parent.Position;
			CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(connectorPos, this.parent.Map, CompPower.recentlyConnectedNets);
			if (compPower == null)
			{
				CompPower.recentlyConnectedNets.Clear();
				compPower = PowerConnectionMaker.BestTransmitterForConnector(connectorPos, this.parent.Map, null);
			}
			if (compPower != null)
			{
				PowerConnectionMaker.DisconnectFromPowerNet(this);
				this.ConnectToTransmitter(compPower, false);
				if (dispayVisuals)
				{
					for (int i = 0; i < 5; i++)
					{
						FleckMaker.ThrowMetaPuff(compPower.parent.Position.ToVector3Shifted(), compPower.parent.Map);
					}
				}
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.PowerGrid);
				this.parent.Map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x06007AB9 RID: 31417 RVA: 0x002BB334 File Offset: 0x002B9534
		public void ConnectToTransmitter(CompPower transmitter, bool reconnectingAfterLoading = false)
		{
			if (this.connectParent != null && (!reconnectingAfterLoading || this.connectParent != transmitter))
			{
				string[] array = new string[7];
				array[0] = "Tried to connect ";
				array[1] = ((this != null) ? this.ToString() : null);
				array[2] = " to transmitter ";
				array[3] = ((transmitter != null) ? transmitter.ToString() : null);
				array[4] = " but it's already connected to ";
				int num = 5;
				CompPower compPower = this.connectParent;
				array[num] = ((compPower != null) ? compPower.ToString() : null);
				array[6] = ".";
				Log.Error(string.Concat(array));
				return;
			}
			this.connectParent = transmitter;
			if (this.connectParent.connectChildren == null)
			{
				this.connectParent.connectChildren = new List<CompPower>();
			}
			transmitter.connectChildren.Add(this);
			PowerNet powerNet = this.PowerNet;
			if (powerNet != null)
			{
				powerNet.RegisterConnector(this);
			}
		}

		// Token: 0x06007ABA RID: 31418 RVA: 0x002BB3FC File Offset: 0x002B95FC
		public override string CompInspectStringExtra()
		{
			if (this.PowerNet == null)
			{
				return "PowerNotConnected".Translate();
			}
			string value = (this.PowerNet.CurrentEnergyGainRate() / CompPower.WattsToWattDaysPerTick).ToString("F0");
			string value2 = this.PowerNet.CurrentStoredEnergy().ToString("F0");
			return "PowerConnectedRateStored".Translate(value, value2);
		}

		// Token: 0x04004D76 RID: 19830
		public PowerNet transNet;

		// Token: 0x04004D77 RID: 19831
		public CompPower connectParent;

		// Token: 0x04004D78 RID: 19832
		public List<CompPower> connectChildren;

		// Token: 0x04004D79 RID: 19833
		private static List<PowerNet> recentlyConnectedNets = new List<PowerNet>();

		// Token: 0x04004D7A RID: 19834
		private static CompPower lastManualReconnector = null;

		// Token: 0x04004D7B RID: 19835
		public static readonly float WattsToWattDaysPerTick = 1.6666667E-05f;
	}
}
