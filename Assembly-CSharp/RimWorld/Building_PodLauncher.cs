using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AC5 RID: 6853
	[StaticConstructorOnStartup]
	public class Building_PodLauncher : Building, INotifyLaunchableLaunch
	{
		// Token: 0x0600AE69 RID: 44649 RVA: 0x003D6E23 File Offset: 0x003D5023
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_PodLauncher.<GetGizmos>d__2 <GetGizmos>d__ = new Building_PodLauncher.<GetGizmos>d__2(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AE6A RID: 44650 RVA: 0x003D6E33 File Offset: 0x003D5033
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.autoPlacePods, "autoPlacePods", false, false);
		}

		// Token: 0x0600AE6B RID: 44651 RVA: 0x003D6E4D File Offset: 0x003D504D
		private void ToggleAutoBuildTransportPods()
		{
			this.autoPlacePods = !this.autoPlacePods;
			if (this.autoPlacePods)
			{
				this.CheckPlacePod();
			}
		}

		// Token: 0x0600AE6C RID: 44652 RVA: 0x003D6E6C File Offset: 0x003D506C
		private void CheckPlacePod()
		{
			if (!this.autoPlacePods)
			{
				return;
			}
			IntVec3 fuelingPortCell = FuelingPortUtility.GetFuelingPortCell(this);
			if (!GenConstruct.CanPlaceBlueprintAt(ThingDefOf.TransportPod, fuelingPortCell, ThingDefOf.TransportPod.defaultPlacingRot, base.Map, false, null, null, null, false, false, false).Accepted)
			{
				return;
			}
			GenConstruct.PlaceBlueprintForBuild(ThingDefOf.TransportPod, fuelingPortCell, base.Map, ThingDefOf.TransportPod.defaultPlacingRot, Faction.OfPlayer, null, null, null, true);
		}

		// Token: 0x0600AE6D RID: 44653 RVA: 0x003D6EDB File Offset: 0x003D50DB
		public void Notify_LaunchableLaunched(CompLaunchable launchable)
		{
			this.CheckPlacePod();
		}

		// Token: 0x040064E8 RID: 25832
		public bool autoPlacePods;

		// Token: 0x040064E9 RID: 25833
		private static readonly Texture2D AutoBuildIcon = ContentFinder<Texture2D>.Get("UI/Commands/AutoBuildTransportPod", true);
	}
}
