using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001963 RID: 6499
	public class CompSealable : ThingComp
	{
		// Token: 0x17001C3F RID: 7231
		// (get) Token: 0x0600A319 RID: 41753 RVA: 0x003A40AD File Offset: 0x003A22AD
		private CompProperties_Sealable Props
		{
			get
			{
				return (CompProperties_Sealable)this.props;
			}
		}

		// Token: 0x17001C40 RID: 7232
		// (get) Token: 0x0600A31A RID: 41754 RVA: 0x003A40BA File Offset: 0x003A22BA
		private MapPortal Portal
		{
			get
			{
				return (MapPortal)this.parent;
			}
		}

		// Token: 0x17001C41 RID: 7233
		// (get) Token: 0x0600A31B RID: 41755 RVA: 0x003A40C8 File Offset: 0x003A22C8
		private Texture2D SealTex
		{
			get
			{
				Texture2D result;
				if ((result = this.cachedSealTex) == null)
				{
					result = (this.cachedSealTex = ContentFinder<Texture2D>.Get(this.Props.sealTexPath, true));
				}
				return result;
			}
		}

		// Token: 0x0600A31C RID: 41756 RVA: 0x003A40F9 File Offset: 0x003A22F9
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.isSealed, "isSealed", false, false);
		}

		// Token: 0x0600A31D RID: 41757 RVA: 0x003A410D File Offset: 0x003A230D
		public override AcceptanceReport CanEnterPortal()
		{
			if (this.isSealed)
			{
				return this.Props.cannotEnterLabel;
			}
			return true;
		}

		// Token: 0x0600A31E RID: 41758 RVA: 0x003A4130 File Offset: 0x003A2330
		public void Seal()
		{
			if (!this.Portal.PocketMapExists)
			{
				Log.Error("Tried to seal portal but pocket map doesn't exist");
				return;
			}
			this.isSealed = true;
			PocketMapUtility.DestroyPocketMap(this.Portal.PocketMap);
			this.Portal.BroadcastCompSignal("Sealed");
			if (this.Props.destroyPortal)
			{
				Thing.allowDestroyNonDestroyable = true;
				this.Portal.Destroy(DestroyMode.Deconstruct);
				Thing.allowDestroyNonDestroyable = false;
				return;
			}
			this.Portal.DirtyMapMesh(this.Portal.Map);
		}

		// Token: 0x0600A31F RID: 41759 RVA: 0x003A41B8 File Offset: 0x003A23B8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompSealable.<CompGetGizmosExtra>d__12 <CompGetGizmosExtra>d__ = new CompSealable.<CompGetGizmosExtra>d__12(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A320 RID: 41760 RVA: 0x003A41C8 File Offset: 0x003A23C8
		private AcceptanceReport ValidateSealer(LocalTargetInfo target)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!pawn.CanReach(this.Portal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate();
			}
			if (pawn.Downed)
			{
				return "DownedLower".Translate();
			}
			return true;
		}

		// Token: 0x0600A321 RID: 41761 RVA: 0x003A4232 File Offset: 0x003A2432
		public override string CompInspectStringExtra()
		{
			if (!this.isSealed)
			{
				return null;
			}
			return this.Props.sealedInspectString;
		}

		// Token: 0x0400605E RID: 24670
		public const string SealedSignal = "Sealed";

		// Token: 0x0400605F RID: 24671
		private bool isSealed;

		// Token: 0x04006060 RID: 24672
		private Texture2D cachedSealTex;
	}
}
