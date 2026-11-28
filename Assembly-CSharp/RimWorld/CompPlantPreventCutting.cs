using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001935 RID: 6453
	[StaticConstructorOnStartup]
	public class CompPlantPreventCutting : ThingComp
	{
		// Token: 0x17001BF9 RID: 7161
		// (get) Token: 0x0600A1E1 RID: 41441 RVA: 0x0039DDCD File Offset: 0x0039BFCD
		public CompProperties_PlantPreventCutting Props
		{
			get
			{
				return (CompProperties_PlantPreventCutting)this.props;
			}
		}

		// Token: 0x17001BFA RID: 7162
		// (get) Token: 0x0600A1E2 RID: 41442 RVA: 0x0039DDDA File Offset: 0x0039BFDA
		// (set) Token: 0x0600A1E3 RID: 41443 RVA: 0x0039DDE4 File Offset: 0x0039BFE4
		public bool PreventCutting
		{
			get
			{
				return this.preventCutting;
			}
			set
			{
				this.preventCutting = value;
				if (!this.preventCutting)
				{
					return;
				}
				this.parent.Map.designationManager.TryRemoveDesignationOn(this.parent, DesignationDefOf.CutPlant);
				this.parent.Map.designationManager.TryRemoveDesignationOn(this.parent, DesignationDefOf.HarvestPlant);
			}
		}

		// Token: 0x0600A1E4 RID: 41444 RVA: 0x0039DE41 File Offset: 0x0039C041
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.preventCutting, "canCut", false, false);
		}

		// Token: 0x0600A1E5 RID: 41445 RVA: 0x0039DE55 File Offset: 0x0039C055
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompPlantPreventCutting.<CompGetGizmosExtra>d__8 <CompGetGizmosExtra>d__ = new CompPlantPreventCutting.<CompGetGizmosExtra>d__8(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005FA2 RID: 24482
		private bool preventCutting = true;

		// Token: 0x04005FA3 RID: 24483
		private static readonly Texture2D IconTexture = ContentFinder<Texture2D>.Get("UI/Designators/PreventCutting", true);
	}
}
