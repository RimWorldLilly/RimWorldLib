using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200136B RID: 4971
	public static class PowerConnectionMaker
	{
		// Token: 0x06007B35 RID: 31541 RVA: 0x002BD468 File Offset: 0x002BB668
		public static void ConnectAllConnectorsToTransmitter(CompPower newTransmitter)
		{
			foreach (CompPower compPower in PowerConnectionMaker.PotentialConnectorsForTransmitter(newTransmitter))
			{
				if (compPower.connectParent == null)
				{
					compPower.ConnectToTransmitter(newTransmitter, false);
				}
			}
		}

		// Token: 0x06007B36 RID: 31542 RVA: 0x002BD4C0 File Offset: 0x002BB6C0
		public static void DisconnectAllFromTransmitterAndSetWantConnect(CompPower deadPc, Map map)
		{
			if (deadPc.connectChildren == null)
			{
				return;
			}
			for (int i = 0; i < deadPc.connectChildren.Count; i++)
			{
				CompPower compPower = deadPc.connectChildren[i];
				compPower.connectParent = null;
				CompPowerTrader compPowerTrader = compPower as CompPowerTrader;
				if (compPowerTrader != null)
				{
					compPowerTrader.PowerOn = false;
				}
				map.powerNetManager.Notify_ConnectorWantsConnect(compPower);
			}
		}

		// Token: 0x06007B37 RID: 31543 RVA: 0x002BD520 File Offset: 0x002BB720
		public static void TryConnectToAnyPowerNet(CompPower pc, List<PowerNet> disallowedNets = null)
		{
			if (pc.connectParent != null)
			{
				return;
			}
			if (!pc.parent.Spawned)
			{
				return;
			}
			CompPower compPower = PowerConnectionMaker.BestTransmitterForConnector(pc.parent.def.building.isAttachment ? GenConstruct.GetWallAttachedTo(pc.parent).Position : pc.parent.Position, pc.parent.Map, disallowedNets);
			if (compPower != null)
			{
				pc.ConnectToTransmitter(compPower, false);
				return;
			}
			pc.connectParent = null;
		}

		// Token: 0x06007B38 RID: 31544 RVA: 0x002BD5A0 File Offset: 0x002BB7A0
		public static void DisconnectFromPowerNet(CompPower pc)
		{
			if (pc.connectParent == null)
			{
				return;
			}
			if (pc.PowerNet != null)
			{
				pc.PowerNet.DeregisterConnector(pc);
			}
			if (pc.connectParent.connectChildren != null)
			{
				pc.connectParent.connectChildren.Remove(pc);
				if (pc.connectParent.connectChildren.Count == 0)
				{
					pc.connectParent.connectChildren = null;
				}
			}
			pc.connectParent = null;
		}

		// Token: 0x06007B39 RID: 31545 RVA: 0x002BD60E File Offset: 0x002BB80E
		private static IEnumerable<CompPower> PotentialConnectorsForTransmitter(CompPower b)
		{
			PowerConnectionMaker.<PotentialConnectorsForTransmitter>d__5 <PotentialConnectorsForTransmitter>d__ = new PowerConnectionMaker.<PotentialConnectorsForTransmitter>d__5(-2);
			<PotentialConnectorsForTransmitter>d__.<>3__b = b;
			return <PotentialConnectorsForTransmitter>d__;
		}

		// Token: 0x06007B3A RID: 31546 RVA: 0x002BD620 File Offset: 0x002BB820
		public static CompPower BestTransmitterForConnector(IntVec3 connectorPos, Map map, List<PowerNet> disallowedNets = null)
		{
			CellRect cellRect = CellRect.SingleCell(connectorPos).ExpandedBy(6).ClipInsideMap(map);
			cellRect.ClipInsideMap(map);
			float num = 999999f;
			CompPower result = null;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					Building transmitter = new IntVec3(j, 0, i).GetTransmitter(map);
					if (transmitter != null && !transmitter.Destroyed)
					{
						CompPower powerComp = transmitter.PowerComp;
						if (powerComp != null && powerComp.TransmitsPowerNow && (transmitter.def.building == null || transmitter.def.building.allowWireConnection) && (disallowedNets == null || !disallowedNets.Contains(powerComp.transNet)))
						{
							float num2 = (float)(transmitter.Position - connectorPos).LengthHorizontalSquared;
							if (num2 < num)
							{
								num = num2;
								result = powerComp;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04004DC1 RID: 19905
		private const int ConnectMaxDist = 6;
	}
}
