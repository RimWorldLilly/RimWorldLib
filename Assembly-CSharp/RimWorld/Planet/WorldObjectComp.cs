using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200236C RID: 9068
	public abstract class WorldObjectComp
	{
		// Token: 0x1700278C RID: 10124
		// (get) Token: 0x0600E0DE RID: 57566 RVA: 0x00538BC9 File Offset: 0x00536DC9
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x1700278D RID: 10125
		// (get) Token: 0x0600E0DF RID: 57567 RVA: 0x00538BD8 File Offset: 0x00536DD8
		public bool ParentHasMap
		{
			get
			{
				MapParent mapParent = this.parent as MapParent;
				return mapParent != null && mapParent.HasMap;
			}
		}

		// Token: 0x0600E0E0 RID: 57568 RVA: 0x00538BFC File Offset: 0x00536DFC
		public virtual void Initialize(WorldObjectCompProperties props)
		{
			this.props = props;
		}

		// Token: 0x0600E0E1 RID: 57569 RVA: 0x00538C05 File Offset: 0x00536E05
		public virtual void CompTick()
		{
		}

		// Token: 0x0600E0E2 RID: 57570 RVA: 0x00538C07 File Offset: 0x00536E07
		public virtual void CompTickInterval(int delta)
		{
		}

		// Token: 0x0600E0E3 RID: 57571 RVA: 0x00538C09 File Offset: 0x00536E09
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x0600E0E4 RID: 57572 RVA: 0x00538C10 File Offset: 0x00536E10
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x0600E0E5 RID: 57573 RVA: 0x00538C17 File Offset: 0x00536E17
		public virtual IEnumerable<Gizmo> GetCaravanGizmos(Caravan caravan)
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x0600E0E6 RID: 57574 RVA: 0x00538C1E File Offset: 0x00536E1E
		public virtual IEnumerable<IncidentTargetTagDef> IncidentTargetTags()
		{
			return Enumerable.Empty<IncidentTargetTagDef>();
		}

		// Token: 0x0600E0E7 RID: 57575 RVA: 0x00538C25 File Offset: 0x00536E25
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x0600E0E8 RID: 57576 RVA: 0x00538C27 File Offset: 0x00536E27
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600E0E9 RID: 57577 RVA: 0x00538C2A File Offset: 0x00536E2A
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x0600E0EA RID: 57578 RVA: 0x00538C2D File Offset: 0x00536E2D
		public virtual void PostPostRemove()
		{
		}

		// Token: 0x0600E0EB RID: 57579 RVA: 0x00538C2F File Offset: 0x00536E2F
		public virtual void PostDestroy()
		{
		}

		// Token: 0x0600E0EC RID: 57580 RVA: 0x00538C31 File Offset: 0x00536E31
		public virtual void PostMyMapRemoved()
		{
		}

		// Token: 0x0600E0ED RID: 57581 RVA: 0x00538C33 File Offset: 0x00536E33
		public void PostMyMapSettled()
		{
		}

		// Token: 0x0600E0EE RID: 57582 RVA: 0x00538C35 File Offset: 0x00536E35
		public virtual void PostMapGenerate()
		{
		}

		// Token: 0x0600E0EF RID: 57583 RVA: 0x00538C37 File Offset: 0x00536E37
		public virtual void PostCaravanFormed(Caravan caravan)
		{
		}

		// Token: 0x0600E0F0 RID: 57584 RVA: 0x00538C39 File Offset: 0x00536E39
		public virtual void PostExposeData()
		{
		}

		// Token: 0x0600E0F1 RID: 57585 RVA: 0x00538C3C File Offset: 0x00536E3C
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 3);
			defaultInterpolatedStringHandler.AppendFormatted(base.GetType().Name);
			defaultInterpolatedStringHandler.AppendLiteral("(parent=");
			defaultInterpolatedStringHandler.AppendFormatted<WorldObject>(this.parent);
			defaultInterpolatedStringHandler.AppendLiteral(" at=");
			defaultInterpolatedStringHandler.AppendFormatted<int>((this.parent != null) ? this.parent.Tile.tileId : -1);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x04009020 RID: 36896
		public WorldObject parent;

		// Token: 0x04009021 RID: 36897
		public WorldObjectCompProperties props;
	}
}
