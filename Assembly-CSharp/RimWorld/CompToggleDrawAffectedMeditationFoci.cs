using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C2 RID: 6594
	[StaticConstructorOnStartup]
	public class CompToggleDrawAffectedMeditationFoci : ThingComp
	{
		// Token: 0x17001CD7 RID: 7383
		// (get) Token: 0x0600A580 RID: 42368 RVA: 0x003B0504 File Offset: 0x003AE704
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		// Token: 0x17001CD8 RID: 7384
		// (get) Token: 0x0600A581 RID: 42369 RVA: 0x003B050C File Offset: 0x003AE70C
		public CompProperties_ToggleDrawAffectedMeditationFoci Props
		{
			get
			{
				return (CompProperties_ToggleDrawAffectedMeditationFoci)this.props;
			}
		}

		// Token: 0x0600A582 RID: 42370 RVA: 0x003B0519 File Offset: 0x003AE719
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.enabled = this.Props.defaultEnabled;
			}
		}

		// Token: 0x0600A583 RID: 42371 RVA: 0x003B053C File Offset: 0x003AE73C
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompToggleDrawAffectedMeditationFoci.<CompGetGizmosExtra>d__7 <CompGetGizmosExtra>d__ = new CompToggleDrawAffectedMeditationFoci.<CompGetGizmosExtra>d__7(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A584 RID: 42372 RVA: 0x003B054C File Offset: 0x003AE74C
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.enabled, "enabled", false, false);
		}

		// Token: 0x040061B5 RID: 25013
		private bool enabled;

		// Token: 0x040061B6 RID: 25014
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints", true);
	}
}
