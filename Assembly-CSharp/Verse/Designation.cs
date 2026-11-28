using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000251 RID: 593
	public class Designation : IExposable
	{
		// Token: 0x17000350 RID: 848
		// (get) Token: 0x0600115B RID: 4443 RVA: 0x00063644 File Offset: 0x00061844
		private Map Map
		{
			get
			{
				return this.designationManager.map;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x0600115C RID: 4444 RVA: 0x00063651 File Offset: 0x00061851
		public float DesignationDrawAltitude
		{
			get
			{
				return AltitudeLayer.MetaOverlays.AltitudeFor();
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x0600115D RID: 4445 RVA: 0x0006365C File Offset: 0x0006185C
		public Material IconMat
		{
			get
			{
				if (this.cachedMaterial == null)
				{
					if (this.colorDef != null)
					{
						this.cachedMaterial = new Material(this.def.iconMat);
						this.cachedMaterial.color = this.colorDef.color;
					}
					else
					{
						this.cachedMaterial = this.def.iconMat;
					}
				}
				return this.cachedMaterial;
			}
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x000636C4 File Offset: 0x000618C4
		public Designation()
		{
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x000636CC File Offset: 0x000618CC
		public Designation(LocalTargetInfo target, DesignationDef def, ColorDef colorDef = null)
		{
			this.target = target;
			this.def = def;
			this.colorDef = colorDef;
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x000636EC File Offset: 0x000618EC
		public void ExposeData()
		{
			Scribe_Defs.Look<DesignationDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.target, "target");
			Scribe_Defs.Look<ColorDef>(ref this.colorDef, "colorDef");
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == DesignationDefOf.Haul && !this.target.HasThing)
			{
				Log.Error("Haul designation has no target! Deleting.");
				this.Delete();
			}
		}

		// Token: 0x06001161 RID: 4449 RVA: 0x0006375B File Offset: 0x0006195B
		public void Notify_Added()
		{
			if (this.def == DesignationDefOf.Haul)
			{
				this.Map.listerHaulables.HaulDesignationAdded(this.target.Thing);
			}
		}

		// Token: 0x06001162 RID: 4450 RVA: 0x00063785 File Offset: 0x00061985
		internal void Notify_Removing()
		{
			if (this.def == DesignationDefOf.Haul && this.target.HasThing)
			{
				this.Map.listerHaulables.HaulDesignationRemoved(this.target.Thing);
			}
		}

		// Token: 0x06001163 RID: 4451 RVA: 0x000637BC File Offset: 0x000619BC
		public Vector3 DrawLoc()
		{
			if (this.target.HasThing)
			{
				Vector3 vector = this.target.Thing.DrawPos;
				vector.y = this.DesignationDrawAltitude;
				if (this.target.Thing.def.building != null && this.target.Thing.def.building.isAttachment)
				{
					vector += (this.target.Thing.Rotation.AsVector2 * 0.5f).ToVector3();
				}
				return vector;
			}
			return this.target.Cell.ToVector3ShiftedWithAltitude(this.DesignationDrawAltitude);
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x00063873 File Offset: 0x00061A73
		public virtual void DesignationDraw()
		{
			if (this.target.HasThing && !this.target.Thing.Spawned)
			{
				return;
			}
			Graphics.DrawMesh(MeshPool.plane10, this.DrawLoc(), Quaternion.identity, this.IconMat, 0);
		}

		// Token: 0x06001165 RID: 4453 RVA: 0x000638B1 File Offset: 0x00061AB1
		public void Delete()
		{
			this.Map.designationManager.RemoveDesignation(this);
		}

		// Token: 0x06001166 RID: 4454 RVA: 0x000638C4 File Offset: 0x00061AC4
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			array[1] = this.def.defName;
			array[2] = " target=";
			int num = 3;
			LocalTargetInfo localTargetInfo = this.target;
			array[num] = localTargetInfo.ToString();
			array[4] = ")";
			return string.Format(string.Concat(array), Array.Empty<object>());
		}

		// Token: 0x04000FDB RID: 4059
		public DesignationManager designationManager;

		// Token: 0x04000FDC RID: 4060
		public DesignationDef def;

		// Token: 0x04000FDD RID: 4061
		public LocalTargetInfo target;

		// Token: 0x04000FDE RID: 4062
		public ColorDef colorDef;

		// Token: 0x04000FDF RID: 4063
		[Unsaved(false)]
		private Material cachedMaterial;

		// Token: 0x04000FE0 RID: 4064
		public const float ClaimedDesignationDrawAltitude = 15f;
	}
}
