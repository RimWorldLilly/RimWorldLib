using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020019C8 RID: 6600
	[StaticConstructorOnStartup]
	public class Command_LoadToTransporter : Command
	{
		// Token: 0x0600A5CE RID: 42446 RVA: 0x003B1E4C File Offset: 0x003B004C
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			if (this.transporters == null)
			{
				this.transporters = new List<CompTransporter>();
			}
			if (!this.transporters.Contains(this.transComp))
			{
				this.transporters.Add(this.transComp);
			}
			CompLaunchable_TransportPod compLaunchable_TransportPod = this.transComp.Launchable as CompLaunchable_TransportPod;
			if (compLaunchable_TransportPod != null)
			{
				CompRefuelable fuelingPortSource = compLaunchable_TransportPod.FuelingPortSource;
				ThingWithComps thingWithComps = (fuelingPortSource != null) ? fuelingPortSource.parent : null;
				if (thingWithComps != null)
				{
					Map map = this.transComp.Map;
					Command_LoadToTransporter.tmpFuelingPortGivers.Clear();
					map.floodFiller.FloodFill(thingWithComps.Position, (IntVec3 x) => FuelingPortUtility.AnyFuelingPortGiverAt(x, map), delegate(IntVec3 x)
					{
						Command_LoadToTransporter.tmpFuelingPortGivers.Add(FuelingPortUtility.FuelingPortGiverAt(x, map));
					}, int.MaxValue, false, null);
					foreach (CompTransporter compTransporter in this.transporters)
					{
						CompLaunchable_TransportPod compLaunchable_TransportPod2 = compTransporter.Launchable as CompLaunchable_TransportPod;
						ThingWithComps thingWithComps2;
						if (compLaunchable_TransportPod2 == null)
						{
							thingWithComps2 = null;
						}
						else
						{
							CompRefuelable fuelingPortSource2 = compLaunchable_TransportPod2.FuelingPortSource;
							thingWithComps2 = ((fuelingPortSource2 != null) ? fuelingPortSource2.parent : null);
						}
						ThingWithComps thingWithComps3 = thingWithComps2;
						if (thingWithComps3 != null && !Command_LoadToTransporter.tmpFuelingPortGivers.Contains(thingWithComps3))
						{
							Messages.Message("MessageTransportersNotAdjacent".Translate(), thingWithComps3, MessageTypeDefOf.RejectInput, false);
							return;
						}
					}
				}
			}
			foreach (CompTransporter compTransporter2 in this.transporters)
			{
				if (compTransporter2 != this.transComp && !this.transComp.Map.reachability.CanReach(this.transComp.parent.Position, compTransporter2.parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)))
				{
					Messages.Message("MessageTransporterUnreachable".Translate(), compTransporter2.parent, MessageTypeDefOf.RejectInput, false);
					return;
				}
			}
			Dialog_LoadTransporters window = new Dialog_LoadTransporters(this.transComp.Map, this.transporters);
			Find.WindowStack.Add(window);
		}

		// Token: 0x0600A5CF RID: 42447 RVA: 0x003B208C File Offset: 0x003B028C
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			if (this.transComp.Props.max1PerGroup)
			{
				return false;
			}
			Command_LoadToTransporter command_LoadToTransporter = (Command_LoadToTransporter)other;
			if (command_LoadToTransporter.transComp.parent.def != this.transComp.parent.def)
			{
				return false;
			}
			if (this.transporters == null)
			{
				this.transporters = new List<CompTransporter>();
			}
			this.transporters.Add(command_LoadToTransporter.transComp);
			return false;
		}

		// Token: 0x040061D6 RID: 25046
		public CompTransporter transComp;

		// Token: 0x040061D7 RID: 25047
		private List<CompTransporter> transporters;

		// Token: 0x040061D8 RID: 25048
		private static HashSet<Building> tmpFuelingPortGivers = new HashSet<Building>();
	}
}
