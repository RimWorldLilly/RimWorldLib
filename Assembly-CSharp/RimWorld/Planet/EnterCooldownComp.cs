using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200235F RID: 9055
	public class EnterCooldownComp : WorldObjectComp
	{
		// Token: 0x1700276F RID: 10095
		// (get) Token: 0x0600E06E RID: 57454 RVA: 0x0053777B File Offset: 0x0053597B
		public WorldObjectCompProperties_EnterCooldown Props
		{
			get
			{
				return (WorldObjectCompProperties_EnterCooldown)this.props;
			}
		}

		// Token: 0x17002770 RID: 10096
		// (get) Token: 0x0600E06F RID: 57455 RVA: 0x00537788 File Offset: 0x00535988
		public bool Active
		{
			get
			{
				return this.endTick > GenTicks.TicksGame;
			}
		}

		// Token: 0x17002771 RID: 10097
		// (get) Token: 0x0600E070 RID: 57456 RVA: 0x00537797 File Offset: 0x00535997
		public bool BlocksEntering
		{
			get
			{
				return this.Active && !base.ParentHasMap;
			}
		}

		// Token: 0x17002772 RID: 10098
		// (get) Token: 0x0600E071 RID: 57457 RVA: 0x005377AC File Offset: 0x005359AC
		public int TicksLeft
		{
			get
			{
				if (!this.Active)
				{
					return 0;
				}
				return this.endTick - GenTicks.TicksGame;
			}
		}

		// Token: 0x17002773 RID: 10099
		// (get) Token: 0x0600E072 RID: 57458 RVA: 0x005377C4 File Offset: 0x005359C4
		public float DaysLeft
		{
			get
			{
				return (float)this.TicksLeft / 60000f;
			}
		}

		// Token: 0x0600E073 RID: 57459 RVA: 0x005377D4 File Offset: 0x005359D4
		public void Start(float? durationDays = null)
		{
			float num = durationDays ?? this.Props.durationDays;
			this.endTick = GenTicks.TicksGame + Mathf.RoundToInt(num * 60000f);
		}

		// Token: 0x0600E074 RID: 57460 RVA: 0x00537819 File Offset: 0x00535A19
		public void Stop()
		{
			this.endTick = 0;
		}

		// Token: 0x0600E075 RID: 57461 RVA: 0x00537822 File Offset: 0x00535A22
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (this.Active)
			{
				this.Stop();
			}
		}

		// Token: 0x0600E076 RID: 57462 RVA: 0x00537838 File Offset: 0x00535A38
		public override void PostMyMapRemoved()
		{
			base.PostMyMapRemoved();
			if (this.Props.autoStartOnMapRemoved)
			{
				this.Start(null);
			}
		}

		// Token: 0x0600E077 RID: 57463 RVA: 0x00537868 File Offset: 0x00535A68
		public override void PostExposeData()
		{
			base.PostExposeData();
			int num = 0;
			Scribe_Values.Look<int>(ref num, "ticksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && num > 0 && this.endTick == 0)
			{
				this.endTick = GenTicks.TicksGame + num;
			}
		}

		// Token: 0x0600E078 RID: 57464 RVA: 0x005378BE File Offset: 0x00535ABE
		public override IEnumerable<Gizmo> GetGizmos()
		{
			EnterCooldownComp.<GetGizmos>d__16 <GetGizmos>d__ = new EnterCooldownComp.<GetGizmos>d__16(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x04009006 RID: 36870
		private int endTick;
	}
}
