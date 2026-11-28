using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AA RID: 6570
	public class CompSubstructureFootprint : ThingComp
	{
		// Token: 0x17001CB2 RID: 7346
		// (get) Token: 0x0600A4F0 RID: 42224 RVA: 0x003AE8AF File Offset: 0x003ACAAF
		public CompProperties_SubstructureFootprint Props
		{
			get
			{
				return (CompProperties_SubstructureFootprint)this.props;
			}
		}

		// Token: 0x17001CB3 RID: 7347
		// (get) Token: 0x0600A4F1 RID: 42225 RVA: 0x003AE8BC File Offset: 0x003ACABC
		public bool DisplaySubstructureOverlay
		{
			get
			{
				return this.Props.displaySubstructureOverlayWhenSelected && this.displaySubstructureOverlay;
			}
		}

		// Token: 0x17001CB4 RID: 7348
		// (get) Token: 0x0600A4F2 RID: 42226 RVA: 0x003AE8D3 File Offset: 0x003ACAD3
		public bool Valid
		{
			get
			{
				return this.facilityComp == null || this.facilityComp.CanBeActive;
			}
		}

		// Token: 0x0600A4F3 RID: 42227 RVA: 0x003AE8EA File Offset: 0x003ACAEA
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			SubstructureGrid substructureGrid = this.parent.Map.substructureGrid;
			if (substructureGrid != null)
			{
				substructureGrid.MarkDirty();
			}
			this.facilityComp = this.parent.TryGetComp<CompGravshipFacility>();
		}

		// Token: 0x0600A4F4 RID: 42228 RVA: 0x003AE91F File Offset: 0x003ACB1F
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			SubstructureGrid substructureGrid = map.substructureGrid;
			if (substructureGrid == null)
			{
				return;
			}
			substructureGrid.MarkDirty();
		}

		// Token: 0x0600A4F5 RID: 42229 RVA: 0x003AE939 File Offset: 0x003ACB39
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompSubstructureFootprint.<CompGetGizmosExtra>d__11 <CompGetGizmosExtra>d__ = new CompSubstructureFootprint.<CompGetGizmosExtra>d__11(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A4F6 RID: 42230 RVA: 0x003AE949 File Offset: 0x003ACB49
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.displaySubstructureOverlay, "displaySubstructureOverlay", true, false);
		}

		// Token: 0x04006177 RID: 24951
		[Unsaved(false)]
		private CompGravshipFacility facilityComp;

		// Token: 0x04006178 RID: 24952
		private bool displaySubstructureOverlay = true;

		// Token: 0x04006179 RID: 24953
		private static readonly CachedTexture DisplaySubstructureTex = new CachedTexture("UI/Commands/ShowSubstructureOverlay");
	}
}
