using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200186D RID: 6253
	public abstract class CompDryadHolder : ThingComp, IThingHolder, ISuspendableThingHolder
	{
		// Token: 0x17001ABA RID: 6842
		// (get) Token: 0x06009C60 RID: 40032 RVA: 0x00384FC3 File Offset: 0x003831C3
		public CompProperties_DryadCocoon Props
		{
			get
			{
				return (CompProperties_DryadCocoon)this.props;
			}
		}

		// Token: 0x17001ABB RID: 6843
		// (get) Token: 0x06009C61 RID: 40033 RVA: 0x00384FD0 File Offset: 0x003831D0
		public bool IsContentsSuspended
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001ABC RID: 6844
		// (get) Token: 0x06009C62 RID: 40034 RVA: 0x00384FD3 File Offset: 0x003831D3
		protected CompTreeConnection TreeComp
		{
			get
			{
				if (this.cachedTreeComp == null)
				{
					Thing thing = this.tree;
					this.cachedTreeComp = ((thing != null) ? thing.TryGetComp<CompTreeConnection>() : null);
				}
				return this.cachedTreeComp;
			}
		}

		// Token: 0x17001ABD RID: 6845
		// (get) Token: 0x06009C63 RID: 40035 RVA: 0x00384FFB File Offset: 0x003831FB
		private Material FrontMat
		{
			get
			{
				if (this.cachedFrontMat == null)
				{
					this.cachedFrontMat = MaterialPool.MatFrom("Things/Building/Misc/DryadSphere/DryadSphereFront", ShaderDatabase.Cutout);
				}
				return this.cachedFrontMat;
			}
		}

		// Token: 0x06009C64 RID: 40036 RVA: 0x00385026 File Offset: 0x00383226
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
			}
		}

		// Token: 0x06009C65 RID: 40037 RVA: 0x00385048 File Offset: 0x00383248
		public override void CompTick()
		{
			if (this.tickComplete < 0)
			{
				return;
			}
			if (this.tree == null || this.tree.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (Find.TickManager.TicksGame >= this.tickComplete)
			{
				this.Complete();
			}
		}

		// Token: 0x06009C66 RID: 40038 RVA: 0x0038509C File Offset: 0x0038329C
		public override void PostDraw()
		{
			if (this.Props.drawContents)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					this.innerContainer[i].DrawNowAt(this.parent.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop), false);
				}
				Matrix4x4 matrix = default(Matrix4x4);
				Vector3 pos = this.parent.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop.AltitudeFor() + 0.01f);
				Quaternion q = Quaternion.Euler(0f, this.parent.Rotation.AsAngle, 0f);
				Vector3 s = new Vector3(this.parent.Graphic.drawSize.x, 1f, this.parent.Graphic.drawSize.y);
				matrix.SetTRS(pos, q, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.FrontMat, 0);
			}
		}

		// Token: 0x06009C67 RID: 40039 RVA: 0x0038519C File Offset: 0x0038339C
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompDryadHolder.<CompGetGizmosExtra>d__17 <CompGetGizmosExtra>d__ = new CompDryadHolder.<CompGetGizmosExtra>d__17(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009C68 RID: 40040 RVA: 0x003851AC File Offset: 0x003833AC
		public virtual void TryAcceptPawn(Pawn p)
		{
			bool flag = p.DeSpawnOrDeselect(DestroyMode.Vanish);
			this.innerContainer.TryAddOrTransfer(p, 1, true);
			if (flag)
			{
				Find.Selector.Select(p, false, false);
			}
			SoundDefOf.Pawn_EnterDryadPod.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			if (p.connections != null)
			{
				foreach (Thing thing in p.connections.ConnectedThings)
				{
					if (thing.TryGetComp<CompTreeConnection>() != null)
					{
						this.tree = thing;
						break;
					}
				}
			}
		}

		// Token: 0x06009C69 RID: 40041 RVA: 0x00385258 File Offset: 0x00383458
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "CasketContains".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst();
			if (this.tickComplete >= 0)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					"TimeLeft".Translate().CapitalizeFirst(),
					": ",
					(this.tickComplete - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)
				});
			}
			return text;
		}

		// Token: 0x06009C6A RID: 40042
		protected abstract void Complete();

		// Token: 0x06009C6B RID: 40043 RVA: 0x00385321 File Offset: 0x00383521
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06009C6C RID: 40044 RVA: 0x00385329 File Offset: 0x00383529
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06009C6D RID: 40045 RVA: 0x00385338 File Offset: 0x00383538
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.tickComplete, "tickComplete", -1, false);
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_References.Look<Thing>(ref this.tree, "tree", false);
		}

		// Token: 0x04005CE8 RID: 23784
		protected int tickComplete = -1;

		// Token: 0x04005CE9 RID: 23785
		protected ThingOwner innerContainer;

		// Token: 0x04005CEA RID: 23786
		protected Thing tree;

		// Token: 0x04005CEB RID: 23787
		private CompTreeConnection cachedTreeComp;

		// Token: 0x04005CEC RID: 23788
		private Material cachedFrontMat;

		// Token: 0x04005CED RID: 23789
		protected const int ExpiryDurationTicks = 600;
	}
}
