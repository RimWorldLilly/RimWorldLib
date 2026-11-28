using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004B5 RID: 1205
	public class Hediff_Level : HediffWithComps
	{
		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x000C6E9C File Offset: 0x000C509C
		public override string Label
		{
			get
			{
				if (!this.def.levelIsQuantity)
				{
					return this.def.label + " (" + "LevelNum".Translate(this.level).ToString() + ")";
				}
				return this.def.label + " x" + this.level.ToString();
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06002175 RID: 8565 RVA: 0x000C6F14 File Offset: 0x000C5114
		public override bool ShouldRemove
		{
			get
			{
				return this.level == 0;
			}
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x000C6F1F File Offset: 0x000C511F
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part == null)
			{
				Log.Error(this.def.defName + " has null Part. It should be set before PostAdd.");
			}
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x000C6F4A File Offset: 0x000C514A
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.Severity = (float)this.level;
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x000C6F60 File Offset: 0x000C5160
		public virtual void ChangeLevel(int levelOffset)
		{
			this.level = (int)Mathf.Clamp((float)(this.level + levelOffset), this.def.minSeverity, this.def.maxSeverity);
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000C6F8D File Offset: 0x000C518D
		public virtual void SetLevelTo(int targetLevel)
		{
			if (targetLevel != this.level)
			{
				this.ChangeLevel(targetLevel - this.level);
			}
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000C6FA8 File Offset: 0x000C51A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.level, "level", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error(base.GetType().Name + " has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
			}
		}

		// Token: 0x0400196D RID: 6509
		public int level = 1;
	}
}
