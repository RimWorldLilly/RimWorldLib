using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CF5 RID: 7413
	public class TransferableOneWay : Transferable
	{
		// Token: 0x170022CA RID: 8906
		// (get) Token: 0x0600BF22 RID: 48930 RVA: 0x00453151 File Offset: 0x00451351
		public override Thing AnyThing
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.things[0];
			}
		}

		// Token: 0x170022CB RID: 8907
		// (get) Token: 0x0600BF23 RID: 48931 RVA: 0x00453169 File Offset: 0x00451369
		public override ThingDef ThingDef
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.AnyThing.def;
			}
		}

		// Token: 0x170022CC RID: 8908
		// (get) Token: 0x0600BF24 RID: 48932 RVA: 0x00453180 File Offset: 0x00451380
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x170022CD RID: 8909
		// (get) Token: 0x0600BF25 RID: 48933 RVA: 0x00453190 File Offset: 0x00451390
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x170022CE RID: 8910
		// (get) Token: 0x0600BF26 RID: 48934 RVA: 0x0045319D File Offset: 0x0045139D
		public override bool Interactive
		{
			get
			{
				return this.interactive;
			}
		}

		// Token: 0x170022CF RID: 8911
		// (get) Token: 0x0600BF27 RID: 48935 RVA: 0x004531A5 File Offset: 0x004513A5
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x170022D0 RID: 8912
		// (get) Token: 0x0600BF28 RID: 48936 RVA: 0x004531A8 File Offset: 0x004513A8
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x170022D1 RID: 8913
		// (get) Token: 0x0600BF29 RID: 48937 RVA: 0x004531C3 File Offset: 0x004513C3
		// (set) Token: 0x0600BF2A RID: 48938 RVA: 0x004531CB File Offset: 0x004513CB
		public override int CountToTransfer
		{
			get
			{
				return this.countToTransfer;
			}
			protected set
			{
				this.countToTransfer = value;
				base.EditBuffer = value.ToStringCached();
			}
		}

		// Token: 0x170022D2 RID: 8914
		// (get) Token: 0x0600BF2B RID: 48939 RVA: 0x004531E0 File Offset: 0x004513E0
		public int MaxCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.things.Count; i++)
				{
					num += this.things[i].stackCount;
				}
				return num;
			}
		}

		// Token: 0x0600BF2C RID: 48940 RVA: 0x0045321A File Offset: 0x0045141A
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x0600BF2D RID: 48941 RVA: 0x0045321D File Offset: 0x0045141D
		public override int GetMaximumToTransfer()
		{
			return this.MaxCount;
		}

		// Token: 0x0600BF2E RID: 48942 RVA: 0x00453225 File Offset: 0x00451425
		public override AcceptanceReport OverflowReport()
		{
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x0600BF2F RID: 48943 RVA: 0x0045323C File Offset: 0x0045143C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.things.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.countToTransfer, "countToTransfer", 0, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.interactive, "interactive", true, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				base.EditBuffer = this.countToTransfer.ToStringCached();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.things.RemoveAll((Thing x) => x == null) != 0)
				{
					Log.Warning("Some of the things were null after loading.");
				}
			}
		}

		// Token: 0x04006DAD RID: 28077
		public List<Thing> things = new List<Thing>();

		// Token: 0x04006DAE RID: 28078
		private int countToTransfer;

		// Token: 0x04006DAF RID: 28079
		public bool interactive = true;
	}
}
