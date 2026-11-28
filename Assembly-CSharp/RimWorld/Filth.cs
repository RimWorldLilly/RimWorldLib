using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B13 RID: 6931
	public class Filth : Thing
	{
		// Token: 0x17001FEF RID: 8175
		// (get) Token: 0x0600B138 RID: 45368 RVA: 0x003E715E File Offset: 0x003E535E
		public bool CanFilthAttachNow
		{
			get
			{
				return this.def.filth.canFilthAttach && this.thickness > 1 && Find.TickManager.TicksGame - this.growTick > 400;
			}
		}

		// Token: 0x17001FF0 RID: 8176
		// (get) Token: 0x0600B139 RID: 45369 RVA: 0x003E7195 File Offset: 0x003E5395
		public bool CanBeThickened
		{
			get
			{
				return this.thickness < 5;
			}
		}

		// Token: 0x17001FF1 RID: 8177
		// (get) Token: 0x0600B13A RID: 45370 RVA: 0x003E71A0 File Offset: 0x003E53A0
		public int TicksSinceThickened
		{
			get
			{
				return Find.TickManager.TicksGame - this.growTick;
			}
		}

		// Token: 0x17001FF2 RID: 8178
		// (get) Token: 0x0600B13B RID: 45371 RVA: 0x003E71B3 File Offset: 0x003E53B3
		public int DisappearAfterTicks
		{
			get
			{
				return this.disappearAfterTicks;
			}
		}

		// Token: 0x17001FF3 RID: 8179
		// (get) Token: 0x0600B13C RID: 45372 RVA: 0x003E71BC File Offset: 0x003E53BC
		public override string Label
		{
			get
			{
				if (this.sources.NullOrEmpty<string>())
				{
					return "FilthLabel".Translate(base.Label, this.thickness.ToString());
				}
				return "FilthLabelWithSource".Translate(base.Label, this.sources.ToCommaList(true, false), this.thickness.ToString());
			}
		}

		// Token: 0x0600B13D RID: 45373 RVA: 0x003E7240 File Offset: 0x003E5440
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.growTick, "growTick", 0, false);
			Scribe_Values.Look<int>(ref this.thickness, "thickness", 1, false);
			Scribe_Values.Look<int>(ref this.disappearAfterTicks, "disappearAfterTicks", 0, false);
			if (Scribe.mode != LoadSaveMode.Saving || this.drawInstances != null)
			{
				Scribe_Collections.Look<Filth.FilthInstance>(ref this.drawInstances, "drawInstances", LookMode.Deep, Array.Empty<object>());
			}
			if (Scribe.mode != LoadSaveMode.Saving || this.sources != null)
			{
				Scribe_Collections.Look<string>(ref this.sources, "sources", LookMode.Value, Array.Empty<object>());
			}
		}

		// Token: 0x0600B13E RID: 45374 RVA: 0x003E72D8 File Offset: 0x003E54D8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing)
			{
				base.Map.listerFilthInHomeArea.Notify_FilthSpawned(this);
			}
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.growTick = Find.TickManager.TicksGame;
				this.disappearAfterTicks = (int)(this.def.filth.disappearsInDays.RandomInRange * 60000f);
			}
		}

		// Token: 0x0600B13F RID: 45375 RVA: 0x003E7344 File Offset: 0x003E5544
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (Current.ProgramState == ProgramState.Playing)
			{
				map.listerFilthInHomeArea.Notify_FilthDespawned(this);
			}
		}

		// Token: 0x0600B140 RID: 45376 RVA: 0x003E7374 File Offset: 0x003E5574
		public void AddSource(string newSource)
		{
			if (this.sources == null)
			{
				this.sources = new List<string>();
			}
			for (int i = 0; i < this.sources.Count; i++)
			{
				if (this.sources[i] == newSource)
				{
					return;
				}
			}
			while (this.sources.Count > 3)
			{
				this.sources.RemoveAt(0);
			}
			this.sources.Add(newSource);
		}

		// Token: 0x0600B141 RID: 45377 RVA: 0x003E73E8 File Offset: 0x003E55E8
		public void AddSources(IEnumerable<string> sources)
		{
			if (sources == null)
			{
				return;
			}
			foreach (string newSource in sources)
			{
				this.AddSource(newSource);
			}
		}

		// Token: 0x0600B142 RID: 45378 RVA: 0x003E7434 File Offset: 0x003E5634
		public virtual void ThickenFilth()
		{
			this.growTick = Find.TickManager.TicksGame;
			if (this.thickness < this.def.filth.maxThickness)
			{
				this.thickness++;
				this.UpdateMesh();
			}
		}

		// Token: 0x0600B143 RID: 45379 RVA: 0x003E7472 File Offset: 0x003E5672
		public void ThinFilth()
		{
			this.thickness--;
			if (base.Spawned)
			{
				if (this.thickness == 0)
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
				this.UpdateMesh();
			}
		}

		// Token: 0x0600B144 RID: 45380 RVA: 0x003E74A0 File Offset: 0x003E56A0
		private void UpdateMesh()
		{
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
			}
		}

		// Token: 0x0600B145 RID: 45381 RVA: 0x003E74CC File Offset: 0x003E56CC
		public void SetOverrideDrawPositionAndRotation(Vector3 drawPos, float rotation)
		{
			if (this.drawInstances == null)
			{
				this.drawInstances = new List<Filth.FilthInstance>();
			}
			this.drawInstances.Add(new Filth.FilthInstance
			{
				drawPos = drawPos,
				rotation = rotation
			});
			this.UpdateMesh();
		}

		// Token: 0x0600B146 RID: 45382 RVA: 0x003E7516 File Offset: 0x003E5716
		public bool CanDropAt(IntVec3 c, Map map, FilthSourceFlags additionalFlags = FilthSourceFlags.None)
		{
			return FilthMaker.CanMakeFilth(c, map, this.def, additionalFlags);
		}

		// Token: 0x04006662 RID: 26210
		public int thickness = 1;

		// Token: 0x04006663 RID: 26211
		public List<string> sources;

		// Token: 0x04006664 RID: 26212
		private int growTick;

		// Token: 0x04006665 RID: 26213
		private int disappearAfterTicks;

		// Token: 0x04006666 RID: 26214
		public List<Filth.FilthInstance> drawInstances;

		// Token: 0x04006667 RID: 26215
		private const int MaxThickness = 5;

		// Token: 0x04006668 RID: 26216
		private const int MinAgeToPickUp = 400;

		// Token: 0x04006669 RID: 26217
		private const int MaxNumSources = 3;

		// Token: 0x02003820 RID: 14368
		public struct FilthInstance : IExposable
		{
			// Token: 0x060147F9 RID: 83961 RVA: 0x006969A8 File Offset: 0x00694BA8
			public void ExposeData()
			{
				Scribe_Values.Look<float>(ref this.rotation, "rotation", 0f, false);
				Scribe_Values.Look<Vector3>(ref this.drawPos, "drawPos", default(Vector3), false);
			}

			// Token: 0x0400E021 RID: 57377
			public float rotation;

			// Token: 0x0400E022 RID: 57378
			public Vector3 drawPos;
		}
	}
}
