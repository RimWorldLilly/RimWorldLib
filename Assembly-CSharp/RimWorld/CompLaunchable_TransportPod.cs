using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D8 RID: 6360
	public class CompLaunchable_TransportPod : CompLaunchable
	{
		// Token: 0x17001B70 RID: 7024
		// (get) Token: 0x06009F88 RID: 40840 RVA: 0x00393719 File Offset: 0x00391919
		private new CompProperties_Launchable_TransportPod Props
		{
			get
			{
				return (CompProperties_Launchable_TransportPod)this.props;
			}
		}

		// Token: 0x17001B71 RID: 7025
		// (get) Token: 0x06009F89 RID: 40841 RVA: 0x00393726 File Offset: 0x00391926
		public CompRefuelable FuelingPortSource
		{
			get
			{
				return FuelingPortUtility.FuelingPortGiverAtFuelingPortCell(this.parent.Position, this.parent.Map).TryGetComp<CompRefuelable>();
			}
		}

		// Token: 0x17001B72 RID: 7026
		// (get) Token: 0x06009F8A RID: 40842 RVA: 0x00393748 File Offset: 0x00391948
		public bool ConnectedToFuelingPort
		{
			get
			{
				return this.FuelingPortSource != null;
			}
		}

		// Token: 0x17001B73 RID: 7027
		// (get) Token: 0x06009F8B RID: 40843 RVA: 0x00393753 File Offset: 0x00391953
		public override CompRefuelable Refuelable
		{
			get
			{
				return this.FuelingPortSource;
			}
		}

		// Token: 0x17001B74 RID: 7028
		// (get) Token: 0x06009F8C RID: 40844 RVA: 0x0039375B File Offset: 0x0039195B
		public override float FuelLevel
		{
			get
			{
				return this.FuelingPortSourceFuel;
			}
		}

		// Token: 0x17001B75 RID: 7029
		// (get) Token: 0x06009F8D RID: 40845 RVA: 0x00393763 File Offset: 0x00391963
		public override bool RequiresFuelingPort
		{
			get
			{
				return this.Props.requiresFuelingPort;
			}
		}

		// Token: 0x17001B76 RID: 7030
		// (get) Token: 0x06009F8E RID: 40846 RVA: 0x00393770 File Offset: 0x00391970
		public override float MaxFuelLevel
		{
			get
			{
				if (!this.Props.requiresFuelingPort)
				{
					return float.PositiveInfinity;
				}
				return this.FuelingPortSource.Props.fuelCapacity;
			}
		}

		// Token: 0x17001B77 RID: 7031
		// (get) Token: 0x06009F8F RID: 40847 RVA: 0x00393795 File Offset: 0x00391995
		private float FuelingPortSourceFuel
		{
			get
			{
				if (!this.RequiresFuelingPort)
				{
					return float.PositiveInfinity;
				}
				if (this.FuelingPortSource == null)
				{
					return 0f;
				}
				return this.FuelingPortSource.Fuel;
			}
		}

		// Token: 0x17001B78 RID: 7032
		// (get) Token: 0x06009F90 RID: 40848 RVA: 0x003937C0 File Offset: 0x003919C0
		private bool AllInGroupConnectedToFuelingPort
		{
			get
			{
				foreach (CompTransporter compTransporter in base.TransportersInGroup)
				{
					CompLaunchable_TransportPod compLaunchable_TransportPod = compTransporter.Launchable as CompLaunchable_TransportPod;
					if (compLaunchable_TransportPod != null && !compLaunchable_TransportPod.ConnectedToFuelingPort)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06009F91 RID: 40849 RVA: 0x00393830 File Offset: 0x00391A30
		public override AcceptanceReport CanLaunch(float? overrideFuelLevel)
		{
			if (base.Transporter.LoadingInProgressOrReadyToLaunch && this.Props.requiresFuelingPort && !this.AllInGroupConnectedToFuelingPort)
			{
				return "CommandLaunchGroupFailNotConnectedToFuelingPort".Translate();
			}
			return base.CanLaunch(overrideFuelLevel);
		}

		// Token: 0x06009F92 RID: 40850 RVA: 0x0039386C File Offset: 0x00391A6C
		public override string CompInspectStringExtra()
		{
			if (base.Transporter.LoadingInProgressOrReadyToLaunch && this.Props.requiresFuelingPort && !this.AllInGroupConnectedToFuelingPort)
			{
				return "NotReadyForLaunch".Translate() + ": " + "NotAllInGroupConnectedToFuelingPort".Translate().CapitalizeFirst() + ".";
			}
			return base.CompInspectStringExtra();
		}
	}
}
