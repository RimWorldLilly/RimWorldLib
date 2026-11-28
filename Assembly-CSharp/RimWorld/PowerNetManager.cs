using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001371 RID: 4977
	public class PowerNetManager
	{
		// Token: 0x06007B5A RID: 31578 RVA: 0x002BEA6F File Offset: 0x002BCC6F
		public PowerNetManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700150E RID: 5390
		// (get) Token: 0x06007B5B RID: 31579 RVA: 0x002BEA94 File Offset: 0x002BCC94
		public List<PowerNet> AllNetsListForReading
		{
			get
			{
				return this.allNets;
			}
		}

		// Token: 0x06007B5C RID: 31580 RVA: 0x002BEA9C File Offset: 0x002BCC9C
		public void Notify_TransmitterSpawned(CompPower newTransmitter)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterTransmitter, newTransmitter));
			this.NotifyDrawersForWireUpdate(newTransmitter.parent.Position);
		}

		// Token: 0x06007B5D RID: 31581 RVA: 0x002BEAC1 File Offset: 0x002BCCC1
		public void Notify_TransmitterDespawned(CompPower oldTransmitter)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterTransmitter, oldTransmitter));
			this.NotifyDrawersForWireUpdate(oldTransmitter.parent.Position);
		}

		// Token: 0x06007B5E RID: 31582 RVA: 0x002BEAE8 File Offset: 0x002BCCE8
		public void Notfiy_TransmitterTransmitsPowerNowChanged(CompPower transmitter)
		{
			if (!transmitter.parent.Spawned)
			{
				return;
			}
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterTransmitter, transmitter));
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterTransmitter, transmitter));
			this.NotifyDrawersForWireUpdate(transmitter.parent.Position);
		}

		// Token: 0x06007B5F RID: 31583 RVA: 0x002BEB38 File Offset: 0x002BCD38
		public void Notify_ConnectorWantsConnect(CompPower wantingCon)
		{
			if (Scribe.mode == LoadSaveMode.Inactive && !this.HasRegisterConnectorDuplicate(wantingCon))
			{
				this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.RegisterConnector, wantingCon));
			}
			this.NotifyDrawersForWireUpdate(wantingCon.parent.Position);
		}

		// Token: 0x06007B60 RID: 31584 RVA: 0x002BEB6D File Offset: 0x002BCD6D
		public void Notify_ConnectorDespawned(CompPower oldCon)
		{
			this.delayedActions.Add(new PowerNetManager.DelayedAction(PowerNetManager.DelayedActionType.DeregisterConnector, oldCon));
			this.NotifyDrawersForWireUpdate(oldCon.parent.Position);
		}

		// Token: 0x06007B61 RID: 31585 RVA: 0x002BEB92 File Offset: 0x002BCD92
		public void NotifyDrawersForWireUpdate(IntVec3 root)
		{
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlagDefOf.Things, true, false);
			this.map.mapDrawer.MapMeshDirty(root, MapMeshFlagDefOf.PowerGrid, true, true);
		}

		// Token: 0x06007B62 RID: 31586 RVA: 0x002BEBCE File Offset: 0x002BCDCE
		public void RegisterPowerNet(PowerNet newNet)
		{
			this.allNets.Add(newNet);
			newNet.powerNetManager = this;
			this.map.powerNetGrid.Notify_PowerNetCreated(newNet);
			PowerNetMaker.UpdateVisualLinkagesFor(newNet);
		}

		// Token: 0x06007B63 RID: 31587 RVA: 0x002BEBFA File Offset: 0x002BCDFA
		public void DeletePowerNet(PowerNet oldNet)
		{
			this.allNets.Remove(oldNet);
			this.map.powerNetGrid.Notify_PowerNetDeleted(oldNet);
		}

		// Token: 0x06007B64 RID: 31588 RVA: 0x002BEC1C File Offset: 0x002BCE1C
		public void PowerNetsTick()
		{
			for (int i = 0; i < this.allNets.Count; i++)
			{
				this.allNets[i].PowerNetTick();
			}
		}

		// Token: 0x06007B65 RID: 31589 RVA: 0x002BEC50 File Offset: 0x002BCE50
		public void UpdatePowerNetsAndConnections_First()
		{
			int count = this.delayedActions.Count;
			int i = 0;
			while (i < count)
			{
				PowerNetManager.DelayedAction delayedAction = this.delayedActions[i];
				PowerNetManager.DelayedActionType type = this.delayedActions[i].type;
				if (type != PowerNetManager.DelayedActionType.RegisterTransmitter)
				{
					if (type == PowerNetManager.DelayedActionType.DeregisterTransmitter)
					{
						goto IL_11C;
					}
				}
				else if (delayedAction.position == delayedAction.compPower.parent.Position)
				{
					ThingWithComps parent = delayedAction.compPower.parent;
					if (this.map.powerNetGrid.TransmittedPowerNetAt(parent.Position) != null)
					{
						string[] array = new string[5];
						array[0] = "Tried to register trasmitter ";
						int num = 1;
						ThingWithComps thingWithComps = parent;
						array[num] = ((thingWithComps != null) ? thingWithComps.ToString() : null);
						array[2] = " at ";
						array[3] = parent.Position.ToString();
						array[4] = ", but there is already a power net here. There can't be two transmitters on the same cell.";
						Log.Warning(string.Concat(array));
					}
					delayedAction.compPower.SetUpPowerVars();
					using (IEnumerator<IntVec3> enumerator = GenAdj.CellsAdjacentCardinal(parent).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntVec3 cell = enumerator.Current;
							this.TryDestroyNetAt(cell);
						}
						goto IL_144;
					}
					goto IL_11C;
				}
				IL_144:
				i++;
				continue;
				IL_11C:
				this.TryDestroyNetAt(delayedAction.position);
				PowerConnectionMaker.DisconnectAllFromTransmitterAndSetWantConnect(delayedAction.compPower, this.map);
				delayedAction.compPower.ResetPowerVars();
				goto IL_144;
			}
			for (int j = 0; j < count; j++)
			{
				PowerNetManager.DelayedAction delayedAction2 = this.delayedActions[j];
				if ((delayedAction2.type == PowerNetManager.DelayedActionType.RegisterTransmitter && delayedAction2.position == delayedAction2.compPower.parent.Position) || delayedAction2.type == PowerNetManager.DelayedActionType.DeregisterTransmitter)
				{
					this.TryCreateNetAt(delayedAction2.position);
					foreach (IntVec3 cell2 in GenAdj.CellsAdjacentCardinal(delayedAction2.position, delayedAction2.rotation, delayedAction2.compPower.parent.def.size))
					{
						this.TryCreateNetAt(cell2);
					}
				}
			}
			for (int k = 0; k < count; k++)
			{
				PowerNetManager.DelayedAction delayedAction3 = this.delayedActions[k];
				PowerNetManager.DelayedActionType type = this.delayedActions[k].type;
				if (type != PowerNetManager.DelayedActionType.RegisterConnector)
				{
					if (type == PowerNetManager.DelayedActionType.DeregisterConnector)
					{
						PowerConnectionMaker.DisconnectFromPowerNet(delayedAction3.compPower);
						delayedAction3.compPower.ResetPowerVars();
					}
				}
				else if (delayedAction3.position == delayedAction3.compPower.parent.Position)
				{
					delayedAction3.compPower.SetUpPowerVars();
					PowerConnectionMaker.TryConnectToAnyPowerNet(delayedAction3.compPower, null);
				}
			}
			this.delayedActions.RemoveRange(0, count);
			if (DebugViewSettings.drawPower)
			{
				this.DrawDebugPowerNets();
			}
		}

		// Token: 0x06007B66 RID: 31590 RVA: 0x002BEF34 File Offset: 0x002BD134
		private bool HasRegisterConnectorDuplicate(CompPower compPower)
		{
			for (int i = this.delayedActions.Count - 1; i >= 0; i--)
			{
				if (this.delayedActions[i].compPower == compPower)
				{
					if (this.delayedActions[i].type == PowerNetManager.DelayedActionType.DeregisterConnector)
					{
						return false;
					}
					if (this.delayedActions[i].type == PowerNetManager.DelayedActionType.RegisterConnector)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007B67 RID: 31591 RVA: 0x002BEF9C File Offset: 0x002BD19C
		private void TryCreateNetAt(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return;
			}
			if (this.map.powerNetGrid.TransmittedPowerNetAt(cell) == null)
			{
				Building transmitter = cell.GetTransmitter(this.map);
				if (transmitter != null && transmitter.TransmitsPowerNow)
				{
					PowerNet powerNet = PowerNetMaker.NewPowerNetStartingFrom(transmitter);
					this.RegisterPowerNet(powerNet);
					for (int i = 0; i < powerNet.transmitters.Count; i++)
					{
						PowerConnectionMaker.ConnectAllConnectorsToTransmitter(powerNet.transmitters[i]);
					}
				}
			}
		}

		// Token: 0x06007B68 RID: 31592 RVA: 0x002BF018 File Offset: 0x002BD218
		private void TryDestroyNetAt(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return;
			}
			PowerNet powerNet = this.map.powerNetGrid.TransmittedPowerNetAt(cell);
			if (powerNet != null)
			{
				this.DeletePowerNet(powerNet);
			}
		}

		// Token: 0x06007B69 RID: 31593 RVA: 0x002BF050 File Offset: 0x002BD250
		private void DrawDebugPowerNets()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (Find.CurrentMap != this.map)
			{
				return;
			}
			int num = 0;
			foreach (PowerNet powerNet in this.allNets)
			{
				foreach (CompPower compPower in powerNet.transmitters.Concat(powerNet.connectors))
				{
					foreach (IntVec3 c in GenAdj.CellsOccupiedBy(compPower.parent))
					{
						CellRenderer.RenderCell(c, (float)num * 0.44f);
					}
				}
				num++;
			}
		}

		// Token: 0x04004DE4 RID: 19940
		public Map map;

		// Token: 0x04004DE5 RID: 19941
		private List<PowerNet> allNets = new List<PowerNet>();

		// Token: 0x04004DE6 RID: 19942
		private List<PowerNetManager.DelayedAction> delayedActions = new List<PowerNetManager.DelayedAction>();

		// Token: 0x02003268 RID: 12904
		private enum DelayedActionType
		{
			// Token: 0x0400CA9D RID: 51869
			RegisterTransmitter,
			// Token: 0x0400CA9E RID: 51870
			DeregisterTransmitter,
			// Token: 0x0400CA9F RID: 51871
			RegisterConnector,
			// Token: 0x0400CAA0 RID: 51872
			DeregisterConnector
		}

		// Token: 0x02003269 RID: 12905
		private struct DelayedAction
		{
			// Token: 0x06012B95 RID: 76693 RVA: 0x0063F357 File Offset: 0x0063D557
			public DelayedAction(PowerNetManager.DelayedActionType type, CompPower compPower)
			{
				this.type = type;
				this.compPower = compPower;
				this.position = compPower.parent.Position;
				this.rotation = compPower.parent.Rotation;
			}

			// Token: 0x0400CAA1 RID: 51873
			public PowerNetManager.DelayedActionType type;

			// Token: 0x0400CAA2 RID: 51874
			public CompPower compPower;

			// Token: 0x0400CAA3 RID: 51875
			public IntVec3 position;

			// Token: 0x0400CAA4 RID: 51876
			public Rot4 rotation;
		}
	}
}
